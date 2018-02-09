using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularComponent : MonoBehaviour {
    public bool isActive = false;
    public int activeSnap = -1;

    public Camera mainCam;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(isActive)
        {
            Vector3 move = new Vector3(Input.GetAxis("Mouse X") * Time.deltaTime * 10.0f, Input.GetAxis("Mouse Y") * Time.deltaTime * 10.0f, 0);
            move = mainCam.transform.TransformDirection(move);
            if(!Input.GetKey(KeyCode.LeftControl))
                move += (mainCam.transform.position - transform.position) * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * -100.0f;
            transform.Translate(move, Space.World);
        }
    }

    public void onChildTriggerEnter(Snapping own, Snapping other)
    {
        if (activeSnap > -1)
            return;

        activeSnap = own.transform.GetSiblingIndex();
        if (!isActive)
        {
            ModularComponent otherParent = other.transform.parent.GetComponent<ModularComponent>();
            if(otherParent.snapTo(other, own))
            {
                GameObject newBlock = Instantiate(Resources.Load("BuildingBlock")) as GameObject;
                ModularComponent newBlockScript = newBlock.GetComponent<ModularComponent>();
                newBlockScript.mainCam = mainCam;
                newBlockScript.isActive = true;
                activeSnap = -1;
            }
        }
    }

    public void onChildTriggerExit(Snapping own)
    {
        if (activeSnap == own.transform.GetSiblingIndex())
            activeSnap = -1;
    }

    public bool snapTo(Snapping own, Snapping target)
    {
        if (!isActive)
            return false;

        Vector3 direction = new Vector3(-target.transform.forward.x, -target.transform.forward.y, -target.transform.forward.z);
        transform.rotation = Quaternion.LookRotation(direction, target.transform.parent.up);
        transform.Rotate(new Vector3(-own.transform.localRotation.eulerAngles.x, -own.transform.localRotation.eulerAngles.y, -own.transform.localRotation.eulerAngles.z));

        transform.Translate(target.transform.position - own.transform.position, Space.World);

        isActive = false;
        activeSnap = -1;

        return true;
    }
}
