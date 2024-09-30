using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // UI references
    public GameObject DialogueParent; // Main container for dialogue UI
    public TextMeshProUGUI DialogTitleText, DialogBodyText; // Text components for title and body
    public GameObject responseButtonPrefab; // Prefab for generating response buttons
    public Transform responseButtonContainer; // Container to hold response buttons
    public Image characterImage;

    public Town1Quests quests;

    public float textSpeed = 0.05f; // Speed of typing effect

    private Coroutine typingCoroutine; // To manage the typing coroutine
    private int currentLineIndex; // Index for current dialogue line

    public delegate void DialogueEndHandler();
    public static event DialogueEndHandler OnDialogueEnd;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of DialogueManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        quests = FindObjectOfType<Town1Quests>();

        // Initially hide the dialogue UI
        HideDialogue();
    }

    // Starts the dialogue with given title and dialogue node
    public void StartDialogue(string title, DialogueNode node)
    {
        quests.SpeakToNPC(title);
        Debug.Log(title);

        // Display the dialogue UI
        ShowDialogue();

        // Set dialogue title
        DialogTitleText.text = title;

        // Stop any previous typing coroutine
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Clear any previous response buttons
        ClearResponseButtons();

        // Start typing the new lines from the dialogue node
        typingCoroutine = StartCoroutine(TypeLines(node));
    }



    // Typing effect for multiple dialogue lines and displaying responses after all lines
    IEnumerator TypeLines(DialogueNode node)
    {
        // Loop through each line of dialogue
        for (int i = 0; i < node.dialogueLines.Count; i++)
        {
            DialogueNode.DialogueLine line = node.dialogueLines[i]; // Get the current line with speaker
            DialogBodyText.text = ""; // Clear current text
            DialogTitleText.text = line.speaker; // Set the speaker's name
            characterImage.sprite = line.image;
            bool skipTyping = false;

            // Type each character one by one
            foreach (char c in line.text.ToCharArray())
            {
                DialogBodyText.text += c; // Add one character at a time
                yield return new WaitForSeconds(textSpeed); // Wait between each character

                // If the player presses the space key, skip typing and display the full text immediately
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    DialogBodyText.text = line.text;
                    skipTyping = true;
                    break;
                }
            }

            // Wait for the player to press E to proceed to the next line
            if (i != node.dialogueLines.Count - 1)
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }

            // If this is the last line of dialogue, check for responses
            if (i == node.dialogueLines.Count - 1)
            {
                // Check if there are responses for this dialogue node
                if (node.responses != null && node.responses.Count > 0)
                {
                    Debug.Log("Show Buttons");
                    ShowResponseButtons(node);
                }
                else
                {
                    // No responses, so the dialogue ends or goes to the next dialogue
                    HideDialogue(); // Or any other logic for continuing dialogue
                }
            }
        }
    }

    // Method to display response buttons after dialogue has been typed out
    private void ShowResponseButtons(DialogueNode node)
    {
        // Create and setup response buttons based on current dialogue node
        foreach (DialogueResponse response in node.responses)
        {
            GameObject buttonObj = Instantiate(responseButtonPrefab, responseButtonContainer);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;

            // Setup button to trigger SelectResponse when clicked
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(response, DialogTitleText.text));
        }
    }

    // Clear existing response buttons
    private void ClearResponseButtons()
    {
        foreach (Transform child in responseButtonContainer)
        {
            Destroy(child.gameObject);
        }
    }

    // Handles response selection and triggers next dialogue node
    public void SelectResponse(DialogueResponse response, string title)
    {

        // Update the moral score based on the response
        if (response.isGoodResponse)
        {
            GameManager.Instance.IncreaseMoralScore();
        }
        if (response.isBadResponse)
        {
            GameManager.Instance.DecreaseMoralScore();
        }

        // Check if there's a follow-up node
        if (!response.nextNode.IsLastNode())
        {
            StartDialogue(title, response.nextNode); // Start next dialogue
        }
        else
        {
            // If no follow-up node, end the dialogue
            HideDialogue();
        }
    }

    // Hide the dialogue UI
    public void HideDialogue()
    {
        DialogueParent.SetActive(false);
        characterImage.gameObject.SetActive(false);
        OnDialogueEnd?.Invoke();
    }

    // Show the dialogue UI
    private void ShowDialogue()
    {
        DialogueParent.SetActive(true);
        characterImage.gameObject.SetActive(true);
    }

    // Check if dialogue is currently active
    public bool IsDialogueActive()
    {
        return DialogueParent.activeSelf;
    }
}
