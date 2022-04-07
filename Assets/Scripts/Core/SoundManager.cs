
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    public static AudioClip attack1;

    static AudioSource audioSrc;

    void Start()
    {   
        instance = this;
        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip _sound)
    {
        audioSrc.PlayOneShot(attack1);
    }
}