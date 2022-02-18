using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData
{
    public class PawnData
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
        public float atkSpeed;
        public float crAtkSpeed;
        public float speed;
        public float crSpeed;
        public float hit;
        public float crHit;
        public float dex;
        public float crDex;

        public int leftResNum;
        public bool moveAvaliable;

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

        public PawnData
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
            float atkSpeed, 
            float crAtkSpeed, 
            float speed, 
            float crSpeed, 
            float hit, 
            float crHit, 
            float dex,
            float crDex,
            int leftResNum,
            bool moveAvaliable,
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
            this.atkSpeed = atkSpeed;
            this.crAtkSpeed = crAtkSpeed;
            this.speed = speed;
            this.crSpeed = crSpeed;
            this.hit = hit;
            this.crHit = crHit;
            this.dex = dex;
            this.leftResNum = leftResNum;
            this.moveAvaliable = moveAvaliable;
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

        
        // Start is called before the first frame update



    }
}