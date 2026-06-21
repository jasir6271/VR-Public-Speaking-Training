using UnityEngine;

public class AudienceGreeting : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip goodMorningClip;
    public AudioClip helloClip;
    public AudioClip hiClip;
    public AudioClip clapClip;

    // 🔥 NEW RESPONSES
    public AudioClip thankYouReplyClip;
    public AudioClip howAreYouClip;
    public AudioClip nameReplyClip;

    public void ReplyGoodMorning()
    {
        PlayClip(goodMorningClip);
    }

    public void ReplyHello()
    {
        PlayClip(helloClip);
    }

    public void ReplyHi()
    {
        PlayClip(hiClip);
    }

    public void PlayClap()
    {
        PlayClip(clapClip);
    }

    // 🔥 NEW FUNCTIONS

    public void ReplyThankYou()
    {
        PlayClip(thankYouReplyClip);
    }

    public void ReplyHowAreYou()
    {
        PlayClip(howAreYouClip);
    }

    public void ReplyName()
    {
        PlayClip(nameReplyClip);
    }

    // ✅ Common function
    void PlayClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}