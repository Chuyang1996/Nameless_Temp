using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Agent
{
    public class EventResultFactory
    {
        public static EventResult GetEventResultById(long id)
        {
            return Get(DataManager.Instance.GetEventResultData(id));
        }
        public static EventResult Get(EventResultData eventResultData)
        {
            List<EventOption> options = new List<EventOption>(); 
            List<long> resultId = StringToLongArray(eventResultData.options);
            for(int i = 0; i < resultId.Count; i++)
            {
                options.Add(EventOptionFactory.GetEventOptionById(resultId[i]));
            }
            Sprite sprite = SpriteManager.Instance.FindSpriteByName(AtlasType.EventImage, eventResultData.Image);
            return new EventResult(eventResultData.id, eventResultData.name, eventResultData.descrption, eventResultData.conditionId, options, sprite);
        }


        private static List<long> StringToLongArray(string stringlist)
        {
            List<long> result = new List<long>();
            long[] array;
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(",") ? Array.ConvertAll<string, long>(stringlist.Split(new char[] { ',' }), s => long.Parse(s)) : new long[1] { long.Parse(stringlist) };
                for (int i = 0; i < array.Length; i++)
                {
                    result.Add(array[i]);
                }
            }
            return result;
        }
    }
}