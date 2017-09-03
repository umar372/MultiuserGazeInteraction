using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public static UIController instance;
      
    public GameObject UISelection, GameTopPannel;
    public GameObject selectedModeObj, selectedTimeObj;
    GameObject prevSelectedModeObj, prevSelectedTimeObje;
    public GameObject gamePlayGadgets;

    public bool isLoadGame,isReloadGame;
    public GameObject ML1, ML2;
    float timer;

    public AudioSource backGround,gameStart;

    public GameObject ml1camera, ml1border, ml2camera, ml2border;
    bool isGameLoaded;
    public void Awake()
    {
      
        if (instance == null)
            instance = this;
    }
    // Use this for initialization
	void Start () {
        backGround.enabled = true;
        gameStart.enabled = false;
        backGround.Play();

        backGround.Play();

        ML1.SetActive(true);
        ML2.SetActive(true);

        
        
        isLoadGame = false;
        prevSelectedModeObj = selectedModeObj;
        prevSelectedTimeObje = selectedTimeObj;
        GameTopPannel.SetActive(false);
        gamePlayGadgets.SetActive(false);
        PlayerPrefs.SetInt("gameMode",0);
        PlayerPrefs.SetInt("gameTime",3);
        isGameLoaded = false;
	}

    void DisableMagicLense()
    {
        if (ml1camera != null)
        {
            ml1camera.SetActive(false);
        }
        if (ml2camera != null)
        {
            ml2camera.SetActive(false);

        }
        if (ml1border != null)
        {
            ml1border.SetActive(false);

        }
        if (ml2border != null)
        {
            ml2border.SetActive(false);
        }
    }
	// Update is called once per frame
	void Update () {
        if(!isGameLoaded)
            DisableMagicLense();
        if (isReloadGame)
        {
            ML1.SetActive(false);
            ML2.SetActive(false);
            UIController.instance.ml1camera.SetActive(false);
            UIController.instance.ml2camera.SetActive(false);
            UIController.instance.ml1border.SetActive(false);
            UIController.instance.ml2border.SetActive(false);
            GameTopPannel.SetActive(false);
            gamePlayGadgets.SetActive(false);
            UISelection.SetActive(true);
            isReloadGame = false;
            backGround.enabled = true;
            gameStart.enabled = false;
            backGround.Play();
            isLoadGame = false;
            

            Debug.Log("Relaod called");
        }
        if (isLoadGame)
        {
            UISelection.SetActive(false);
            GameTopPannel.SetActive(true);
            gamePlayGadgets.SetActive(true);
            isLoadGame = false;
            ImagesHolder.instance.isDoImages = true;
            backGround.enabled = false;
            gameStart.enabled = true;
            gameStart.Play();
            MagicLenseControl.instance.isRestarted = true;
            isGameLoaded = true;

            Debug.Log("Load  called");

        }
        else if (prevSelectedModeObj != selectedModeObj)
        {
            prevSelectedModeObj.transform.GetChild(1).gameObject.SetActive(false);
            selectedModeObj.transform.GetChild(1).gameObject.SetActive(true);
            prevSelectedModeObj = selectedModeObj;
            setGameMode(selectedModeObj.name);

        }
        else if (prevSelectedTimeObje != selectedTimeObj)
        {
            prevSelectedTimeObje.transform.GetChild(1).gameObject.SetActive(false);
            selectedTimeObj.transform.GetChild(1).gameObject.SetActive(true);
            prevSelectedTimeObje = selectedTimeObj;
            setTime(selectedTimeObj.name);

        }
    }

    void setGameMode(string name)
    {
        Debug.Log("Setting value for " + name);
        switch (name)
        {
            case "easy":
                PlayerPrefs.SetInt("gameMode", 0);
                break;
            case "medium":
                PlayerPrefs.SetInt("gameMode", 1);
                Debug.Log("The value settke is  " + PlayerPrefs.GetInt("gameMode"));

                break;
            case "hard":
                PlayerPrefs.SetInt("gameMode", 2);
                break;
        }
    }

    void setTime(string timeSel)
    {
        switch (timeSel)
        {
            case "1min":
                PlayerPrefs.SetInt("gameTime", 1);
                break;
            case "2min":
                PlayerPrefs.SetInt("gameTime", 2);
                break;
            case "3min":
                PlayerPrefs.SetInt("gameTime", 3);
                break;
            case "4min":
                PlayerPrefs.SetInt("gameTime", 4);
                break;
            case "5min":
                PlayerPrefs.SetInt("gameTime", 5);
                break;
        }
    }
}
