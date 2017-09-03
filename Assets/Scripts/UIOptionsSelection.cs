using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptionsSelection : MonoBehaviour {

    GameObject highlight,selected;
    float timer;

	// Use this for initialization
	void Start () {
        highlight = transform.GetChild(0).gameObject;
        selected = transform.GetChild(1).gameObject; 
    }
	
	// Update is called once per frame
	void Update () {
        if (this.gameObject == FocusedOject.instance.focusedObject)
        {
            
            
                highlight.gameObject.SetActive(true);
                timer += Time.deltaTime;
                // Debug.Log("timer Valeue  " + timer);
                if (timer >= 2f)
                {
                if (FocusedOject.instance.layerNum == 14)
                    UIController.instance.isLoadGame=true;
                if (FocusedOject.instance.layerNum == 12)
                    UIController.instance.selectedModeObj = gameObject;
                else if (FocusedOject.instance.layerNum == 13)
                    UIController.instance.selectedTimeObj = gameObject;
                timer = 0f;

                }
            

        }
        else if(highlight.activeSelf){
            highlight.gameObject.SetActive(false);
            timer = 0;

        }
    }
}
