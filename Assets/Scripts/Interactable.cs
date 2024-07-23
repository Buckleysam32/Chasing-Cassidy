using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Animator myAnim;
    private bool open;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        open = false;
    }

    public void UpdateDoor()
    {
        if (open)
        {
            open = false;
            myAnim.SetTrigger("Shut");
            Debug.Log("shut");
        }
        else
        {
            open = true;
            myAnim.SetTrigger("Open");
            Debug.Log("open");
        }
    }
}
