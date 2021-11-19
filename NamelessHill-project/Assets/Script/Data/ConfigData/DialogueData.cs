using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.ConfigData {
    public class DialogueData
    {
        public long id;
        public string dialogue;
        public string condition;
        public bool isAuto;
        public float waitTime;
        public float speedPos;
        public float zoom;
        public float zoomSpeed;
        public long nextId;
        public long nextPawn;

        public DialogueData(long id, string dialogue,string condition,bool isAuto, float speedPos, float zoom, float zoomSpeed, float waitTime,long nextId,long nextPawn)
        {
            this.id = id;
            this.dialogue = dialogue;
            this.condition = condition;
            this.isAuto = isAuto;
            this.waitTime = waitTime;
            this.speedPos = speedPos;
            this.zoom = zoom;
            this.zoomSpeed = zoomSpeed;
            this.nextId = nextId;
            this.nextPawn = nextPawn;
        }
    }
}
