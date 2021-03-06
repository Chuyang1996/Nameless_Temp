using Nameless.Agent;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public class Pawn
    {
        public long id;
        public string name;
        public string descrption;

        public float maxHealth;
        public float maxAttack;
        public float maxMorale;
        public int maxAmmo;
        public float maxAtkSpeed;
        public float maxSpeed;
        public float maxHit;
        public float maxDex;
        public float maxDefend;

        public int leftResNum;


        public string fallBackTxt;
        public string pinchTxt;
        public string surroundTxt;
        public string winTxt;

        public float curHealth
        {
            set
            {
                if (value <= 0)
                {
                    this.currentHeatlh = 0;
                }
                else if( value >= this.maxHealth)
                {
                    this.currentHeatlh = this.maxHealth;
                }
                else
                {
                    this.currentHeatlh = value;
                }
            }
            get
            {
                return this.currentHeatlh;
            }
        }
        public float curAttack
        {
            set
            {
                if (value <= 0)
                {
                    this.currentAttack = 0;
                }
                else
                {
                    this.currentAttack = value;
                }
            }
            get
            {
                return currentAttack;
            }
        }
        public float curMorale
        {
            set
            {
                if (value <= 0)
                {
                    this.currentMorale = 0;
                }
                else if (value >= this.maxMorale)
                {
                    this.currentMorale = this.maxMorale;
                }
                else
                {
                    this.currentMorale = value;
                }
            }
            get
            {
                return currentMorale;
            }
        }
        public float curAmmo
        {
            set
            {
                if (value <= 0)
                {
                    this.currentAmmo = 0;
                }
                else if (value >= this.maxAmmo)
                {
                    this.currentAmmo = this.maxAmmo;
                }
                else
                {
                    this.currentAmmo = value;
                }
            }
            get
            {
                return currentAmmo;
            }
        }
        public float curAtkSpeed
        {
            set
            {
                if (value <= 0)
                {
                    this.currentAtkSpeed = 0;
                }
                else if (value >= this.maxAtkSpeed)
                {
                    this.currentAtkSpeed = this.maxAtkSpeed;
                }
                else
                {
                    this.currentAtkSpeed = value;
                }
            }
            get
            {
                return currentAtkSpeed;
            }
        }
        public float curSpeed
        {
            set
            {
                if (value <= 0)
                {
                    this.currentSpeed = 0;
                }
                else if (value >= this.maxSpeed)
                {
                    this.currentSpeed = this.maxSpeed;
                }
                else
                {
                    this.currentSpeed = value;
                }
            }
            get
            {
                return this.currentSpeed;
            }
        }
        public float curHit
        {
            set
            {
                if (value <= 0)
                {
                    this.currentHit = 0;
                }
                else if (value >= this.maxHit)
                {
                    this.currentHit = this.maxHit;
                }
                else
                {
                    this.currentHit = value;
                }
            }
            get
            {
                return this.currentHit;
            }
        }
        public float curDex
        {
            set
            {
                if (value <= 0)
                {
                    this.currentDex = 0;
                }
                else if (value >= this.maxDex)
                {
                    this.currentDex = this.maxDex;
                }
                else
                {
                    this.currentDex = value;
                }
            }
            get
            {
                return this.currentDex;
            }
        }
        public float curDefend
        {
            set
            {
                if (value <= 0)
                {
                    this.currentDefend = 0;
                }
                else if (value >= this.maxDex)
                {
                    this.currentDefend = this.maxDefend;
                }
                else
                {
                    this.currentDefend = value;
                }
            }
            get
            {
                return this.currentDefend;
            }
        }

        private float currentHeatlh;
        private float currentAttack;
        private float currentMorale;
        private float currentAmmo;
        private float currentAtkSpeed;
        private float currentSpeed;
        private float currentHit;
        private float currentDex;
        private float currentDefend;


        public List<Skill> skills = new List<Skill>();

        //public Dictionary<long, DialogueGroup> dialogueDic = new Dictionary<long, DialogueGroup>();

        public bool moveAvaliable;

        public string animPrefabName;
        public Sprite selectIcon;
        public Sprite campIcon;
        public Sprite campMarkIcon;
        public int campPosIndex;
        public int leftOrRight;
        public Dictionary<long, Conversation> conversationMapDic = new Dictionary<long, Conversation>();//??????????new Dictionary<long, List<Conversation>>()????????????????

        #region//????????????????
        public Stack<Conversation> conversationsInCamp = new Stack<Conversation>();
        #endregion
        public Pawn(
            long id, 
            string name, 
            string descrption, 
            float health, float crHealth, 
            float attack, float crAttack, 
            float morale, float crMorale, 
            float atkSpeed, float crAtkSpeed, 
            int ammo, float crAmmo, 
            float speed, float crSpeed, 
            float hit, float crHit, 
            float dex, float crDex, 
            float defend, float crDefend, 
            int leftResNum,
            bool moveAvaliable,
            List<long> fightSkillIds, 
            List<long> supportSkillIds, 
            List<long> buildSkillIds, 
            //Dictionary<long, DialogueGroup> dialogueDic,
            string animPrefab,string selectIcon, string campIcon, int campPosIndex, int btnLRpos, Dictionary<long, Conversation> conversationMapDic)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.maxHealth = health;
            this.maxAttack = attack;
            this.maxMorale = morale;
            this.maxAmmo = ammo;
            this.maxAtkSpeed = atkSpeed;
            this.maxSpeed = speed;
            this.maxHit = hit;
            this.maxDex = dex;
            this.maxDefend = defend;

            this.curHealth = this.maxHealth * crHealth;
            this.curAttack = this.maxAttack * crAttack;
            this.curMorale = this.maxMorale * crMorale;
            this.curAmmo = this.maxAmmo * crAmmo;
            this.curAtkSpeed = this.maxAtkSpeed * crAtkSpeed;
            this.curSpeed = this.maxSpeed * crSpeed;
            this.curHit = this.maxHit * crHit;
            this.curDex = this.maxDex * crDex;
            this.curDefend = this.maxDex * crDefend;
            this.leftResNum = leftResNum;
            this.moveAvaliable = moveAvaliable;

            this.fallBackTxt = "Fall back!! Fall back!!";
            this.pinchTxt = "We are attacked by two sides!!";
            this.surroundTxt = "They are too many!!";
            this.winTxt = "We won!! Charge!!";



            //this.dialogueDic = dialogueDic;
            this.animPrefabName = animPrefab;
            this.selectIcon = SpriteManager.Instance.FindSpriteByName(AtlasType.CharacterImage, selectIcon);
            this.campIcon = SpriteManager.Instance.FindSpriteByName(AtlasType.CharacterCampImage, campIcon);
            this.campMarkIcon = SpriteManager.Instance.FindSpriteByName(AtlasType.CharacterCampMarkImage, campIcon);
            this.campPosIndex = campPosIndex;
            this.leftOrRight = btnLRpos;
            this.conversationMapDic = conversationMapDic;
            this.conversationsInCamp = new Stack<Conversation>();

            this.skills = new List<Skill>();

            for (int i = 0; i < fightSkillIds.Count; i++)
            {
                this.skills.Add(SkillFactory.GetSkillById(SkillFactoryType.FightSkill, fightSkillIds[i]));
            }
            for (int i = 0; i < supportSkillIds.Count; i++)
            {
                this.skills.Add(SkillFactory.GetSkillById(SkillFactoryType.SupportSkill, supportSkillIds[i]));
            }
            for (int i = 0; i < buildSkillIds.Count; i++)
            {
                this.skills.Add(SkillFactory.GetSkillById(SkillFactoryType.BuildSkill, buildSkillIds[i]));
            }
        }



        public Pawn(PawnSaveData pawnSaveData)
        {
            this.id = pawnSaveData.id;
            this.name = pawnSaveData.name;
            this.descrption = pawnSaveData.descrption;

            this.maxHealth = pawnSaveData.maxHealth;
            this.maxAttack = pawnSaveData.maxAttack;
            this.maxMorale = pawnSaveData.maxMorale;
            this.maxAmmo = pawnSaveData.maxAmmo;
            this.maxAtkSpeed = pawnSaveData.maxAtkSpeed;
            this.maxSpeed = pawnSaveData.maxSpeed;
            this.maxHit = pawnSaveData.maxHit;
            this.maxDex = pawnSaveData.maxDex;
            this.maxDefend = pawnSaveData.maxDefend;

            this.curHealth = pawnSaveData.curHealth;
            this.curAttack = pawnSaveData.curAttack;
            this.curMorale = pawnSaveData.curMorale;
            this.curAmmo = pawnSaveData.curAmmo;
            this.curAtkSpeed = pawnSaveData.curAtkSpeed;
            this.curSpeed = pawnSaveData.curSpeed;
            this.curHit = pawnSaveData.curHit;
            this.curDex = pawnSaveData.curDex;
            this.curDefend = pawnSaveData.curDefend;

            this.fallBackTxt = pawnSaveData.fallBackTxt;
            this.pinchTxt = pawnSaveData.pinchTxt;
            this.surroundTxt = pawnSaveData.surroundTxt;
            this.winTxt = pawnSaveData.winTxt;

            this.skills = new List<Skill>();

            for (int i = 0; i < pawnSaveData.fightSkillIds.Count; i++)
            {
                skills.Add(SkillFactory.GetSkillById(SkillFactoryType.FightSkill, pawnSaveData.fightSkillIds[i]));
            }
            for (int i = 0; i < pawnSaveData.supportSkillIds.Count; i++)
            {
                skills.Add(SkillFactory.GetSkillById(SkillFactoryType.SupportSkill, pawnSaveData.supportSkillIds[i]));
            }
            for (int i = 0; i < pawnSaveData.buildSkillIds.Count; i++)
            {
                skills.Add(SkillFactory.GetSkillById(SkillFactoryType.BuildSkill, pawnSaveData.buildSkillIds[i]));
            }

            this.animPrefabName = pawnSaveData.animPrefab;
            this.selectIcon = SpriteManager.Instance.FindSpriteByName(AtlasType.CharacterImage, pawnSaveData.selectIcon);
            this.campIcon = SpriteManager.Instance.FindSpriteByName(AtlasType.CharacterCampImage, pawnSaveData.campIcon);
            this.campMarkIcon = SpriteManager.Instance.FindSpriteByName(AtlasType.CharacterCampMarkImage, pawnSaveData.campMarkIcon);

            for(int i = 0;i< pawnSaveData.conversDefaultMapId.Count; i++)
            {
                this.conversationMapDic.Add(pawnSaveData.conversDefaultMapId[i],ConversationFactory.GetConversationById(pawnSaveData.conversDefaultIds[i]));
            }
            this.conversationsInCamp = new Stack<Conversation>();
            for(int i = 0; i < pawnSaveData.conversationInCamp.Count; i++)
            {
                this.conversationsInCamp.Push(ConversationFactory.GetConversationById(pawnSaveData.conversationInCamp[i]));
            }
            this.leftResNum = pawnSaveData.leftResNum;
            this.leftOrRight = pawnSaveData.leftOrRight;
            this.moveAvaliable = pawnSaveData.moveAvaliable;
            this.campPosIndex = pawnSaveData.campPosIndex;
        }

        public void ResetConversation()
        {
            this.conversationsInCamp = new Stack<Conversation>();
        }
    }
}