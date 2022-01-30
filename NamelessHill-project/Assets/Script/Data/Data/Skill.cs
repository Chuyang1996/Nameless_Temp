using Nameless.DataMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Data
{
    public enum SkillConditionType
    {
        LowHealth = 0,
        HighHealth = 1,
        LowAttack = 2,
        HightAttack = 3,
        LowDefend = 4,
        HighDefend = 5,
        LowMorale = 6,
        HighMorale = 7,
        LowAmmo = 8,
        HighAmmo = 9,
        IsAttacker = 10,
        IsDefender = 11,
        //LowMedicine = 10,
        //HighMedicine = 11,
    }


    public struct PropertySkillEffect
    {
        public float changeAttack;
        public float changeDefend;

        public float changeAmmo;
        public float changeMedicine;
        public List<Buff> buffs;
        public PropertySkillEffect(float changeAttack, float changeDefend, float changeAmmo,float changeMedicine, List<Buff> buffs)
        {
            this.changeAttack = changeAttack;
            this.changeDefend = changeDefend;
            this.changeAmmo = changeAmmo;
            this.changeMedicine = changeMedicine;
            this.buffs = buffs;
        }
    }
    public class Skill
    {
        public long id;
        public string name;
        public string descrption;
        public Sprite icon;
        public Dictionary<SkillConditionType, float> conditions = new Dictionary<SkillConditionType, float>();

        public bool IsActive(PawnAvatar pawnAvatar)
        {
            foreach(var child in this.conditions)
            {
                if(child.Key == SkillConditionType.LowHealth)
                {
                    if(pawnAvatar.pawnAgent.pawn.curHealth <= child.Value * pawnAvatar.pawnAgent.pawn.maxHealth)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == SkillConditionType.HighHealth)
                {
                    if (pawnAvatar.pawnAgent.pawn.curHealth > child.Value * pawnAvatar.pawnAgent.pawn.maxHealth)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == SkillConditionType.LowAttack)
                {
                    if (pawnAvatar.pawnAgent.pawn.curAttack <= child.Value * pawnAvatar.pawnAgent.pawn.maxAttack)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == SkillConditionType.HightAttack)
                {
                    if (pawnAvatar.pawnAgent.pawn.curAttack > child.Value * pawnAvatar.pawnAgent.pawn.maxAttack)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == SkillConditionType.LowDefend)
                {
                    if (pawnAvatar.pawnAgent.pawn.curDefend <= child.Value * pawnAvatar.pawnAgent.pawn.maxDefend)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == SkillConditionType.HighDefend)
                {
                    if (pawnAvatar.pawnAgent.pawn.curDefend > child.Value * pawnAvatar.pawnAgent.pawn.maxDefend)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == SkillConditionType.LowMorale)
                {
                    if (pawnAvatar.pawnAgent.pawn.curMorale <= child.Value * pawnAvatar.pawnAgent.pawn.maxMorale)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == SkillConditionType.HighMorale)
                {
                    if (pawnAvatar.pawnAgent.pawn.curMorale > child.Value * pawnAvatar.pawnAgent.pawn.maxMorale)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == SkillConditionType.LowAmmo)
                {
                    if (pawnAvatar.pawnAgent.pawn.curAmmo <= child.Value * pawnAvatar.pawnAgent.pawn.maxAmmo)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (child.Key == SkillConditionType.HighAmmo)
                {
                    if (pawnAvatar.pawnAgent.pawn.curAmmo > child.Value * pawnAvatar.pawnAgent.pawn.maxAmmo)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if(child.Key == SkillConditionType.IsAttacker)
                {
                    if (pawnAvatar.pawnAgent.battleSideDic.ContainsValue(BattleSide.Attacker))
                    {
                        continue;
                    }
                    else
                        return false;
                }
                else if (child.Key == SkillConditionType.IsDefender)
                {
                    if (pawnAvatar.pawnAgent.battleSideDic.ContainsValue(BattleSide.Defender))
                    {
                        continue;
                    }
                    else
                        return false;
                }

            }
            return true;
        }

        public PropertySkillEffect Execute(PawnAvatar condition, PawnAvatar contribute)
        {
            List<Buff> buffs = new List<Buff>();
            if (this.IsActive(condition))
            {
                if (this is FightSkill)
                {
                    FightSkill fightSkill = (FightSkill)this;
                    return new PropertySkillEffect(fightSkill.ExtraAttack(condition, contribute), fightSkill.ExtraDefend(condition, contribute), 0, 0, buffs);
                }
                else if(this is SupportSkill)
                {
                    SupportSkill supportSkill = (SupportSkill)this;
                    buffs = supportSkill.buffs;
                    return new PropertySkillEffect(supportSkill.ExtraAttack(condition, contribute), supportSkill.ExtraDefend(condition, contribute), 0, 0, buffs);
                }
                else
                {
                    return new PropertySkillEffect(0, 0, 0, 0, buffs);
                }
            }
            else
            {
                return new PropertySkillEffect(0, 0, 0, 0, buffs);
            }
        }

    }

    public class FightSkill : Skill
    {
        public float attackRate;
        public float defendRate;
        public FightSkill(long id, string name, string descrption, Dictionary<SkillConditionType,float> conditions, float attackRate, float defendRate, Sprite iconName)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.conditions = conditions;
            this.attackRate = attackRate;
            this.defendRate = defendRate;
            this.icon = iconName;
        }

        public float ExtraAttack(PawnAvatar conditioner, PawnAvatar receiver)
        {
            if (this.IsActive(conditioner))
                return attackRate * receiver.pawnAgent.pawn.maxAttack;
            else
                return 0.0f;
        }

        public float ExtraDefend(PawnAvatar conditioner, PawnAvatar receiver)
        {
            if (this.IsActive(conditioner))
                return defendRate * receiver.pawnAgent.pawn.maxDefend;
            else
                return 0.0f;
        }
    }

    public class SupportSkill : Skill
    {
        public List<Buff> buffs;
        public float attackRate;
        public float defendRate;
        public SupportSkill(long id, string name, string descrption, Dictionary<SkillConditionType, float> conditions, List<Buff> buffs, float attackRate, float defendRate, Sprite iconName)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.conditions = conditions;
            this.buffs = buffs;
            this.attackRate = attackRate;
            this.defendRate = defendRate;
            this.icon = iconName;
        }
        public float ExtraAttack(PawnAvatar conditioner, PawnAvatar receiver)
        {
            if (this.IsActive(conditioner))
                return attackRate * receiver.pawnAgent.pawn.maxAttack;
            else
                return 0.0f;
        }

        public float ExtraDefend(PawnAvatar conditioner, PawnAvatar receiver)
        {
            if (this.IsActive(conditioner))
                return defendRate * receiver.pawnAgent.pawn.maxDefend;
            else
                return 0.0f;
        }
    }


    public class BuildSkill : Skill
    {
        public Build build;
        public BuildType buildType;
        public BuildSkill(long id, string name, string descrption, Dictionary<SkillConditionType, float> conditions, BuildType buildType, Build build, Sprite iconName)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.conditions = conditions;
            this.build = build;
            this.buildType = buildType;
            this.icon = iconName;
        }

        //用于AI建造时用
        public bool ExecuteBuild(PawnAvatar pawnAvatar,Area area)
        {
            if (this.IsActive(pawnAvatar))
            {
                this.build.ExecuteBuild(pawnAvatar,area);
                return true;
            }
            else
                return false;
        }



    }
}