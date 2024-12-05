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
        SceneManager.LoadScene(2);
    }

    public void LoadEnding()
    {
        AkSoundEngine.PostEvent("Punch", this.gameObject);
        SceneManager.LoadScene(3);
    }

    public void LoadMenu()
    {
        AkSoundEngine.PostEvent("Gunshot", this.gameObject);
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
