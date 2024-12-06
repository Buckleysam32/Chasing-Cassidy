using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public Animator animator;
    public GameObject trans;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void LoadTown2()
    {
        AkSoundEngine.PostEvent("Punch", this.gameObject);
        AkSoundEngine.PostEvent("StopWind", this.gameObject);
        SceneManager.LoadScene(2);
    }

    public void LoadEnding()
    {
        AkSoundEngine.PostEvent("Punch", this.gameObject);
        AkSoundEngine.PostEvent("StopRain", this.gameObject);
        AkSoundEngine.PostEvent("Start_Game", this.gameObject);
        SceneManager.LoadScene(3);
    }

    public void LoadMenu()
    {
        AkSoundEngine.PostEvent("StopWind", this.gameObject);
        AkSoundEngine.PostEvent("Gunshot", this.gameObject);
        SceneManager.LoadScene(4);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Credits()
    {
        SceneManager.LoadScene(0);
    }

    public void StartTrans()
    {
        trans.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
