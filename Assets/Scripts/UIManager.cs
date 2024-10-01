using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject playerHud;

    private void Start()
    {
        playerHud.SetActive(true);
    }
}
