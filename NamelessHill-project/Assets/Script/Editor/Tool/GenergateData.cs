using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Excel;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using System;

namespace Nameless.Editor
{
    public class GenergateData
    {
        [MenuItem("策划工具/导出表格数据")]
        public static void LoadExcel()
        {
            //D:\Workspace\University\ucsc\GAME 271\project - Nameless Hill\NamelessHill\NamelessHill\NamelessHill - project\Library\ScriptAssemblies\Assembly - CSharp - Editor.dll
            string currentPath = Assembly.GetExecutingAssembly().Location;
            Debug.Log(currentPath);
            string excelPath = currentPath + "/../../../..";
            string savePath = currentPath + "/../../../.." + "/NamelessHill-project/Assets/StreamingAssets";

            int excelLength = (excelPath + "/GameDesign/ExcelData").Length;
            int serialLength = savePath.Length;

            string[] strFiles = Directory.GetFiles(excelPath + "/GameDesign/ExcelData", "*.xlsx");
            string[] serialFiles = Directory.GetFiles(savePath, "*.txt");
            string tempFile = "";
            for (int n = 0; n < serialFiles.Length; n++)
            {
                tempFile = serialFiles[n].ToString().Remove(0, serialLength).Remove(0, 1).Replace(".txt", "");
                FileInfo file = new FileInfo(savePath + "/" + tempFile + ".txt");
                file.Delete();
            }
            for (int m = 0; m < strFiles.Length; m++)
            {
                try
                {


                    tempFile = strFiles[m].ToString().Remove(0, excelLength).Remove(0, 1).Replace(".xlsx", "");
                    FileStream fileStream = File.Open(excelPath + "/GameDesign/ExcelData/" + tempFile + ".xlsx", FileMode.Open, FileAccess.Read);
                    IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                    // 表格数据全部读取到result里(引入：DataSet（using System.Data;） 需引入 System.Data.dll到项目中去)
                    DataSet result = excelDataReader.AsDataSet();

                    // 获取表格有多少列 
                    int columns = result.Tables[0].Columns.Count;
                    // 获取表格有多少行 
                    int rows = result.Tables[0].Rows.Count;
                    // 根据行列依次打印表格中的每个数据 

                    string value;
                    string all;
                    string newfilepathExcel = savePath + "/" + tempFile + ".txt";
                    FileStream newfileExcel = new FileStream(newfilepathExcel, FileMode.Create, FileAccess.ReadWrite);
                    StreamWriter swExcel = new StreamWriter(newfileExcel);
                    Dictionary<long, Dictionary<string, string>> dataList = new Dictionary<long, Dictionary<string, string>>();
                    string[] title = new string[columns];
                    for (int n = 0; n < title.Length; n++)
                    {
                        title[n] = result.Tables[0].Rows[1][n].ToString();
                    }

                    for (int i = 2; i < rows; i++)
                    {
                        value = null;
                        all = result.Tables[0].Rows[i][0].ToString();
                        long id = long.Parse(all);
                        Dictionary<string, string> data = new Dictionary<string, string>();
                        for (int j = 1; j < columns; j++)
                        {
                            // 获取表格中指定行指定列的数据 
                            value = result.Tables[0].Rows[i][j].ToString();
                            if (value == "")
                            {
                                continue;
                            }

                            data.Add(title[j], value);
                        }
                        dataList.Add(id, data);
                    }
                    string serialDataExcel = JsonConvert.SerializeObject(dataList);
                    swExcel.WriteLine(serialDataExcel);
                    swExcel.Flush();
                    swExcel.Close();
                    newfileExcel.Close();
                    fileStream.Close();
                    Debug.Log(tempFile + ":导出完成");
                }
                catch (Exception e)
                {
                    Debug.LogError(tempFile + ": 导出失败: " + e.Message);
                }
            }
            Debug.Log("导出成功!!!!!!!!!!!!!");

            currentPath = Assembly.GetExecutingAssembly().Location;

            savePath = currentPath + "/../../../.." + "/NamelessHill-project/Assets/StreamingAssets";

            string newfilepath = savePath + "/" + "AudiosResources" + ".txt";
            FileStream newfile = new FileStream(newfilepath, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(newfile);

            int pathLength = (Application.dataPath + "/Resources/Audio").Length;
            string[] audioList = System.IO.Directory.GetFiles(Application.dataPath + "/Resources/Audio", "*.wav");
            for (int i = 0; i < audioList.Length; i++)
            {
                audioList[i] = audioList[i].ToString().Remove(0, pathLength).Remove(0, 1).Replace(".wav", "");
                Debug.Log(audioList[i]);
            }
            string serialData = JsonConvert.SerializeObject(audioList);
            sw.WriteLine(serialData);
            sw.Flush();
            sw.Close();
            newfile.Close();
            Debug.Log("音效资源导出成功!!!!!!!!!!!!!");
        }
    }
}