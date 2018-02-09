using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCameraContainer : MonoBehaviour {

    private bool isPanned = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            isPanned = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isPanned = false;
        }

        if (isPanned)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * Time.deltaTime * 200.0f, 0, Space.World);
        }
    }
}
