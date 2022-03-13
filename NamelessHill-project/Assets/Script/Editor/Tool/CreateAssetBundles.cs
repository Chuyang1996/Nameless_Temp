using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("策划工具/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/AssetBundle";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        Debug.Log("打包成功");
    }
}