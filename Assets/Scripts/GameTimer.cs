using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{

    public int TimeInSec = 180;
    public Text timerleft;
    public static bool isStartTimer;
    public AudioSource gameOverSound;
    // Use this for initialization
    void Start()
    {
     


    }

    // Update is called once per frame
    void Update()
    {
        if(isStartTimer)
        {

            TimeInSec = PlayerPrefs.GetInt("gameTime") * 60;
           // Debug.Log("Time get " + TimeInSec);
            timerleft.text = "Time : " + TimeInSec;
            StartCoroutine(Countdown());
            isStartTimer = false;
        }

    }
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1f);
        TimeInSec -= 1;
        timerleft.text = "Time : " + TimeInSec;
        if (TimeInSec > 0)
            StartCoroutine(Countdown());
        else
        {
            gameOverSound.Play();
            GameController.instance.isTimeUp = true;
        }
    }
}
