using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 1.5f;
    public float smoothing = 1.5f;

    private Vector2 mouseInput;
    private Vector2 smoothMouseInput;
    private Vector2 currentLookingPos;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        GetInput();
        ModifyInput();
        MovePlayer();
    }

    public void GetInput()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    }

    public void ModifyInput()
    {
        mouseInput = Vector2.Scale(mouseInput, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothMouseInput.x = Mathf.Lerp(smoothMouseInput.x, mouseInput.x, 1f / smoothing);
        smoothMouseInput.y = Mathf.Lerp(smoothMouseInput.y, mouseInput.y, 1f / smoothing);
    }

    public void MovePlayer()
    {
        currentLookingPos += smoothMouseInput;

        currentLookingPos.y = Mathf.Clamp(currentLookingPos.y, -90f, 90f);

        transform.localRotation = Quaternion.AngleAxis(currentLookingPos.x, Vector3.up);
        Camera.main.transform.localRotation = Quaternion.AngleAxis(-currentLookingPos.y, Vector3.right);
    }
}
