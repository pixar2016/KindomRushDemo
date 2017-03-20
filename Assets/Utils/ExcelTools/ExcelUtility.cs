using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Excel;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System;

public class ExcelUtility
{

	/// <summary>
	/// 表格数据集合
	/// </summary>
	private DataSet mResultSet;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="excelFile">Excel file.</param>
	public ExcelUtility (string excelFile)
	{
		FileStream mStream = File.Open (excelFile, FileMode.Open, FileAccess.Read);
		IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader (mStream);
		mResultSet = mExcelReader.AsDataSet ();
	}
			
	/// <summary>
	/// 转换为实体类列表
	/// </summary>
	public List<T> ConvertToList<T> ()
	{
		//判断Excel文件中是否存在数据表
		if (mResultSet.Tables.Count < 1)
			return null;
		//默认读取第一个数据表
		DataTable mSheet = mResultSet.Tables [0];
			
		//判断数据表内是否存在数据
		if (mSheet.Rows.Count < 1)
			return null;

		//读取数据表行数和列数
		int rowCount = mSheet.Rows.Count;
		int colCount = mSheet.Columns.Count;
				
		//准备一个列表以保存全部数据
		List<T> list = new List<T> ();
				
		//读取数据
		for (int i=1; i<rowCount; i++) 
		{
			//创建实例
			Type t = typeof(T);
			ConstructorInfo ct = t.GetConstructor (System.Type.EmptyTypes);
			T target = (T)ct.Invoke (null);
			for (int j=0; j<colCount; j++) 
			{
				//读取第1行数据作为表头字段
				string field = mSheet.Rows [0] [j].ToString ();
				object value = mSheet.Rows [i] [j];
				//设置属性值
				SetTargetProperty (target, field, value);
			}
					
			//添加至列表
			list.Add (target);
		}
				
		return list;
	}

	/// <summary>
	/// 转换为Json
	/// </summary>
	/// <param name="JsonPath">Json文件路径</param>
	/// <param name="Header">表头行数</param>
	public void ConvertToJson (string JsonPath, Encoding encoding)
	{
		//判断Excel文件中是否存在数据表
		if (mResultSet.Tables.Count < 1)
			return;

		//默认读取第一个数据表
		DataTable mSheet = mResultSet.Tables [0];

		//判断数据表内是否存在数据
		if (mSheet.Rows.Count < 1)
			return;

		//读取数据表行数和列数
		int rowCount = mSheet.Rows.Count;
		int colCount = mSheet.Columns.Count;

		//准备一个列表存储整个表的数据
        List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();

		//读取数据
		for (int i = 2; i < rowCount; i++) {
			//准备一个字典存储每一行的数据
			Dictionary<string, object> row = new Dictionary<string, object> ();
            if (mSheet.Rows[i][0].ToString() == "")
            {
                continue;
            }
			for (int j = 0; j < colCount; j++) {
				//读取第1行数据作为表头字段
				string field = mSheet.Rows [0] [j].ToString ();
				//Key-Value对应
				row [field] = mSheet.Rows [i] [j];
			}
            //若该行不为空,添加到表数据中
            if (row.Count > 0)
            {
                table.Add(row);
            }
		}

		//生成Json字符串
		string json = JsonConvert.SerializeObject (table, Newtonsoft.Json.Formatting.Indented);
		//写入文件
        Debug.Log(JsonPath);
		using (FileStream fileStream=new FileStream(JsonPath,FileMode.Create,FileAccess.Write)) {
			using (TextWriter textWriter = new StreamWriter(fileStream, encoding)) {
				textWriter.Write (json);
			}
		}
	}

    public void Test(string JsonPath, Encoding encoding)
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;

        //默认读取第一个数据表
        DataTable mSheet = mResultSet.Tables[0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //准备一个列表存储整个表的数据
        //List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();
        Dictionary<int, Dictionary<string, object>> table1 = new Dictionary<int, Dictionary<string, object>>();
        for (int i = 1; i < 3; i++)
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            for (int j = 0; j < 4; j++)
            {
                string field = mSheet.Rows[0][j].ToString();
                if (j == 3)
                {
                    Dictionary<int, object> temp = new Dictionary<int, object>();
                    temp[1] = 0;
                    row[field] = temp;
                }
                else
                {
                    row[field] = 0;
                }
            }
            
            table1[i] = row;
        }

        //生成Json字符串
        string json = JsonConvert.SerializeObject(table1, Newtonsoft.Json.Formatting.Indented);
        //写入文件
        using (FileStream fileStream = new FileStream(JsonPath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
            {
                textWriter.Write(json);
            }
        }
    }

    /// <summary>
	/// 转换为lua
	/// </summary>
	/// <param name="luaPath">lua文件路径</param>
	public void ConvertToLua(string luaPath, Encoding encoding)
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("local datas = {");
        stringBuilder.Append("\r\n");

