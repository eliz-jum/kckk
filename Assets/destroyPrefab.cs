using UnityEngine;
using System.Collections;

public class destroyPrefab : MonoBehaviour {
	public LayerMask myLayerMask;
	void FixedUpdate () {
		if (Input.GetKey(KeyCode.UpArrow)) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.up,myLayerMask);
			if (hit.collider != null) {
				Debug.Log ("Raycast trafił. Odległość to " + hit.distance);
				Debug.Log (hit.collider.gameObject.name);
				Destroy(hit.transform.gameObject);
			}
		}
		 

	}
}
