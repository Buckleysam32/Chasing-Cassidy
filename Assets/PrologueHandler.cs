using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueHandler : MonoBehaviour
{
    public GameObject nextButton;
    public Animator textAnim;
    public int textCount;
    public AK.Wwise.Event menuMusic;
    public AK.Wwise.Event uiSound;

    private void Awake()
    {
        textCount = 0;
        textAnim = this.GetComponent<Animator>();
    }

    private void Start()
    {
        menuMusic.Post(gameObject);
    }

    public void Next()
    {
        if(textCount < 4)
        {
            uiSound.Post(gameObject);
            textCount++;
            textAnim.SetTrigger("Next");
        }
        else
        {
            uiSound.Post(gameObject);
            menuMusic.Stop(gameObject);
            SceneManager.LoadScene(1);
        }
    }

}
