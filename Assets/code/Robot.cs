using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {

	public float speed;
	void Start () {
	
	}

	void Update () {
		if (Input.GetKey(KeyCode.W)) {
			rigidbody2D.transform.position += Vector3.up * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.S)) {
			rigidbody2D.transform.position += Vector3.down * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.A)) {
			rigidbody2D.transform.position += Vector3.left * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.D)) {
			rigidbody2D.transform.position += Vector3.right * speed * Time.deltaTime;
		}
	}
}
