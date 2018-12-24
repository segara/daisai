using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExamplePlayer : MonoBehaviour {

    public float flySpeed, lookSpeed;
    public string togglePhysicsKey;
    private float rotX, rotY;
    public GameObject previewBall;
    private GameObject lastBall;
    public static bool moved;
	
	void Update () {
        //A variable so that only new Ghosts are rendered if something in the scene has changed. 
        moved = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0 || Input.mouseScrollDelta.y != 0;

        //Player movement, multiply input values by time.deltaTime to make the movement independent of the frame rate
        transform.position += transform.forward * Input.GetAxis("Vertical") * flySpeed * Time.deltaTime;
        transform.position += transform.right * Input.GetAxis("Horizontal") * flySpeed * Time.deltaTime;
        rotX -= Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -90, 90);
        transform.localRotation = Quaternion.Euler(rotX, rotY, 0);

        //Ball Instantiation
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(lastBall);
            Instantiate<GameObject>(previewBall, transform.position, Quaternion.identity).GetComponent<Rigidbody>().velocity = transform.forward * ExampleUI.strength.value;
            moved = true;
        }

        //Control wether the physics should run or not
        if (Input.GetKeyDown(togglePhysicsKey))
        {
            if (PhysicsPreview.timeIsFreezed)
            {
                Destroy(lastBall);
                //PhysicsPreview.RemoveGhosts();
                PhysicsPreview.SetPhysicsStart();
            }
            else
            {
                PhysicsPreview.Preview();
                PhysicsPreview.SetPhysicsStop();
            }
        }
    }

    void LateUpdate()
    {
        //Physics Preview done in late Update, so that any player movement is completed before the frames are drawn
        if (moved && PhysicsPreview.timeIsFreezed)
        {
            if (lastBall != null) { Destroy(lastBall); }
            PhysicsPreview.RemoveGhosts();
            PhysicsPreview.Preview();
            lastBall = Instantiate<GameObject>(previewBall, transform.position, Quaternion.identity);
            lastBall.GetComponent<Rigidbody>().velocity = transform.forward * ExampleUI.strength.value;
        }
    }
}
