using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            Ragdoll ragdoll = other.GetComponent<Ragdoll>();

            if (!ragdoll.hit)
            {
                Debug.Log("hit");

                other.GetComponent<Animator>().enabled = false;

                Vector3 direction = other.transform.position - transform.position;

                foreach (Rigidbody rb in ragdoll.rigidbodies)
                {
                    rb.useGravity = true;
                    rb.isKinematic = false;
                    rb.AddForce(direction * 100, ForceMode.Impulse);
                }
                ragdoll.hit = true;
            }
        }
    }
}
