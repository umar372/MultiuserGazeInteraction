using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusedOject : MonoBehaviour {

    public static FocusedOject instance;

    RaycastHit hitInfo;
    public GameObject focusedObject;

    public string layerName;

    public int layerNum;
    // Use this for initialization
    void Start () {
        if (instance == null)
        {
            instance = this;
        }
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hitInfo))
        {
            focusedObject = hitInfo.transform.gameObject;
            layerNum = hitInfo.transform.gameObject.layer;
            //Debug.Log(layerNum);
            //print("There is something in front of the object!" + hitInfo.transform.gameObject.name);
            Debug.DrawRay(transform.position, fwd, Color.red);
        }
        else {
            focusedObject = null;

        }
            
    }
}
