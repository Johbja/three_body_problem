using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCollider : MonoBehaviour {

    [SerializeField] private float moveSpeed;

    private bool hasHit;
    private GameObject hitPlanet;
    private PlanetPhysicsHandler pph;

    private void Start() {
        hasHit = false;
        hitPlanet = null;
        pph = GetComponent<PlanetPhysicsHandler>();
    }

    void Update() {

        if(Input.GetMouseButtonDown(0)) {                                                           //check if left mouse button was pressed

            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);                         //creates raycast from the musese position directed in the camera direction
            RaycastHit hit;                                                                         //variable to hold collision info

            Debug.DrawRay(camRay.origin, camRay.direction.normalized * 100, Color.red, 10);       //draw ray in inspector for debuging

            if(Physics.Raycast(camRay.origin, camRay.direction.normalized, out hit, Mathf.Infinity)) {  //checks if the ray hit something
                hasHit = true;
                hitPlanet = hit.collider.gameObject;
            }
        }

        if(Input.GetMouseButtonUp(0)) {
            hasHit = false;
            hitPlanet = null;
        }

        if(Input.GetMouseButton(0) && hasHit && hitPlanet != null) {

            PlanetUiData uiHit = pph.uiSettings.Find(ui => ui.instance == hitPlanet);

            if(uiHit != null) {
                //move x
                if(Input.GetKey(KeyCode.X)) {
                    Vector3 newPos = uiHit.instance.transform.position + new Vector3(Input.GetAxis("Mouse X") * moveSpeed, 0, 0);
                    uiHit.UpdatePosition(newPos);
                    uiHit.UpdateForce();
                }

                //move y if not z
                if(Input.GetKey(KeyCode.Y) && !Input.GetKey(KeyCode.Z)) {
                    Vector3 newPos = uiHit.instance.transform.position + new Vector3(0, Input.GetAxis("Mouse Y") * moveSpeed, 0);
                    uiHit.UpdatePosition(newPos);
                    uiHit.UpdateForce();
                }

                // move z if not y
                if(Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.Y)) {
                    Vector3 newPos = uiHit.instance.transform.position + new Vector3(0, 0, Input.GetAxis("Mouse Y") * moveSpeed);
                    uiHit.UpdatePosition(newPos);
                    uiHit.UpdateForce();
                }
            }
        }
    }
}
