using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public DisplayText textScript; // Reference to the DisplayText script on the canvas object
    public string npcName;
    public TextMeshProUGUI nameText; // Reference to character's name
    public TextAsset assignedTextFile; // Text file to be assigned in the Inspector
    public bool dialogueInProgress = false; // Flag to track dialogue progress

    public GameObject talkButton;
    public GameObject crossHair;

    //public Animator npcAnim;

    //public AudioClip[] textSounds = new AudioClip[0];

    public Vector3 startPosition;

    private bool inRange = false;
    void Start()
    {
        talkButton.SetActive(false);
        // Store the initial position of the object
        startPosition = transform.position;
        //npcAnim = this.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("in range");
            inRange = true;
            crossHair.SetActive(false);
            talkButton.SetActive(true);
            textScript.textAsset = assignedTextFile;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            Debug.Log("not in range");
            crossHair.SetActive(true);
            talkButton.SetActive(false);
            textScript.textAsset = null; // Clear the assigned text file when leaving the NPC
        }
    }

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E) && dialogueInProgress == false)
        {
            Debug.Log("Start talking");
            if (assignedTextFile != null && !dialogueInProgress)
            {
                nameText.text = npcName;
                talkButton.SetActive(false);
                crossHair.SetActive(false);
                textScript.StartDialogue(this); // Pass the current NPCInteraction instance
            }
            else
            {
                Debug.LogWarning("Text file not assigned in the Inspector or dialogue is already in progress.");
            }
        }
    }
}
