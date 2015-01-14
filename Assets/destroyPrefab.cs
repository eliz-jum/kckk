using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class destroyPrefab : MonoBehaviour {
	public LayerMask myLayerMask;
	public float i = 0.8f;
	public float x,y = 0;
	public Transform parposition;

	bool containsNietBud(string slowo) {
		if (String.Equals(slowo, "opera"))
			return true;
		if (String.Equals(slowo, "railway"))
			return true;
		if (String.Equals(slowo, "fountain"))
			return true;
		if (String.Equals(slowo, "castle"))
			return true;
		if (String.Equals(slowo, "okraglak"))
			return true;
		return false;
	}
	bool containsBuilding(string slowo) {
		if (String.Equals(slowo, "shed"))
			return true;
		if (String.Equals(slowo, "house"))
			return true;
		if (String.Equals(slowo, "villa"))
			return true;
		return false;
	}
	void addPrefab(float x, float y, string prefab_name) {
		Debug.Log ("jestem w addprfeab name = " + prefab_name);
		Instantiate (Resources.Load (prefab_name), parposition.position + new Vector3 (x, y, 0), parposition.rotation);
	}
	void matchRemains(string prefab_name, string direction) {
		Debug.Log ("jetem w matchremains");
		if (String.Equals(prefab_name, "tree")) {
			destroy_Prefab(direction);
			addPrefab(x, y, "trunks");
			Debug.Log ("string equals trunks");
		}
		else if (String.Equals(prefab_name, "shed")) {
			destroy_Prefab(direction);
			addPrefab(x, y, "ashes");
		}
		else {
			destroy_Prefab(direction);
			addPrefab(x, y, "ruins");
			Debug.Log ("sadfghgfg");
		}
	}
	Vector2 translateDirection (string slowo) {
		if (String.Equals(slowo, "north"))
			return Vector2.up;
		if (String.Equals(slowo, "south"))
			return -Vector2.up;
		if (String.Equals(slowo, "east"))
			return Vector2.right;
		else //if (String.Equals(slowo, "west"))
			return -Vector2.right;
	}
	void destroy_Prefab (string direction) {
		Vector2 vector = translateDirection(direction); 
		RaycastHit2D hit = Physics2D.Raycast (transform.position, vector, myLayerMask);	
		if (hit.collider != null) 
			Destroy(hit.transform.gameObject);
		else
			Debug.Log ("raycast nie trafił, object = null");
	}
	void FixedUpdate () {
		if (Input.GetKey(KeyCode.UpArrow)) {
			y = i;
			RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.up,myLayerMask);
			if (hit.collider != null) {
				if (containsNietBud(hit.collider.gameObject.name)) {
					Debug.Log ("You mustn't destroy city's property!");
				}
				else if (containsBuilding(hit.collider.gameObject.name) || (String.Equals("tree",hit.collider.gameObject.name))) {
						matchRemains(hit.collider.gameObject.name, "north");
					}
				/*else if (hit.collider != null) {
					Debug.Log ("Raycast trafił. Odległość to " + hit.distance);
					Debug.Log (hit.collider.gameObject.name);
					Destroy(hit.transform.gameObject);
				}*/
			}
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector2.up,myLayerMask);
			if (hit.collider != null) {
				Debug.Log ("Raycast trafił. Odległość to " + hit.distance);
				Debug.Log (hit.collider.gameObject.name);
				Destroy(hit.transform.gameObject);
			}
		} 
		if (Input.GetKey(KeyCode.LeftArrow)) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector2.right,myLayerMask);
			if (hit.collider != null) {
				Debug.Log ("Raycast trafił. Odległość to " + hit.distance);
				Debug.Log (hit.collider.gameObject.name);
				Destroy(hit.transform.gameObject);
			}
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.right,myLayerMask);
			if (hit.collider != null) {
				Debug.Log ("Raycast trafił. Odległość to " + hit.distance);
				Debug.Log (hit.collider.gameObject.name);
				Destroy(hit.transform.gameObject);
			}
		}
	}
}
