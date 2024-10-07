using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using System.Collections;
using UnityEditor;


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

    private AudioManager AM;

    private bool checkTag;


    void Start()
    {
        myCC = GetComponent<CharacterController>();
        canWalk = true;
        AM = FindObjectOfType<AudioManager>();
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
        RaycastHit hit;
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float maxDistance = 2f;

        Debug.DrawRay(origin, direction * maxDistance, Color.red);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.CompareTag("Sand"))
            {
                currentTerrain = CURRENT_TERRAIN.SAND;
                Debug.Log("SAND");
            }
            else if (hit.transform.gameObject.CompareTag("Wood"))
            {
                currentTerrain = CURRENT_TERRAIN.WOOD;
                Debug.Log("WOOD");
            }
            else if (hit.transform.gameObject.CompareTag("CompactSand"))
            {
                currentTerrain = CURRENT_TERRAIN.COMPACTSAND;
                Debug.Log("COMPACTSAND");
            }
        }
    }

    //Playing Correct Audio

    private void PlayFootstep(int terrain)
    {
        terrainSwitch[terrain].SetValue(this.gameObject);
        AM.PlayAudioClip(footstepsEvent, this.gameObject);
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

    //Detecting Wwise States

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "SaloonStateTrigger")
        {
            AudioManager.SetAreaSaloon();
        }
        else if (other.gameObject.tag == "GrocerStateTrigger")
        {
            AudioManager.SetAreaGrocer();
        }
        else if (other.gameObject.tag == "ButcherStateTrigger")
        {
            AudioManager.SetAreaButcher();
        }
        else if (other.gameObject.tag == "GunsmithStateTrigger")
        {
            AudioManager.SetAreaGunsmith();
        }
        else
        {
            AudioManager.SetAreaOutside();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SaloonStateTrigger")
        {
            AudioManager.SetAreaOutside();
        }
        else if (other.gameObject.tag == "GrocerStateTrigger")
        {
            AudioManager.SetAreaOutside();
        }
        else if (other.gameObject.tag == "ButcherStateTrigger")
        {
            AudioManager.SetAreaOutside();
        }
        else if (other.gameObject.tag == "GunsmithStateTrigger")
        {
            AudioManager.SetAreaOutside();
        }
    }
}