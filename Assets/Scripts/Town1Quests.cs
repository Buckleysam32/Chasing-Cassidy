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
    public bool quest1TurnedIn = false;
    public bool returnedToCarson = false;
    public bool returnedToCarson2 = false;
    public bool hasSpokeToRed = false;
    public NPCInteraction carsonScript;
    public GameObject carsonPoint;
    public GameObject anniePoint;
    public GameObject jedPoint;
    public GameObject morrisPoint;
    public GameObject redPoint;
    private MissionWaypoint waypointManager;
    public Actor carsonActor;
    public GameObject horse;
    public Transform horsePoint;

    // Start is called before the first frame update
    void Start()
    {
        waypointManager = FindObjectOfType<MissionWaypoint>();
        carsonPoint = waypointManager.waypoint1.img.gameObject;
        anniePoint = waypointManager.waypoint2.img.gameObject;
        jedPoint = waypointManager.waypoint3.img.gameObject;
        morrisPoint = waypointManager.waypoint4.img.gameObject;
        redPoint = waypointManager.waypoint5.img.gameObject;
        carsonActor = GameObject.FindWithTag("Carson").GetComponent<Actor>();
        AddObjective("- Find Info at the saloon");
    }


    public void SpeakToNPC(string npcName, Actor actor)
    {
        Debug.Log("testing");
        switch (npcName)
        {
            case "Carson Smith":
                if (!hasSpokeToCarson)
                {
                    hasSpokeToCarson = true;
                    RemoveObjective("- Find Info at the saloon");
                    AddObjective("- Speak to Annie");
                    AddObjective("- Speak to Jed");
                    AddObjective("- Speak to Morris");
                    carsonPoint.SetActive(false);
                    anniePoint.SetActive(true);
                    jedPoint.SetActive(true);
                    morrisPoint.SetActive(true);
                    actor.NextDialogue();
                }
                if (isDoneQuest1 && !quest1TurnedIn)
                {
                    RemoveObjective("- Return to Carson");
                    AddObjective("- Ride Horse to Red's Property");
                    AddObjective("- Speak to Red");
                    carsonPoint.SetActive(false);
                    carsonActor.NextDialogue();
                    redPoint.SetActive(true);
                    quest1TurnedIn = true;
                }
                if(hasSpokeToRed)
                {
                    returnedToCarson2 = true;
                    RemoveObjective("- Return to Carson");
                    AddObjective("- Meet Blaze Adams");
                    carsonPoint.SetActive(false);
                    actor.NextDialogue();
                }
                break;
            case "Annie Brown":
                if (!hasSpokeToAnnie)
                {
                    hasSpokeToAnnie = true;
                    RemoveObjective("- Speak to Annie");
                    anniePoint.SetActive(false);
                    actor.NextDialogue();
                }
                if (hasSpokeToAnnie && hasSpokeToJed && hasSpokeToMorris && !isDoneQuest1)
                {
                    FinishQuestOne();
                }
                break;
            case "Jed Harris":
                if (!hasSpokeToJed)
                {
                    hasSpokeToJed = true;
                    RemoveObjective("- Speak to Jed");
                    jedPoint.SetActive(false);
                    actor.NextDialogue();
                }
                if (hasSpokeToAnnie && hasSpokeToJed && hasSpokeToMorris && !isDoneQuest1)
                {
                    FinishQuestOne();
                }
                break;
            case "Morris Hill":
                if (!hasSpokeToMorris)
                {
                    hasSpokeToMorris = true;
                    RemoveObjective("- Speak to Morris");
                    morrisPoint.SetActive(false);
                    actor.NextDialogue();
                }
                if (hasSpokeToAnnie && hasSpokeToJed && hasSpokeToMorris && !isDoneQuest1)
                {
                    FinishQuestOne();
                }
                break;
            case "Red":
                if (!hasSpokeToRed)
                {
                    hasSpokeToRed = true;
                    RemoveObjective("- Ride Horse to Red's Property");
                    RemoveObjective("- Speak to Red");
                    AddObjective("- Return to Carson");
                    redPoint.SetActive(false);
                    carsonPoint.SetActive(true);
                    actor.NextDialogue();
                    carsonActor.NextDialogue();
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
        carsonPoint.SetActive(true);
        carsonActor.NextDialogue();
        horse.transform.position = horsePoint.position;
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
