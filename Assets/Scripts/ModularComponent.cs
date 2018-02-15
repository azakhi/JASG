using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularComponent : MonoBehaviour {
    public bool isActive = false;
    public int activeSnap = -1;
    public float snapAngle = 10;

    public Camera mainCam;

    private float snapMove = 0;
    private ModularComponent snapped = null;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(isActive && !Input.GetMouseButton(1))
        {
            Vector3 move = new Vector3(Input.GetAxis("Mouse X") * Time.deltaTime * 10.0f, Input.GetAxis("Mouse Y") * Time.deltaTime * 10.0f, 0);
            move = mainCam.transform.TransformDirection(move);
            if(!Input.GetKey(KeyCode.LeftControl))
                move += (mainCam.transform.position - transform.position) * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * -100.0f;
            transform.Translate(move, Space.World);
        }

        if(snapped && !Input.GetMouseButton(1))
        {
            if (Input.GetKey(KeyCode.E))
            {
                snapped.activeSnap = -1;
                Component[] snappings;
                snappings = GetComponentsInChildren<Snapping>();
                foreach (Snapping snapping in snappings)
                    snapping.disableIfColliding();

                snappings = snapped.GetComponentsInChildren<Snapping>();
                foreach (Snapping snapping in snappings)
                    snapping.disableIfColliding();
                snapped = null;
                GameObject newBlock = Instantiate(Resources.Load("BuildingBlock")) as GameObject;
                ModularComponent newBlockScript = newBlock.GetComponent<ModularComponent>();
                newBlockScript.mainCam = mainCam;
                newBlockScript.isActive = true;
                activeSnap = -1;
                snapMove = 0;
            }
            else
            {
                snapMove += Input.GetAxis("Mouse X") * Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y") * Input.GetAxis("Mouse Y");

                if (snapMove > 3)
                {
                    snapped.isActive = true;
                    snapped = null;
                    snapMove = 0;
                }
            }
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
                snapped = otherParent;
                otherParent.isActive = false;
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

        if (Vector3.Angle(own.transform.forward, target.transform.forward) < 135)
            return false;

        Vector3 proj = Vector3.ProjectOnPlane(own.transform.up, target.transform.forward);
        Debug.Log(Vector3.Angle(proj, target.transform.up));

        Vector3 direction = new Vector3(-target.transform.forward.x, -target.transform.forward.y, -target.transform.forward.z);
        transform.rotation = Quaternion.LookRotation(direction, proj);
        transform.Rotate(new Vector3(-own.transform.localRotation.eulerAngles.x, -own.transform.localRotation.eulerAngles.y, -own.transform.localRotation.eulerAngles.z), Space.Self);

        transform.Translate(target.transform.position - own.transform.position, Space.World);

        if (snapAngle != 0)
        {
            float angBetween = Vector3.Angle(own.transform.up, target.transform.up);
            Debug.Log(angBetween);

            transform.RotateAround(own.transform.position, own.transform.forward, 1);
            if (angBetween < Vector3.Angle(own.transform.up, target.transform.up))
                angBetween = -angBetween;

            transform.RotateAround(own.transform.position, own.transform.forward, -1);

            angBetween %= snapAngle;
            Debug.Log(angBetween);

            angBetween = angBetween - Mathf.Round(angBetween / snapAngle) * snapAngle;

            transform.RotateAround(own.transform.position, own.transform.forward, angBetween);
        }

        if ((target.transform.position - own.transform.position).magnitude > 0.1f)
            Debug.Log(target.transform.GetSiblingIndex() + " - " + own.transform.GetSiblingIndex());

        return true;
    }
}
