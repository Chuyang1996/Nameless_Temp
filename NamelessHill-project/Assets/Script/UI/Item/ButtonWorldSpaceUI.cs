using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless.UI
{
    public class ButtonWorldSpaceUI : Button
    {
        public bool isSelected = false;
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            switch (state)
            {
                case SelectionState.Normal:
                    this.isSelected = false;
                    break;
                case SelectionState.Pressed:
                    this.isSelected = true;
                    break;
                case SelectionState.Highlighted:
                    {
                       // Debug.LogError("light£¡£¡£¡");
                        this.isSelected = true;
                        break;
                    }
                case SelectionState.Disabled:
                    this.isSelected = false;
                    break;

            }
        }
    }
}