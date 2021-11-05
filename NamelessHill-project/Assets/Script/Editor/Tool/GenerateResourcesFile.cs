using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class GenerateResourcesFile
{
    [MenuItem("策划工具/生成资源名文件")]
    public static void GenerateResourcesName()
    {
        string currentPath = Assembly.GetExecutingAssembly().Location;

        string savePath = currentPath + "/../../../.." + "/WarChessProject/Assets/StreamingAssets";

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

    }

}
