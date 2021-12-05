using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Agent
{
    public static class ConversationOptionFactory
    {
        public static ConversationOption GetConversationOptionById(long id)
        {
            return Get(DataManager.Instance.GetConversationOptionData(id));
        }

        public static ConversationOption Get(ConversationOptionData conversationOptionData)
        {
            long[] effects = StringToLongArray(conversationOptionData.effects);
            List<ConversationEffect> conversationEffects = new List<ConversationEffect>();
            for (int i = 0; i < effects.Length; i++)
            {
                conversationEffects.Add(ConversationEffectFactory.GetConversationEffectById(effects[i]));
            }
            return new ConversationOption(conversationOptionData.id, conversationOptionData.name, conversationOptionData.descrption, conversationEffects);
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