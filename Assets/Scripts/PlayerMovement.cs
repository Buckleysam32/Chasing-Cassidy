using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 8f;
    public float momentumDamping = 5;
    public CharacterController myCC;

    private Vector3 inputVector;
    private Vector3 movementVector;
    private float gravity = -10;

    //public Animator camAnim;
    public bool isWalking;

    //public AudioSource footstepAudioSource;
    //public AudioClip footstepSound;

    void Start()
    {
        myCC = GetComponent<CharacterController>();
    }

    void Update()
    {
        GetInput();
        MovePlayer();
        //PlayFootstepSound();

        //camAnim.SetBool("isWalking", isWalking);

        /*
        if (!isWalking)
        {
            footstepAudioSource.Stop();
        }*/
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            inputVector.Normalize();
            inputVector = transform.TransformDirection(inputVector);
            isWalking = true;
        }
        else
        {
            inputVector = Vector3.Lerp(inputVector, Vector3.zero, momentumDamping * Time.deltaTime);

            isWalking = false;
        }

        movementVector = (inputVector * playerSpeed) + (Vector3.up * gravity);
    }

    void MovePlayer()
    {
        myCC.Move(movementVector * Time.deltaTime);
    }

    void PlayFootstepSound()
    {
        /*
        if (isWalking && !footstepAudioSource.isPlaying)
        {
            footstepAudioSource.PlayOneShot(footstepSound);
        }*/
    }
}