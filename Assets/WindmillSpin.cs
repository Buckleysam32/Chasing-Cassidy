using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillSpin : MonoBehaviour
{
    private Transform transform;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        transform = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = transform.localEulerAngles;
        rotation.z += speed;
        transform.localEulerAngles = rotation;
    }
}
