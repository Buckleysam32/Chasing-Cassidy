using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int moralScore; // Player's moral score

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

    // increase moral score
    public void IncreaseMoralScore()
    {
        moralScore++;
        Debug.Log("Moral Score Increased: " + moralScore);
    }

    // decrease moral score
    public void DecreaseMoralScore()
    {
        moralScore--;
        Debug.Log("Moral Score Decreased: " + moralScore);
    }
}
