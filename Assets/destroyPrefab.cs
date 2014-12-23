using UnityEngine;
using System.Collections;

public class destroyPrefab : MonoBehaviour {
	void FixedUpdate () {
		if (Input.GetKey(KeyCode.UpArrow)) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.up,1 << 8);
			if (hit.collider != null) {
				Debug.Log ("Raycast trafił. Odległość to" + hit.distance);
				Debug.Log (hit.collider.gameObject.name);
			}
		}
		 

	}
}
