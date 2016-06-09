using UnityEngine;
using System.Collections;

public class BossAnimation : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator != null)
        {
            animator.SetBool("Punching", false);
        }
    }
}
