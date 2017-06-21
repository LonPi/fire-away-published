using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMeleeSound : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       SoundManager.instance.SpellPlayOneShot(SoundManager.instance.meleeSFX);
    }
}
