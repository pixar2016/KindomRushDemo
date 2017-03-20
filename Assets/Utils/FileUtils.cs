using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class FileUtils : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //1.保存文件
    public static void SaveFile(string path, string name, string info)
    {
        StreamWriter sw;
        FileInfo fi = new FileInfo(path + "//" + name);
        if (!fi.Exists)
        {
            //1.文件不存在则创建
            sw = fi.CreateText();
        }
        else
        {
            //1.文件存在着打开该文件
            //sw = fi.AppendText();
            fi.Delete();
            sw = fi.CreateText();
        }
        //2.以行的形式写入
        sw.WriteLine(info);

        sw.Close();
        sw.Dispose();
    }
    //2.读取文件
    public static string LoadFile(string path, string name)
    {
        string infos = "";
        FileInfo fi = new FileInfo(path + "//" + name);
        if (fi.Exists)
        {
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(path + "//" + name);
            }
            catch (Exception e)
            {
                return null;
            }
            string line;
            ArrayList arrayList1 = new ArrayList();
            while ((line = sr.ReadLine()) != null)
            {
                //一行一行读取，当还有数据的时候
                arrayList1.Add(line);
            }

            sr.Close();
            sr.Dispose();

            foreach (string item in arrayList1)
            {
                infos += item;
            }
            return infos;

        }
        else
        {
            return null;
        }
    }




}


/*
1.Json处理框架：LitJson操作.txt

读取Json字符串:
        string infos = FileUtils.LoadFile(Application.dataPath, "AnimatonSwitchFile");
        if (infos != null)
        {
            JsonData AnimJson = JsonMapper.ToObject(infos);
            bool AnimSwitch = (bool)AnimJson["AnimSwitch"];
        }

保存Json字符串:
        JsonData data = new JsonData();
        data["AnimSwitch"] = true;
        string dataStr = data.ToJson();
        FileUtils.SaveFile(Application.dataPath, "AnimatonSwitchFile", dataStr);

*/
