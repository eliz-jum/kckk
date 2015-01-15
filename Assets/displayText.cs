using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using public class textScript : MonoBehaviour;

public class displayText : MonoBehaviour {
	Text txtx;
	public GameObject txt;
	private textScript textScriptInstantion;

	// Use this for initialization
	void Start () {
		textScriptInstantion = (textScript)txt.GetComponent (typeof(textScript));
		txtx = GetComponent<Text> ();
		//Debug.Log (textScriptInstantion.output);
		txtx.text = textScriptInstantion.output;
	}
	
	// Update is called once per frame
	void Update () {
		txtx.text = textScriptInstantion.output;
		//txt.text = textScript.output;
	}
}
