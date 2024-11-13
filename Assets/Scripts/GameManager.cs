using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int moralScore; // Player's moral score

    private UIManager uiManager;

    public bool isTown1 = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    // increase moral score
    public void IncreaseMoralScore()
    {
        UpdateReputation();
        uiManager.goodChoice();
        moralScore++;
        Debug.Log("Moral Score Increased: " + moralScore);
    }

    // decrease moral score
    public void DecreaseMoralScore()
    {
        UpdateReputation();
        uiManager.badChoice();
        moralScore--;
        Debug.Log("Moral Score Decreased: " + moralScore);
    }

    public void UpdateReputation()
    {
        if(moralScore == 0)
        {
            uiManager.reputation.text = "Reputation: Average";
        }
        if (moralScore > 0)
        {
            uiManager.reputation.text = "Reputation: Saint";
        }
        if (moralScore < 0)
        {
            uiManager.reputation.text = "Reputation: Devil";
        }
    }
}
