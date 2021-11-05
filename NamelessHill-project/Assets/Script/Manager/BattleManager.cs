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
        public Dictionary<Battle, BattleController> battleDic = new Dictionary<Battle, BattleController>();
        public void GenerateBattle(PawnAvatar attacker, PawnAvatar defender)
        {
            
            GameObject newBattle = Instantiate( Resources.Load("Prefabs/Battle")) as GameObject;
            newBattle.GetComponent<BattleController>().Init(attacker, defender);
            attacker.pawnAgent.curOpponent = defender;
            defender.pawnAgent.curOpponent = attacker;
            Battle battle = new Battle(attacker, defender);
            this.battleDic.Add(battle, newBattle.GetComponent<BattleController>());
            //defender.CheckBattleState();
            StartCoroutine(newBattle.GetComponent<BattleController>().ProcessBattle());
        }
    }
}