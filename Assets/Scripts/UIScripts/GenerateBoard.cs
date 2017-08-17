using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBoard : MonoBehaviour {

    public GameObject card;
	// Use this for initialization
	void Start () {
        InitializeBoard();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitializeBoard()
    {
        float xDist=0f, yDist;
        for (int i = 0; i < 24; i++)
        {
            if (i > 0 && i % 4 == 0)
            {
                xDist += 1f;
            }
            else if(i>0)
            {
                xDist = xDist+ 0.9f; //* 0.9f;

            }
            for (int j = 0; j < 13; j++)
            { 
            //GameObject localCard = new GameObject("card" + i);
            GameObject localCard = Instantiate(card);
                localCard.name = "card_" + i+"_"+j;
            localCard.transform.position = new Vector3(this.transform.position.x + xDist,
                this.transform.position.y + j-0.2f, this.transform.position.z);
            }
        }
    }
}
