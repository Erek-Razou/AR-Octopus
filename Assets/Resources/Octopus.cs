using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : MonoBehaviour
{
    private GameObject octopus;
    
    private State state = State.Idle;
    private Vector3 targetPosition;
    private float speed = 0.16f;

    // Start is called before the first frame update
    void Start()
    {
        octopus = GameObject.Find("octopus");
    }

    private enum State {
        Idle,
        Dodging
    }

    // Update is called once per frame
    void Update()
   {
    proccessTouch();
    if (state == State.Dodging) {
        if (Vector3.Distance(octopus.transform.position, targetPosition) < 0.01) {
            state = State.Idle;
            lookTowardsCamera();
        } else {
            dodge();
        }
    }
   }

   void setRandomTarget() {
    Debug.Log("Setting random");
    float x = Random.Range(-0.3f, 0.3f);
    float y = Random.Range(0.22f, 0.39f);
    float z = Random.Range(-0.17f, 0.1f);
    targetPosition = new Vector3(x, y, z);
   }

   void proccessTouch() {
    if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
        Touch touch = Input.touches[0];
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.CompareTag("Player")) {
                setRandomTarget();
                state = State.Dodging;
            }              
        }
    }
   }

   void dodge() {
    octopus.transform.LookAt(targetPosition);
    octopus.transform.Rotate(90.0f, 0.0f, 0.0f);
    octopus.transform.position = Vector3.MoveTowards(octopus.transform.position, targetPosition, speed * Time.deltaTime);
   }

   void lookTowardsCamera() {
    GameObject camera = GameObject.Find("ARCamera");
    octopus.transform.LookAt(camera.transform.position);

    octopus.transform.rotation = Quaternion.Euler(0, octopus.transform.rotation.eulerAngles.y, 0);
   }
}
