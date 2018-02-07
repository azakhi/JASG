using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicFlight : MonoBehaviour {
    public Crosshair crosshair;
    public Text debug;

    private Vector3 velocity = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var spaceKey = Input.GetKey("space");

        if(spaceKey)
        {
            velocity += transform.forward * 1 * Time.deltaTime;
        }

        transform.Rotate(0, 0, -crosshair.PosX * Time.deltaTime);

        Vector3 itd = transform.TransformDirection(new Vector3(0, 0.01f, 0)) * crosshair.PosY * Time.deltaTime;
        velocity += itd;

        transform.Translate(velocity, Space.World);

        Quaternion rotation = Quaternion.LookRotation(velocity, transform.up);
        transform.rotation = rotation;
        
        //debug.text = "rot: " + transform.rotation.z;
    }
}
