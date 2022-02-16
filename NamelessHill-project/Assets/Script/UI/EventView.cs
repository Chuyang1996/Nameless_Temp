using Nameless.Data;
using Nameless.DataUI;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI
{
    public class EventView : MonoBehaviour
    {
        public Image pictureImage;
        public Text content;
        public GameObject optionContent;
        public EventOptionUI eventOptionTemplate;

        private List<EventOptionUI> eventOptionUIs = new List<EventOptionUI>();
        // Start is called before the first frame update

        public void NewEvent()
        {
            if (EventTriggerManager.Instance.currentAllEvent.Count > 0)
            {
                this.gameObject.SetActive(true);
                EventResult eventResult = EventTriggerManager.Instance.currentAllEvent.Pop();
                FrontManager.Instance.UpdateEventForAllPlayers(eventResult.id);
                this.ResetPanel(eventResult);
            }
            else
            {
                this.gameObject.SetActive(false);
                GameManager.Instance.PauseOrPlay(true);
            }
        }

        void ResetPanel(EventResult eventResult)
        {
            for (int i = 0; i < this.eventOptionUIs.Count; i++)
                DestroyImmediate(this.eventOptionUIs[i].gameObject);
            this.eventOptionUIs.Clear();

            for(int i = 0; i < eventResult.options.Count; i++)
            {
                GameObject eventOption = Instantiate(this.eventOptionTemplate.gameObject, this.optionContent.transform);
                eventOption.SetActive(true);
                eventOption.GetComponent<EventOptionUI>().InitOption(eventResult.options[i]);
                this.eventOptionUIs.Add(eventOption.GetComponent<EventOptionUI>());

            }
            this.pictureImage.sprite = eventResult.eventImage;
            this.content.text = eventResult.descrption;


            LayoutRebuilder.ForceRebuildLayoutImmediate(this.optionContent.GetComponent<RectTransform>());
        }
    }
}