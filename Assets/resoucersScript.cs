using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class resoucersScript : MonoBehaviour 
{
	public Text surowiec1text, surowiec2text, surowiec3text,kasatext;
	private int surowiec1, surowiec2, surowiec3, kasa;
	// Use this for initialization
	void Start () {
		surowiec1 = 1;
		surowiec2 = 56;
		surowiec3 = 130;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Update_surowiecText ();
	}
	void Update_surowiecText(){
		surowiec1text.text = "surowiec 1: " + surowiec1.ToString();
		surowiec2text.text = "surowiec 2: " + surowiec2.ToString();
		surowiec3text.text = "surowiec 3: " + surowiec3.ToString();
		kasatext.text = "kasa: " + kasa.ToString();
	}
}