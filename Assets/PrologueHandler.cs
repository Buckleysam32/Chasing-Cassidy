using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueHandler : MonoBehaviour
{
    public GameObject nextButton;
    public Animator textAnim;
    public int textCount;

    private void Awake()
    {
        textCount = 0;
        textAnim = this.GetComponent<Animator>();
    }

    public void Next()
    {
        if(textCount < 4)
        {
            textCount++;
            textAnim.SetTrigger("Next");
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

}
