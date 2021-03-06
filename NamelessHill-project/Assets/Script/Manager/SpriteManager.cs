using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Nameless.Manager
{
    public enum AtlasType
    {
        EventImage = 0,
        CharacterImage = 1,
        CharacterCampImage = 2,
        CharacterCampMarkImage = 3,
        NoteImage = 4,
        IconImage = 5,
    }
    public class SpriteManager : Singleton<SpriteManager>
    {
        private const string loadPath = "TexturePackage/";
        private Dictionary<string, AtlasType> atlasIndex = new Dictionary<string, AtlasType>
        {
            { "EventImage", AtlasType.EventImage},
            { "CharacterImage", AtlasType.CharacterImage},
            { "CampCharacterImage", AtlasType.CharacterCampImage},
            { "CampCharacterMarkImage", AtlasType.CharacterCampMarkImage},
            { "NoteImage", AtlasType.NoteImage},
            { "IconImage", AtlasType.IconImage},
         };
        public Dictionary<AtlasType, SpriteAtlas> atlasCollection = new Dictionary<AtlasType, SpriteAtlas>();
        // Start is called before the first frame update

        public void InitTexturePackage()
        {
            AssetBundle atlasData = AssetLoad.atlasAsset;
            this.atlasCollection = new Dictionary<AtlasType, SpriteAtlas>();
            foreach (var child in this.atlasIndex)
            {

                SpriteAtlas temp = atlasData.LoadAsset(child.Key) as SpriteAtlas;
                this.atlasCollection.Add(child.Value, temp);

            }
        }


        public Sprite FindSpriteByName(AtlasType _atlasType, string _spriteName)
        {
            if (this.atlasCollection.ContainsKey(_atlasType))
            {
                if (this.atlasCollection[_atlasType].GetSprite(_spriteName))
                {

                    return this.atlasCollection[_atlasType].GetSprite(_spriteName);
                }
                Debug.LogError("尚未找到该图片" + _spriteName + "资源名称 请检查配表是否正确，是否对字典进行过初始化");
                return null;
            }
            Debug.LogError("尚未找到该图片"+ _spriteName + "资源的图集枚举 请检查前面字典是否配置了该枚举对应的图集");
            return null;
        }

        public bool IsSpriteExit(AtlasType _atlasType, string _spriteName)
        {
            if (this.atlasCollection.ContainsKey(_atlasType))
            {
                if (this.atlasCollection[_atlasType].GetSprite(_spriteName))
                {

                    return true;
                }
                return false;
            }
            return false;
        }
    }
}