using Nameless.Agent;
using Nameless.Data;
using Nameless.DataMono;
using Nameless.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.Manager {



    public class EventTriggerManager : Singleton<EventTriggerManager>
    {

        // Start is called before the first frame update
        public Dictionary<EventTrigger, List<EventResult>> eventTriggerDic = new Dictionary<EventTrigger, List<EventResult>>();
        public Stack<EventResult> currentAllEvent = new Stack<EventResult>();
        
        public void InitEventTrigger(List<EventResult> allEventResults)
        {
            Dictionary<long, List<EventResult>> tempDic = new Dictionary<long, List<EventResult>>();
            for(int i = 0; i < allEventResults.Count; i++)
            {
                if (tempDic.ContainsKey(allEventResults[i].conditionId))
                    tempDic[allEventResults[i].conditionId].Add(allEventResults[i]);
                else
                {
                    List<EventResult> tempResults = new List<EventResult>();
                    tempResults.Add(allEventResults[i]);
                    tempDic.Add(allEventResults[i].conditionId, tempResults);
                }
            }

            foreach(var child in tempDic)
            {
                this.eventTriggerDic.Add(EventTriggerFactory.GetEventTriggerById(child.Key), child.Value);
            }
        }
        public void ClearEvent()
        {
            this.eventTriggerDic.Clear();
            this.currentAllEvent.Clear();
        }
        public void CheckRelateTimeEvent(int currentTime)
        {
            foreach (var child in this.eventTriggerDic)
            {
                if (child.Key.type == EventTriggerType.TimePass)
                {
                    int passTime = GameManager.Instance.totalTime - currentTime;
                    if (((EventTimePass)child.Key).IsTrigger(passTime))
                    {
                        for (int i = 0; i < child.Value.Count; i++)
                        {
                            this.currentAllEvent.Push(child.Value[i]);
                        }
                    }
                }
            }
        }

        public void CheckRelateMilitaryResEvent(int cost)
        {
            foreach(var child in this.eventTriggerDic)
            {
                if(child.Key.type == EventTriggerType.MilitaryResLess)
                {

                    int lastAmmo = GameManager.Instance.totalMilitaryRes;
                    int afterAmmo = GameManager.Instance.totalMilitaryRes + cost;
                    if (((EventMilitaryResLess)child.Key).IsTrigger(lastAmmo,afterAmmo))
                    {
                       for(int i = 0; i < child.Value.Count; i++)
                        {
                            this.currentAllEvent.Push(child.Value[i]);
                        }
                    }
                }
            }
        }


        public void CheckRelateEnemyKillEvent(int num)
        {
            foreach (var child in this.eventTriggerDic)
            {
                if (child.Key.type == EventTriggerType.EnemyKillNum)
                {
                    int lastEnemies = GameManager.Instance.enemiesDieNum;
                    int afterEnemies = GameManager.Instance.enemiesDieNum + num;
                    if (((EventEnemyKillLess)child.Key).IsTrigger(lastEnemies,afterEnemies))
                    {
                        for (int i = 0; i < child.Value.Count; i++)
                        {
                            this.currentAllEvent.Push(child.Value[i]);
                        }
                    }
                }
            }
        }

        public void CheckPawnArriveArea(long pawnId, int areaLocalId)
        {
            foreach (var child in this.eventTriggerDic)
            {
                if (child.Key.type == EventTriggerType.PawnArriveOnArea)
                {
                    if (((EventPawnArrive)child.Key).IsTrigger(pawnId, areaLocalId))
                    {
                        for (int i = 0; i < child.Value.Count; i++)
                        {
                            this.currentAllEvent.Push(child.Value[i]);
                        }
                    }
                }
            }
        }

        public void CheckEventBuildOnArea(BuildType buildType)
        {
            foreach (var child in this.eventTriggerDic)
            {
                if (child.Key.type == EventTriggerType.BuildOnArea)
                {
                    if (((EventBuildOnArea)child.Key).IsTrigger(buildType))
                    {
                        for (int i = 0; i < child.Value.Count; i++)
                        {
                            this.currentAllEvent.Push(child.Value[i]);
                        }
                    }
                }
            }
        }

        public void CheckPawnStartBattle(long pawnId)
        {
            foreach (var child in this.eventTriggerDic)
            {
                if (child.Key.type == EventTriggerType.PawnEnterBattle)
                {
                    if (((EventPawnStartBattle)child.Key).IsTrigger(pawnId))
                    {
                        for (int i = 0; i < child.Value.Count; i++)
                        {
                            this.currentAllEvent.Push(child.Value[i]);
                        }
                    }
                }
            }
        }
        public void AddNewEvent(long eventId)
        {
            if (eventId != -1)
            {
                EventResult eventResult = EventResultFactory.GetEventResultById(eventId);
                this.currentAllEvent.Push(eventResult);
            }
        }

        public IEnumerator StartListenEvent()
        {
            while (true)
            {
                if (GameManager.Instance.GameScene == GameScene.Camp)
                    yield break;
                //Debug.Log("ÎÒ»¹ÔÚ¼àÌý");
                if (this.currentAllEvent.Count > 0)
                {
                    GameManager.Instance.PauseOrPlay(false);
                    GameManager.Instance.eventView.NewEvent();

                }
                yield return null;
            }
        }
    }
}