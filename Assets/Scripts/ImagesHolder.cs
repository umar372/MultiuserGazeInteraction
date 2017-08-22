using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagesHolder : MonoBehaviour {

    public static ImagesHolder instance;
    public Sprite[] cardImages;
    public Sprite[] shuffledImages;


    Hashtable cardHash;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
	// Use this for initialization
	void Start () {
        cardHash = new Hashtable();

        suffleImages();
        //shuffle();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void shuffle()
    {
        Sprite[] tempImages = new Sprite[cardImages.Length * 2];
        for (int i = 0; i < cardImages.Length; i++)
        {
            tempImages[i] = cardImages[i];
            tempImages[i + cardImages.Length] = cardImages[i];
        }

        Hashtable tempHash = new Hashtable();

        for (int i = 0; i < tempImages.Length; i++)
        {
            if (tempHash.Contains(tempImages[i].name))
            {
                tempHash.Add(tempImages[i].name + "_a", tempImages[i]);
            }
            else {

            }
            
        }

        


    }

    void suffleImages()
    {
        List<Sprite> tempImages = new List<Sprite>();
        shuffledImages = new Sprite[cardImages.Length*2];

        for (int i = 0; i < cardImages.Length; i++)
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
