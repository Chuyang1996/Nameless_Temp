using Nameless.Data;
using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI
{
    public class NoteBookView : MonoBehaviour
    {
        public Button closeBtn;

        public Button leftBtn;
        public Button rightBtn;

        public GameObject leftPage;
        public GameObject rightPage;

        private List<GameObject> noteUIs = new List<GameObject>();

        private List<NotePage> notePages = new List<NotePage>();
        private int IndexPage
        {
            set
            {
                
                if(value < 0 || this.notePages.Count == 0)
                {
                    indexPage = 0;
                }
                else if(value > this.notePages.Count)
                {
                    indexPage = this.notePages.Count - 1;
                }
                else
                {
                    indexPage = value;
                }
            }
            get
            {
                return indexPage;
            }
        }
        private int indexPage;
        // Start is called before the first frame update
        void Start()
        {
            this.leftBtn.onClick.AddListener(this.PageToLeft);
            this.rightBtn.onClick.AddListener(this.PageToRight);
            this.closeBtn.onClick.AddListener(this.ExitNoteBook);
        }

        public void InitNoteBook()
        {
            this.gameObject.SetActive(true);
            this.IndexPage = 0;
            this.notePages.Clear();
            foreach(var child in NoteManager.Instance.notePageDic)
            {
                this.notePages.Add(child.Value);
            }
            this.ResetNotePage();
        }
        public void ExitNoteBook()
        {
            this.gameObject.SetActive(false);
        }
        public void PageToLeft()
        {
            this.IndexPage--;
            this.ResetNotePage();
        }

        public void PageToRight()
        {
            this.IndexPage++;
            this.ResetNotePage();
        }

        public void ResetNotePage()
        {
            for (int i = 0; i < this.noteUIs.Count; i++)
                DestroyImmediate(this.noteUIs[i].gameObject);
            this.noteUIs.Clear();

            if ( this.IndexPage < this.notePages.Count)
            {
                GameObject leftObj = Instantiate(Resources.Load(NoteManager.Instance.loadPath + this.notePages[this.IndexPage].noteUIname) as GameObject, this.leftPage.transform);
                leftObj.transform.localPosition = new Vector3(0, 0, 0);
                leftObj.GetComponent<NoteUI>().RefreshNotePage(this.notePages[this.IndexPage].noteInfos);
                this.noteUIs.Add(leftObj);
            }

            if((this.IndexPage + 1)< this.notePages.Count)
            {
                GameObject rightObj = Instantiate(Resources.Load(NoteManager.Instance.loadPath + this.notePages[this.IndexPage + 1].noteUIname) as GameObject, this.rightPage.transform);
                rightObj.transform.localPosition = new Vector3(0, 0, 0);
                rightObj.GetComponent<NoteUI>().RefreshNotePage(this.notePages[this.IndexPage + 1].noteInfos);
                this.noteUIs.Add(rightObj);
            }

        }

    }
}