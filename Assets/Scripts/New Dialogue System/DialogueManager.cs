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

    public Sprite davidArt;

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
        DialogueParent.SetActive(false);
        characterImage.gameObject.SetActive(false);
        OnDialogueEnd?.Invoke();
    }

    // Starts the dialogue with given title and dialogue node
    public void StartDialogue(string title, DialogueNode node, Actor actor)
    {
        quests.SpeakToNPC(title, actor);
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
        typingCoroutine = StartCoroutine(TypeLines(node, actor));
    }



    // Typing effect for multiple dialogue lines and displaying responses after all lines
    IEnumerator TypeLines(DialogueNode node, Actor actor)
    {
        for (int i = 0; i < node.dialogueLines.Count; i++)
        {
            DialogueNode.DialogueLine line = node.dialogueLines[i];
            DialogBodyText.text = ""; // Clear current text
            DialogTitleText.text = line.speaker; // Set the speaker's name

            if (line.speaker != "David")
            {
                actor.characterAnim.SetTrigger("talk");
                characterImage.sprite = actor.gameObject.GetComponent<NPCInteraction>().characterArt;
            }
            if (line.speaker == "David")
            {
                actor.characterAnim.SetTrigger("stop");
                characterImage.sprite = davidArt;
            }

            // Typing effect for the current line
            yield return StartCoroutine(TypeLineText(line.text));

            // If it's the last line, skip waiting for the input to show response buttons
            if (i == node.dialogueLines.Count - 1)
            {
                // Show response buttons after typing the last line
                if (node.responses != null && node.responses.Count > 0)
                {
                    ShowResponseButtons(node, actor);
                }
                else
                {
                    HideDialogue(actor); // Hide dialogue if there are no responses
                }
            }
            else
            {
                // After pressing E, clear the body text for the next line
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
                DialogBodyText.text = ""; // Clear the text for the next line
            }
        }
    }

    IEnumerator TypeLineText(string text)
    {
        for (int charIndex = 0; charIndex < text.Length; charIndex++)
        {
            DialogBodyText.text += text[charIndex]; // Add each character

            // Allow skipping typing if any key is pressed (e.g., space to skip)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DialogBodyText.text = text; // Immediately display the full text
                yield break; // Exit the typing loop
            }

            // Wait for the textSpeed delay only if no key is pressed
            float delay = textSpeed;
            while (delay > 0)
            {
                // Check for key input each frame
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    DialogBodyText.text = text;
                    yield break;
                }
                delay -= Time.deltaTime;
                yield return null; // Yield to next frame to check input frequently
            }
        }
    }

    // Method to display response buttons after dialogue has been typed out
    private void ShowResponseButtons(DialogueNode node, Actor actor)
    {
        for (int i = 0; i < node.responses.Count; i++)
        {
            DialogueResponse response = node.responses[i];

            // Instantiate the button prefab and add it to the container
            GameObject buttonObj = Instantiate(responseButtonPrefab, responseButtonContainer);

            // Set the text with a number before the response text (e.g., "1. Response text")
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = $"{i + 1}. {response.responseText}";

            // Setup button to trigger SelectResponse when clicked
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(response, DialogTitleText.text, actor));
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
    public void SelectResponse(DialogueResponse response, string title, Actor actor)
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
            StartDialogue(title, response.nextNode, actor); // Start next dialogue
        }
        else
        {
            // If no follow-up node, end the dialogue
            HideDialogue(actor);
        }
    }

    // Hide the dialogue UI
    public void HideDialogue(Actor actor)
    {
        DialogueParent.SetActive(false);
        characterImage.gameObject.SetActive(false);
        if(actor.name == "Blaze")
        {

            if (!quests.hasBlaze2 && quests.hasBlaze1)
            {
                actor.characterAnim.SetTrigger("B1");
            }
            else if (!quests.hasBlaze3 && quests.hasBlaze2)
            {
                actor.characterAnim.SetTrigger("B2");
            }
            else if (!quests.hasBlaze4 && quests.hasBlaze3)
            {
                actor.characterAnim.SetTrigger("B3");
            }
            else if (!quests.hasBlaze5 && quests.hasBlaze4)
            {
                actor.characterAnim.SetTrigger("B4");
                Debug.Log("Bruh 4");
            }
            else if (!quests.hasBlaze6 && quests.hasBlaze5)
            {
                actor.characterAnim.SetTrigger("B5");
                Debug.Log("Bruh 5");
            }
            else if (!quests.hasBlaze7 && quests.hasBlaze6)
            {
                actor.characterAnim.SetTrigger("B6");
            }
            else if (!quests.hasBlaze8 && quests.hasBlaze7)
            {
                actor.characterAnim.SetTrigger("B7");
            }
            else if (!quests.hasBlaze9 && quests.hasBlaze8)
            {
                actor.characterAnim.SetTrigger("B8");
            }
            else if (!quests.hasBlaze10 && quests.hasBlaze9)
            {
                actor.characterAnim.SetTrigger("B9");
            }
            else if (!quests.hasBlaze11 && quests.hasBlaze10)
            {
                actor.characterAnim.SetTrigger("B10");
            }
            else if (!quests.hasBlaze12 && quests.hasBlaze11)
            {
                actor.characterAnim.SetTrigger("B11");
            }
            else if (quests.hasBlaze12)
            {
                actor.characterAnim.SetTrigger("B12");
            }
        }
        OnDialogueEnd?.Invoke();
    }

    // Show the dialogue UI
    private void ShowDialogue()
    {
        Debug.Log("Show Dialogue");
        DialogueParent.SetActive(true);
        characterImage.gameObject.SetActive(true);
    }

    // Check if dialogue is currently active
    public bool IsDialogueActive()
    {
        return DialogueParent.activeSelf;
    }
}