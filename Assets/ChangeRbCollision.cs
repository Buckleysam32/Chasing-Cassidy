using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRbCollision : MonoBehaviour
{
    Rigidbody[] rbs;
    void Start()
    {
        rbs = FindObjectsOfType<Rigidbody>();

        foreach (Rigidbody rb in rbs)
        {
            if(rb.transform.tag == "CanPickUp")
            {
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rb.isKinematic = true;
                if (rb.GetComponent<MeshCollider>() != null )
                {
                    rb.GetComponent<MeshCollider>().enabled = true;
                    rb.GetComponent<MeshCollider>().convex = true;
                }
            }
        }
    }
}