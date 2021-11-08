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
            return Get(DataManager.Instance.GetCharacter(id));
        }

        public static Pawn Get(CharacterData characterData)
        {
            float crAmmo = characterData.crAmmo > 1 ? 1.0f : characterData.crAmmo;
            float crAttack = characterData.crAttack > 1 ? 1.0f : characterData.crAttack;
            float crDefend = characterData.crDefend > 1 ? 1.0f : characterData.crDefend;
            float crDex = characterData.crDex > 1 ? 1.0f : characterData.crDex;
            float crHealth = characterData.crHealth > 1 ? 1.0f : characterData.crHealth;
            float crHit = characterData.crHit > 1 ? 1.0f : characterData.crHit;
            float crMorale = characterData.crMorale > 1 ? 1.0f : characterData.crMorale;
            float crSpeed = characterData.crSpeed > 1 ? 1.0f : characterData.crSpeed;

            return new Pawn(
                characterData.Id,
                characterData.name,
                characterData.health, 
                crHealth, 
                characterData.attack, 
                crAttack, 
                characterData.morale, 
                crMorale, 
                characterData.ammo, 
                crAmmo, 
                characterData.speed, 
                crSpeed, 
                characterData.hit, 
                crHit, 
                characterData.dex, 
                crDex, 
                characterData.defend, 
                crDefend,
                StringToLongArray(characterData.fightSkills),
                StringToLongArray(characterData.supportSkills),
                StringToLongArray(characterData.buildSkills)
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
    }
}