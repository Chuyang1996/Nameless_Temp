using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Agent
{
    public static class ConversationFactory
    {
        public static Conversation GetConversationById(long id)
        {
            return Get(DataManager.Instance.GetConversationData(id));
        }

        public static Conversation Get(ConversationData conversationData)
        {
            long[] pawnsId = StringToLongArray(conversationData.conversationPawns);
            long[] optionsId = StringToLongArray(conversationData.options);
            List<ConversationOption> conversationOptions = new List<ConversationOption>();
            if (optionsId[0] != -1)
            {
                for (int i = 0; i < optionsId.Length; i++)
                {
                    conversationOptions.Add(ConversationOptionFactory.GetConversationOptionById(optionsId[i]));
                }
            }
            return new Conversation(conversationData.id, conversationData.name, conversationData.descrption, pawnsId, conversationOptions, conversationData.side);
            // Start is called before the first frame update
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
                array[0] = -1;
            }
            return array;
        }
    }
}