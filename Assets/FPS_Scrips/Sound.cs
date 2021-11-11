using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField]
    AudioSource sound;
    [SerializeField]
    AudioSource boss_sound;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] playSounds = GetComponents<AudioSource>();
        sound = playSounds[0];
        boss_sound = playSounds[1];
        sound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void  BossSound()
    {
        sound.Stop();
        boss_sound.Play();
    }
}
