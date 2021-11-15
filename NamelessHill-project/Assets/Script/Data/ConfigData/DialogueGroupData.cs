using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nameless.ConfigData
{
    public class DialogueGroupData
    {
        public long id;
        public string dialogueIds;

        public DialogueGroupData(long id, string dialogueIds)
        {
            this.id = id;
            this.dialogueIds = dialogueIds;
        }
    }
}