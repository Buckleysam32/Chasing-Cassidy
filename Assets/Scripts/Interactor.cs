using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public Transform interactSource;
    public float interactRange;
    public LayerMask interactableLayer;
    public GameObject crosshair;
    public GameObject interactUI;
    public NPCInteraction blazeInteract;
    public Town1Quests questManager;

    void Update()
    {
        RaycastHit hit;
        Vector3 rayOrigin = interactSource.position;
        Vector3 rayDirection = interactSource.forward;

        if(Physics.Raycast(rayOrigin, rayDirection, out hit, interactRange, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            crosshair.SetActive(false);
            interactUI.SetActive(true);
            if(interactable != null)
            {
                if(hit.collider.name == "Door" && Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Interact Door");
                    interactable.UpdateDoor();
                }
                if (hit.collider.name == "Back Door" && Input.GetKeyDown(KeyCode.E) && !questManager.hasBlaze1 && questManager.hasSpokeToRed)
                {
                    Debug.Log("Interact Door");
                    blazeInteract.StartDialogue();
                    interactable.UpdateDoor();

                }
                if (hit.collider.name == "Chair" && Input.GetKeyDown(KeyCode.E))
                {
                    interactable.SitPlayer();
                    blazeInteract.StartDialogue();
                }
            }
        }
        else
        {
            crosshair.SetActive(true);
            interactUI.SetActive(false);
        }
    }
}