        //读取数据表
        foreach (DataTable mSheet in mResultSet.Tables)
        {
            //判断数据表内是否存在数据
            if (mSheet.Rows.Count < 1)
                continue;

            //读取数据表行数和列数
            int rowCount = mSheet.Rows.Count;
            int colCount = mSheet.Columns.Count;

            //准备一个列表存储整个表的数据
            List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();

            //读取数据
            for (int i = 1; i < rowCount; i++)
            {
                //准备一个字典存储每一行的数据
                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int j = 0; j < colCount; j++)
                {
                    //读取第1行数据作为表头字段
                    string field = mSheet.Rows[0][j].ToString();
                    //Key-Value对应
                    row[field] = mSheet.Rows[i][j];
                }
                //添加到表数据中
                table.Add(row);
            }
            stringBuilder.Append(string.Format("\t\"{0}\" = ", mSheet.TableName));
            stringBuilder.Append("{\r\n");
            foreach (Dictionary<string, object> dic in table)
            {
                stringBuilder.Append("\t\t{\r\n");
                foreach (string key in dic.Keys)
                {
                    if (dic[key].GetType().Name == "String")
                        stringBuilder.Append(string.Format("\t\t\t\"{0}\" = \"{1}\",\r\n", key, dic[key]));
                    else
                        stringBuilder.Append(string.Format("\t\t\t\"{0}\" = {1},\r\n", key, dic[key]));
                }
                stringBuilder.Append("\t\t},\r\n");
            }
            stringBuilder.Append("\t}\r\n");
        }

        stringBuilder.Append("}\r\n");
        stringBuilder.Append("return datas");

        //写入文件
        using (FileStream fileStream = new FileStream(luaPath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
            {
                textWriter.Write(stringBuilder.ToString());
            }
        }
    }

    public void ConvertToCS(string CSPath, Encoding encoding)
    {
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;
        string path = Application.dataPath + "/Data/CSModel.txt";
        FileStream mStream = File.Open(path, FileMode.Open, FileAccess.Read);
        int Len = (int)mStream.Length;
        byte[] heByte = new byte[Len];
        mStream.Read(heByte, 0, heByte.Length);
        string temp = System.Text.Encoding.UTF8.GetString(heByte);
        StringBuilder myStr = new StringBuilder(temp);
        //Debug.Log(myStr);
        //读取数据表
        foreach (DataTable mSheet in mResultSet.Tables)
        {
            //判断数据表内是否存在数据
            if (mSheet.Rows.Count < 1)
                continue;

            //读取数据表行数和列数
            int rowCount = mSheet.Rows.Count;
            int colCount = mSheet.Columns.Count;

            //准备一个列表存储整个表的数据
            List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();

            List<string> fields = new List<string>();
            List<string> fieldTypes = new List<string>();
            int i;
            for (i = 0; i < colCount; i++)
            {
                if (mSheet.Rows[0][i].ToString() == "")
                {
                    break;
                }
                //读取第1行数据作为表头字段
                fields.Add(mSheet.Rows[0][i].ToString());
                //读取第2行数据作为数据类型
                fieldTypes.Add(mSheet.Rows[1][i].ToString());
            }
            StringBuilder fieldString = new StringBuilder();
            for (i = 0; i < fields.Count; i++)
            {
                fieldString.Append("\tpublic ");
                fieldString.Append(fieldTypes[i]);
                fieldString.Append(" _");
                fieldString.Append(fields[i]);
                fieldString.Append(";\n");
            }
            StringBuilder fieldTypeString = new StringBuilder();
            for (i = 0; i < fields.Count; i++)
            {
                //fieldTypeString.Append("\t\t\tinfo._");
                //fieldTypeString.Append(fields[i]);
                //fieldTypeString.Append(" = " + fieldTypes[i]);
                //fieldTypeString.Append(".Parse(jsonObject[\"");
                //fieldTypeString.Append(fields[i]);
                //fieldTypeString.Append("\"].ToString());\n");
                fieldTypeString.Append(ConvertTypeString(fields[i], fieldTypes[i]));
            }
            myStr.Replace("@name", mSheet.TableName);
            myStr.Replace("@field1", fieldString.ToString());
            myStr.Replace("@field2", fieldTypeString.ToString());
        }
        //Debug.Log(myStr);
        mStream.Close();
        //写入文件
        using (FileStream fileStream = new FileStream(CSPath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
            {
                textWriter.Write(myStr.ToString());
            }
        }
    }

    public string ConvertTypeString(string field, string fieldTypeString)
    {
        StringBuilder fieldType = new StringBuilder();
        if (fieldTypeString == "int")
        {
            fieldType.Append("\t\t\tif(jsonObject[\"");
            fieldType.Append(field);
            fieldType.Append("\"] != null){\n");
            fieldType.Append("\t\t\t\tinfo._");
            fieldType.Append(field);
            fieldType.Append(" = " + fieldTypeString);
            fieldType.Append(".Parse(jsonObject[\"");
            fieldType.Append(field);
            fieldType.Append("\"].ToString());\n");
            fieldType.Append("\t\t\t}\n");
        }
        else if (fieldTypeString == "string")
        {
            fieldType.Append("\t\t\tif(jsonObject[\"");
            fieldType.Append(field);
            fieldType.Append("\"] != null){\n");
            fieldType.Append("\t\t\t\tinfo._");
            fieldType.Append(field);
            fieldType.Append(" = jsonObject[\"");
            fieldType.Append(field);
            fieldType.Append("\"].ToString();\n");
            fieldType.Append("\t\t\t}\n");
        }
        return fieldType.ToString();
    }


    /// <summary>
    /// 转换为CSV
    /// </summary>
    public void ConvertToCSV (string CSVPath, Encoding encoding)
	{
		//判断Excel文件中是否存在数据表
		if (mResultSet.Tables.Count < 1)
			return;

		//默认读取第一个数据表
		DataTable mSheet = mResultSet.Tables [0];

		//判断数据表内是否存在数据
		if (mSheet.Rows.Count < 1)
			return;

		//读取数据表行数和列数
		int rowCount = mSheet.Rows.Count;
		int colCount = mSheet.Columns.Count;

		//创建一个StringBuilder存储数据
		StringBuilder stringBuilder = new StringBuilder ();

		//读取数据
		for (int i = 0; i < rowCount; i++) {
			for (int j = 0; j < colCount; j++) {
				//使用","分割每一个数值
				stringBuilder.Append (mSheet.Rows [i] [j] + ",");
			}
			//使用换行符分割每一行
			stringBuilder.Append ("\r\n");
		}

		//写入文件
		using (FileStream fileStream = new FileStream(CSVPath, FileMode.Create, FileAccess.Write)) {
			using (TextWriter textWriter = new StreamWriter(fileStream, encoding)) {
				textWriter.Write (stringBuilder.ToString ());
			}
		}
	}

	/// <summary>
	/// 导出为Xml
	/// </summary>
	public void ConvertToXml (string XmlFile)
	{
		//判断Excel文件中是否存在数据表
		if (mResultSet.Tables.Count < 1)
			return;

		//默认读取第一个数据表
		DataTable mSheet = mResultSet.Tables [0];

		//判断数据表内是否存在数据
		if (mSheet.Rows.Count < 1)
			return;

		//读取数据表行数和列数
		int rowCount = mSheet.Rows.Count;
		int colCount = mSheet.Columns.Count;

		//创建一个StringBuilder存储数据
		StringBuilder stringBuilder = new StringBuilder ();
		//创建Xml文件头
		stringBuilder.Append ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		stringBuilder.Append ("\r\n");
		//创建根节点
		stringBuilder.Append ("<Table>");
		stringBuilder.Append ("\r\n");
		//读取数据
		for (int i = 1; i < rowCount; i++) {
			//创建子节点
			stringBuilder.Append ("  <Row>");
			stringBuilder.Append ("\r\n");
			for (int j = 0; j < colCount; j++) {
				stringBuilder.Append ("   <" + mSheet.Rows [0] [j].ToString () + ">");
				stringBuilder.Append (mSheet.Rows [i] [j].ToString ());
				stringBuilder.Append ("</" + mSheet.Rows [0] [j].ToString () + ">");
				stringBuilder.Append ("\r\n");
			}
			//使用换行符分割每一行
			stringBuilder.Append ("  </Row>");
			stringBuilder.Append ("\r\n");
		}
		//闭合标签
		stringBuilder.Append ("</Table>");
		//写入文件
		using (FileStream fileStream = new FileStream(XmlFile, FileMode.Create, FileAccess.Write)) {
			using (TextWriter textWriter = new StreamWriter(fileStream,Encoding.GetEncoding("utf-8"))) {
				textWriter.Write (stringBuilder.ToString ());
			}
		}
	}

	/// <summary>
	/// 设置目标实例的属性
	/// </summary>
	private void SetTargetProperty (object target, string propertyName, object propertyValue)
	{
		//获取类型
		Type mType = target.GetType ();
		//获取属性集合
		PropertyInfo[] mPropertys = mType.GetProperties ();
		foreach (PropertyInfo property in mPropertys) {
			if (property.Name == propertyName) {
				property.SetValue (target, Convert.ChangeType (propertyValue, property.PropertyType), null);
			}
		}
	}
}

