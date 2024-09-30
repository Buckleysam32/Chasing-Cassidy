using UnityEngine;

public class Actor : MonoBehaviour
{
    public string Name;
    public Dialogue Dialogue;
    public MouseLook camScript;
    public PlayerMovement movement;
    public Rigidbody playerBody;

    private void OnEnable()
    {
        DialogueManager.OnDialogueEnd += RevertControls; // Subscribe to the event
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueEnd -= RevertControls; // Unsubscribe from the event
    }

    // Trigger dialogue for this actor
    public void SpeakTo()
    {
        DialogueManager.Instance.StartDialogue(Name, Dialogue.RootNode);
        this.GetComponent<NPCInteraction>().dialogueInProgress = true;
        // Disable player controls
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        camScript.enabled = false;
        playerBody.constraints = RigidbodyConstraints.FreezeAll;
        movement.canWalk = false;
    }

    // Method to revert controls after dialogue ends
    private void RevertControls()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
        camScript.enabled = true; // Enable camera control
        playerBody.constraints = RigidbodyConstraints.None;
        movement.canWalk = true;
        this.GetComponent<NPCInteraction>().dialogueInProgress = false; // End dialogue state
    }
}
