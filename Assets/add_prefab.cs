using UnityEngine;
using System.Collections;

public class add_prefab : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	//private GameObject prefab = 
	public Transform parposition;
	private float i = 0.8f;
	private bool b=true;
	void Update () {

		while (b) {
						Instantiate (Resources.Load ("tree"), parposition.position + new Vector3 (0, i, 0), parposition.rotation);
						Instantiate (Resources.Load ("tree"), parposition.position + new Vector3 (-i, i, 0), parposition.rotation);
						Instantiate (Resources.Load ("tree"), parposition.position + new Vector3 (0, -i, 0), parposition.rotation);
			b=false;
		}
		if (Input.GetKey(KeyCode.LeftArrow)) 
			Destroy(GameObject.Find("tree(Clone)"));
		

	
	}
}
