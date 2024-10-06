using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void PlayAudioClip(AK.Wwise.Event eventID, GameObject source)
    {
        AkSoundEngine.PostEvent(eventID.Id, source);
    }
}
