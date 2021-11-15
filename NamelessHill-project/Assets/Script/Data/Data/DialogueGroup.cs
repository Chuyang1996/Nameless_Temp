using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public class DialogueGroup
    {
        public long id;
        public List<Dialogue> dialogues;

        public DialogueGroup(long id, List<Dialogue> dialogues)
        {
            this.id = id;
            this.dialogues = dialogues;
        }
    }

    public enum ConditionDialogue
    {
        GameStart = 1,
        TimeFlowing = 2,
    }
    public class Dialogue
    {
        public long id;
        public string dialogueTxt;
        public Dictionary<ConditionDialogue, float> conditionDic = new Dictionary<ConditionDialogue, float>();
        public bool isAuto;
        public float waitTime;
        public long nextId;

        public Dialogue(long id, string dialogueTxt, Dictionary<ConditionDialogue,float> conditionDic,bool isAuto,float waitTime,long nextId)
        {
            this.id = id;
            this.dialogueTxt = dialogueTxt;
            this.conditionDic = conditionDic;
            this.isAuto = isAuto;
            this.waitTime = waitTime;
            this.nextId = nextId;
        }


    }
}