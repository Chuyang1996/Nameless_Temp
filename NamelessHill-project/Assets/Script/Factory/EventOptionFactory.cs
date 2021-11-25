using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Agent
{
    public static class EventOptionFactory 
    {
        // Start is called before the first frame update
        public static EventOption GetEventOptionById(long id)
        {
            return Get(DataManager.Instance.GetEventOptionData(id));
        }

        public static EventOption Get(EventOptionData eventOptionData)
        {
            List<EventEffect> effects = new List<EventEffect>();
            string[] effectStr = StringToStringArray(eventOptionData.effects);
            for(int i = 0; i < effectStr.Length; i++)
            {
                int[] intArray = StringToIntArray(effectStr[i]);
                if(intArray[0] == (int)EventEffectType.MoraleChange)
                {
                    effects.Add(new MoraleEventEffect((long)intArray[1], intArray[2]));
                }
                else if(intArray[0] == (int)EventEffectType.AllMoraleChange)
                {
                    effects.Add(new AllMoraleEventEffect(intArray[1]));
                }
                else if(intArray[0] == (int)EventEffectType.MilitaryResourceChange)
                {
                    effects.Add(new AmmoEventEffect((float)intArray[1]));
                }
                else if(intArray[0] == (int)EventEffectType.NextEvent)
                {
                    effects.Add(new NextEventEffect((long)intArray[1]));
                }
            }

            return new EventOption(eventOptionData.id, eventOptionData.name, eventOptionData.descrption, effects);
        }

        private static string[] StringToStringArray(string stringlist)
        {
            string[] array;
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(",") ? stringlist.Split(',') : new string[1] { stringlist };
            }
            else
            {
                array = new string[1];
                array[0] = "";
            }
            return array;
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