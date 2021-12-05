using Nameless.ConfigData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI
{
    public class NoteUI : MonoBehaviour
    {

        public Image[] imageNote;

        public void RefreshNotePage(List<NoteInfo> noteInfos)
        {
            for(int i = 0; i < imageNote.Length; i++)
            {
                this.imageNote[i].gameObject.SetActive(false);
            }

            for(int i = 0; i < noteInfos.Count; i++)
            {
                if(i < this.imageNote.Length)
                {
                    this.imageNote[i].sprite = noteInfos[i].noteImage;
                    this.imageNote[i].gameObject.SetActive(true);
                }
            }
        }
    }
}