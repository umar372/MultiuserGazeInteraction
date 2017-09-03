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
    public bool isTimeUp;
    public Text winText;
    public GameObject cardHolder;

    public AudioSource flippingSound,flipping2, matchSound;

    float timerP1, timerP2;
    bool isTimerP1, isTimerP2,isMatchedP1,isMatchedP2;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        setGameInitial();


    }

    void setGameInitial()
    {
        timerP1 = 0f;
        timerP2 = 0f;
        isTimerP1 = false;
        isTimerP2 = false;
        isMatchedP1 = false;
        isMatchedP2 = false;
        winText.enabled = false;
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

        Debug.Log("isTimer P! " + isTimerP1);

        if (isTimerP1)
        {
            timerP1 += Time.deltaTime;
            Debug.Log("In timer p1 "+timerP1);

            if (timerP1 >= 0.2f)
            {
                waitBeforeClosingCardsP1(isMatchedP1);
                timerP1 = 0f;
                isTimerP1 = false;
            }
        }

        if (isTimerP2)
        {
            timerP2 += Time.deltaTime;
            Debug.Log("In timer p2 " + timerP2);

            if (timerP2 >= 0.2f)
            {
                waitBeforeClosingCardsP2(isMatchedP2);
                timerP2 = 0f;
                isTimerP2 = false;
            }
        }



        if (isTimeUp)
        {
            foreach (Transform child in cardHolder.transform)
            {
                Destroy(child.gameObject);
            }
            winText.enabled = true;
            if (scoreCountP1 > scoreCountP2)
            {
                winText.text = "Congratulations Player1 won";
            }
            else if (scoreCountP2 > scoreCountP1)
            {
                winText.text = "Congratulation Player 2 won";
            }
            else {
                winText.text = "Match Tie";

            }
            StartCoroutine(ReloadGame());
            isTimeUp = false;
        }
	}

    IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(2f);
        winText.enabled = false;
        setGameInitial();
        UIController.instance.isReloadGame = true;
    }
    #region BlinksCode
    void Player1BlinkOccurs(double x, double y, string name)
    {
        //Debug.Log("Player1 Blinkc");
        GameObject toFlop = GameObject.Find(highLightCardNameP1);
       // toFlop.transform.GetChild(4).gameObject.SetActive(true);
        if (toFlop != null && isPlayer1Flip && prevOpenedP1 != toFlop)
        {
            
            //Debug.Log("Player1 Opened " + toFlop.GetComponent<Highlighter>().idName);

            if (cardOpenCountP1 < 2)
            {
                prevOpenedP1 = toFlop;
                openCardHolderP1[cardOpenCountP1] = toFlop;
                openCardHolderP1[cardOpenCountP1].GetComponent<Highlighter>().isThisCardOpen = true;

                cardOpenCountP1 += 1;
                foreach (Transform t in toFlop.transform)
                {
                    t.gameObject.SetActive(false);
                    flippingSound.Play();
                }

            }
            else if(cardOpenCountP1>=2){
                prevOpenedP1 = null;
                if (openCardHolderP1[0].GetComponent<Highlighter>().idName == openCardHolderP1[1].GetComponent<Highlighter>().idName)
                    {
                        scoreCountP1 += 1;
                        txScoreP1.text = "Score : " + scoreCountP1;
                        isMatchedP1 = true;
                        isTimerP1 = true;
                        isPlayer1Flip = false;
                        Destroy(openCardHolderP1[0]);
                        Destroy(openCardHolderP1[1]);
                        Debug.Log("Matchedd");
                        matchSound.Play();
                    }
                else
                {
                    Debug.Log("Not Matchedd");
                    isPlayer1Flip = false;
                    cardOpenCountP1 = 0;
                    isMatchedP1 = false;
                    isTimerP1 = true;
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
          //  Debug.Log("Player2 Opened "+toFlop.GetComponent<Highlighter>().idName);

            if (cardOpenCountP2 < 2)
            {
                prevOpenedP2 = toFlop;
                openCardHolderP2[cardOpenCountP2] = toFlop;
                openCardHolderP2[cardOpenCountP2].GetComponent<Highlighter>().isThisCardOpen = true;
                cardOpenCountP2 += 1;

                foreach (Transform t in toFlop.transform)
                {
                    t.gameObject.SetActive(false);
                    flipping2.Play();
                }

            }
            else if(cardOpenCountP2 >=2)
            {
                prevOpenedP2 = null;
               if (openCardHolderP2[0].GetComponent<Highlighter>().idName == openCardHolderP2[1].GetComponent<Highlighter>().idName)
                {
                        scoreCountP2+=1;
                        txScoreP2.text = "Score : " + scoreCountP2;
                        isMatchedP2 = true;
                        isTimerP2 = true;
                        isPlayer2Flitp = false;

                        Destroy(openCardHolderP2[0]);
                        Destroy(openCardHolderP2[1]);
                        Debug.Log("Matchedd Player2");
                        matchSound.Play();
                    
                }
                else
                {
                    Debug.Log("Not Matchedd Player2");
                    isPlayer2Flitp = false;
                    isMatchedP2 = false;
                    isTimerP2 = true;
                    cardOpenCountP2 = 0;

                }

            }
        }
    }


    #endregion

    #region CoRoutines

    void waitBeforeClosingCardsP1(bool isMatched)
    {
        if (!isMatched)
        {
            Debug.Log("Called P1 not matched");
            openCardHolderP1[0].transform.GetChild(0).gameObject.SetActive(true);
            openCardHolderP1[1].transform.GetChild(0).gameObject.SetActive(true);
            openCardHolderP1[0].transform.GetChild(4).gameObject.SetActive(true);
            openCardHolderP1[1].transform.GetChild(4).gameObject.SetActive(true);
            openCardHolderP1[0].GetComponent<Highlighter>().isThisCardOpen = false;
            openCardHolderP1[1].GetComponent<Highlighter>().isThisCardOpen = false;

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

    void waitBeforeClosingCardsP2(bool isMatched)
    {
        if (!isMatched)
        {
            openCardHolderP2[0].transform.GetChild(0).gameObject.SetActive(true);
            openCardHolderP2[1].transform.GetChild(0).gameObject.SetActive(true);
            openCardHolderP2[0].transform.GetChild(5).gameObject.SetActive(true);
            openCardHolderP2[1].transform.GetChild(5).gameObject.SetActive(true);
            openCardHolderP2[0].GetComponent<Highlighter>().isThisCardOpen = false;

            openCardHolderP2[1].GetComponent<Highlighter>().isThisCardOpen = false;
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
