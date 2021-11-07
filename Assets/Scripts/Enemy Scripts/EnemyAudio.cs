using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
   
    private AudioSource audiosource;
    [SerializeField]
    private AudioClip scream_Clip, die_Clip;
    [SerializeField]
    private AudioClip[] attack_Clips;

    void Awake()
    {
        audiosource = GetComponent<AudioSource>();


    }
    public void PlayScreamSound()
    {
        audiosource.clip = scream_Clip;
        audiosource.Play();
    }
    public void PlayDeadSound()
    {
        audiosource.clip = die_Clip;
        audiosource.Play();
    }
    public void PlayAttackSound()
    {
        audiosource.clip = attack_Clips[Random.Range(0, attack_Clips.Length)];
        audiosource.Play();
    }

}
