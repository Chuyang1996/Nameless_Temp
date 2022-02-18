using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Agent 
{
    public static class PawnFactory
    {
        public static Pawn GetPawnById(long id)
        {
            return Get(DataManager.Instance.GetPawn(id));
        }

        public static Pawn Get(PawnData pawnData)
        {
            float crAmmo     = pawnData.crAmmo     > 1 ? 1.0f : pawnData.crAmmo;
            float crAtkSpeed = pawnData.crAtkSpeed > 1 ? 1.0f : pawnData.atkSpeed;
            float crAttack   = pawnData.crAttack   > 1 ? 1.0f : pawnData.crAttack;
            float crDefend   = pawnData.crDefend   > 1 ? 1.0f : pawnData.crDefend;
            float crDex      = pawnData.crDex      > 1 ? 1.0f : pawnData.crDex;
            float crHealth   = pawnData.crHealth   > 1 ? 1.0f : pawnData.crHealth;
            float crHit      = pawnData.crHit      > 1 ? 1.0f : pawnData.crHit;
            float crMorale   = pawnData.crMorale   > 1 ? 1.0f : pawnData.crMorale;
            float crSpeed    = pawnData.crSpeed    > 1 ? 1.0f : pawnData.crSpeed;

            Dictionary<long, DialogueGroup> dialogueGroupDic = new Dictionary<long, DialogueGroup>();
            List<string> dialogueString = StringToStringArray(pawnData.dialogue);
            for(int i = 0; i < dialogueString.Count; i++)
            {
                long[] temp = StringToLong2Array(dialogueString[i]);
                dialogueGroupDic.Add(temp[0], DialogueGroupFactory.GetDialogueGroupById(temp[1]));
            }
            List<string> conversationInfo = StringToStringArray(pawnData.converIds);
            Dictionary<long, Conversation> conversationMapDic = new Dictionary<long, Conversation>();
            for(int i = 0; i < conversationInfo.Count; i++)
            {
                long[] temp = StringToLong2Array(conversationInfo[i]);
                conversationMapDic.Add(temp[0], ConversationFactory.GetConversationById(temp[1]));
            }
            return new Pawn(
                pawnData.Id,
                pawnData.name,
                pawnData.descrption,
                pawnData.health, 
                crHealth, 
                pawnData.attack, 
                crAttack, 
                pawnData.morale, 
                crMorale,
                pawnData.atkSpeed,
                crAtkSpeed,
                pawnData.ammo, 
                crAmmo, 
                pawnData.speed, 
                crSpeed, 
                pawnData.hit, 
                crHit, 
                pawnData.dex, 
                crDex,
                pawnData.defend, 
                crDefend,
                pawnData.leftResNum,
                pawnData.moveAvaliable,
                StringToLongArray(pawnData.fightSkills),
                StringToLongArray(pawnData.supportSkills),
                StringToLongArray(pawnData.buildSkills),
                dialogueGroupDic,
                pawnData.animPrefab,
                pawnData.selectIcon,
                pawnData.campIcon,
                pawnData.campPosIndex,
                pawnData.btnLRpos,
                conversationMapDic
                );
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
                for(int i = 0; i < array.Length; i++)
                {
                    result.Add(array[i]);
                }
            }
            return result;
        }
        private static long[] StringToLong2Array(string stringlist)
        {
            long[] array;
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(":") ? Array.ConvertAll<string, long>(stringlist.Split(new char[] { ':' }), s => long.Parse(s)) : new long[1] { long.Parse(stringlist) };
            }
            else
            {
                array = new long[1];
                array[0] = 0;
            }
            return array;
        }
        private static List<string> StringToStringArray(string stringlist)
        {
            List<string> result = new List<string>();
            string[] array;
            if (stringlist.Contains("]") && stringlist.Contains("["))
            {
                stringlist = stringlist.Remove(0, 1);
                stringlist = stringlist.Remove(stringlist.Length - 1, 1);
                array = stringlist.Contains(",") ? stringlist.Split(new char[] { ',' }) : new string[1] { stringlist };
                for (int i = 0; i < array.Length; i++)
                {
                    result.Add(array[i]);
                }
            }
            return result;
        }


    }
}