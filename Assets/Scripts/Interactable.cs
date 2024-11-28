using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class Interactable : MonoBehaviour
{
    private Animator myAnim;
    private bool open;
    private AudioManager AM;
    public GameObject player;
    public Transform chairPos;
    public Transform campPos;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        open = false;
        AM = FindObjectOfType<AudioManager>();
        player = GameObject.FindGameObjectWithTag("Player");
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


    public void SitPlayer()
    {
        Debug.Log("Sit Player");
        player.GetComponent<PlayerMovement>().enabled = false;
        player.transform.position = chairPos.position;
        FindObjectOfType<MouseLook>().lookAtBlaze = true;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //Start Dialogue
    }

    public void CampfireSit()
    {
        Debug.Log("Sit Player");
        player.GetComponent<PlayerMovement>().enabled = false;
        player.transform.position = campPos.position;
        FindObjectOfType<MouseLook>().lookAtJol = true;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    //Assigning Wwise Events

    [SerializeField]
    private AK.Wwise.Event doorCloseEvent;

    [SerializeField]
    private AK.Wwise.Event doorOpenEvent;
}
