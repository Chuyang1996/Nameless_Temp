using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class CharacterData
    {
        public long Id;
        public string name;
        public string descrption;
        public float health;
        public float crHealth;
        public float attack;
        public float crAttack;
        public float defend;
        public float crDefend;
        public float morale;
        public float crMorale;
        public int ammo;
        public float crAmmo;
        public float speed;
        public float crSpeed;
        public float hit;
        public float crHit;
        public float dex;
        public float crDex;

        public string fightSkills;
        public string supportSkills;
        public string buildSkills;

        public string dialogue;
        public string animPrefab;
        public string selectIcon;
        public string campIcon;
        public int campPosIndex;
        public int btnLRpos;
        public string converIds;

        public CharacterData
            (
            long Id, 
            string name, 
            string descrption, 
            float health, 
            float crHealth, 
            float attack, 
            float crAttack,
            float defend, 
            float crDefend, 
            float morale, 
            float crMorale, 
            int ammo, 
            float crAmmo, 
            float speed, 
            float crSpeed, 
            float hit, 
            float crHit, 
            float dex, 
            float crDex,
            string fightSkills,
            string supportSkills,
            string buildSkills,
            string dialogue,
            string animPrefab,
            string selectIcon,
            string campIcon,
            int campPosIndex,
            int btnLRpos,
            string converIds
            )
        {
            this.Id = Id;
            this.name = name;
            this.descrption = descrption;
            this.health = health;
            this.crHealth = crHealth;
            this.attack = attack;
            this.crAttack = crAttack;
            this.defend = defend;
            this.crDefend = crDefend;
            this.morale = morale;
            this.crMorale = crMorale;
            this.ammo = ammo;
            this.crAmmo = crAmmo;
            this.speed = speed;
            this.crSpeed = crSpeed;
            this.hit = hit;
            this.crHit = crHit;
            this.dex = dex;
            this.crDex = crDex;
            this.fightSkills = fightSkills;
            this.supportSkills = supportSkills;
            this.buildSkills = buildSkills;
            this.dialogue = dialogue;
            this.animPrefab = animPrefab;
            this.selectIcon = selectIcon;
            this.campIcon = campIcon;
            this.campPosIndex = campPosIndex;
            this.btnLRpos = btnLRpos;
            this.converIds = converIds;

        }

        public void Init
    (
            long Id,
            string name,
            string descrption,
    float health,
    float crHealth,
    float attack,
    float crAttack,
    float defend,
    float crDefend,
    float morale,
    float crMorale,
    int ammo,
    float crAmmo,
    float speed,
    float crSpeed,
    float hit,
    float crHit,
    float dex,
    float crDex,
            string fightSkills,
            string supportSkills,
            string buildSkills,
            string dialogue,
            string animName,
            string iconName,
            string campName,
            int campPosIndex,
            int btnLRpos,
            string converIds
            )
        {
            this.Id = Id;
            this.name = name;
            this.name = descrption;
            this.health = health;
            this.crHealth = crHealth;
            this.attack = attack;
            this.crAttack = crAttack;
            this.defend = defend;
            this.crDefend = crDefend;
            this.morale = morale;
            this.crMorale = crMorale;
            this.ammo = ammo;
            this.crAmmo = crAmmo;
            this.speed = speed;
            this.crSpeed = crSpeed;
            this.hit = hit;
            this.crHit = crHit;
            this.dex = dex;
            this.crDex = crDex;
            this.fightSkills = fightSkills;
            this.supportSkills = supportSkills;
            this.buildSkills = buildSkills;
            this.dialogue = dialogue;
            this.animPrefab = animName;
            this.selectIcon = iconName;
            this.campIcon = campName;
            this.campPosIndex = campPosIndex;
            this.btnLRpos = btnLRpos;
            this.converIds = converIds;

        }
        // Start is called before the first frame update



    }
}