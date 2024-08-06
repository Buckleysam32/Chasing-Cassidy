using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public Rigidbody[] rigidbodies;
    public Animator anim;
    public bool hit;
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();


        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
    }


    // Update is called once per frame
}
