using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nameless.ConfigData {
    public class EventResultData
    {
        public long id;
        public string name;
        public string descrption;
        public long conditionId;
        public string options;
        public string Image;


        public EventResultData(long id, string name, string descrption, long conditionId,string options,string Image)
        {
            this.id = id;
            this.name = name;
            this.descrption = descrption;
            this.conditionId = conditionId;
            this.options = options;
            this.Image = Image;
        }
        // Start is called before the first frame update
    }
}