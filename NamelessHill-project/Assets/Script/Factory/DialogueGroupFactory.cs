using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Nameless.Agent
{
    public static class DialogueGroupFactory 
    {

        public static DialogueGroup GetDialogueGroupById(long id)
        {
            return Get(DataManager.Instance.GetDialogueGroupData(id));
        }

        public static DialogueGroup Get(DialogueGroupData dialogueGroupData)
        {
            List<Dialogue> dialogues = new List<Dialogue>();
            long[] ids = StringToLongArray(dialogueGroupData.dialogueIds);
            for(int i = 0; i < ids.Length; i++)
            {
                dialogues.Add(DialogueFactory.GetDialogueById(ids[i]));
            }
            return new DialogueGroup(dialogueGroupData.id, dialogues);
        }

        private static long[] StringToLongArray(string stringlist)
        {
            long[] array;
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(",") ? Array.ConvertAll<string, long>(stringlist.Split(new char[] { ',' }), s => long.Parse(s)) : new long[1] { long.Parse(stringlist) };
            }
            else
            {
                array = new long[1];
                array[0] = 0;
            }
            return array;
        }
    }
}