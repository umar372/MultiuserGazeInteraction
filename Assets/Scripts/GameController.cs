using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour {

    public static GameController instance;

    public BlinkDetection blnkDetP1, blnkDetP2;
    public string highLightCardNameP1, highLightCardNameP2;

    int cardOpenCountP1, cardOpenCountP2;
    GameObject[] openCardHolderP1, openCardHolderP2;

    int scoreCountP1, scoreCountP2;
    public Text txScoreP1, txScoreP2;

    bool isPlayer1Flip, isPlayer2Flitp;

    GameObject prevOpenedP1 = null, prevOpenedP2=null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        scoreCountP1 = 0;
        scoreCountP2 = 0;
        txScoreP1.text = "Score : 0";
        txScoreP2.text = "Score : 0";

        cardOpenCountP1 = 0;
        cardOpenCountP2 = 0;
        openCardHolderP1 = new GameObject[2];
        openCardHolderP2 = new GameObject[2];

        blnkDetP1.onBlinkHappen += Player1BlinkOccurs;
        blnkDetP2.onBlinkHappen += Player2BlinkOccurs;

        isPlayer1Flip = true;
        isPlayer2Flitp = true;


    }

    // Update is called once per frame
    void Update () {
		
	}

    #region BlinksCode
    void Player1BlinkOccurs(double x, double y, string name)
    {
        Debug.Log("Player1 Blinkc");
        GameObject toFlop = GameObject.Find(highLightCardNameP1);
       // toFlop.transform.GetChild(4).gameObject.SetActive(true);
        if (toFlop != null && isPlayer1Flip && prevOpenedP1 != toFlop)
        {
            
            Debug.Log("Player1 Opened " + toFlop.GetComponent<Highlighter>().idName);

            if (cardOpenCountP1 < 2)
            {
                prevOpenedP1 = toFlop;
                openCardHolderP1[cardOpenCountP1] = toFlop;
                cardOpenCountP1 += 1;
                foreach (Transform t in toFlop.transform)
                {
                    t.gameObject.SetActive(false);
                }

            }
            else {
                prevOpenedP1 = null;
                if (openCardHolderP1[0].GetComponent<Highlighter>().idName == openCardHolderP1[1].GetComponent<Highlighter>().idName)
                {
                    scoreCountP1 += 1;
                    txScoreP1.text = "Score : " + scoreCountP1;
                    StartCoroutine(waitBeforeClosingCardsP1(true));
                    isPlayer1Flip = false;
                    Destroy(openCardHolderP1[0]);
                    Destroy(openCardHolderP1[1]);
                    Debug.Log("Matchedd");
                }
                else {
                    Debug.Log("Not Matchedd");
                    StartCoroutine(waitBeforeClosingCardsP1(false));
                    isPlayer1Flip = false;

                }

            }
                
            Debug.Log("Found flop");
            
            
        }
    }
    void Player2BlinkOccurs(double x, double y, string name)
    {

        GameObject toFlop = GameObject.Find(highLightCardNameP2);
      //  toFlop.transform.GetChild(5).gameObject.SetActive(true);
        if (toFlop != null && isPlayer2Flitp && prevOpenedP2 != toFlop)
        {
            Debug.Log("Player2 Opened "+toFlop.GetComponent<Highlighter>().idName);

            if (cardOpenCountP2 < 2)
            {
                prevOpenedP2 = toFlop;
                openCardHolderP2[cardOpenCountP2] = toFlop;
                cardOpenCountP2 += 1;
                foreach (Transform t in toFlop.transform)
                {
                    t.gameObject.SetActive(false);
                }

            }
            else
            {
                prevOpenedP2 = null;
                if (openCardHolderP2[0].GetComponent<Highlighter>().idName == openCardHolderP2[1].GetComponent<Highlighter>().idName)
                {
                    scoreCountP2 += 1;
                    txScoreP2.text = "Score : " + scoreCountP2;
                    StartCoroutine(waitBeforeClosingCardsP2(true));
                    isPlayer2Flitp = false;
                    
                    Destroy(openCardHolderP2[0]);
                    Destroy(openCardHolderP2[1]);
                    Debug.Log("Matchedd Player2");
                }
                else
                {
                    Debug.Log("Not Matchedd Player2");
                    isPlayer2Flitp = false;
                    StartCoroutine(waitBeforeClosingCardsP2(false));


                }

            }
        }
    }


    #endregion

    #region CoRoutines

    IEnumerator waitBeforeClosingCardsP1(bool isMatched)
    {
        yield return new WaitForSeconds(1f);
        if (!isMatched)
        {
            openCardHolderP1[0].transform.GetChild(0).gameObject.SetActive(true);
            openCardHolderP1[1].transform.GetChild(0).gameObject.SetActive(true);
            openCardHolderP1[0].transform.GetChild(4).gameObject.SetActive(true);
            openCardHolderP1[1].transform.GetChild(4).gameObject.SetActive(true);
        }
        else {
           // Debug.Log(openCardHolderP1[0].gameObject+"  "+
             //   openCardHolderP1[0].gameObject);
            

        }

        openCardHolderP1.Equals(null);
        openCardHolderP1 = new GameObject[2];
        isPlayer1Flip = true;
        cardOpenCountP1 = 0;
    }

    IEnumerator waitBeforeClosingCardsP2(bool isMatched)
    {
        yield return new WaitForSeconds(1f);
        if (!isMatched)
        {
            openCardHolderP2[0].transform.GetChild(0).gameObject.SetActive(true);
            openCardHolderP2[1].transform.GetChild(0).gameObject.SetActive(true);
            openCardHolderP2[0].transform.GetChild(5).gameObject.SetActive(true);
            openCardHolderP2[1].transform.GetChild(5).gameObject.SetActive(true);
        }
        else
        {

            

        }
        openCardHolderP2.Equals(null);
        openCardHolderP2 = new GameObject[2];
        isPlayer2Flitp = true;
        cardOpenCountP2 = 0;
    }
    #endregion

}
