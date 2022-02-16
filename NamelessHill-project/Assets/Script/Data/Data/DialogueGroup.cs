using Nameless.Agent;
using Nameless.DataMono;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public enum ConditionDialogueType
    {
        None = 0,
        GameStart = 1,
        TimeFlowing = 2,
    }
    public struct ConditionDialogue
    {
        public ConditionDialogueType conditionType;
        public float time;

        public ConditionDialogue(ConditionDialogueType type,float time)
        {
            this.conditionType = type;
            this.time = time;
        }
    }
    public class Dialogue
    {
        public long id;
        public string dialogueTxt;
        public ConditionDialogue conditionDialogue;
        public bool isAuto;
        public float waitTime;
        public float speedPos;
        public float zoom;
        public float zoomSpeed;
        public long nextId;
        public long nextPawnId;

        public Dialogue(long id, string dialogueTxt, ConditionDialogue conditionDialogue, bool isAuto, float waitTime, float speedPos, float zoom, float zoomSpeed, long nextId, long nextPawn)
        {
            this.id = id;
            this.dialogueTxt = dialogueTxt;
            this.conditionDialogue = conditionDialogue;
            this.isAuto = isAuto;
            this.waitTime = waitTime;
            this.speedPos = speedPos;
            this.zoom = zoom;
            this.zoomSpeed = zoomSpeed;
            this.nextId = nextId;
            this.nextPawnId = nextPawn;
        }

        public Dialogue NextDialogue()
        {
            return this.nextId != -1 ? DialogueFactory.GetDialogueById(this.nextId) : null;
        }

        public PawnAvatar FindTargetDialoguePawn()//���޸� ����һ���Ƿ�Ҫɾ��������ܻ��߻��ɱ��
        {
            if (this.nextPawnId != -1)
            {
                var pawn = FrontManager.Instance.GetPawnAvatars(FrontManager.Instance.localPlayer).Where(_pawn => _pawn.pawnAgent.pawn.id == this.nextPawnId).FirstOrDefault();
                if (pawn != null) return pawn;
                pawn = FrontManager.Instance.GetPawnAvatars(FrontManager.Instance.localPlayer).Where(_pawn => _pawn.pawnAgent.pawn.id == this.nextPawnId).FirstOrDefault();
                if (pawn != null) return pawn;
                else return null;

            }
            else
                return null; 
        }
    }
}