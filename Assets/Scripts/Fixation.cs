using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fixations : MonoBehaviour {

    double xPos = -1;
    double yPos = -1;
    float confidence = -1;
    bool isGazeOnSurface = false;
    public GazeListner glPlayer1;
    int counter = 0;
    bool routineNotRunning = true;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
      //Debug.Log(glPlayer1.itemData);

        this.Cursor = new Cursor(Cursor.Current.Handle);
        Cursor.Position = new Point(glPlayer1.xpos, glPlayer1.ypos);
        Cursor.Clip = new Rectangle(this.Location, this.Size);
	}

    //To call this use StartCoroutine(FunctionForWait());
    IEnumerator FunctionForWait()
    {
        yield return new WaitForSeconds(0.2f);

    }
}
