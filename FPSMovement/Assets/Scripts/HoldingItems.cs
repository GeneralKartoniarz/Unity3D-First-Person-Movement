using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingItems : MonoBehaviour
{
    public static GameObject heldObject;
    public bool isHeld;
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform placeOfholding;
    [SerializeField] float rangeOfHand;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isHeld)
        {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward) * rangeOfHand, out RaycastHit hit) && hit.collider.gameObject.layer == LayerMask.NameToLayer("grabbable"))
            {
                heldObject = hit.collider.gameObject;
                isHeld = true;
            }
        }
        else if (Input.GetMouseButtonDown(0) && isHeld)
        {
            isHeld = false;
        }
    }
    private void FixedUpdate()
    {
        if (isHeld)
        {
            heldObject.transform.position = placeOfholding.position;
        }
        else
        {
            heldObject = null;
        }

    }
}
