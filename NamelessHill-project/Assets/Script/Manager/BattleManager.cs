using Nameless.Data;
using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public class Battle
    {
        PawnAvatar attacker;
        PawnAvatar defender;

        public Battle(PawnAvatar attacker, PawnAvatar defender)
        {
            this.attacker = attacker;
            this.defender = defender;
        }

        public override bool Equals(object obj)
        {
            Battle temp = (Battle)obj;
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
}
namespace Nameless.Manager 
{
    public class BattleManager : SingletonMono<BattleManager>
    {
        public Dictionary<Battle, BattleRound> battleDic = new Dictionary<Battle, BattleRound>();
        public void GenerateBattle(PawnAvatar attacker, PawnAvatar defender, bool defenderisInBattle)
        {
            
            GameObject newBattle = Instantiate( Resources.Load("Prefabs/Battle")) as GameObject;
            newBattle.GetComponent<BattleRound>().Init(attacker, defender);
            newBattle.gameObject.transform.parent = MapManager.Instance.currentMap.Battle.transform;//待修改 加了Map数据之后
            attacker.UpdateCurrentOppo(defender);
            if (!defenderisInBattle)
            {
                defender.UpdateCurrentOppo(attacker);
            }
            Battle battle = new Battle(attacker, defender);
            this.battleDic.Add(battle, newBattle.GetComponent<BattleRound>());
            //defender.CheckBattleState();
            newBattle.transform.parent = this.transform;
            StartCoroutine(newBattle.GetComponent<BattleRound>().ProcessBattle(defenderisInBattle));
        }
    }
}