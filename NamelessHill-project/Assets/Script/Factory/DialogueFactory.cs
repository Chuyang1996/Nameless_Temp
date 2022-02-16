using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using System;

namespace Nameless.Agent
{
    public static class DialogueFactory
    {
        public static Dialogue GetDialogueById(long id)
        {
            return Get(DataManager.Instance.GetDialogueData(id));
        }

        public static Dialogue Get(DialogueData dialogueData)
        {
            int[] condition = StringToIntArray(dialogueData.condition);
            ConditionDialogue conditionDialogue = new ConditionDialogue((ConditionDialogueType)condition[0],(float)condition[1]);
            return new Dialogue(dialogueData.id, dialogueData.dialogue, conditionDialogue, dialogueData.isAuto,  dialogueData.waitTime, dialogueData.speedPos, dialogueData.zoom, dialogueData.zoomSpeed, dialogueData.nextId,dialogueData.nextPawn);
        }

        private static int[] StringToIntArray(string stringlist)
        {
            int[] array;
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(":") ? Array.ConvertAll<string, int>(stringlist.Split(new char[] { ':' }), s => int.Parse(s)) : new int[1] { int.Parse(stringlist) };
            }
            else
            {
                array = new int[1];
                array[0] = 0;
            }
            return array;
        }
    }

   
}