using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class textScript : MonoBehaviour {
	public InputField userInput;
	public Text roboTalk;
	public string output="\nWhat do you want me to do?";
	private string commandText, roboText = "";
	private int processState;
	public List<string> wholeCommand = new List<string>();
	private List<string> commandList = new List<string>();
	private static Hashtable commands = new Hashtable();
	public float i = 0.8f;
	public float x, y = 0;
	public LayerMask myLayerMask;
	public Transform parposition;
	private bool specialCommand;
	private int shedMoney = 25;
	private int houseMoney = 50;
	private int villaMoney = 75;
    private bool no=false, constructions=false, directions=false, question=false;
    public GameObject obj; //txt 
    private DisplayMoney DisplayMoneyInstantion;
	private string[] randomAnswers = new string[10];
    System.Random rnd = new System.Random();

	void OnSubmit(string line) {
		string commandLog;
		//Debug.Log ("OnSubmit("+line+")");
		commandText = DateTime.Now.ToString("h:mm:ss tt") +  "\nUzytkownik: \n "+ line;
		//		roboText = Parser (line);
		output += "\n[user]> "+line;
		commandLog = Parser (line);
        specialCommand = false;
        constructions = false;
        directions = false;
		if (String.Equals(commandLog, "pusta"))
        {
			//Debug.Log("jestem w pusta");
            if (question == false) {
				//Debug.Log("jestem questions false");
                if (!no)
                    output += "\n[robot]> I don't understand you.";
                else {
                    output += "\n[robot]> Ok.";
                    no = false;
                }
                question = true;
            }
		} 
		else
        { 
          no = false;
          fork();
        }
		//output += commandText + "Robot:" + roboText; 
	}
	void Awake () { 
		userInput = GameObject.Find ("userInput").GetComponent<InputField>();
		userInput.onEndEdit.AddListener (((value) => OnSubmit(value)));
		//userInput.validation = InputField.Validation.Alphanumeric;	
		//tworzenie tablicy komend
		commands.Add("build",1);//drugi argument w nawiasie to będzie np. jakiś wskaźnik na funkcje
		commands.Add("destroy",1);
		commands.Add("clear",1);
		commands.Add("money",1);
		commands.Add("go",1);
		commands.Add("up",2);
		commands.Add("down",2);
		commands.Add("left",2);
		commands.Add("right",2);
		commands.Add("house",3);
		commands.Add("shed",3);
        commands.Add("villa",3);
        commands.Add("opera",3);
        commands.Add("fountain",3);
        commands.Add("castle",3);
        commands.Add("railway",3);
        commands.Add("okraglak",3);
        commands.Add("bank",3);
        commands.Add("warehouse",3);
        commands.Add("trunks",3);
        commands.Add("ruins",3);
        commands.Add("ashes",3);
        commands.Add("tree",3);
		commands.Add("north",2);
		commands.Add("south",2);
		commands.Add("east",2);
		commands.Add("west", 2);
		commands.Add("no",0);
	//	commands.Add("dont't",0);
	//	commands.Add("Don't",0);

		randomAnswers[0] = "\n[robot]> Sorry, I don't know.";
		randomAnswers[1] = "\n[robot]> That's a hard question.";
		randomAnswers[2] = "\n[robot]> Why do you ask?";
		randomAnswers[3] = "\n[robot]> It's a secret.";
		randomAnswers[4] = "\n[robot]> I'm too busy to answer right now.";
		randomAnswers[5] = "\n[robot]> You're too young to know.";
		randomAnswers[6] = "\n[robot]> Do not ask me SUCH questions!";
		randomAnswers[7] = "\n[robot]> I forgot.";
		randomAnswers[8] = "\n[robot]> If I charged you $1 for each question, would you still be asking?";
		randomAnswers[9] = "\n[robot]> 42";
	}

   
    // Use this for initialization
    void Start () {
        DisplayMoneyInstantion = (DisplayMoney)obj.GetComponent (typeof(DisplayMoney));
      //Debug.Log(DisplayMoneyInstantion.money);
      //objCash = GetComponent<Text> ();
      //Debug.Log (textScriptInstantion.output);
      //txtx.text = textScriptInstantion.output;
    }

 	string Parser(string command) {
		//JEŚLI ZNAK ZAPYTANA TO LOSUJ JEDNĄ Z ODPOWIEDZI ELSE PONIŻSZE
        command = command.ToLower();
		string[] words = command.Split(new Char [] {' ', ',', '.', ':', '\t', '?', '!' }); // wyrzucić "?"
		List<string> tempCommand = new List<string>();
		string buildWord = "";
		int insertAt = 0;
        int randomNr = rnd.Next(1,10);
        bool isHow = false, isHowQuestion = false;
		//posortować stringa
        if ( command.Contains("?") ) {
			//if (
            foreach (var word in words) {
                if (String.Equals(word,"how"))
                    isHow = true;
                if (isHow && String.Equals(word, "villa")) {
                    output += "\n[robot]> Villa costs $" + villaMoney + " .";
                    isHowQuestion = true;
                }
                if (isHow && String.Equals(word, "shed")) {
                    output += "\n[robot]> Shed costs $" + shedMoney + " .";
                    isHowQuestion = true;
                }
                if (isHow && String.Equals(word, "house")) {
                    output += "\n[robot]> House costs $" + houseMoney + " .";
                    isHowQuestion = true;
                }
            }
            if (!isHowQuestion) 
                output += randomAnswers[randomNr];
            question = true;
        }
        else {
    		foreach (var word in words) {
    			//if  exists word < processState break i powiedz jestem w trakcie wykonywania polecenia nie mogę zbudować/ zburzyć 
    			//robot powtarza jeszcze raz czego potrzebuje
                
    			if (commands.ContainsValue(commands[word])) {
    				if (String.Equals(commands[word],1) ) {
    					buildWord = word;
    					tempCommand.Add (word);
    				}
    				if (String.Equals(commands[word],2) ) {
    					//if (!(String.Equals(tempCommand[tempCommand.Count - 1], buildWord))) //sprawdzić czy tempCommand.Count - 1 > 0
    					//	tempCommand.Add (buildWord); // jeśli ostatnie słowo nie jest buildWord to dodaje buildWord przed word
    					tempCommand.Add (word);
    				}
    				if (String.Equals(commands[word],3))  {
    					tempCommand.Add (word);
    				}
    				if (String.Equals(word,"no"))  {
    					no = true;
    				}
			//		if (String.Equals(word,"don't") || String.Equals(word,"Don't") )  {
						
			//		}

                    //odpowiedź użytkownika na zadanie pytania 
                    if (specialCommand) {
                        //sprawdza czy podal kierunek do funkcji destroy
                        if ( (String.Equals(commandList[0],"destroy") || (String.Equals(commandList[0],"clear"))) && containsDirection(word))
                            commandList.Insert(1,word); // wciska kierunek na commanList[1]
    						
                        

						//build kierunek
    					if (String.Equals(commandList[0],"build") && (directions) && (containsDirection(word)) ) {
    						if ( commandList.Count > 1 && containsDirection(commandList[1]) )//jesli byl tam stary kierunek
    							commandList[1]=word;
    						else
    							commandList.Insert(1,word); //wsadz kierunek na commandList[1]
    					}
    					//build budynek
    					if (String.Equals(commandList[0],"build") && (constructions) && (containsBuilding(word)) ) {	
    						if ( commandList.Count>2 && containsBuilding(commandList[2]) )//jesli byl tam stary budynek
    							commandList[2] = word;
    						else {
    							commandList.Insert(2,word); //wsadz budynek na commandList[2]
    						}
                        }
						//go kierunek
						if (String.Equals(commandList[0],"go") && (containsDirectionGo(word)) ) {	
							//Debug.Log("wstawiam kierunek do go");
							commandList.Insert(1,word);

						}
                    }
    			}
    		}
        }
   		if (no) {
			//Debug.Log("jestem w no");
			//do go
			if ( String.Equals(commandList[0], "go") ){
				if (commandList.Count>1)
					commandList.RemoveRange(0,2);
				else
					commandList.RemoveRange(0,1);
			}
			//do destroy	clear	build
			else{
				if (commandList.Count>2 && containsDirection(commandList[1]) && containsBuilding(commandList[2]))
				    commandList.RemoveRange(0, 3);
				else if (commandList.Count>1)
	                commandList.RemoveRange(0, 2);
				else{
					//Debug.Log("usowam destroy");
					commandList.RemoveRange(0, 1);
				}
			}
		}
                
      
        if (!specialCommand) {
            swap(ref tempCommand);
		    commandList.InsertRange(commandList.Count, tempCommand); // wstawia na koniec
        }	
		if (commandList.Count == 0)
			return "pusta"; //lista pusta lub zła komenda
		else
			return "poprawna"; // command list zawiera jakieś poprawne słowa
	}
	void swap(ref List<string> list) { //uproszoczny swap
        string temp;
        for(int i = 0; i < list.Count; i++) {
      		if ( (list.Count > i + 2) && (String.Equals(commands[list[i+1]],3)) 
      		&& (String.Equals(commands[list[i+2]],2)) ) {
      			temp = list[i+1];
      			list[i+1] = list[i+2];
      			list[i+2] = temp;
      		}
        }
	}

//-----------------------------------------------------------------fork-------------------------------------------

	void fork() {

        if ( commandList.Count > 0 && String.Equals(commandList[0], "go") ) {
  			go();
  		}
		else if ( commandList.Count > 0 && String.Equals(commandList[0], "money") ) {
			takeMoney();
		}		
		
		else if ( commandList.Count > 0 && String.Equals(commandList[0], "build") ){
  			build();
  		}

		else if ( commandList.Count > 0 && String.Equals(commandList[0], "destroy") ){
  		    destroy();  
        }
		else if ( commandList.Count > 0 && String.Equals(commandList[0], "clear") ){
			destroy();
  		}
		else {
			output = "\n[robot}> I don't understand you. Can you rephrase?";
			commandList.RemoveRange(0, commandList.Count);
		}
		if ( !specialCommand && commandList.Count > 0 )
			fork();
	}
//------------------------------------------------------------TAKE MONEY---------------------------------------------
	void takeMoney(){
		//jesli jest przy banku
		if ( (String.Equals (hitColliderName("north"), "bank")) || (String.Equals (hitColliderName("south"), "bank")) ||
			(String.Equals (hitColliderName("east"), "bank")) || (String.Equals (hitColliderName("west"), "bank")) ) {
			//jesli ma wystarczajaco pieniedzy
			if (DisplayMoneyInstantion.money > 299)
				output += "\n[robot]> You have enough! Don't be greedy!";
			else{
				DisplayMoneyInstantion.money = DisplayMoneyInstantion.money + 100;
				output += "\n[robot]> I've withdrawn $100 for you.";
			}
		}
		//jesli nie ma go brzy banku
		else
			output += "\n[robot]> You have to go to the bank.";
		commandList.RemoveRange (0, commandList.Count);
	}

//------------------------------------------------------------GO---------------------------------------------

	int go() {
		//Debug.Log(commandList [0]);
		//Debug.Log(commandList [1]);
		//Debug.Log(commandList [2]);
		// jeśli podał kierunek
		int isDirection = 0;
		//jesli podal kierunek

		if (commandList.Count > 1 && String.Equals (commandList [1], "up")) {
			if (hitCollider ("north") == false) {//jesli nic nie stoi na przeszkodzie
				//Debug.Log("czysto");
				rigidbody2D.transform.position += new Vector3 (0, i, 0); /* Time.deltaTime*/   
			} else {
				//Debug.Log("przesszkoda!");
				output += "\n[robot]> You can't go in this direction. The " + (hitColliderName ("north")) + " is there.";
				//NIE SPRAWDZA GDZIE MOZE ISC, nie PYTA gdzie moze
			}
			isDirection=1;
			commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
			return 0;
		}


		if (commandList.Count > 1 && String.Equals (commandList [1], "down")) {
			if (hitCollider ("south") == false) {
					rigidbody2D.transform.position += new Vector3 (0, -i, 0);
			} else {
					output += "\n[robot]> You can't go in this direction. The " + hitColliderName ("south") + " is there.";
			}
			isDirection=1;
			commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
			return 0;
		}
		if (commandList.Count > 1 && String.Equals (commandList [1], "right")) {
			if (hitCollider ("east") == false) {
					//Debug.Log("czysto");
					rigidbody2D.transform.position += new Vector3 (i, 0, 0);
			} else {
					output += "\n[robot]> You can't go in this direction. The " + hitColliderName ("east") + " is there.";
			}
			isDirection=1;
			commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
			return 0;
		}

		if (commandList.Count > 1 && String.Equals (commandList [1], "left")) {
			if (hitCollider ("west") == false) {
					rigidbody2D.transform.position += new Vector3 (-i, 0, 0);
			} else {
					output += "\n[robot]> You can't go in this direction. The " + hitColliderName ("west") + " is there.";
			}
			isDirection=1;
			commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
			return 0;
		}

		if (isDirection==0){//jesli nie podal kierunku		
			output += "\n[robot]> You didn't say where!\nIf you want, name your direction, if not just say no.";
			specialCommand=true;
            return 0;
		}
        return 0;
	}


//-----------------------------------------------------------------------BUILD----------------------------------------------


	void offerHouse(){
		if (DisplayMoneyInstantion.money < shedMoney) {//nie ma na nic pieniedzy
			output += "\nIn fact, you don't have money for any construction. You have to go to the bank and withdraw the money.";
			if (containsDirection(commandList[1]) && commandList.Count>2 && containsBuilding(commandList[2]))
				commandList.RemoveRange(0, 3);
			else
				commandList.RemoveRange(0, 2);
		}
		else {
			if (DisplayMoneyInstantion.money >= villaMoney)
					output += "You can build a villa, a house or a shed there.";

			else if (DisplayMoneyInstantion.money >= houseMoney)
					output += "\nYou can build a house or a shed.";
			else
					output += "\nYou can build only a shed.";

			output += "\nIf you want to build a construction I offered - name it. If you don't just say 'no'";

			specialCommand = true;
			constructions = true;
		}
	}
	
	
	
	
	

	//GDZIE BUDUJE
		
	void build(){
        x = 0;
        y = 0;
		if (commandList.Count > 1 && containsDirection (commandList [1])) {   //jesli podal kierunek
			Debug.Log(hitColliderName(commandList[1]));
			if (String.Equals (commandList[1], "north"))
					y = i;
			if (String.Equals (commandList[1], "south"))
					y = -i;
			if (String.Equals (commandList[1], "west"))
					x = -i;
			if (String.Equals (commandList[1], "east"))
					x = i;

			//jesli tam jest trawa to buduj
			if (String.Equals (hitColliderName (commandList [1]), "grass")) 
				construct ();
			else if (!(hitCollider (commandList [1]))) {
				output += "\n[robot]> You mustn't build on the road!";
				if (containsDirection(commandList[1]) && commandList.Count>2 && containsBuilding(commandList[2]))
					commandList.RemoveRange(0, 3);
				else
					commandList.RemoveRange(0, 2);
			}
			else {//podal zly kierunek - tam cos jest
				if (containsNietBud (hitColliderName(commandList[1])) )
					output += "\n[robot]> You mustn't destroy city's property!";
				else
					output += "\n[robot]> First you have to get rid of " + hitColliderName (commandList [1]) + ".";
 
				offerDirections ();

			}
		}
		
		else { //jesli nie podal kierunku
			output += "\n[robot]> You didn't say where.";
			offerDirections();
		}
		
	}
	
	//PROPONOWANIE KIERUNKOW
	void offerDirections(){

		string N="";
		string S="";
		string E="";
		string W="";
 
				
		if ( String.Equals(hitColliderName("north"), "grass") ) //jesli jest na polnocy trawa
			N="north";
		
		if ( String.Equals(hitColliderName("south"), "grass") ) 
			S="south";
		
		if ( String.Equals(hitColliderName("east"), "grass") )
			E="east";
		
		if ( String.Equals(hitColliderName("west"), "grass") )
			W="west";
		


		if ( String.Equals(N,"") && String.Equals(E,"") && String.Equals(W,"") && String.Equals(S,"") ) { //nigdzie nie ma trawy
			output += "\n[robot]> There is no ground you can build on. you have to either move or clear the area.";
			if (containsDirection(commandList[1]) && commandList.Count>2 && containsBuilding(commandList[2]))
				commandList.RemoveRange(0, 3);
			else
				commandList.RemoveRange(0, 2);
		}
		else {//gdzies jest trawa
			output += "\nIf you want you can build in the "+N+" "+S+" "+E+" "+W+"\nIf so choose a direction, if not just say 'no'.";
			specialCommand=true;//z kierunkiem
			directions=true;

		}

	}

	

	
	
	//CO BUDUJE
		
	void construct(){
		//jesli nie podal co ma zbudowac
			
		if (commandList.Count>2 && containsBuilding(commandList[2]) ){ //jesli podal co ma zbudowac
			if ( String.Equals(commandList[2], "shed") ){
				if ( DisplayMoneyInstantion.money < shedMoney ){
					output += "\n[robot]> You don't have enough money, because a shed costs $"+shedMoney;
					offerHouse();
				}
				else{
					addPrefab(x, y, "shed" );
					DisplayMoneyInstantion.money = DisplayMoneyInstantion.money - shedMoney;
					output += "\n[robot]> I've built a "+commandList[2]+" in the "+commandList[1]+" for you.";
					commandList.RemoveRange(0, 3);
				}
			}

			else if ( String.Equals(commandList[2], "house") ){
				if ( DisplayMoneyInstantion.money < houseMoney ){
					output += "\n[robot]> You don't have enough money, because a house costs $"+houseMoney;
					offerHouse();
				}
				else{
					addPrefab(x, y, "house" );
					DisplayMoneyInstantion.money = DisplayMoneyInstantion.money - houseMoney;
					output += "\n[robot]> I've built a "+commandList[2]+" in the "+commandList[1]+" for you.";
					commandList.RemoveRange(0, 3);
				}
			}

			else if ( String.Equals(commandList[2], "villa") ){
				if ( DisplayMoneyInstantion.money < villaMoney ){
					output += "\n[robot]> You don't have enough money, because a villa costs $"+villaMoney;
					offerHouse();
				}
				else{
					addPrefab (x, y, "villa" );
					DisplayMoneyInstantion.money = DisplayMoneyInstantion.money - villaMoney;
					output += "\n[robot]> I've built a "+commandList[2]+" in the "+commandList[1]+" for you.";
					commandList.RemoveRange(0, 3);
				}
			}		
		}
		else {//nie podal co ma zbudowac
			output += "\n[robot]> You didn't say what.";
			offerHouse();
		}	
	}	







	bool containsDirectionGo(string slowo) {
		if (String.Equals(slowo, "up"))
			return true;
		if (String.Equals(slowo, "down"))
			return true;
		if (String.Equals(slowo, "left"))
			return true;
		if (String.Equals(slowo, "right"))
			return true;
		return false;
	}
	bool containsDirection(string slowo) {
		if (String.Equals(slowo, "north"))
			return true;
		if (String.Equals(slowo, "south"))
			return true;
		if (String.Equals(slowo, "east"))
			return true;
		if (String.Equals(slowo, "west"))
			return true;
		return false;
	}
	bool containsBuilding(string slowo) {
		if ( (String.Equals(slowo, "shed")) || (String.Equals(slowo, "shed(Clone)")) )
			return true;
	    if ( (String.Equals(slowo, "house")) || (String.Equals(slowo, "house(Clone)")) )
			return true;
		if ( (String.Equals(slowo, "villa")) || (String.Equals(slowo, "villa(Clone)")) )
			return true;
		return false;
	}
	
	bool containsRemains(string slowo) {
		if ( (String.Equals(slowo, "trunks")) || (String.Equals(slowo, "trunks(Clone)")) )
			return true;
		if ( (String.Equals(slowo, "ruins")) || (String.Equals(slowo, "ruins(Clone)")) )
			return true;
		if ( (String.Equals(slowo, "ashes")) || (String.Equals(slowo, "ashes(Clone)")) )
			return true;
		return false;
	}

	bool containsNietBud(string slowo) {
		if ( (String.Equals(slowo, "opera")) )
			return true;
		if ( (String.Equals(slowo, "railway")) )
			return true;
		if ( (String.Equals(slowo, "fountain")) )
			return true;
		if ( (String.Equals(slowo, "castle")) )
			return true;
		if ( (String.Equals(slowo, "okraglak")) )
			return true;
		if ( (String.Equals(slowo, "bank")) )
			return true;
        if ( (String.Equals(slowo, "warehouse")) )
            return true;
		return false;
	}

	Vector2 translateDirection (string slowo) {
		if (String.Equals(slowo, "north"))
			return Vector2.up;
		if (String.Equals(slowo, "south"))
			return -Vector2.up;
		if (String.Equals(slowo, "east"))
			return Vector2.right;
		if (String.Equals(slowo, "west"))
			return -Vector2.right;
		return Vector2.zero;
	}
    
	string hitColliderName (string direction) {
		Vector2 vector = translateDirection(direction); 
		RaycastHit2D hit = Physics2D.Raycast (transform.position, vector, i, myLayerMask);	
		if (hit.collider != null) 
			return hit.collider.gameObject.name;	
		else 
			return "";//nie masz prawa!!! Wyrąbywać nullpointerów!!!!!!!!! 
	}
	
	bool hitCollider (string direction) {
		Vector2 vector = translateDirection(direction); 
		RaycastHit2D hit = Physics2D.Raycast (transform.position, vector, i, myLayerMask);
		if (hit.collider != null) {
			//Debug.Log (hitColliderName(direction));
			return true;
		}
		else {
			//Debug.Log ("W nic nie trafiłem ");
			return false;
		}
	}

	void destroyPrefab (string direction) {
		Vector2 vector = translateDirection(direction); 
		RaycastHit2D hit = Physics2D.Raycast (transform.position, vector, myLayerMask);	
		if (hit.collider != null) 
            Destroy(hit.transform.gameObject);
        //else
            //Debug.Log ("raycast nie trafił, object = null");
	}

	void addPrefab(float x, float y, string prefab_name){
		Instantiate (Resources.Load (prefab_name), parposition.position + new Vector3 (x, y, 0), parposition.rotation);
	}

	void matchRemains(string prefab_name, string direction){
		if ( (String.Equals(prefab_name, "tree")) || (String.Equals(prefab_name, "tree(Clone)")) ){
			destroyPrefab(direction);
			addPrefab(x, y, "trunks");
			output += "\n[robot]> I've cut down the tree.";
		}
		else if ( (String.Equals(prefab_name, "shed")) || (String.Equals(prefab_name, "shed(Clone)")) ){
			destroyPrefab(direction);
			addPrefab(x, y, "ashes");
			output += "\n[robot]> I've burned the shed.";
		}
		else {
			destroyPrefab(direction);
			addPrefab(x, y, "ruins");
			output += "\n[robot]> I've demolished the "+prefab_name;
		}
	}

    int destroy(){
    	x=0;
    	y=0;
    		// destroy north house
    	if (commandList.Count > 1 && containsDirection (commandList [1])) { // jeśli podał kierunek
    			
						if (String.Equals (commandList [1], "north"))
								y = i;
						if (String.Equals (commandList [1], "south"))
								y = -i;
						if (String.Equals (commandList [1], "west"))
								x = -i;
						if (String.Equals (commandList [1], "east"))
								x = i;	
						//Debug.Log(commandList.Count);
						if (commandList.Count > 2 && ((containsBuilding (commandList [2])) || (String.Equals (commandList [2], "tree")))) { //jesli podal budynek lub drzewo
								if ((containsBuilding (hitColliderName (commandList [1]))) || (String.Equals (hitColliderName (commandList [1]), "tree"))) {//czy w kierunku ktory podal jest budynek/drzewo ktory podal

										matchRemains (commandList [2], commandList [1]);
										commandList.RemoveRange (0, 3); //USUN jedną KOMENDĘ
										return 0;
								} else { //w tym kierunku jest cos innego
										// np na gorze jest home a on pisze "Destroy shed in the north."
										output += "\n[robot]> You can't destroy the " + commandList [2]; //You can't destroy a shed
										output += "\nIn the " + commandList [1] + " there is/are the " + hitColliderName (commandList [1]); //in the north there is a home.
										if (!hitCollider (commandList [1]))
												output += "road.";
										commandList.RemoveRange (0, 3); //USUN jedną KOMENDĘ
										return 0;
    						
    						
								}
						}
						//jesli podal cokolwiek do clear
						if (commandList.Count > 2 && containsRemains (commandList [2])) { //podal co wyczyscic
    				 
								//hitColliderName dostaje kierunek zwraca nazwe collidera
								//containsRemains porownuje slowo dostane z ruins/ashes/trunks
								if (containsRemains (hitColliderName (commandList [1]))) {//czy w kierunku ktory podal jest pozostalosc ktora podal
										destroyPrefab (commandList [1]);
										commandList.RemoveRange (0, 3); //USUN jedną KOMENDĘ
										return 0;
								} else {
										output += "\n[robot]> You can't clear " + commandList [2]; 
										output += "\n[robot]> In the " + commandList [1] + " there is the " + hitColliderName (commandList [1]);
										if (!hitCollider (commandList [1]))
												output += "road.";
										commandList.RemoveRange (0, 3); //USUN jedną KOMENDĘ
										return 0;
								}
						} else { //jesli nie podal co usunac tylko kierunek
								if (String.Equals ((hitColliderName (commandList [1])), "grass")) {
										output += "\n[robot]> There is nothing to destroy.";
										commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
										return 0;
								} else if (!hitCollider (commandList [1])) {
										output += "\n[robot]> You mustn't destroy the road!";
										commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
										return 0;
								} else if (containsNietBud (hitColliderName (commandList [1]))) {
										output += "\n[robot]> You mustn't destroy city's property!";
										commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
										return 0;
								} else if (containsRemains (hitColliderName (commandList [1]))) { //TO DO clear
										destroyPrefab (commandList [1]);
										output += "\n[robot]> I've cleared what you'd wanted me to.";
										commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
										return 0;
								} else {
										matchRemains (hitColliderName (commandList [1]), commandList [1]);
										commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
										return 0;
								}
						}
				
				}
		
		//nie bylo kierunku ale napisal co zniszczyc
		else if (commandList.Count > 1 && (containsNietBud (commandList [1]) || containsBuilding (commandList [1])
						|| containsRemains (commandList [1]) || String.Equals (commandList [1], "tree") || String.Equals (commandList [1], "tree(Clone)"))) {

						if (containsNietBud (commandList [1])) {
								output += "\n[robot]> You mustn't destroy city's property!";
								commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
								return 0;
						}
						string N = "";
						string S = "";
						string E = "";
						string W = "";
						x = 0;
						y = 0; 

		    
						if (hitCollider ("north") && hitColliderName ("north").Contains (commandList [1])) { //jesli jest na polnocy to co chce skasowac
								N = "north";
						}
						if (hitCollider ("south") && hitColliderName ("south").Contains (commandList [1])) {
								S = "south";
						}
						if (hitCollider ("east") && hitColliderName ("east").Contains (commandList [1])) {
								E = "east";
						}
						if (hitCollider ("west") && hitColliderName ("west").Contains (commandList [1])) {
								W = "west";
						}
			
						if (String.Equals (N, "") && String.Equals (E, "") && String.Equals (W, "") && String.Equals (S, "")) { //nic nie jest prawdziwe
								output += "\n[robot]> The thing you are trying to destroy is not here.";
								commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
								return 0;
						} else if ((N.Length + S.Length + W.Length + E.Length) < 6) { //tylko jedno prawdziwe
								if (String.Equals (N, "north")) {//jesli cos jest na polnocy
										y = i;
										//clear
										if (containsRemains (commandList [1])) { //sprawdza czy to pieńki/popioły/ruiny
												destroyPrefab ("north");
												output += "\n[robot]> I've cleared " + commandList [1] + ".";
												commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
												return 0;
										} else {
												matchRemains (commandList [1], "north");
												commandList.RemoveRange (0, 2);
												return 0;  
										}
								}
								if (String.Equals (S, "south")) {//jesli cos jest na polnocy
										y = -i;
										//clear
										if (containsRemains (commandList [1])) { //sprawdza czy to pieńki/popioły/ruiny
												destroyPrefab ("south");
												output += "\n[robot]> I've cleared " + commandList [1] + ".";
												commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
												return 0;
										} else {
												matchRemains (commandList [1], "south");
												commandList.RemoveRange (0, 2);
												return 0;  
										}
								}
								if (String.Equals (E, "east")) {//jesli cos jest na polnocy
										x = i;
										//clear
										if (containsRemains (commandList [1])) { //sprawdza czy to pieńki/popioły/ruiny
												destroyPrefab ("east");
												output += "\n[robot]> I've cleared " + commandList [1] + ".";
												commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
												return 0;
										} else {
												matchRemains (commandList [1], "east");
												commandList.RemoveRange (0, 2);
												return 0;  
										}
								}
								if (String.Equals (W, "west")) {//jesli cos jest na polnocy
										x = -i;
										//clear
										if (containsRemains (commandList [1])) { //sprawdza czy to pieńki/popioły/ruiny
												destroyPrefab ("west");
												output += "\n[robot]> I've cleared " + commandList [1] + ".";
												commandList.RemoveRange (0, 2); //USUN jedną KOMENDĘ
												return 0;
										} else {
												matchRemains (commandList [1], "west");
												commandList.RemoveRange (0, 2);
												return 0;    
										} 
								}
						} else { //jesli wiecej niz 1 jest prawdziwe

								output += "\n[robot]> You can destroy a " + commandList [1] + " in the " + N + " " + S + " " + E + " " + W + ". Which one do you choose?";
				

								//specjalna komenda
								specialCommand = true;
								return 0;

								//z odpowiedzi uzytkownika wyciaga kierunek i wciska go na 2 miejsce commandList[1]
								//potem wykonuje fork od nowa posiadajac juz cala komende
								//jeśli nie podał kierunku to fork znowu to zweryfikuje

						}
						return 0;	
				}
		//nie podal kierunku ani co zniszczyc
		else {
			output += "\n[robot]> You didn't say in what direction. If you want, name your direction, if not just say no.";
			specialCommand=true;
			return 0;
		}
		

	}	

}







