using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayMoney : MonoBehaviour {

    Text moneytext;
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
