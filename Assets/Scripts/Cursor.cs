using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Vector3 gazePosition;
    public PlayerData playerInstance;
    Vector3 gazeToWorldPos;

    public GameObject mCursor;
    // Use this for initialization
    void Start()
    {

        gazePosition = new Vector3(0f, 0f, 0f);

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInstance != null)
            gazePosition = new Vector3((float)playerInstance.xpos * Screen.width, (float)playerInstance.ypos * Screen.height, 0f);

        gazeToWorldPos = getWorldPosition(gazePosition);
        gazeToWorldPos = new Vector3(gazeToWorldPos.x, gazeToWorldPos.y,0f);
        mCursor.transform.position = gazeToWorldPos;

    }


    public Vector3 getWorldPosition(Vector3 screenPos)
    {
        Vector3 worldPos;
        if (Camera.main.orthographic)
        {
            worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = Camera.main.transform.position.z;
        }
        else
        {
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.transform.position.z));
            worldPos.x *= -1;
            worldPos.y *= -1;
        }
        // Debug.Log("World Position "+worldPos);
        return worldPos;
    }
}
