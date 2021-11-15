using Nameless.Agent;
using Nameless.Data;
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


        public void CheckRelateAmmoEvent(int cost)
        {
            foreach(var child in this.eventTriggerDic)
            {
                if(child.Key.type == EventTriggerType.AmmoLess)
                {
                    int lastAmmo = GameManager.Instance.totalAmmo;
                    int afterAmmo = GameManager.Instance.totalAmmo + cost;
                    if(afterAmmo < (int)child.Key.parameter  && (int)child.Key.parameter <= lastAmmo)
                    {
                       for(int i = 0; i < child.Value.Count; i++)
                        {
                            this.currentAllEvent.Push(child.Value[i]);
                        }
                    }
                }
            }
        }

        public void CheckRelateMedicineEvent(int cost)
        {
            foreach (var child in this.eventTriggerDic)
            {
                if (child.Key.type == EventTriggerType.MedicineLess)
                {
                    int lastMedicine = GameManager.Instance.totalMedicine;
                    int afterMedicine = GameManager.Instance.totalMedicine + cost;
                    if (afterMedicine < (int)child.Key.parameter && (int)child.Key.parameter <= lastMedicine)
                    {
                        for (int i = 0; i < child.Value.Count; i++)
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
                    if (afterEnemies < (int)child.Key.parameter && (int)child.Key.parameter <= lastEnemies)
                    {
                        for (int i = 0; i < child.Value.Count; i++)
                        {
                            this.currentAllEvent.Push(child.Value[i]);
                        }
                    }
                }
            }
        }

        public void CheckRelateTimeEvent(int currentTime)
        {
            foreach (var child in this.eventTriggerDic)
            {
                if (child.Key.type == EventTriggerType.TimePass)
                {
                    int passTime = GameManager.Instance.totalTime - currentTime;
                    if (((float)passTime )== child.Key.parameter)
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
                //Debug.Log("ÎÒ»¹ÔÚ¼àÌý");
                if (this.currentAllEvent.Count > 0)
                {
                    GameManager.Instance.eventView.NewEvent();
                    GameManager.Instance.PauseOrPlay(false);
                }
                yield return null;
            }
        }
    }
}