using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData {
    public class DialogueData
    {
        public long id;
        public string dialogue;
        public string condition;
        public bool isAuto;
        public float waitTime;
        public long nextId;


        public DialogueData(long id, string dialogue,string condition,bool isAuto,float waitTime,long nextId)
        {
            this.id = id;
            this.dialogue = dialogue;
            this.condition = condition;
            this.isAuto = isAuto;
            this.waitTime = waitTime;
            this.nextId = nextId;
        }
    }
}
