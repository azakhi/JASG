using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCameraControl : MonoBehaviour {
    public GameObject spaceCraft;

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
            transform.Translate(new Vector3(0, Input.GetAxis("Mouse Y") * Time.deltaTime * -200.0f, 0));

            Quaternion rotation = Quaternion.LookRotation(spaceCraft.transform.position - transform.position, transform.up);
            transform.rotation = rotation;
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 200.0f));
        }
    }
}
