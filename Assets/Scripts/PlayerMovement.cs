using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using System.Collections;


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

    public bool canWalk;

    //public AudioSource footstepAudioSource;
    //public AudioClip footstepSound;

    void Start()
    {
        myCC = GetComponent<CharacterController>();
        canWalk = true;
    }

    void Update()
    {
        if (canWalk)
        {
            GetInput();
            MovePlayer();
            CheckTerrain();
        }


        if (isWalking)
        {
            if (timer > footstepSpeed)
            {
                SelectAndPlayFootstep();
                timer = 0.0f;
            }
            timer += Time.deltaTime;
        }

        //camAnim.SetBool("isWalking", isWalking);

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

    //Footstep Audio

    private enum CURRENT_TERRAIN { SAND, WOOD, COMPACTSAND };

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    [SerializeField]
    private AK.Wwise.Event footstepsEvent;

    [SerializeField]
    private AK.Wwise.Switch[] terrainSwitch;

    //Detecting Layer

    private void CheckTerrain()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position, Vector3.down, 1.0f);

        foreach (RaycastHit rayhit in hit)
        {
            if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Sand"))
            {
                currentTerrain = CURRENT_TERRAIN.SAND;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Wood"))
            {
                currentTerrain = CURRENT_TERRAIN.WOOD;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("CompactSand"))
            {
                currentTerrain = CURRENT_TERRAIN.COMPACTSAND;
            }
        }
    }

    //Playing Correct Audio

    private void PlayFootstep(int terrain)
    {
        terrainSwitch[terrain].SetValue(this.gameObject);
        AkSoundEngine.PostEvent(footstepsEvent.Id, this.gameObject);
    }
    public void SelectAndPlayFootstep()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.SAND:
                PlayFootstep(0);
                break;

            case CURRENT_TERRAIN.WOOD:
                PlayFootstep(1);
                break;

            case CURRENT_TERRAIN.COMPACTSAND:
                PlayFootstep(2);
                break;

            default:
                PlayFootstep(0);
                break;
        }
    }

    //Establishing Float Timer

    private Rigidbody rb;

    float timer = 0.0f;

    [SerializeField]
    float footstepSpeed = 0.3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
