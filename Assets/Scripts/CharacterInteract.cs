using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteract : MonoBehaviour, IInteractable
{
    public GameObject textBox;

    public void Interact()
    {
        Debug.Log("Blah Blah Blah, character dialogue");
        textBox.SetActive(true);
    }
}
