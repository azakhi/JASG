using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapping : MonoBehaviour {
    public Collider coll;

    private bool isColliding = false;
	// Use this for initialization
	void Start () {
        coll = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void disableIfColliding()
    {
        if (isColliding)
            coll.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        isColliding = true;
        Snapping otherAs = other.GetComponent<Snapping>();
        if (otherAs == null)
            return;

        ModularComponent parentScript = transform.parent.GetComponent<ModularComponent>();
        
        parentScript.onChildTriggerEnter(this, otherAs);
    }

    private void OnTriggerExit(Collider other)
    {
        isColliding = false;
        Snapping otherAs = other.GetComponent<Snapping>();
        if (otherAs == null)
            return;

        ModularComponent parentScript = transform.parent.GetComponent<ModularComponent>();

        parentScript.onChildTriggerExit(this);
    }
}
