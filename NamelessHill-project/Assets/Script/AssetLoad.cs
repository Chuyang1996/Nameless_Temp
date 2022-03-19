using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetLoad
{
    public static AssetBundle gameDataAsset;
    public static AssetBundle atlasAsset;
    public static AssetBundle mapAsset;
    public static AssetBundle transInfoShowAsset;
    public static AssetBundle campAsset;
    public static AssetBundle characterAsset;
    public static AssetBundle buildAsset;
    public static AssetBundle notesAsset;
    public static AssetBundle audioAsset;

    public static void InitAssetLoad()
    {
        if (gameDataAsset == null)
            gameDataAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/gamedata.nameless");
        if (atlasAsset == null)
            atlasAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/atlas.nameless");
        if (mapAsset == null)
            mapAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/map.nameless");
        if (transInfoShowAsset == null)
            transInfoShowAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/transInfoShow.nameless");
        if (campAsset == null)
            campAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/camp.nameless");
        if (characterAsset == null)
            characterAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/character.nameless");
        if (buildAsset == null)
            buildAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/build.nameless");
        if (notesAsset == null)
            notesAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/note.nameless");
        if (audioAsset == null)
            audioAsset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundle/audio.nameless");
    }
}
