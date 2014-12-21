using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class textScript : MonoBehaviour {
	public InputField userInput;
	public Text roboTalk;
	private string commandText, output, roboText = "";
	private int processState;
	public List<string> wholeCommand = new List<string>();
	private List<string> commandStack = new List<string>();
	private static Hashtable commands = new Hashtable();

	void OnSubmit(string line) {	
		Debug.Log ("OnSubmit("+line+")");
		commandText = DateTime.Now.ToString("h:mm:ss tt") +  "\nUzytkownik: \n   "+ line + "\n" ;
//		roboText = Parser (line);
		output += commandText + "Robot:\n" + "   "+roboText;
	}
	void Awake () { 
		userInput = GameObject.Find ("userInput").GetComponent<InputField>();
		userInput.onEndEdit.AddListener (((value) => OnSubmit(value)));
		//userInput.validation = InputField.Validation.Alphanumeric;	
		//tworzenie tablicy komend
		commands.Add("build",1);//drugi argument w nawiasie to będzie np. jakiś wskaźnik na funkcje
		commands.Add("destroy",1);
		commands.Add("house",2);
		commands.Add("shed",2);
		commands.Add("cheap",3);
		commands.Add("north",4);
		commands.Add("?",0);
	}
	void Parser(string command) {
		string[] words = command.Split(new Char [] {' ', ',', '.', ':', '\t', '?' });
		List<string> tempCommand = new List<string>();
		string buildWord = "";
		foreach (var word in words) {
			Debug.Log(word);
			if (commands.ContainsValue(commands[word])) {
				Debug.Log ("commands zawiera" + word);
				if (String.Equals(commands[word],1) && (processState == 1)) {
					buildWord = word;
					tempCommand.Add (word);
				}
				if (String.Equals(commands[word],2) && (processState == 2)) {
					if (!(String.Equals(tempCommand[tempCommand.Count - 1], buildWord))) //sprawdzić czy tempCommand.Count - 1 >0
						tempCommand.Add (buildWord);
					tempCommand.Add (word);
				}
				if (String.Equals(commands[word],3) || String.Equals(commands[word],4)) {
					tempCommand.Add (word);
				}
			}
		}
		commandStack.InsertRange(0,tempCommand);
	}
	void Update () {
		switch (processState) {
			case 1:
					if (String.Equals (commands[commandStack [0]], processState))
							commandStack.RemoveAt (0);
		//return Process_main(word); miejsce na funkcje
					break;
			case 2:
					if (String.Equals (commandStack [0], processState))
							commandStack.RemoveAt (0);
		//return Process_second_tier(word); miejsce na funkcje
					break;
			case 3:
					if (String.Equals (commandStack [0], processState)) 
							commandStack.RemoveAt (0);
		//return Process_third_tier(word); miejsce na funkcje
					break;
			case 4:
					if (String.Equals (commandStack [0], processState)) 
							commandStack.RemoveAt (0);
		//return Process_thourth_tier(word);
					break;
		}
	}
	

	string Process_main(string word) {
		processState = 2;
		wholeCommand.Add(word);
		//miejsce na funkcję w zależności od słowa word
		return "Which structure would you like to "+word+" ?";
	}
	string Process_second_tier(string word){
		wholeCommand.Add (word);
		processState = 3;
		return "Okay i will " + wholeCommand[0] + " the " + word;
	}
	string Process_third_tier(string word){
		wholeCommand.Add (word);
		processState = 4;
		return "Okay i will ";
	}
	string Process_thourth_tier(string word){
		wholeCommand.Add (word);
		processState = 1;
		return "Okay i will " +  " the " + word;
	}
	

	void OnGUI () {
		GUI.TextArea (new Rect(668 ,125, 130,300),output + "",250);	
	}
}
