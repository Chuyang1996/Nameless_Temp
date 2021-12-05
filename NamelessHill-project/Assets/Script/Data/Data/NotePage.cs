using Nameless.ConfigData;
using Nameless.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public class NotePage
    {
        public long id;
        public string name;
        public string descrption;
        public string noteUIname;
        public List<NoteInfo> noteInfos;

        public NotePage(long id, string name,string descrption, string noteUIname, List<NoteInfo> noteInfos)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.noteUIname = noteUIname;
            this.noteInfos = noteInfos;
        }
    }
}