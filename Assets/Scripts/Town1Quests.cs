using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AK.Wwise;

public class Town1Quests : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    public List<string> objectives = new List<string>();
    [SerializeField]
    private AK.Wwise.Event objectiveUpdateEvent;
    [SerializeField]
    private AK.Wwise.Event questCompleteEvent;

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

    public bool hasBlaze1 = false;
    public bool hasBlaze2 = false;
    public bool hasBlaze3 = false;
    public bool hasBlaze4 = false;
    public bool hasBlaze5 = false;
    public bool hasBlaze6 = false;
    public bool hasBlaze7 = false;
    public bool hasBlaze8 = false;
    public bool hasBlaze9 = false;
    public bool hasBlaze10 = false;
    public bool hasBlaze11 = false;
    public bool hasBlaze12 = false;

    public NPCInteraction carsonScript;
    public GameObject carsonPoint;
    public GameObject anniePoint;
    public GameObject jedPoint;
    public GameObject morrisPoint;
    public GameObject redPoint;
    public GameObject hangPoint;
    public GameObject digSitePoint;
    private MissionWaypoint waypointManager;
    public Actor carsonActor;
    public Actor blazeActor;
    public GameObject horse;
    public Transform horsePoint;

    public GameObject backDoor;

    private GameManager gm;

    public Interactable jailDoor;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        waypointManager = FindObjectOfType<MissionWaypoint>();
        if (gm.isTown1)
        {
            carsonPoint = waypointManager.waypoint1.img.gameObject;
            anniePoint = waypointManager.waypoint2.img.gameObject;
            jedPoint = waypointManager.waypoint3.img.gameObject;
            morrisPoint = waypointManager.waypoint4.img.gameObject;
            redPoint = waypointManager.waypoint5.img.gameObject;
        }
        if (!gm.isTown1)
        {
            hangPoint = waypointManager.waypoint1.img.gameObject;
            digSitePoint = waypointManager.waypoint2.img.gameObject;
        }
        carsonActor = GameObject.FindWithTag("Carson").GetComponent<Actor>();
        AddObjective("- Find Info at the saloon");
        AkSoundEngine.PostEvent("ObjectiveUpdate", gameObject);
    }


    public void SpeakToNPC(string npcName, Actor actor)
    {
        Debug.Log("testing");
        if (gm.isTown1)
        {
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
                        AkSoundEngine.PostEvent("ObjectiveUpdate", gameObject);
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
                        AkSoundEngine.PostEvent("ObjectiveUpdate", gameObject);
                        carsonPoint.SetActive(false);
                        carsonActor.NextDialogue();
                        redPoint.SetActive(true);
                        quest1TurnedIn = true;
                    }
                    if (hasSpokeToRed)
                    {
                        returnedToCarson2 = true;
                        RemoveObjective("- Return to Carson");
                        AkSoundEngine.PostEvent("QuestComplete", gameObject);
                        AddObjective("- Meet Blaze Adams");
                        carsonPoint.SetActive(false);
                        actor.NextDialogue();
                        backDoor.GetComponent<Interactable>().enabled = true;
                    }
                    break;
                case "Annie Brown":
                    if (!hasSpokeToAnnie)
                    {
                        hasSpokeToAnnie = true;
                        RemoveObjective("- Speak to Annie");
                        AkSoundEngine.PostEvent("ObjectiveUpdate", gameObject);
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
                        AkSoundEngine.PostEvent("ObjectiveUpdate", gameObject);
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
                        AkSoundEngine.PostEvent("ObjectiveUpdate", gameObject);
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
                        AkSoundEngine.PostEvent("ObjectiveUpdate", gameObject);
                        redPoint.SetActive(false);
                        carsonPoint.SetActive(true);
                        actor.NextDialogue();
                        carsonActor.NextDialogue();
                    }
                    break;
                case "Blaze":
                    if (!hasBlaze1)
                    {
                        blazeActor.NextDialogue();
                        hasBlaze1 = true;
                    }
                    else if (!hasBlaze2 && hasBlaze1)
                    {
                        blazeActor.NextDialogue();
                        hasBlaze2 = true;
                    }
                    else if (!hasBlaze3 && hasBlaze2)
                    {
                        blazeActor.NextDialogue();
                        hasBlaze3 = true;
                    }
                    else if (!hasBlaze4 && hasBlaze3)
                    {
                        blazeActor.NextDialogue();
                        hasBlaze4 = true;
                    }
                    else if (!hasBlaze5 && hasBlaze4)
                    {
                        blazeActor.NextDialogue();
                        hasBlaze5 = true;
                    }
                    else if (!hasBlaze6 && hasBlaze5)
                    {
                        blazeActor.NextDialogue();
                        hasBlaze6 = true;
                    }
                    else if (!hasBlaze7 && hasBlaze6)
                    {
                        blazeActor.NextDialogue();
                        hasBlaze7 = true;
                    }
                    else if (!hasBlaze8 && hasBlaze7)
                    {
                        blazeActor.NextDialogue();
                        hasBlaze8 = true;
                    }
                    else if (!hasBlaze9 && hasBlaze8)
                    {
                        blazeActor.NextDialogue();
                        hasBlaze9 = true;
                    }
                    else if (!hasBlaze10 && hasBlaze9)
                    {
                        blazeActor.NextDialogue();
                        hasBlaze10 = true;
                    }
                    else if (!hasBlaze11 && hasBlaze10)
                    {
                        blazeActor.NextDialogue();
                        hasBlaze11 = true;
                    }
                    else if (!hasBlaze12 && hasBlaze11)
                    {
                        blazeActor.NextDialogue();
                        hasBlaze12 = true;
                    }
                    break;
                default:
                    Debug.LogWarning("Unknown NPC name: " + npcName);
                    break;
            }
        }
        if (!gm.isTown1)
        {
            switch (npcName)
            {
                case "Sherrif":
                    Debug.Log("pennor");
                    jailDoor.enabled = true;
                    AddObjective("- Speak with the Hanged Man");
                    hangPoint.SetActive(true);
                    break;
                case "Hanging Man":
                    RemoveObjective("- Speak with the Hanged Man");
                    AddObjective("- Search the Digsite");
                    hangPoint.SetActive(false);
                    digSitePoint.SetActive(true);

                    break;
                default:
                    Debug.LogWarning("Unknown NPC name: " + npcName);
                    break;
            }
        }
        UpdateObjectiveText();
    }

    private void FinishQuestOne()
    {
        isDoneQuest1 = true;
        AddObjective("- Return to Carson");
        AkSoundEngine.PostEvent("ObjectiveUpdate", gameObject);
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
