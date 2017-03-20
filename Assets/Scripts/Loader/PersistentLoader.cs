using UnityEngine;
using System.Collections;
using System.IO;
using HeroView;

namespace Hero
{
    public class PersistentLoader : MonoBehaviour
    {
        private static System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding(false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="data">数据</param>
        public static void WriteAllBytesSync(string fileName,byte[] data)
        {
            string path = ViewDefine.DataPathSync + fileName;
            CheckDir(path);
            try
            {
                File.WriteAllBytes(path, data);
            }
            catch (System.Exception e)
            {

                Debug.LogError("PersistentLoader::WriteAllBytes: " + e.ToString());
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static byte[] ReadAllBytesSync(string fileName)
        {
            string path = ViewDefine.DataPathSync + fileName;
            if(!File.Exists(path))
            {
                Debug.Log("文件不存在，path:" + path);
                return null;
            }
            byte[] data = null;
            try
            {
                data =File.ReadAllBytes(path);
            }
            catch (System.Exception e)
            {

                Debug.LogError("PersistentLoader::ReadAllBytes: " + e.ToString());
            }
            return data;
        }

        public static void WriteAllTextSync(string fileName, string data)
        {
            string path = ViewDefine.DataPathSync + fileName;
            CheckDir(path);
            try
            {
                File.WriteAllText(path, data, encoding);
            }
            catch (System.Exception e)
            {

                Debug.LogError("PersistentLoader::WriteAllText: " + e.ToString());
            }
            
        }
        public static string ReadAllTextSync(string fileName)
        {
            string path = ViewDefine.DataPathSync + fileName;
            if (!File.Exists(path))
            {
                Debug.Log("文件不存在，path:" + path);
                return null;
            }
            string text=null;
            try
            {
                text= File.ReadAllText(path,encoding);
            }
            catch (System.Exception e)
            {

                Debug.LogError("PersistentLoader::ReadAllText: " + e.ToString());
            }
            return text;
        }

        public static void WriteAllBytesAsync(string fileName, byte[] data)
        {
            string path = ViewDefine.DataPathSync + fileName;
            CheckDir(path);
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                
            }
            catch (System.Exception e)
            {
                Debug.Log("文件占用或者文件不存在,path:"+path);
                return;
            }
            fs.BeginWrite(data, 0, data.Length, WriteAllBytesAsyncCB, fs);
        }

        public static void WriteAllBytesAsyncCB(System.IAsyncResult ar)
        {
            FileStream fs = (FileStream)ar.AsyncState;
            fs.EndWrite(ar);
            fs.Flush();
            fs.Close();
        }

        public static void WriteAllTextAsync(string fileName, string data)
        {
            string path = ViewDefine.DataPathSync + fileName;
            CheckDir(path);
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);

            }
            catch (System.Exception e)
            {
                Debug.Log("文件占用或者文件不存在,path:" + path);
                return;
            }
            byte[] target = encoding.GetBytes(data);

            fs.BeginWrite(target, 0, target.Length, WriteAllTextAsyncCB, fs);
        }
        public static void WriteAllTextAsyncCB(System.IAsyncResult ar)
        {
            FileStream fs = (FileStream)ar.AsyncState;
            fs.EndWrite(ar);
            fs.Flush();
            fs.Close();
        }
        private static void CheckDir(string filePath)
        {
            string dirPath = filePath.Substring(0, filePath.LastIndexOf("/"));
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }




    }
}
