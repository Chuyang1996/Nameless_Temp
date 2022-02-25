using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Agent
{
    public static class ConversationEffectFactory
    {
        public static ConversationEffect GetConversationEffectById(long id)
        {
            return Get(DataManager.Instance.GetConversationEffectData(id));
        }

        public static ConversationEffect Get(ConversationEffectData conversationEffectData)
        {
            if (conversationEffectData.type == 100)
            {
                long[] tempLong = StringToLongArray(conversationEffectData.parameter);
                return new ConversationMoraleChange(conversationEffectData.id, conversationEffectData.name, conversationEffectData.descrption,  tempLong[0],(int)tempLong[1]);
            } 
            else if (conversationEffectData.type == 101)
            {
                long[] tempLong = StringToLongArray(conversationEffectData.parameter);
                return new ConversationResourceChange(conversationEffectData.id, conversationEffectData.name, conversationEffectData.descrption, (int)tempLong[0]);
            }
            else if (conversationEffectData.type == 102)
            {
                long[] tempLong = StringToLongArray(conversationEffectData.parameter);
                List<NoteInfo> noteInfos = new List<NoteInfo>();
                for(int i = 1; i < tempLong.Length; i++)
                {
                    noteInfos.Add(new NoteInfo( DataManager.Instance.GetNoteData(tempLong[i])));
                }
                return new ConversationUnlockNote(conversationEffectData.id, conversationEffectData.name, conversationEffectData.descrption, tempLong[0],noteInfos);
            }
            else if (conversationEffectData.type == 103)
            {
                long[] tempLong = StringToLongArray(conversationEffectData.parameter);
                if(tempLong[0] != -1)
                    return new ConversationNext(conversationEffectData.id, conversationEffectData.name, conversationEffectData.descrption, ConversationFactory.GetConversationById(tempLong[0]));
                else
                    return new ConversationNext(conversationEffectData.id, conversationEffectData.name, conversationEffectData.descrption, null);
            }
            else
            {
                long[] tempLong = StringToLongArray(conversationEffectData.parameter);
                return new ConversationGoToEnd(conversationEffectData.id, conversationEffectData.name, conversationEffectData.descrption, (int)tempLong[0]);
            }
            // Start is called before the first frame update
        }
        private static long[] StringToLongArray(string stringlist)
        {
            long[] array;
            if (stringlist.Contains("]") && stringlist.Contains("[") && stringlist != "[]")
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(":") ? Array.ConvertAll<string, long>(stringlist.Split(new char[] { ':' }), s => long.Parse(s)) : new long[1] { long.Parse(stringlist) };
            }
            else
            {
                array = new long[1];
                array[0] = -1;
            }
            return array;
        }
    }
}