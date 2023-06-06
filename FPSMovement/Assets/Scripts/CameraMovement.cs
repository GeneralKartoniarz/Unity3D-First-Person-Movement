using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //assign that to camera object
    [SerializeField] Camera mainCamera;
    [SerializeField] float mouseSens = 20f;
    [SerializeField] float angleYmax = 20f;
    private float yRot;
    void Start()
    {
        //locking cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        //getting inputs
        float xInput = Input.GetAxis("Mouse X") * mouseSens;
        float yInput = Input.GetAxis("Mouse Y") * mouseSens;
        //rotating camera on Y axis and adding max angle
        yRot -= yInput;
        yRot = Mathf.Clamp(yRot, -angleYmax, angleYmax);
        mainCamera.transform.localEulerAngles = Vector3.right * yRot;
        //rotating player object
        mainCamera.transform.parent.Rotate(Vector3.up * xInput);
    }
}
