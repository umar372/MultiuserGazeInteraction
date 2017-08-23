using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBoard : MonoBehaviour {

    public static GenerateBoard instance;
    public GameObject card;
    public PlayerData p1, p2;
    Sprite[] shuffeledImages;
    double xFactor, yFactor;
    GameObject higlightCard;
    BlinkDetection bldet;
    public BlinkDetection blnkDetP1;

    public string highLightCardName;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Use this for initialization
    void Start () {
        bldet = BlinkDetection.instance;
        InitializeBoard();
        if (ImagesHolder.instance != null)
        {
            shuffeledImages = new Sprite[ImagesHolder.instance.shuffledImages.Length];
        }

        xFactor = 1 / 25.0f;
        yFactor = 1 / 12.0f;

        bldet.onBlinkHappen += (double x, double y, string name) => {
            GameObject toFlop = GameObject.Find(highLightCardName);
            if (toFlop != null)
            {
                Debug.Log("Found flop");
                foreach(Transform t in toFlop.transform)
                {
                    t.gameObject.SetActive(false);
                }
            }
        };
        //for(int i = 0)
    }
	
	// Update is called once per frame
	void Update () {
       // HiglightPlayer1();

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
        
        string suffix = getGazeToCardPos(p1.xpos,p1.ypos);
        GameObject higlightCardLocal = GameObject.Find("card"+suffix);
        /*if (higlightCard != null && higlightCardLocal != null && higlightCard != higlightCardLocal)
        {
            higlightCard.transform.GetChild(1).gameObject.SetActive(false);
            higlightCard = higlightCardLocal;

        }*/
        
        
        
        //Debug.Log("card" + suffix);

        if (higlightCardLocal != null) //&& higlightCard != higlightCardLocal)
        {
            //Debug.Log(higlightCardLocal.name);
            higlightCardLocal.transform.GetChild(1).gameObject.SetActive(true);
            higlightCard = higlightCardLocal;
        }
    }

    string getGazeToCardPos(double xPos,double yPos)
    {
        double xpos_c, ypos_c;
        //Debug.Log("X Factor "+xFactor+ "YFactor" + yFactor + "xPos "+p1.xpos+"ypos "+p1.ypos);
       // Debug.Log("P1 xpos ypos "+xPos+"    "+yPos);
        xpos_c = Mathf.RoundToInt((float)(xPos /xFactor));
        ypos_c = Mathf.RoundToInt((float)(yPos /yFactor));

        return "_"+xpos_c.ToString()+"_"+ypos_c.ToString();
    }


}
