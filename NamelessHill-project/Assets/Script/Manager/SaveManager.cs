using Nameless.Agent;
using Nameless.ConfigData;
using Nameless.Data;
using Nameless.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Nameless.Manager
{
    [System.Serializable]
    public class GameData
    {
        public PlayerSaveData playerSaveData;
        public NotePageSaveData notePageSaveData;
        public long mapId = -1;
        public long campId = -1;

        public GameData(Player player, Dictionary<long, NotePage> notePageDic,long mapId, long campId)
        {
            this.playerSaveData = new PlayerSaveData(player);
            this.notePageSaveData = new NotePageSaveData(notePageDic);
            this.mapId = mapId;
            this.campId = campId;
        }
    }
    [System.Serializable]
    public class PlayerSaveData
    {
        public List<PawnSaveData> pawnSaveDatas = new List<PawnSaveData>();
        public EventCollectionSaveData eventCollectionSaveData;
        public int totalMilitaryRes = 0;
        public PlayerSaveData(Player player)
        {
            this.pawnSaveDatas = new List<PawnSaveData>();
            for (int i = 0; i < player.pawns.Count; i++)
            {
                this.pawnSaveDatas.Add(new PawnSaveData(player.pawns[i]));
            }
            this.eventCollectionSaveData = new EventCollectionSaveData(player.eventCollections);
            this.totalMilitaryRes = player.totalMilitaryRes;
        }

        public Player GetPlayer()
        {
            return new Player(this);
        }
    }

    [System.Serializable]
    public class EventCollectionSaveData
    {
        public List<long> pawnKilledIds = new List<long>();
        public List<long> eventOptionId = new List<long>();
        public List<PawnSaveData> leavePawn = new List<PawnSaveData>();

        public EventCollectionSaveData(EventCollections eventCollections)
        {
            this.pawnKilledIds = eventCollections.AllPawnSkilledIds();
            this.eventOptionId = eventCollections.AllEventOptionIds();
            List<Pawn> pawns = eventCollections.AllLeavePawns();
            for(int i = 0; i < pawns.Count; i++)
            {
                this.leavePawn.Add(new PawnSaveData(pawns[i]));
            }
        }

        public EventCollections GetEventCollections()
        {
            return new EventCollections(this);
        }
    }
    [System.Serializable]
    public class PawnSaveData
    {
        #region//角色信息
        public long id;
        public string name;
        public string descrption;
        #endregion

        #region//角色属性
        public float maxHealth;
        public float maxAttack;
        public float maxMorale;
        public int maxAmmo;
        public float maxAtkSpeed;
        public float maxSpeed;
        public float maxHit;
        public float maxDex;
        public float maxDefend;

        public float curHealth;
        public float curAttack;
        public float curMorale;
        public float curAmmo;
        public float curAtkSpeed;
        public float curSpeed;
        public float curHit;
        public float curDex;
        public float curDefend;
        #endregion

        #region//角色文本
        public string fallBackTxt;
        public string pinchTxt;
        public string surroundTxt;
        public string winTxt;
        #endregion

        #region//角色技能
        public List<long> fightSkillIds = new List<long>();
        public List<long> supportSkillIds = new List<long>();
        public List<long> buildSkillIds = new List<long>();
        #endregion

        #region//角色美术
        public string animPrefab;
        public string selectIcon;
        public string campIcon;
        public string campMarkIcon;
        #endregion

        #region//角色对话
        public List<long> conversDefaultMapId = new List<long>();
        public List<long> conversDefaultIds = new List<long>();

        public List<long> conversationInCamp = new List<long>();
        #endregion

        public int leftResNum;
        public bool moveAvaliable;
        public int campPosIndex;
        public int leftOrRight;

        public PawnSaveData(Pawn pawn)
        {
            PawnData pawnData = DataManager.Instance.GetPawn(pawn.id);
            this.id = pawn.id;
            this.name = pawn.name;
            this.descrption = pawn.descrption;

            this.maxHealth = pawn.maxHealth;
            this.maxAttack = pawn.maxAttack;
            this.maxMorale = pawn.maxMorale;
            this.maxAmmo = pawn.maxAmmo;
            this.maxAtkSpeed = pawn.maxAtkSpeed;
            this.maxSpeed = pawn.maxSpeed;
            this.maxHit = pawn.maxHit;
            this.maxDex = pawn.maxDex;
            this.maxDefend = pawn.maxDefend;

            this.curHealth = pawn.curHealth;
            this.curAttack = pawn.curAttack;
            this.curMorale = pawn.curMorale;
            this.curAmmo = pawn.curAmmo;
            this.curAtkSpeed = pawn.curAtkSpeed;
            this.curSpeed = pawn.curSpeed;
            this.curHit = pawn.curHit;
            this.curDex = pawn.curDex;
            this.curDefend = pawn.curDefend;

            this.fallBackTxt = pawn.fallBackTxt;
            this.pinchTxt = pawn.pinchTxt;
            this.surroundTxt = pawn.surroundTxt;
            this.winTxt = pawn.winTxt;

            this.fightSkillIds = this.StringToLongArray(pawnData.fightSkills);
            this.supportSkillIds = this.StringToLongArray(pawnData.supportSkills);
            this.buildSkillIds = this.StringToLongArray(pawnData.buildSkills);

            this.animPrefab = pawnData.animPrefab;
            this.selectIcon = pawnData.selectIcon;
            this.campIcon = pawnData.campIcon;
            this.campMarkIcon = pawnData.campIcon;

            foreach(var child in pawn.conversationMapDic)
            {
                conversDefaultMapId.Add(child.Key);
                conversDefaultIds.Add(child.Value.id);
            }
            foreach(var child in pawn.conversationsInCamp)
                this.conversationInCamp.Add(child.id);
            

            this.leftResNum = pawn.leftResNum;
            this.leftOrRight = pawn.leftOrRight;
            this.moveAvaliable = pawn.moveAvaliable;
            this.campPosIndex = pawn.campPosIndex;

        }


        public Pawn GetPawn()
        {
            return new Pawn(this);

        }

        private List<long> StringToLongArray(string stringlist)
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
    [System.Serializable]
    public class NotePageSaveData
    {
        public List<long> notePageId = new List<long>();

        public NotePageSaveData(Dictionary<long,NotePage> notePageDic)
        {
            this.notePageId = new List<long>();
            foreach(var child in notePageDic)
            {
                this.notePageId.Add(child.Key);
            }
        }
        public Dictionary<long, NotePage> NotePageDictionary()
        {
            Dictionary<long, NotePage> notePageDic = new Dictionary<long, NotePage>();
            foreach(var child in this.notePageId)
            {
                notePageDic.Add(child, NotePageFactory.GetNotePageById(child));
            }
            return notePageDic;
        }

    }


    public class SaveManager : Singleton<SaveManager>
    {
        public GameData gameData;
        public void Init()
        {

        }
        public bool IfSaveExist()
        {
            string path = Application.persistentDataPath + "/" + "DefaultSave.fun";
            Debug.Log(path);
            if (File.Exists(path))
                return true;
            else
                return false;
        }
        public void SaveData(Player player, Dictionary<long, NotePage> notePageDic, long mapId, long campId)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/DefaultSave.fun";
            FileStream stream = new FileStream(path, FileMode.Create);

            GameData data = new GameData(player, notePageDic, mapId, campId);
            formatter.Serialize(stream, data);
            Debug.Log(path);
            stream.Close();

        }

        public GameData LoadData()
        {
            string path = Application.persistentDataPath + "/" + "DefaultSave.fun";
            Debug.Log(path);
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                GameData data = formatter.Deserialize(stream) as GameData;

                stream.Close();
                return data;
            }
            else
            {
                Debug.LogError("Save File not found in" + path);
                return null;
            }
        }
    }
}