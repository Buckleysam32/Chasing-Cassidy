using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject playerHud;

    public GameObject choiceIcon;

    public GameObject dialouge;

    public GameObject pauseMenu;

    public GameObject codex;

    public TextMeshProUGUI reputation;

    public GameObject transition;

    public GameObject t2Transition;

    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            isPaused = true;
            MouseLook mouseLook = FindObjectOfType<MouseLook>();
            mouseLook.canMove = false;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

/*        if (Input.GetKeyDown(KeyCode.Tab) && !isPaused)
        {
            isPaused = true;
            MouseLook mouseLook = FindObjectOfType<MouseLook>();
            mouseLook.canMove = false;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            codex.SetActive(true);
        }*/
    }

/*    public void CloseCodex()
    {
        isPaused = false;
        Debug.Log("Balls");
        codex.SetActive(false);
        MouseLook mouseLook = FindObjectOfType<MouseLook>();
        mouseLook.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }*/

    public void CloseMenu()
    {
        Debug.Log("Close Menu");
        isPaused = false;
        MouseLook mouseLook = FindObjectOfType<MouseLook>();
        mouseLook.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Close Game");
        Application.Quit();
    }

    public void goodChoice()
    {
        choiceIcon.GetComponent<Animator>().SetTrigger("good");
    }

    public void badChoice()
    {
        choiceIcon.GetComponent<Animator>().SetTrigger("bad");
    }

    private void Start()
    {
        playerHud.SetActive(true);
        dialouge.SetActive(false);
    }
}
