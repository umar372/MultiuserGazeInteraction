using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MagicLense : MonoBehaviour
{
    private Camera magnifyCamera;
    private GameObject magnifyBorders;
    private LineRenderer LeftBorder, RightBorder, TopBorder, BottomBorder; // Reference for lines of magnify glass borders
    private float MGOX, MG0Y; // Magnify Glass Origin X and Y position
    private float MGWidth = Screen.width / 10f, MGHeight = Screen.width / 10f; // Magnify glass width and height
    private Vector3 mousePos;
    private Vector3 gazePosition;
    public PlayerData playerInstance;
    public string playerName;
    public GameObject lenseFrame;
    float xPosLens, yPosLense;

    public Color color;
    public Material lineMat;

    void Start()
    {
        playerName = playerInstance.playerName;
        createMagnifyGlass();
        gazePosition = new Vector3(0f, 0f, 0f);

    }

    void Update()
    {
        if (playerInstance != null)
            gazePosition = new Vector3((float)playerInstance.xpos * Screen.width, (float)playerInstance.ypos * Screen.height, 0f);

        // Following lines set the camera's pixelRect and camera position at mouse position
        magnifyCamera.pixelRect = new Rect(gazePosition.x - MGWidth / 2.0f, gazePosition.y - MGHeight / 2.0f, MGWidth, MGHeight);
        if (Input.GetKey(KeyCode.A))
        {
            gazePosition = new Vector3(gazePosition.x - 2f, gazePosition.y, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            gazePosition = new Vector3(gazePosition.x + 2f, gazePosition.y, 0f);
        }
        if (Input.GetKey(KeyCode.W))
        {
            gazePosition = new Vector3(gazePosition.x, gazePosition.y + 2f, 0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            gazePosition = new Vector3(gazePosition.x, gazePosition.y - 2f, 0f);
        }

        mousePos = getWorldPosition(gazePosition);
        magnifyCamera.transform.position = mousePos;
        mousePos.z = 0;
        magnifyBorders.transform.position = mousePos;

    }

    // Following method creates MagnifyGlass
    private void createMagnifyGlass()
    {
        GameObject camera = new GameObject(playerName + "Camera");
        MGOX = Screen.width / 2f - MGWidth / 2f;
        MG0Y = Screen.height / 2f - MGHeight / 2f;
        magnifyCamera = camera.AddComponent<Camera>();

        if (playerName == "player1"){
            magnifyCamera.cullingMask = 1 << 9 | 1<<10;
        }
        else if (playerName == "player2")
        {
            magnifyCamera.cullingMask = 1 << 9 | 1 << 11;
        }
        magnifyCamera.pixelRect = new Rect(MGOX, MG0Y, MGWidth, MGHeight);
        magnifyCamera.clearFlags = Camera.main.GetComponent<Camera>().clearFlags;
        magnifyCamera.backgroundColor = Camera.main.GetComponent<Camera>().backgroundColor;
        magnifyCamera.transform.position = new Vector3(0, 0, 0);
        if (Camera.main.orthographic)
        {
            magnifyCamera.orthographic = true;
            magnifyCamera.orthographicSize = Camera.main.orthographicSize / 10.0f;//+ 1.0f;
            createBordersForMagniyGlass();
        }
        else
        {
            magnifyCamera.orthographic = false;
            magnifyCamera.fieldOfView = Camera.main.fieldOfView / 10.0f;//3.0f;
        }

    }

    // Following method sets border of MagnifyGlass
    private void createBordersForMagniyGlass()
    {
        magnifyBorders = new GameObject(playerName + "border");
        LeftBorder = getLine();
        LeftBorder.SetVertexCount(2);
        LeftBorder.SetPosition(0, new Vector3(getWorldPosition(new Vector3(MGOX, MG0Y, 0)).x, getWorldPosition(new Vector3(MGOX, MG0Y, 0)).y - 0.1f, -1));
        LeftBorder.SetPosition(1, new Vector3(getWorldPosition(new Vector3(MGOX, MG0Y + MGHeight, 0)).x, getWorldPosition(new Vector3(MGOX, MG0Y + MGHeight, 0)).y + 0.1f, -1));
        LeftBorder.transform.parent = magnifyBorders.transform;

        TopBorder = getLine();
        TopBorder.SetVertexCount(2);
        TopBorder.SetPosition(0, new Vector3(getWorldPosition(new Vector3(MGOX, MG0Y + MGHeight, 0)).x, getWorldPosition(new Vector3(MGOX, MG0Y + MGHeight, 0)).y, -1));
        TopBorder.SetPosition(1, new Vector3(getWorldPosition(new Vector3(MGOX + MGWidth, MG0Y + MGHeight, 0)).x, getWorldPosition(new Vector3(MGOX + MGWidth, MG0Y + MGHeight, 0)).y, -1));
        TopBorder.transform.parent = magnifyBorders.transform;

        RightBorder = getLine();
        RightBorder.SetVertexCount(2);
        RightBorder.SetPosition(0, new Vector3(getWorldPosition(new Vector3(MGOX + MGWidth, MG0Y + MGWidth, 0)).x, getWorldPosition(new Vector3(MGOX + MGWidth, MG0Y + MGWidth, 0)).y + 0.1f, -1));
        RightBorder.SetPosition(1, new Vector3(getWorldPosition(new Vector3(MGOX + MGWidth, MG0Y, 0)).x, getWorldPosition(new Vector3(MGOX + MGWidth, MG0Y, 0)).y - 0.1f, -1));
        RightBorder.transform.parent = magnifyBorders.transform;

        BottomBorder = getLine();
        BottomBorder.SetVertexCount(2);
        BottomBorder.SetPosition(0, new Vector3(getWorldPosition(new Vector3(MGOX + MGWidth, MG0Y, 0)).x, getWorldPosition(new Vector3(MGOX + MGWidth, MG0Y, 0)).y, -1));
        BottomBorder.SetPosition(1, new Vector3(getWorldPosition(new Vector3(MGOX, MG0Y, 0)).x, getWorldPosition(new Vector3(MGOX, MG0Y, 0)).y, -1));
        BottomBorder.transform.parent = magnifyBorders.transform;
    }

    // Following method creates new line for MagnifyGlass's border
    private LineRenderer getLine()
    {
        LineRenderer line = new GameObject("Line").AddComponent<LineRenderer>();

        // line.material = new Material(Shader.Find("Diffuse"));
        line.GetComponent<LineRenderer>().material = lineMat;

        line.SetVertexCount(2);
        line.SetWidth(0.2f, 0.2f);
        line.SetColors(color, color);
        line.useWorldSpace = false;
        return line;
    }
    private void setLine(LineRenderer line)
    {
        //line.material = new Material(Shader.Find("Diffuse"));
        line.GetComponent<LineRenderer>().material = lineMat;
        line.SetVertexCount(2);
        line.SetWidth(0.2f, 0.2f);
        line.SetColors(color, color);
        line.useWorldSpace = false;
    }

    // Following method calculates world's point from screen point as per camera's projection type
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
