using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using public class textScript : MonoBehaviour;

public class displayText : MonoBehaviour {
	Text txt;
	// Use this for initialization
	void Start () {
		txt = GetComponent<Text> ();
		txt.text = "La la la la la la la la";
	}
	
	// Update is called once per frame
	void Update () {
		//txt.text = textScript.output;
	}
}
