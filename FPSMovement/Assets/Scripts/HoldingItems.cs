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
    [SerializeField] float throwForce;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isHeld)
        {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.TransformDirection(Vector3.forward) * rangeOfHand, out RaycastHit hit) && hit.collider.gameObject.layer == LayerMask.NameToLayer("grabbable") && hit.collider.gameObject.GetComponent<Rigidbody>() != null)
            {
                heldObject = hit.collider.gameObject;
                isHeld = true;
            }
        }
        else if (Input.GetMouseButtonDown(1) && isHeld)
        {
            isHeld = false;
            Rigidbody heldObjectRb = heldObject.GetComponent<Rigidbody>();
            heldObjectRb.AddForce(mainCamera.transform.forward * throwForce, ForceMode.Impulse);
        }
        else if (Input.GetMouseButtonDown(0) && isHeld)
            isHeld = false;

    }
    private void FixedUpdate()
    {
        if (isHeld)
        {
            heldObject.transform.position = Vector3.Lerp(heldObject.transform.position, placeOfholding.position, 0.2f);
            heldObject.GetComponent<Rigidbody>().useGravity = false;
            heldObject.GetComponent<Collider>().enabled = false;
        }
        else if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().useGravity = true;
            heldObject.GetComponent<Collider>().enabled = true;
            heldObject = null;
        }

    }
}
