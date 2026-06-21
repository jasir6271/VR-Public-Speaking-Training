using UnityEngine;

public class TalkingReaction : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartTalking()
    {
        if (animator != null)
            animator.SetTrigger("TalkTrigger");
    }
}