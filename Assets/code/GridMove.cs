using System.Collections;
using UnityEngine;

class GridMove : MonoBehaviour {
	public float moveSpeed = 3f;
	public float gridSize = 0.8f;
	private enum Orientation {
		Horizontal,
		Vertical
	};
	private Orientation gridOrientation = Orientation.Vertical;
	private bool allowDiagonals = false;
	private bool correctDiagonalSpeed = true;
	private Vector2 input;
	private bool isMoving = false;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private float t;
	private float factor;
	private bool shouldMove = true;

	public void FixedUpdate() {
		if (!isMoving) {
			input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			if (!allowDiagonals) {
				if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) {
					input.y = 0;
				} else {
					input.x = 0;
				}
			}
			
			if (input != Vector2.zero && shouldMove) {
				StartCoroutine(move(transform));
			}
		}
	}
	
	public IEnumerator move(Transform transform) {
		isMoving = true;
		startPosition = transform.position;
		t = 0;

		endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
			                          startPosition.y + System.Math.Sign(input.y) * gridSize, startPosition.z);

		
		if(allowDiagonals && correctDiagonalSpeed && input.x != 0 && input.y != 0) {
			factor = 0.7071f;
		} else {
			factor = 1f;
		}
		
		while (t < 1f) {
			t += Time.deltaTime * (moveSpeed/gridSize) * factor;
			transform.position = Vector3.Lerp(startPosition, endPosition, t);
			yield return null;
		}
		
		isMoving = false;
		yield return 0;
	}
}