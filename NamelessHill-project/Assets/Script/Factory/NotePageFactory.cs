using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using Nameless.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Agent
{
    public static class NotePageFactory
    {
        public static string loadPath = "Prefabs/UI/Item/";
        // Start is called before the first frame update
        public static NotePage GetNotePageById(long id)
        {
            return Get(DataManager.Instance.GetNotePageData(id));
        }

        public static NotePage Get(NotePageData notePage)
        {
            List<NoteInfo> noteInfos = new List<NoteInfo>();
            List<long> noteIds = StringToLongArray(notePage.noteIds);
            for(int i = 0; i < noteIds.Count; i++)
            {
                noteInfos.Add(new NoteInfo(DataManager.Instance.GetNoteData(noteIds[i])));
            }
            GameObject ui = Resources.Load(loadPath + notePage.noteTemplate) as GameObject;
            return new NotePage(notePage.id, notePage.name, notePage.descrption, notePage.noteTemplate, noteInfos);
        }

        private static List<long> StringToLongArray(string stringlist)
        {
            long[] array;
            List<long> list = new List<long>();
            if (stringlist.Contains("]") && stringlist.Contains("[") && stringlist != "[]")
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(",") ? Array.ConvertAll<string, long>(stringlist.Split(new char[] { ',' }), s => long.Parse(s)) : new long[1] { long.Parse(stringlist) };
                for(int i = 0; i < array.Length; i++)
                {
                    list.Add(array[i]);
                }
            }
            return list;
        }
    }
}