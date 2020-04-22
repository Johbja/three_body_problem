using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlanetPhysicsHandler controller;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float idelOffsetDistance;

    private Vector3 direction;

    private void Update() {

        Vector3 centerOfMass = controller.CalculateCenterOfMass();
        
        //zoom in and out
        if(Input.mouseScrollDelta.magnitude > 0) {
            direction = centerOfMass - transform.position;
            transform.position += direction.normalized * Input.mouseScrollDelta.y * scrollSpeed;
            idelOffsetDistance = Mathf.Clamp(Vector3.Distance(centerOfMass, transform.position), 2, 100000);
        }

        //move the camera position around the planets
        if(Input.GetMouseButton(1)) {
            transform.RotateAround(centerOfMass, transform.up, Input.GetAxis("Mouse X") * rotationSpeed);
            transform.RotateAround(centerOfMass, transform.right, Input.GetAxis("Mouse Y") * rotationSpeed);
        }

        //apply the changes
        transform.LookAt(centerOfMass);
        direction = centerOfMass - transform.position;
        transform.position = centerOfMass + direction.normalized * -idelOffsetDistance;
    }
}
