using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nameless.ConfigData
{
    public class NotePageData
    {
        // Start is called before the first frame update
        public long id;
        public string name;
        public string descrption;
        public string noteTemplate;
        public string noteIds;

        public NotePageData(long id, string name,string descrption,string noteTemplate,string noteIds)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.noteTemplate = noteTemplate;
            this.noteIds = noteIds;
        }
    }
}