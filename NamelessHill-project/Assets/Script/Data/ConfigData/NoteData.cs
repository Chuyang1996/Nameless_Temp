using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class NoteData
    {
        public long id;
        public string name;
        public string descrption;
        public string noteImage;

        public NoteData(long id, string name, string descrption, string noteImage)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.noteImage = noteImage;
        }

    }

    public class NoteInfo
    {
        public long id;
        public Sprite noteImage;

        public NoteInfo(NoteData noteData)
        {
            this.id = noteData.id;
            this.noteImage = SpriteManager.Instance.FindSpriteByName( AtlasType.NoteImage ,noteData.noteImage);
        }
    }
}

