using UnityEngine;

public class AudienceReaction : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayBored()
    {
        anim.SetTrigger("BoredTrigger");
    }
}