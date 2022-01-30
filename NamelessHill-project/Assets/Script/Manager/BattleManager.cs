using Nameless.Data;
using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public class BattlePawn
    {
        PawnAvatar attacker;
        PawnAvatar defender;

        public BattlePawn(PawnAvatar attacker, PawnAvatar defender)
        {
            this.attacker = attacker;
            this.defender = defender;
        }

        public override bool Equals(object obj)
        {
            BattlePawn temp = (BattlePawn)obj;
            if(this.attacker == temp.attacker && this.defender == temp.defender)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 1;
        }
    }
    public class BattleBuild
    {
        PawnAvatar attacker;
        BuildAvatar defender;

        public BattleBuild(PawnAvatar attacker, BuildAvatar defender)
        {
            this.attacker = attacker;
            this.defender = defender;
        }

        public override bool Equals(object obj)
        {
            BattleBuild temp = (BattleBuild)obj;
            if (this.attacker == temp.attacker && this.defender == temp.defender)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 1;
        }
    }
}
namespace Nameless.Manager 
{
    public class BattleManager : SingletonMono<BattleManager>
    {
        public Dictionary<BattlePawn, BattlePawnRound> battlePawnDic = new Dictionary<BattlePawn, BattlePawnRound>();
        public Dictionary<BattleBuild, BattleBuildRound> battleBuildDic = new Dictionary<BattleBuild, BattleBuildRound>();
        public void GenerateBattlePawn(PawnAvatar attacker, PawnAvatar defender, bool defenderisInBattle)
        {
            
            GameObject newBattle = Instantiate( Resources.Load("Prefabs/Battle/BattlePawn")) as GameObject;
            newBattle.GetComponent<BattlePawnRound>().Init(attacker, defender);
            newBattle.gameObject.transform.parent = MapManager.Instance.currentMap.BattleCollect.transform;//待修改 加了Map数据之后
            attacker.UpdateCurrentOppo(defender);
            if (!defenderisInBattle)
            {
                defender.UpdateCurrentOppo(attacker);
            }
            //defender.CheckBattleState();
            StartCoroutine(newBattle.GetComponent<BattlePawnRound>().ProcessBattle(defenderisInBattle));
        }

        public void GenerateBattleBuild(PawnAvatar pawnAvatar, BuildAvatar buildAvatar)
        {
            GameObject newBattle = Instantiate(Resources.Load("Prefabs/Battle/BattleBuild")) as GameObject;
            newBattle.GetComponent<BattleBuildRound>().Init(pawnAvatar, buildAvatar);
            newBattle.gameObject.transform.parent = MapManager.Instance.currentMap.BattleCollect.transform;//待修改 加了Map数据之后

            StartCoroutine(newBattle.GetComponent<BattleBuildRound>().ProcessBattle());
        }

        public void ClearBattle()
        {
            this.battlePawnDic.Clear();
            this.battleBuildDic.Clear();
        }
    }
}