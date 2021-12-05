using Nameless.Agent;
using Nameless.ConfigData;
using Nameless.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Manager {
    public class NoteManager : Singleton<NoteManager>
    {
        public string loadPath = "Prefabs/UI/Item/";
        // Start is called before the first frame update
        public Dictionary<long, NotePage> notePageDic = new Dictionary<long, NotePage>();

        public void InitNote()
        {
            this.notePageDic = new Dictionary<long, NotePage>();
        }
        public void InitNoteBook()
        {
            GameManager.Instance.noteBookView.InitNoteBook();
        }

        public bool AddNewNote(long id)
        {
            if (!this.notePageDic.ContainsKey(id))
            {
                this.notePageDic.Add(id, NotePageFactory.GetNotePageById(id));
                return true;
            }
            return false;
        }

        public void AddOldNote(long id, List<NoteInfo> noteInfos)
        {
            if (this.notePageDic.ContainsKey(id))
            {
                for (int i = 0; i < noteInfos.Count; i++) {
                    this.notePageDic[id].noteInfos.Add(noteInfos[i]);
                }
            }
        }
    }
}