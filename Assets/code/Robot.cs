using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Robot : MonoBehaviour {
	public float speed;
	public GameObject txt;
	private textScript textScriptInstantion;
	void Start () {
		textScriptInstantion = (textScript)txt.GetComponent (typeof(textScript));
		Debug.Log (textScriptInstantion.output);
		/*GameObject userInput = GameObject.Find("userInput");
		textScript gowno = userInput.GetComponent<textScript>();
		string gowienko = gowno.output;
		*/
	}

	void Update () {
		if (String.Equals(textScriptInstantion.output,"up")) {
			rigidbody2D.transform.position += new Vector3 (0, 0.8F, 0) * speed * Time.deltaTime;
		}
		//Debug.Log (textScriptInstantion.output);
		if (Input.GetKey(KeyCode.S)) {
			rigidbody2D.transform.position += new Vector3 (0, -0.8F, 0) * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.A)) {
			rigidbody2D.transform.position += new Vector3 (-0.8F, 0, 0) * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.D)) {
			rigidbody2D.transform.position += new Vector3 (0.8F, 0, 0) * speed * Time.deltaTime;
		}
	}
}
