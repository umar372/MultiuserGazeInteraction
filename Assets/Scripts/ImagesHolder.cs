using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagesHolder : MonoBehaviour {

    public static ImagesHolder instance;
    public Sprite[] cardImages;
    public Sprite[] shuffledImages;


    public enum GameModes {EASY,MEDIUM,HARD};
    public int gameMode,cardLength;

    Hashtable cardHash;
    public bool isDoImages;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
	// Use this for initialization
	void Start () {

    

    }
	
	// Update is called once per frame
	void Update () {
        if (isDoImages)
        {
            cardHash = new Hashtable();
            gameMode = PlayerPrefs.GetInt("gameMode");
           // Debug.Log("Value Gets is " + gameMode);

            switch (gameMode)
            {
                case 0:
                    cardLength = 20;
                    break;
                case 1:
                    cardLength = 80;
                    break;
                case 2:
                    cardLength = cardImages.Length;
                    break;
                default:
                    break;
            }

            suffleImages();
            GenerateBoard.instance.isInitializeBoard = true;
            isDoImages = false;
        }	
	}

    void suffleImages()
    {
        
        List<Sprite> tempImages = new List<Sprite>();
        shuffledImages = new Sprite[cardLength*2];

        for (int i = 0; i < cardLength; i++)
        {
            tempImages.Add(cardImages[i]);
            tempImages.Add(cardImages[i]);
        }
        int index = 0;
        while (tempImages.Count > 0)
        {
            int ranNum = Random.Range(0,tempImages.Count-1);
            shuffledImages[index] = tempImages[ranNum];
            tempImages.RemoveAt(ranNum);
            index++;
           
        }

        for (int i = 0; i < tempImages.Count; i++)
        {
            Debug.Log(tempImages[i].name);
            cardHash.Add(tempImages[i].name,i);
        }

    }
}
