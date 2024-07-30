using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Town1Quests : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    public List<string> objectives = new List<string>();

    // Quest 1 checks
    public bool hasSpokeToCarson = false;
    public bool hasSpokeToAnnie = false;
    public bool hasSpokeToJed = false;
    public bool hasSpokeToMorris = false;
    public bool isDoneQuest1 = false;
    public NPCInteraction carsonScript;

    // Start is called before the first frame update
    void Start()
    {
        AddObjective("- Find Info at the saloon");
    }


    public void SpeakToNPC(string npcName, NPCInteraction npcInteraction)
    {

        switch (npcName)
        {
            case "Carson":
                if (!hasSpokeToCarson)
                {
                    hasSpokeToCarson = true;
                    RemoveObjective("- Find Info at the saloon");
                    AddObjective("- Speak to Annie");
                    AddObjective("- Speak to Jed");
                    AddObjective("- Speak to Morris");
                    npcInteraction.SetNextDialogue();
                }
                break;
            case "Annie":
                if (!hasSpokeToAnnie)
                {
                    hasSpokeToAnnie = true;
                    RemoveObjective("- Speak to Annie");
                    npcInteraction.SetNextDialogue();
                }
                if (hasSpokeToAnnie && hasSpokeToJed && hasSpokeToMorris && !isDoneQuest1)
                {
                    FinishQuestOne();
                }
                break;
            case "Jed":
                if (!hasSpokeToJed)
                {
                    hasSpokeToJed = true;
                    RemoveObjective("- Speak to Jed");
                    npcInteraction.SetNextDialogue();
                }
                if (hasSpokeToAnnie && hasSpokeToJed && hasSpokeToMorris && !isDoneQuest1)
                {
                    FinishQuestOne();
                }
                break;
            case "Morris":
                if (!hasSpokeToMorris)
                {
                    hasSpokeToMorris = true;
                    RemoveObjective("- Speak to Morris");
                    npcInteraction.SetNextDialogue();
                }
                if (hasSpokeToAnnie && hasSpokeToJed && hasSpokeToMorris && !isDoneQuest1)
                {
                    FinishQuestOne();
                }
                break;
            default:
                Debug.LogWarning("Unknown NPC name: " + npcName);
                break;
        }

        UpdateObjectiveText();
    }

    private void FinishQuestOne()
    {
        isDoneQuest1 = true;
        AddObjective("- Return to Carson");
        carsonScript.currentDialogueIndex ++;
    }

    public void AddObjective(string newObjective)
    {
        objectives.Add(newObjective);
        UpdateObjectiveText();
    }

    public void RemoveObjective(string objectiveToRemove)
    {
        objectives.Remove(objectiveToRemove);
        UpdateObjectiveText();
    }

    private void UpdateObjectiveText()
    {
        objectiveText.text = string.Join("\n", objectives);
    }
}
