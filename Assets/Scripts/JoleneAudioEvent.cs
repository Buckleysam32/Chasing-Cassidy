using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoleneAudioEvent : MonoBehaviour
{
    private AudioManager AM;

    [SerializeField]
    private AK.Wwise.Event footstepEvent;
    [SerializeField]
    private AK.Wwise.Event handlingEvent;
    [SerializeField]
    private AK.Wwise.Event gunshotEvent;
    [SerializeField]
    private AK.Wwise.Event bodyThudEvent;
 
    void Start()
    {
        AkSoundEngine.RegisterGameObj(gameObject);
        AM = FindObjectOfType<AudioManager>();
    }

    void FootstepSound()
    {
        AM.PlayAudioClip(footstepEvent, this.gameObject);
    }

    void HandlingSound()
    {
        AM.PlayAudioClip(handlingEvent, this.gameObject);
    }

    void GunshotSound()
    {
        AM.PlayAudioClip(gunshotEvent, this.gameObject);
    }

    void BodyThudSound()
    {
        AM.PlayAudioClip(bodyThudEvent, this.gameObject);
    }
}
