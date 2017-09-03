using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLenseControl : MonoBehaviour {
    public GameObject ML1, ML2;
    public bool isRestarted;

    public static MagicLenseControl instance;
    bool isP1Active, isP2Active;
    public DoubleBlink dbP1, dbP2;
    public AudioSource magicenable, magicdisable;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }
	// Use this for initialization
	void Start () {
        dbP1.onDoubleBlinkHappen += (double xpos, double ypos, string pname) => {
            if (isP1Active)
            {
                deActiveP1();
            }
            else if (!isP1Active)
            {
                activeP1();
            }
        };

        dbP2.onDoubleBlinkHappen += (double xpos, double ypos, string pname) => {
            if (isP2Active)
            {
                deActiveP2();
            }
            else if (!isP2Active)
            {
                activeP2();
            }
        };
    }
	
	// Update is called once per frame
	void Update () {
        if (isRestarted)
        {
            if (PlayerPrefs.GetInt("gameMode") == 1 || PlayerPrefs.GetInt("gameMode") == 2)
            {
                ML1.SetActive(true);
                ML2.SetActive(true);
                UIController.instance.ml1camera.SetActive(true);
                UIController.instance.ml2camera.SetActive(true);
                UIController.instance.ml1border.SetActive(true);
                UIController.instance.ml2border.SetActive(true);
                isP1Active = true;
                isP2Active = true;

            }
            else
            {
                ML1.SetActive(false);
                ML2.SetActive(false);
                UIController.instance.ml1camera.SetActive(false);
                UIController.instance.ml2camera.SetActive(false);
                UIController.instance.ml1border.SetActive(false);
                UIController.instance.ml2border.SetActive(false);
                isP1Active = false;
                isP2Active = false;


            }
            isRestarted = false;
        }
	}

    void activeP1()
    {
        if (PlayerPrefs.GetInt("gameMode") == 1 || PlayerPrefs.GetInt("gameMode") == 2)
        {
            ML1.SetActive(true);
            UIController.instance.ml1camera.SetActive(true);
            UIController.instance.ml1border.SetActive(true);
            isP1Active = true;
            magicenable.Play();

        }


    }
    void deActiveP1()
    {
        if (PlayerPrefs.GetInt("gameMode") == 1 || PlayerPrefs.GetInt("gameMode") == 2)
        {
            ML1.SetActive(false);
            UIController.instance.ml1camera.SetActive(false);
            UIController.instance.ml1border.SetActive(false);

            isP1Active = false;
            magicdisable.Play();

        }
    }

    void activeP2()
    {
        if (PlayerPrefs.GetInt("gameMode") == 1 || PlayerPrefs.GetInt("gameMode") == 2)
        {
            ML2.SetActive(true);
            UIController.instance.ml2camera.SetActive(true);
            UIController.instance.ml2border.SetActive(true);
            isP2Active = true;
            magicenable.Play();

        }
    }

    void deActiveP2()
    {
        if (PlayerPrefs.GetInt("gameMode") == 1 || PlayerPrefs.GetInt("gameMode") == 2)
        {
            ML2.SetActive(false);
            UIController.instance.ml2camera.SetActive(false);
            UIController.instance.ml2border.SetActive(false);
            isP2Active = false;
            magicdisable.Play();
        }
    }
}
