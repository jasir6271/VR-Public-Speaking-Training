using UnityEngine;

public class AudienceClap : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayClap()
    {
        if (animator != null)
        {
            animator.SetTrigger("ClapTrigger");
        }
    }
}