using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MalbersAnimations.Controller.MDirectionalDamage;
using UnityEngine.AI;
using MalbersAnimations.Controller;

public class Ragdoll : MonoBehaviour
{
    Rigidbody rb;
    bool hit;
    public bool idle;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (!hit && !idle)
        {
            transform.Translate(Vector3.forward * 2 * Time.deltaTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!hit)
        {
            transform.root.gameObject.GetComponent<Animator>().enabled = false;

            Rigidbody[] zombieRigidbodys = transform.root.gameObject.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rig in zombieRigidbodys)
            {
                rig.isKinematic = false;
                rig.useGravity = true;
            }

            transform.GetComponent<Rigidbody>().AddForce(other.transform.root.transform.position - transform.position * 5, ForceMode.Impulse);

            transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;  
            hit = true;
        }
    }
}
