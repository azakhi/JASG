using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {
    public float PosX = 0;
    public float PosY = 0;
    public Text debug;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        var mx = Input.GetAxis("Mouse X") * Time.deltaTime * 100.0f + PosX;
        var my = Input.GetAxis("Mouse Y") * Time.deltaTime * 100.0f + PosY;
        
        float dist = (mx * mx + my * my);
        if (dist > 2500)
        {
            dist = 50 / Mathf.Sqrt(dist);
            mx *= dist;
            my *= dist;
        }

        transform.Translate(mx - PosX, my - PosY, 0);
        PosX = mx;
        PosY = my;

        //debug.text = "x: " + PosX + " y: " + PosY;
    }
}
