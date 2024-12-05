using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    public Sprite banditArt;
    public Sprite joleneArt;

    public AudioClip[] textSounds = new AudioClip[0];
    public AudioSource AS;

    public Animator barrelAnim;
    public Animator gateAnim;

    public Animator banditAnim;
    public Animator bandit2Anim;


    public bool joleneBad = false;
    public bool joleneGood = false;

    public Actor jolActor;

    public float skipDelay = 3f;
    public float delayCounter = 0;

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

    public void StartDialogue(string title, DialogueNode node, Actor actor)
    {
        quests.SpeakToNPC(title, actor);
        textSounds = actor.textSounds;
        Debug.Log(title);
        ShowDialogue();

        DialogTitleText.text = title;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        ClearResponseButtons();

        typingCoroutine = StartCoroutine(TypeLines(node, actor));
    }



    IEnumerator TypeLines(DialogueNode node, Actor actor)
    {
        for (int i = 0; i < node.dialogueLines.Count; i++)
        {
            DialogueNode.DialogueLine line = node.dialogueLines[i];
            DialogBodyText.text = ""; // Clear current text
            DialogTitleText.text = line.speaker; // Set the speaker's name
            if (line.speaker == "David")
            {
                actor.characterAnim.SetBool("idle", true);
                actor.characterAnim.SetBool("talking", false);
                characterImage.sprite = davidArt;
            }
            else if (line.speaker == "Bandits")
            {
                actor.characterAnim.SetBool("idle", false);
                actor.characterAnim.SetBool("talking", true);
                characterImage.sprite = banditArt;
            }
            else if (line.speaker == "Jolene")
            {
                actor.characterAnim.SetBool("idle", false);
                actor.characterAnim.SetBool("talking", true);
                characterImage.sprite = joleneArt;
            }
            else if (line.speaker != "David" && line.speaker != "Jolene" && line.speaker != "Bandits")
            {
                actor.characterAnim.SetBool("idle", false);
                actor.characterAnim.SetBool("talking", true);
                characterImage.sprite = actor.gameObject.GetComponent<NPCInteraction>().characterArt;
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
                    actor.characterAnim.SetBool("idle", true);
                    actor.characterAnim.SetBool("talking", false);
                }
                else
                {
                    HideDialogue(actor); // Hide dialogue if there are no responses
                    actor.characterAnim.SetBool("idle", true);
                    actor.characterAnim.SetBool("talking", false);
                }
            }
            else
            {
                // After pressing E, clear the body text for the next line
                if (DialogBodyText.text == line.text && delayCounter >= skipDelay)
                {
                    yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                    DialogBodyText.text = ""; // Clear the text for the next line
                }
            }
        }
    }

    IEnumerator TypeLineText(string text)
    {
        for (int charIndex = 0; charIndex < text.Length; charIndex++)
        {
            DialogBodyText.text += text[charIndex]; // Add each character

            int randomI = Random.Range(0, textSounds.Length - 1);

            AudioClip randomClip = textSounds[randomI];

            if (randomClip != null && AS != null)
            {
                AS.PlayOneShot(randomClip);
                //Debug.Log("Played: " + randomClip + "from: " + AS);
            }


            float delay = textSpeed;
            while (delay > 0)
            {
                // Check for key input each frame
                if (Input.GetMouseButtonDown(1))
                {
                    // Make the typing finish instantly
                    DialogBodyText.text = text;
                    yield break;
                }
                delay -= Time.deltaTime;
                yield return null; // Yield to next frame to check input frequently
            }
        }
    }

    private void Update()
    {
        if (delayCounter <= skipDelay)
        {
            delayCounter += Time.deltaTime;
        }
    }

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

    private void ClearResponseButtons()
    {
        foreach (Transform child in responseButtonContainer)
        {
            Destroy(child.gameObject);
        }
    }



    public void SelectResponse(DialogueResponse response, string title, Actor actor)
    {
        if(actor.Name == "Hanging Man")
        {
            if (response.isGoodResponse)
            {
               FindObjectOfType<GameManager>().IncreaseMoralScore();
                
            }
            if (response.isBadResponse)
            {
                FindObjectOfType<GameManager>().DecreaseMoralScore();
                barrelAnim.SetTrigger("Move");
            }
        }

        if(actor.Name == "Bandit 1")
        {
            gateAnim.SetTrigger("Open");
        }

        if (actor.name == "Jolene")
        {
            if(!quests.hasJol2 && quests.hasJol1)
            {
                banditAnim.SetTrigger("Walk");
            }
            else if (!quests.hasJol3 && quests.hasJol2 && response.isGoodResponse)
            {
                joleneGood = true;
                jolActor.currentDialogue = jolActor.Dialogues[3];
                banditAnim.SetTrigger("Miss");
            }
            else if(!quests.hasJol4 && quests.hasJol3 && joleneGood)
            {
                bandit2Anim.SetTrigger("ShootJol");
            }
            else if (!quests.hasJol3 && quests.hasJol2 && response.isBadResponse)
            {
                joleneBad = true;
                jolActor.currentDialogue = jolActor.Dialogues[2];
                banditAnim.SetTrigger("PlayerShoot");
            }
            else if (!quests.hasJol4 && quests.hasJol3 && joleneBad)
            {
                FindAnyObjectByType<UIManager>().t2Transition.SetActive(true);
            }

        }

        if(actor.name == "Cassidy")
        {
            if (!quests.hasCas2 && quests.hasCas1 )
            {
                actor.characterAnim.SetTrigger("PullGun");
            }

            if(!quests.hasCas3 && quests.hasCas2)
            {
                if(FindObjectOfType<GameManager>().moralScore <= 0)
                {
                    actor.StartTrans();
                    Debug.Log("Start Ending Transition");
                }
            }

            if (quests.hasCas4)
            {
                actor.StartTrans();
                Debug.Log("Start Ending Transition");
            }
        }

        // Update the moral score based on the response
        if (response.isGoodResponse && actor.name != "Jolene")
        {
            FindObjectOfType<GameManager>().IncreaseMoralScore();
        }
        if (response.isBadResponse && actor.name != "Jolene")
        {
            FindObjectOfType<GameManager>().DecreaseMoralScore();
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

    private void ShowDialogue()
    {
        Debug.Log("Show Dialogue");
        DialogueParent.SetActive(true);
        characterImage.gameObject.SetActive(true);
    }

    public bool IsDialogueActive()
    {
        return DialogueParent.activeSelf;
    }
}