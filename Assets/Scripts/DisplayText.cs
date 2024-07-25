using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI nameComponent;
    public TextAsset textAsset;
    public string[] lines;
    public string[] speakers;
    public float textSpeed;
    private int index;
    public float shakeIntensity = 0.5f;
    public float shakeSpeed = 50f;
    private Vector3 originalPosition;
    public GameObject dialogueBox;
    public MouseLook playerMovement;
    public GameObject talkButton;
    public GameObject crossHair;

    public GameObject[] choiceButtons;
    public TextMeshProUGUI[] choiceTexts;

    private bool dialogueInProgress = false;
    private Dictionary<int, ChoiceData> choices;

    void Start()
    {
        textComponent.text = string.Empty;
        nameComponent.text = string.Empty;
        originalPosition = textComponent.rectTransform.localPosition;
        gameObject.SetActive(false);
    }

    private void LoadTextFromAsset()
    {
        string[] allLines = textAsset.text.Split('\n');
        lines = new string[allLines.Length];
        speakers = new string[allLines.Length];
        choices = new Dictionary<int, ChoiceData>();

        for (int i = 0; i < allLines.Length; i++)
        {
            string[] parts = allLines[i].Split(':');
            if (parts.Length >= 2)
            {
                speakers[i] = parts[0].Trim();
                lines[i] = parts[1].Trim();

                if (parts[1].Contains("[1]") && parts[1].Contains("[2]"))
                {
                    string[] options = parts[1].Split(new string[] { "[1]", "[2]" }, System.StringSplitOptions.None);
                    lines[i] = options[0].Trim();

                    ChoiceData choiceData = new ChoiceData();
                    choiceData.choices = new string[2];
                    choiceData.nextIndices = new int[2];

                    choiceData.choices[0] = options[1].Trim();
                    choiceData.choices[1] = options[2].Trim();

                    // These need to be set manually or using a specific pattern in the text file
                    // For now, we'll set them based on the example given
                    choiceData.nextIndices[0] = i + 1;
                    choiceData.nextIndices[1] = i + 3;

                    choices[i] = choiceData;

                    Debug.Log($"Parsed choices for line {i}: {choiceData.choices[0]} -> {choiceData.nextIndices[0]}, {choiceData.choices[1]} -> {choiceData.nextIndices[1]}");
                }
            }
        }
    }

    void Update()
    {
        if (dialogueInProgress && Input.GetKeyDown(KeyCode.E))
        {
            if (textComponent.text == lines[index])
            {
                if (choices.ContainsKey(index))
                {
                    DisplayChoices();
                }
                else
                {
                    NextLine();
                }
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
        if (dialogueInProgress)
        {
            talkButton.SetActive(false);
        }
    }

    public void StartDialogue(NPCInteraction npcInteraction)
    {
        if (!dialogueInProgress)
        {
            npcInteraction.dialogueInProgress = true;
            dialogueInProgress = true;

            if (textAsset != null)
            {
                LoadTextFromAsset();
            }
            else
            {
                Debug.LogWarning("Text file not assigned.");
            }

            if (lines == null || lines.Length == 0)
            {
                Debug.LogWarning("No dialogue lines loaded.");
                return;
            }

            gameObject.SetActive(true);
            index = 0;
            StartCoroutine(TypeLine());
        }
    }

    IEnumerator TypeLine()
    {
        UpdateSpeakerName();

        string lineToDisplay = lines[index].TrimEnd();

        foreach (char c in lineToDisplay.ToCharArray())
        {
            textComponent.text += c;
            textComponent.rectTransform.localPosition = originalPosition + GetShakeOffset();
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
        dialogueInProgress = false;
        crossHair.SetActive(true);

        foreach (var npc in FindObjectsOfType<NPCInteraction>())
        {
            npc.dialogueInProgress = false;
            npc.transform.position = npc.startPosition;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement.enabled = true;
    }

    private void UpdateSpeakerName()
    {
        nameComponent.text = speakers[index];
    }

    private Vector3 GetShakeOffset()
    {
        float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;
        float offsetY = Mathf.Cos(Time.time * shakeSpeed) * shakeIntensity;
        return new Vector3(offsetX, offsetY, 0f);
    }

    private void DisplayChoices()
    {
        for (int i = 0; i < choices[index].choices.Length; i++)
        {
            choiceButtons[i].SetActive(true);
            choiceTexts[i].text = choices[index].choices[i];
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerMovement.enabled = false;
    }

    public void SelectChoice(int choiceIndex)
    {
        Debug.Log($"Selected choice {choiceIndex} for line {index}");
        if (choices.ContainsKey(index) && choiceIndex < choices[index].nextIndices.Length)
        {
            index = choices[index].nextIndices[choiceIndex];
            HideChoices();
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            Debug.LogError($"Choice indices for line {index} or choice index {choiceIndex} not found.");
        }
    }

    private void HideChoices()
    {
        foreach (var button in choiceButtons)
        {
            button.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement.enabled = true;
    }

    private class ChoiceData
    {
        public string[] choices;
        public int[] nextIndices;
    }
}
