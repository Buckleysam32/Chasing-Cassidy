using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;


public class GameData
{
    public int d_town;
    public int d_moralScore;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int moralScore; // Player's moral score

    private UIManager uiManager;

    public bool isTown1 = false;

    private string saveFilePath;

    public int town;


    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        LoadGame();

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

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            town = data.d_town;
            moralScore = data.d_moralScore;
        }
    }

    public void SaveGame()
    {
        GameData data = new GameData
        {
            d_town = town,
            d_moralScore = moralScore
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
    }

    public void ClearData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save data cleared.");
        }
        else
        {
            Debug.Log("No save data found to clear.");
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SaveGame();
            Debug.Log("Game Saved");
        }
    }
}
