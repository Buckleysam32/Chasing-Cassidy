using UnityEngine;
using System.Collections.Generic; // For List

public class Actor : MonoBehaviour
{
    public string Name;
    public List<Dialogue> Dialogues; // List of dialogues for the NPC
    private int currentDialogueIndex = 0; // To track the current dialogue index
    public Dialogue currentDialogue; // Rename the variable to avoid conflict with the Dialogue class name
    public MouseLook camScript;
    public PlayerMovement movement;
    public Rigidbody playerBody;
    private UIManager uIManager;
    public Animator characterAnim;

    private void Awake()
    {
        uIManager = FindAnyObjectByType<UIManager>();
        camScript = FindObjectOfType<MouseLook>();
        movement = FindObjectOfType<PlayerMovement>();
        playerBody = movement.gameObject.GetComponent<Rigidbody>();
        characterAnim = GetComponent<Animator>();

        // Ensure the current dialogue is set to the first one in the list at start
        if (Dialogues != null && Dialogues.Count > 0)
        {
            currentDialogue = Dialogues[currentDialogueIndex]; // Set the initial dialogue
        }
    }

    private void OnEnable()
    {
        DialogueManager.OnDialogueEnd += RevertControls; // Subscribe to the event
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueEnd -= RevertControls; // Unsubscribe from the event
    }

    // Trigger dialogue for this actor
    public void SpeakTo(Actor actor)
    {
        if (currentDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(Name, currentDialogue.RootNode, actor);
            this.GetComponent<NPCInteraction>().dialogueInProgress = true;
            // Disable player controls
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            camScript.enabled = false;
            playerBody.constraints = RigidbodyConstraints.FreezeAll;
            movement.canWalk = false;
            uIManager.playerHud.SetActive(false);

        }
    }

    // Function to increment and set the next dialogue
    public void NextDialogue()
    {
        if (Dialogues != null && Dialogues.Count > 0)
        {
            currentDialogueIndex++;
            Debug.Log("Next Dialogue");

            // If we exceed the list, we can loop back or stop at the last dialogue (optional)
            if (currentDialogueIndex >= Dialogues.Count)
            {
                currentDialogueIndex = Dialogues.Count - 1; // Stay at the last dialogue or wrap around
                // Or if you want it to loop, use:
                // currentDialogueIndex = 0;
            }

            // Set the current dialogue to the next one
            currentDialogue = Dialogues[currentDialogueIndex];
        }
    }

    // Method to revert controls after dialogue ends
    private void RevertControls()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
        camScript.enabled = true; // Enable camera control
        playerBody.constraints = RigidbodyConstraints.None;
        movement.canWalk = true;
        uIManager.playerHud.SetActive(true);
        characterAnim.SetTrigger("stop");
        this.GetComponent<NPCInteraction>().dialogueInProgress = false; // End dialogue state

    }
}