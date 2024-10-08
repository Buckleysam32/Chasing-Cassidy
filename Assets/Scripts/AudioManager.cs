using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class AudioManager : MonoBehaviour
{
    private GameObject player;
    static public AK.Wwise.Event objectiveUpdate;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        uint eventId = AkSoundEngine.PostEvent("Outside", gameObject);
        AkSoundEngine.SetState("Town1", "Outside");
    }

    public void PlayAudioClip(AK.Wwise.Event eventID, GameObject source)
    {
        AkSoundEngine.PostEvent(eventID.Id, source);
    }

    //States

    static public void SetAreaSaloon()
    {
        AkSoundEngine.SetState("Town1", "Saloon");
    }

    static public void SetAreaGrocer()
    {
        AkSoundEngine.SetState("Town1", "Grocer");
    }

    static public void SetAreaGunsmith()
    {
        AkSoundEngine.SetState("Town1", "Gunsmith");
    }

    static public void SetAreaButcher()
    {
        AkSoundEngine.SetState("Town1", "Butcher");
    }

    static public void SetAreaOutside()
    {
        AkSoundEngine.SetState("Town1", "Outside");
    }

    
}
