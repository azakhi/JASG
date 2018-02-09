using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapping : MonoBehaviour {
    public Collider coll;

	// Use this for initialization
	void Start () {
        coll = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Snapping otherAs = other.GetComponent<Snapping>();
        if (otherAs == null)
            return;

        ModularComponent parentScript = transform.parent.GetComponent<ModularComponent>();
        
        parentScript.onChildTriggerEnter(this, otherAs);
    }

    private void OnTriggerExit(Collider other)
    {
        Snapping otherAs = other.GetComponent<Snapping>();
        if (otherAs == null)
            return;

        ModularComponent parentScript = transform.parent.GetComponent<ModularComponent>();

        parentScript.onChildTriggerExit(this);
    }

    private void OnTriggerStay(Collider other)
    {
        Snapping otherAs = other.GetComponent<Snapping>();
        if (otherAs == null)
            return;
        
        ModularComponent parentScript = transform.parent.GetComponent<ModularComponent>();
        if(!parentScript.isActive)
        {
            ModularComponent otherparent = other.transform.parent.GetComponent<ModularComponent>();
            if (!otherparent.isActive)
            {
                coll.enabled = false;
                otherAs.coll.enabled = false;
                parentScript.activeSnap = -1;
                otherparent.activeSnap = -1;
            }
        }
    }
}
