using Nameless.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{


    public void AttackEvent()
    {
        AudioManager.Instance.PlayAudio(transform, "Attack");
    }

    public void WalkEvent()
    {
        //AudioManager.Instance.PlayAudio(transform, "Walk");
    }

}
