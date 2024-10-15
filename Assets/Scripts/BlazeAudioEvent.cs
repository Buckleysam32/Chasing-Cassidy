using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeAudioEvent : MonoBehaviour
{
    private AudioManager AM;

    [SerializeField]
    private AK.Wwise.Event gunshotEvent;

    void Start()
    {
        AkSoundEngine.RegisterGameObj(gameObject);
        AM = FindObjectOfType<AudioManager>();
    }

    
    void GunshotSound()
    {
        AM.PlayAudioClip(gunshotEvent, this.gameObject);
    }
}
