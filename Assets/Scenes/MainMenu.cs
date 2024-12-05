using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject cam;
    [SerializeField] private Quaternion mainRotation;
    [SerializeField] private Quaternion settingsRotation;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void NewGame()
    {
        AkSoundEngine.PostEvent("UI", this.gameObject);
        gameManager.ClearData();
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(gameManager.town);
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        StartCoroutine(MoveOverSeconds(cam, settingsRotation, 1f, settingsMenu));
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        StartCoroutine(MoveOverSeconds(cam, mainRotation, 1f, mainMenu));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator MoveOverSeconds(GameObject objectToMove, Quaternion end, float seconds, GameObject menu)
    {
        float elapsedTime = 0;
        Quaternion startingPos = objectToMove.transform.rotation;
            
        while (elapsedTime < seconds)
        {
            elapsedTime += Time.deltaTime / seconds;

            objectToMove.transform.rotation = Quaternion.Lerp(startingPos, end, Mathf.SmoothStep(0, 1, elapsedTime));
            yield return new WaitForEndOfFrame();
        }
        menu.SetActive(true);
        objectToMove.transform.rotation = end;
    }
}
