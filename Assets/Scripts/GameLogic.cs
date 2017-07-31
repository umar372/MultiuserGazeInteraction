using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

    public GazeListner gazeListner;
    public GameObject[] cards;

    double xPos, yPos;
	// Use this for initialization
	void Start () {
        gazeListner.onBlinkHappen += (xPos, yPos) =>
        {
            if (xPos >= 0 && xPos <= 0.25 && yPos >= 0 && yPos <= 0.33)
            {
                Transform cover = cards[0].transform.GetChild(0);
                StartCoroutine(reloadCover(cover));
                cover.gameObject.SetActive(false);
            }
            if (xPos >= 0.25 && xPos <= 0.5 && yPos >= 0 && yPos <= 0.33)
            {
                Transform cover = cards[1].transform.GetChild(0);
                StartCoroutine(reloadCover(cover));
                cover.gameObject.SetActive(false);
            }
            if (xPos >= 0.5 && xPos <= 0.75 && yPos >= 0 && yPos <= 0.33)
            {
                Transform cover = cards[2].transform.GetChild(0);
                StartCoroutine(reloadCover(cover));
                cover.gameObject.SetActive(false);
            }
            if (xPos >= 0.75 && xPos <= 1.0 && yPos >= 0 && yPos <= 0.33)
            {
                Transform cover = cards[3].transform.GetChild(0);
                StartCoroutine(reloadCover(cover));
                cover.gameObject.SetActive(false);
            }
            /////////////////////////////////////////////////////////////

            if (xPos >= 0 && xPos <= 0.25 && yPos >= 0.33 && yPos <= 0.67)
            {
                Transform cover = cards[7].transform.GetChild(0);
                StartCoroutine(reloadCover(cover));
                cover.gameObject.SetActive(false);
            }
            if (xPos >= 0.25 && xPos <= 0.5 && yPos >= 0.33 && yPos <= 0.67)
            {
                Transform cover = cards[6].transform.GetChild(0);
                StartCoroutine(reloadCover(cover));
                cover.gameObject.SetActive(false);
            }
            if (xPos >= 0.5 && xPos <= 0.75 && yPos >= 0.33 && yPos <= 0.67)
            {
                Transform cover = cards[5].transform.GetChild(0);
                StartCoroutine(reloadCover(cover));
                cover.gameObject.SetActive(false);
            }
            if (xPos >= 0.75 && xPos <= 1.0 && yPos >= 0.33 && yPos <= 0.67)
            {
                Transform cover = cards[4].transform.GetChild(0);
                StartCoroutine(reloadCover(cover));
                cover.gameObject.SetActive(false);
            }
            /////////////////////////////////3rd top row//////////////////////////////////////////
            if (xPos >= 0 && xPos <= 0.25 && yPos >= 0.67 && yPos <= 1.0)
            {
                Transform cover = cards[8].transform.GetChild(0);
                StartCoroutine(reloadCover(cover));
                cover.gameObject.SetActive(false);
            }
            if (xPos >= 0.25 && xPos <= 0.5 && yPos >= 0.67 && yPos <= 1.0)
            {
                Transform cover = cards[9].transform.GetChild(0);
                StartCoroutine(reloadCover(cover));
                cover.gameObject.SetActive(false);
            }
            if (xPos >= 0.5 && xPos <= 0.75 && yPos >= 0.67 && yPos <= 1.0)
            {
                Transform cover = cards[10].transform.GetChild(0);
                StartCoroutine(reloadCover(cover));
                cover.gameObject.SetActive(false);
            }
            if (xPos >= 0.75 && xPos <= 1.0 && yPos >= 0.67 && yPos <= 1.0)
            {
                Transform cover = cards[11].transform.GetChild(0);
                StartCoroutine(reloadCover(cover));
                cover.gameObject.SetActive(false);
            }

        };
	}

    IEnumerator reloadCover(Transform cover)
    {
        yield return new WaitForSeconds(2f);
        cover.gameObject.SetActive(true);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
