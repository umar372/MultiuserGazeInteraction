using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBoard : MonoBehaviour {

    public GameObject card;
    public PlayerData p1, p2;
    Sprite[] shuffeledImages;
    double xFactor, yFactor;
	// Use this for initialization
	void Start () {
        InitializeBoard();
        if (ImagesHolder.instance != null)
        {
            shuffeledImages = new Sprite[ImagesHolder.instance.shuffledImages.Length];
        }

        xFactor = 1 / 25.0f;
        yFactor = 1 / 12.0f;

        //for(int i = 0)
	}
	
	// Update is called once per frame
	void Update () {
        HiglightPlayer1();

    }

    void InitializeBoard()
    {
        int index = 0;
        float xDist=0f, yDist;
        for (int i = 0; i < 25; i++)
        {
            if (i > 0 && i % 4 == 0)
            {
                xDist += 1f;
            }
            else if(i>0)
            {
                xDist = xDist+ 0.9f; //* 0.9f;

            }
            for (int j = 0; j < 12; j++)
            {
                //GameObject localCard = new GameObject("card" + i);
                GameObject localCard = Instantiate(card);
                localCard.name = "card_" + (i+1)+"_"+(j+1);
                localCard.transform.position = new Vector3(this.transform.position.x + xDist,this.transform.position.y + j-0.2f, this.transform.position.z);
                localCard.GetComponent<SpriteRenderer>().sprite = ImagesHolder.instance.shuffledImages[index];
                
                index++;
            }
        }
    }

    void HiglightPlayer1()
    {
        string suffix = getGazeToCardPos();
        GameObject higlightCard = GameObject.Find("card"+suffix);

        if (higlightCard != null)
        {
            Debug.Log(higlightCard.name);
            higlightCard.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    string getGazeToCardPos()
    {
        double xpos, ypos;
        //Debug.Log("X Factor "+xFactor+ "YFactor" + yFactor + "xPos "+p1.xpos+"ypos "+p1.ypos);

        xpos = Mathf.RoundToInt((float)(p1.xpos /xFactor));
        ypos = Mathf.RoundToInt((float)(p1.ypos /yFactor));

        Debug.Log("card"+ "_" + xpos.ToString() + "_" + ypos.ToString());
        return "_"+xpos.ToString()+"_"+ypos.ToString();
    }
}
