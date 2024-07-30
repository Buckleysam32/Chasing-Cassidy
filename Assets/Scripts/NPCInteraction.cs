using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public DisplayText textScript; // Reference to the DisplayText script on the canvas object
    public string npcName;
    public TextMeshProUGUI nameText; // Reference to character's name
    public List<TextAsset> dialogueFiles; // List of text files for dialogues
    public bool dialogueInProgress = false; // Flag to track dialogue progress
    private Town1Quests town1Quests;

    public GameObject talkButton;
    public GameObject crossHair;

    public Vector3 startPosition;

    private bool inRange = false;
    public int currentDialogueIndex = 0; // Index to track the current dialogue

    void Start()
    {
        town1Quests = FindObjectOfType<Town1Quests>();
        talkButton.SetActive(false);
        startPosition = transform.position;

        // Initialize dialogue if there are files
        if (dialogueFiles.Count > 0)
        {
            textScript.textAsset = dialogueFiles[0];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            crossHair.SetActive(false);
            talkButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            crossHair.SetActive(true);
            talkButton.SetActive(false);
        }
    }

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E) && !dialogueInProgress)
        {
            if (currentDialogueIndex < dialogueFiles.Count)
            {
                nameText.text = npcName;
                talkButton.SetActive(false);
                crossHair.SetActive(false);
                textScript.textAsset = dialogueFiles[currentDialogueIndex]; // Ensure correct text asset is used
                textScript.StartDialogue(this);
                town1Quests.SpeakToNPC(npcName, this); // Ensure the NPC's status is updated
            }
            else
            {
                Debug.LogWarning("No more dialogues available for this NPC.");
            }
        }
    }


    public void SetNextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogueFiles.Count)
        {
            textScript.textAsset = dialogueFiles[currentDialogueIndex];
        }
        else
        {
            Debug.LogWarning("No more dialogues available for this NPC.");
        }
    }
}
