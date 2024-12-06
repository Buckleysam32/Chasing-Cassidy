using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public DisplayText textScript; // Reference to the DisplayText script on the canvas object
    public string npcName;
    public Sprite characterArt;
    public Image artObject;
    public TextMeshProUGUI nameText; // Reference to character's name
    public List<TextAsset> dialogueFiles; // List of text files for dialogues
    public bool dialogueInProgress = false; // Flag to track dialogue progress
    private Town1Quests town1Quests;

    public GameObject talkButton;
    public GameObject crossHair;

    public Vector3 startPosition;

    private bool inRange = false;
    public int currentDialogueIndex = 0; // Index to track the current dialogue

    public AudioClip[] textSounds = new AudioClip[0];

    private PlayerInteractor interactScript;

    public bool isLookingAtNPC = false;

    void Start()
    {
        town1Quests = FindObjectOfType<Town1Quests>();
        talkButton.SetActive(false);
        startPosition = transform.position;
        interactScript = FindObjectOfType<PlayerInteractor>();

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
            Debug.Log("In Range");
            inRange = true;
            if(this.gameObject.name == "Cassidy")
            {
                crossHair.SetActive(false);
                talkButton.SetActive(true);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            if (this.gameObject.name == "Cassidy")
            {
                crossHair.SetActive(true);
                talkButton.SetActive(false);
            }
        }
    }


    private void Update()
    {
        Debug.Log(isLookingAtNPC);
        if (inRange && Input.GetKeyDown(KeyCode.E) && !dialogueInProgress && isLookingAtNPC)
        {
            this.GetComponent<Actor>().SpeakTo(this.GetComponent<Actor>());
        }
        if(this.gameObject.name == "Cassidy" && inRange && Input.GetKeyDown(KeyCode.E) && !dialogueInProgress)
        {
            this.GetComponent<Actor>().SpeakTo(this.GetComponent<Actor>());
        }
    }

    public void StartDialogue()
    {
        this.GetComponent<Actor>().SpeakTo(this.GetComponent<Actor>());
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
