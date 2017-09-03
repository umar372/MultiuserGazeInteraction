using UnityEngine;
using System.Collections;

public class SplashOnTap : MonoBehaviour {

	public GameObject splatter;
    [SerializeField]
    private LayerMask splatterLayer;//layermask to identify specific objects

	GameObject splat;

	//Random rnd = new Random();

	private Bounds bounds;
	private GameObject splatt;

    // Use this for initialization
    void Start () {
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = Camera.main.orthographicSize * 2;
		bounds = new Bounds(
			Camera.main.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
		Tap();

	}
	
	// Update is called once per frame
	void Update ()
    {

    }
    //methode which create splatter on tap
    void Tap()
    {
		int r1 = Random.Range(1,20);
		//if (r1 >= 15) {
			float sizex = Random.Range (1, 4);
			float posx = Random.Range (bounds.min.x, bounds.max.x);
			float posy = Random.Range (bounds.min.y, bounds.max.y);
			Vector3 pos = new Vector3 (posx, posy, 0);
			splat = Instantiate (splatter, pos, Quaternion.identity);
			splat.transform.localScale = new Vector3 (sizex, sizex, 0);
			//splat.GetComponent<Renderer> ().material.color.a = 0.5f;

		//}
		StartCoroutine (WaitToDestroy ());
        /*if (Input.GetMouseButtonDown(0))
        {
            //get the mouse click position
            Ray hitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //check for the physics on it and only do it for objects on splatterLayer
            RaycastHit2D hit = Physics2D.GetRayIntersection(hitRay, Mathf.Infinity, splatterLayer);
            //if not hitting collider returns
            if (hit.collider == null)
                return;
            //if yes creates the splatter
            if (hit.collider != null)
            {
                Instantiate(splatter, hit.point, Quaternion.identity);
            }
        }*/
    }

	IEnumerator WaitToDestroy(){
		yield return new WaitForSeconds (2f);

		Destroy (splat);
		Tap ();
	}
}
