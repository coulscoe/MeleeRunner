using UnityEngine;

public class ResetTriggers : StateMachineBehaviour
{
    public string[] triggersToReset;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (string trigger in triggersToReset)
        {
            animator.ResetTrigger(trigger);
        }
    }
}
