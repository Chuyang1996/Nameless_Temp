using Nameless.Data;
using Nameless.DataMono;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Manager
{
    public class DialogueTriggerManager : Singleton<DialogueTriggerManager>
    {
        // Start is called before the first frame update
        public Action<int> TimeTriggerEvent;
        public Stack<DialoguePawn> dialoguePawns = new Stack<DialoguePawn>();
        public bool isShowDialogue = false;


        public void CheckGameStartEvent(PawnAvatar pawn)
        {
            if (pawn.pawnAgent.dialogueGroup != null)
            {
                List<Dialogue> dialogues = pawn.pawnAgent.dialogueGroup.dialogues;
                for (int i = dialogues.Count-1; i >= 0; i--)
                {
                    if (dialogues[i].conditionDialogue.conditionType == ConditionType.GameStart)
                    {
                        this.dialoguePawns.Push(new DialoguePawn( pawn, dialogues[i]));
                    }
                }
            }
        }

        public void CheckTimeTriggerEvent(int cost)
        {
            if (this.TimeTriggerEvent != null)
            {
                this.TimeTriggerEvent(cost);
            }
        }

        public void CheckTimeflowEvent(PawnAvatar pawn, int time)
        {
            if (pawn.pawnAgent.dialogueGroup != null)
            {
                List<Dialogue> dialogues = pawn.pawnAgent.dialogueGroup.dialogues;
                for (int i = 0; i < dialogues.Count; i++)
                {
                    if (dialogues[i].conditionDialogue.conditionType == ConditionType.TimeFlowing && dialogues[i].conditionDialogue.time == time)
                    {
                        this.dialoguePawns.Push(new DialoguePawn(pawn, dialogues[i]));
                    }
                }
            }
        }

        public void PushNewDialoguePawn(DialoguePawn dialoguePawn)
        {
            this.dialoguePawns.Push(dialoguePawn);
        }

        public IEnumerator StartListenEvent()
        {
            while (true)
            {
                //Debug.Log("ÎÒ»¹ÔÚ¼àÌý");
                if (this.dialoguePawns.Count > 0 && !this.isShowDialogue)
                {
                    this.isShowDialogue = true;
                    GameManager.Instance.PlayCamera(this.dialoguePawns);

                }
                yield return null;
            }
        }
    }
}