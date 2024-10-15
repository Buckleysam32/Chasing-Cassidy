using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeAudioEvent : MonoBehaviour
{
    private AudioManager AM;

    [SerializeField]
    private AK.Wwise.Event gunshotEvent;
    [SerializeField]
    private AK.Wwise.Event dryfireEvent;
    [SerializeField]
    private AK.Wwise.Event handlingEvent;
    [SerializeField]
    private AK.Wwise.Event placeEvent;
    [SerializeField]
    private AK.Wwise.Event barrellrotateEvent;
    [SerializeField]
    private AK.Wwise.Event gunSlideEvent;
    [SerializeField]
    private AK.Wwise.Event bodyThudEvent;

    void Start()
    {
        AkSoundEngine.RegisterGameObj(gameObject);
        AM = FindObjectOfType<AudioManager>();
    }

    
    void GunshotSound()
    {
        AM.PlayAudioClip(gunshotEvent, this.gameObject);
    }

    void DryfireSound()
    {
        AM.PlayAudioClip(dryfireEvent, this.gameObject);
    }

    void HandlingSound()
    {
        AM.PlayAudioClip(handlingEvent, this.gameObject);
    }

    void PlaceSound()
    {
        AM.PlayAudioClip(placeEvent, this.gameObject);
    }

    void BarrellSound()
    {
        AM.PlayAudioClip(barrellrotateEvent, this.gameObject);
    }

    void SlideSound()
    {
        AM.PlayAudioClip(gunSlideEvent, this.gameObject);
    }

    void BodythudSound()
    {
        AM.PlayAudioClip(bodyThudEvent, this.gameObject);
    }
}
