using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBoard : MonoBehaviour {

    public static GenerateBoard instance;
    public GameObject card;
    Sprite[] shuffeledImages;
    double xFactor, yFactor;
    GameObject higlightCard;

    float xDistOffset, yDistOffset;
    int xLoop, yLoop;
    Vector3 cardSize;
    public bool isInitializeBoard;
    public GameObject cardHolder;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is call ed once per frame
    void Update () {
        if (isInitializeBoard)
        {
            if (ImagesHolder.instance != null)
            {
                shuffeledImages = new Sprite[ImagesHolder.instance.shuffledImages.Length];
                Debug.Log("Shuffeled Images Length " + shuffeledImages);
            }

            switch (ImagesHolder.instance.gameMode)
            {
                case 0:
                    xDistOffset = 3.3f;
                    yDistOffset = 3f;
                    xLoop = 8;
                    yLoop = 5;
                    cardSize = new Vector3(0.6f, 0.6f, 0.6f);
                    break;
                case 1:
                    xDistOffset = 1.5f;
                    yDistOffset = 1.3f;
                    xLoop = 16;
                    yLoop = 10;
                    cardSize = new Vector3(0.3f, 0.3f, 0.3f);

                    break;
                case 2:
                    xDistOffset = 1f;
                    yDistOffset = 1f;
                    xLoop = 25;
                    yLoop = 12;
                    cardSize = new Vector3(0.22f, 0.22f, 0.21f);
                    break;
                default:
                    break;
            }
            InitializeBoard();
            isInitializeBoard = false;
        }
    }

    void InitializeBoard()
    {
        int index = 0,jPrev=0;
        float xDist=0f, yDist=0f;
        for (int i = 0; i < xLoop; i++)
        {
            /*if (i > 0 && i % 4 == 0)
                xDist += 1f;
            else if(i>0)
                xDist = xDist+ 0.9f;

*/
            if(i>0)
                xDist = xDist + xDistOffset;
            

            for (int j = 0; j < yLoop; j++)
            {
                if (j > 0 && jPrev != j)
                {
                    yDist = yDist + yDistOffset;
                }
                else {
                    yDist = 0f;

                }
                GameObject localCard = Instantiate(card);
                localCard.name = "card_" + (i+1)+"_"+(j+1);
                localCard.transform.position = new Vector3(this.transform.position.x + xDist,this.transform.position.y +yDist, this.transform.position.z);
                localCard.transform.localScale = cardSize;
                localCard.GetComponent<SpriteRenderer>().sprite = ImagesHolder.instance.shuffledImages[index];
                localCard.transform.parent = cardHolder.transform;
                index++;
                jPrev = j;
            }
        }
        GameTimer.isStartTimer = true;
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
