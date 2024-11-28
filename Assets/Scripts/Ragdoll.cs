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
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
            transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;

            transform.GetComponent<Rigidbody>().AddExplosionForce(350, transform.position, 25, 50, ForceMode.Impulse);

            hit = true;
        }
    }
}
