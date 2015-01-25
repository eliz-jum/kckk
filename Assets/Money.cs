using UnityEngine;
using System.Collections;

public class Money : MonoBehaviour {


	public Text moneytext;
	public int money;
	// Use this for initialization
	void Start () {
		money = 100;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		moneytext.text = "Money: $" + money.ToString();
	}


	

}
