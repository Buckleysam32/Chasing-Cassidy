using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class Interactable : MonoBehaviour
{
    private Animator myAnim;
    private bool open;
    private AudioManager AM;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        open = false;
        AM = FindObjectOfType<AudioManager>();
    }

    public void UpdateDoor()
    {
        if (open)
        {
            open = false;
            myAnim.SetTrigger("Shut");
            AM.PlayAudioClip(doorCloseEvent, this.gameObject);
            Debug.Log("shut");
        }
        else
        {
            open = true;
            myAnim.SetTrigger("Open");
            AM.PlayAudioClip(doorOpenEvent, this.gameObject);
            Debug.Log("open");
        }
    }

    //Assigning Wwise Events

    [SerializeField]
    private AK.Wwise.Event doorCloseEvent;

    [SerializeField]
    private AK.Wwise.Event doorOpenEvent;
}
