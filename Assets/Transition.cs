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

    public void NextScene()
    {
        SceneManager.LoadScene("Town2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
