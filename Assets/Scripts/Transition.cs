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
        SceneManager.LoadScene(2);
    }

    public void LoadEnding()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadMenu()
    {
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
