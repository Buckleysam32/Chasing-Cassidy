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
    public AK.Wwise.Event menuMusic;
    public AK.Wwise.Event windSound;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        menuMusic.Post(gameObject);
        windSound.Stop(gameObject);
    }

    public void NewGame()
    {
        AkSoundEngine.PostEvent("UISound", this.gameObject);
        gameManager.ClearData();
        menuMusic.Stop(gameObject);
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        AkSoundEngine.PostEvent("UISound", this.gameObject);
        menuMusic.Stop(gameObject);
        SceneManager.LoadScene(gameManager.town);
    }

    public void OpenSettings()
    {
        AkSoundEngine.PostEvent("UISound", this.gameObject);
        mainMenu.SetActive(false);
        StartCoroutine(MoveOverSeconds(cam, settingsRotation, 1f, settingsMenu));
    }

    public void CloseSettings()
    {
        AkSoundEngine.PostEvent("UISound", this.gameObject);
        settingsMenu.SetActive(false);
        StartCoroutine(MoveOverSeconds(cam, mainRotation, 1f, mainMenu));
    }

    public void ExitGame()
    {
        AkSoundEngine.PostEvent("UISound", this.gameObject);
        menuMusic.Stop(gameObject);
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
