using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class textScript : MonoBehaviour {
	public InputField userInput;
	public Text roboTalk;
	public string output="\nCzekam na polecenia.";
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
	private bool no=false, constructions=false, directions=false;
	
	void OnSubmit(string line) {
		string commandLog;
		//Debug.Log ("OnSubmit("+line+")");
		commandText = DateTime.Now.ToString("h:mm:ss tt") +  "\nUzytkownik: \n "+ line;
		//		roboText = Parser (line);
		output += "\n[user]> "+line;
		commandLog = Parser (line);
		if ( String.Equals(commandLog,"pusta") ) {
			output += "\n[robot]> I don't understand you.";
		}	
		else 
			stage_1 ();
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
        commands.Add("trunks",3);
        commands.Add("ruins",3);
        commands.Add("ashes",3);
        commands.Add("tree",3);
		commands.Add("north",2);
		commands.Add("south",2);
		commands.Add("east",2);
		commands.Add("west", 2);
		commands.Add("?",0);
	}
	string Parser(string command) {
		//JEŚLI ZNAK ZAPYTANA TO LOSUJ JEDNĄ Z ODPOWIEDZI ELSE PONIŻSZE
		string[] words = command.Split(new Char [] {' ', ',', '.', ':', '\t', '?' }); // wyrzucić "?"
		List<string> tempCommand = new List<string>();
		string buildWord = "";
		//posortować stringa
		foreach (var word in words) {
			//if  exists word < processState break i powiedz jestem w trakcie wykonywania polecenia nie mogę zbudować/ zburzyć 
			//robot powtarza jeszcze raz czego potrzebuje
			if (commands.ContainsValue(commands[word])) {
				if (String.Equals(commands[word],1) /*&& (1 > processState)*/) {
					buildWord = word;
					tempCommand.Add (word);
				}
				if (String.Equals(commands[word],2) /*&& (processState == 2)*/) {
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

                //odpowiedź użytkownika na zadanie pytania 
                if (specialCommand) {
                    //sprawdza czy podal kierunek do funkcji destroy
                    if ( (String.Equals(commandList[0],"destroy") || (String.Equals(commandList[0],"clear"))) && containsDirection(word)) { 
                        commandList.Insert(1,word); // wciska kierunek na commanList[1]
                    }


					//pytanie: do you want to build ..
					//odp: I don't want to build a shed
					//to wcisnie do listy
					//a potem, skoro no==true to usunie komende

					//zapytanie o kierunek
					if (String.Equals(commandList[0],"build") && (directions) && (containsDirection(word)) ) {

					 
						if ( containsDirection(commandList[1]) )//jesli byl tam stary kierunek
							commandList[1]=word;
						else
							commandList.Insert(1,word); //wsadz kierunek na commandList[1]
					}
					//zapytanie o budynek
					if (String.Equals(commandList[0],"build") && (constructions) && (containsBuilding(word)) ) {	
						if ( containsBuilding(commandList[2]) )//jesli byl tam stary budynek
							commandList[1]=word;
						else
							commandList.Insert(2,word); //wsadz budynek na commandList[2]
                    }
                }
			}
		} 
   		if (no) {
			commandList.RemoveRange(0, 3);
		}
                
      
        if (!specialCommand) {
            swap(tempCommand);
		    commandList.InsertRange(commandList.Count, tempCommand); // wstawia na koniec
        }	
        specialCommand = false;
		if (commandList.Count == 0)
			return "pusta"; //lista pusta lub zła komenda
		else
			return "poprawna"; // command list zawiera jakieś poprawne słowa
	}
	void swap(List<string> list) { //uproszoczny swap
		if ( (commandList.Count > 2) && (String.Equals(commands[commandList[1]],3)) 
		&& (String.Equals(commands[commandList[2]],2)) ) {
			string temp;
			temp = commandList[1];
			commandList[1] = commandList[2];
			commandList[2] = temp;
		}    
	}

//-----------------------------------------------------------------STAGE_1-------------------------------------------

	void stage_1() {
		Debug.Log ("jestem w stage_1");
		Debug.Log("slowo 1 = "+commandList[0]);

        if ( commandList.Count > 0 && String.Equals(commandList[0], "go") ) {
  			go();
  		}
		if ( commandList.Count > 0 && String.Equals(commandList[0], "money") ) {
			takeMoney();
		}		
		
		if ( commandList.Count > 0 && String.Equals(commandList[0], "build") ){
  			build();
  		}

        if ( commandList.Count > 0 && String.Equals(commandList[0], "destroy") ){
  		    destroy();  
        }
        if ( commandList.Count > 0 && String.Equals(commandList[0], "clear") ){
			destroy();
  		}
		if ( !specialCommand && commandList.Count > 0 )
			stage_1();
	}
//------------------------------------------------------------TAKE MONEY---------------------------------------------
	void takeMoney(){
		//jesli jest przy banku
		if ( (String.Equals (hitColliderName("north"), "bank")) || (String.Equals (hitColliderName("south"), "bank")) ||
			(String.Equals (hitColliderName("east"), "bank")) || (String.Equals (hitColliderName("west"), "bank")) ) {
			//jesli ma wystarczajaco pieniedzy
			if (Money.money > 299)
				output += "\n[robot]> You have enough! Don't be greedy!";
			else{
				Money.money = Money.money + 100;
				output += "\n[robot]> I've withdrawn $100 for you.";
			}
		}
		//jesli nie ma go brzy banku
		else
			output += "\n[robot]> You have to go to the bank.";
	}

//------------------------------------------------------------GO---------------------------------------------

	int go() {
		//Debug.Log(commandList [0]);
		//Debug.Log(commandList [1]);
		//Debug.Log(commandList [2]);
		// jeśli podał kierunek
		int isDirection = 0;
		//Debug.Log("sprawdzam up");
		if ( commandList.Count > 1 && String.Equals(commandList[1],"up") ) {
			isDirection=1;
			//Debug.Log("jestem w up");
			if ( hitCollider("north")==false ) {//jesli nic nie stoi na przeszkodzie
				//Debug.Log("czysto");
				rigidbody2D.transform.position += new Vector3 (0, i, 0) /* Time.deltaTime*/;
			}
			else {
				//Debug.Log("przesszkoda!");
				output +=  "You can't go in this direction. The "+(hitColliderName("north"))+" is there." ;
			}
			commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
			return 0;
		}
	///JESLI NIE PODA SIE KIERUNKU CZYLI SAMO "GO" ON SIE WYSYPUJE NA SPRAWDZANIU UP I KAPUT		CZEMU?????
			//Debug.Log("sprawdzam down");
		if ( commandList.Count > 1 && String.Equals(commandList[1],"down") ) {
				isDirection=1;
				if ( hitCollider("south")==false ) {
					rigidbody2D.transform.position += new Vector3 (0, -i, 0);
				}
				else {
					output += "\n[robot]> You can't go in this direction. The "+hitColliderName("south")+" is there." ;
				}
			commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
			return 0;
		}
		if ( commandList.Count > 1 && String.Equals(commandList[1],"right") ) {
			isDirection=1;
			if ( hitCollider("east")==false ) {
				//Debug.Log("czysto");
				rigidbody2D.transform.position += new Vector3 (i, 0, 0);
			}
			else {
				output += "\n[robot]> You can't go in this direction. The "+hitColliderName("east")+" is there." ;
			}
			commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
			return 0;
		}

		if ( commandList.Count > 1 && String.Equals(commandList[1],"left") ) {
			isDirection=1;
			if ( hitCollider("west")==false ) {
				rigidbody2D.transform.position += new Vector3 (-i, 0, 0);
			}
			else{
				output += "\n[robot]> You can't go in this direction. The "+hitColliderName("west")+" is there." ;
			}
			commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
			return 0;
		}


		//jesli nie podal kierunku
		if (isDirection==0) {
			output += "\n[robot]> You didn't say where!";
            commandList.RemoveRange(0,1);
            return 0;
		}
        return 0;
	}


//-----------------------------------------------------------------------BUILD----------------------------------------------


	void offerHouse(){
		if ( Money.money < shedMoney ) //nie ma na nic pieniedzy
			output += "\n[robot]> You don't have money for any construction. You have to go to the bank and withdraw the money.";
		
		if ( Money.money >= villaMoney )
			output += "\n[robot]> You can build a villa, a house or a shed there. Which one do you choose?";
		
		if ( Money.money >= houseMoney )
			output += "\n[robot]> You can build a house or a shed. Which one do you choose?";
		else
			output += "\n[robot]> You can build only a shed.";
		
		output += "\n[robot]> If you want to build a cheaper construction name it. If you don't just say 'no'";
		
		specialCommand=true;
		constructions=true;

	}
	
	
	
	
	

	//GDZIE BUDUJE
		
	void build(){
		if (containsDirection (commandList [1])) {   //jesli podal kierunek
		
			x = 0;
			y = 0;
			if (String.Equals (commandList [1], "north"))
					y = i;
			if (String.Equals (commandList [1], "south"))
					y = -i;
			if (String.Equals (commandList [1], "west"))
					x = -i;
			if (String.Equals (commandList [1], "east"))
					x = i;

			//jesli tam jest trawa to buduj
			if (String.Equals (hitColliderName (commandList [1]), "grass")) 
				construct (x, y);

			else {//podal zly kierunek - tam cos jest
				if (containsNietBud (commandList [1]))
					output += "\n[robot]> You mustn't destroy city's property!";
				else
					output += "\n[robot]> First you have to get rid of " + hitColliderName (commandList [1]) + ".";
 
				offerDirections ();

			}
		}
		
		else  //jesli nie podal kierunku
			offerDirections();

		
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
		


		if ( String.Equals(N,"") && String.Equals(E,"") && String.Equals(W,"") && String.Equals(S,"") ) //nigdzie nie ma trawy
			output += "\n[robot]> There is no ground you can build on. you have to either move or clear the area.";

		else{//gdzies jest trawa
			output += "\n[robot]> If you want you can build in the "+N+" "+S+" "+E+" "+W+".\nIf so choose a direction, if not just say 'no'.";
			specialCommand=true;//z kierunkiem
			directions=true;

		}

	}

	

	
	
	//CO BUDUJE
		
	void construct(int x, int y){
		//jesli nie podal co ma zbudowac
		if ( !containsBuilding(commandList[2]) )
			offerHouse();
			
		else { //jesli podal co ma zbudowac
			if ( String.Equals(commandList[2], "shed") ){
				if ( Money.money < shedMoney ){
					output += "\n[robot]> You don't have enough money";
					offerHouse();
				}
				else
					addPrefab(x, y, "shed" );
			}

			if ( String.Equals(commandList[2], "house") ){
				if ( Money.money < shedMoney ){
					output += "\n[robot]> You don't have enough money";
					offerHouse();
				}
				else
					addPrefab(x, y, "house" );
			}

			if ( String.Equals(commandList[2], "villa") ){
				if ( Money.money < shedMoney ){
					output += "\n[robot]> You don't have enough money";
					offerHouse();
				}
				else
					addPrefab (x, y, "villa" );
			}		
		}
			
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
    	if (containsDirection(commandList[1])) { // jeśli podał kierunek
    			
    	    if ( String.Equals(commandList[1],"north") )
    			y=i;
    		if ( String.Equals(commandList[1],"south") )
    			y=-i;
    		if ( String.Equals(commandList[1],"west") )
    			x=-i;
    		if ( String.Equals(commandList[1],"east") )
    			x=i;	
            //Debug.Log(commandList.Count);
            if ( commandList.Count > 2 && ((containsBuilding(commandList[2])) || (String.Equals(commandList[2],"tree"))) )  { //jesli podal budynek lub drzewo
    		    if ( (containsBuilding(hitColliderName(commandList[1]))) || (String.Equals(hitColliderName(commandList[1]),"tree")) ){//czy w kierunku ktory podal jest budynek/drzewo ktory podal

    				matchRemains( commandList[2], commandList[1]);
    				commandList.RemoveRange(0,3); //USUN jedną KOMENDĘ
    				return 0;
    			}
    			else { //w tym kierunku jest cos innego
    				// np na gorze jest home a on pisze "Destroy shed in the north."
    				output += "\n[robot]> You can't destroy the "+commandList[2]; //You can't destroy a shed
    				output += "\n[robot]> In the "+commandList[1]+" there is/are the "+hitColliderName(commandList[1]); //in the north there is a home.
    				commandList.RemoveRange(0,3); //USUN jedną KOMENDĘ
    				return 0;
    						
    						
    			}
    		}
    			//jesli podal cokolwiek do clear
            if ( commandList.Count > 2 && containsRemains(commandList[2]) ) { //podal co wyczyscic
    				 
    				//hitColliderName dostaje kierunek zwraca nazwe collidera
    				//containsRemains porownuje slowo dostane z ruins/ashes/trunks
    			if (containsRemains(hitColliderName(commandList[1])) ){//czy w kierunku ktory podal jest pozostalosc ktora podal
    				destroyPrefab(commandList[1]);
    				commandList.RemoveRange(0,3); //USUN jedną KOMENDĘ
    				return 0;
    			}
    			else {
    				output += "\n[robot]> You can't clear "+commandList[2]; 
    				output += "\n[robot]> In the "+commandList[1]+" there is the " + hitColliderName(commandList[1]);
    				commandList.RemoveRange(0,3); //USUN jedną KOMENDĘ
    				return 0;
    			}
    		}
			
			else { //jesli nie podal co usunac tylko kierunek
				if (String.Equals ((hitColliderName(commandList[1])), "grass")) {
					output += "\n[robot]> There is nothing to destroy.";
					commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
					return 0;
				}
				if (containsNietBud(hitColliderName(commandList[1])) ){
					output += "\n[robot]> You mustn't destroy city's property!";
					commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
					return 0;
				}
				if ( containsRemains(hitColliderName(commandList[1])) ){ //TO DO clear
					destroyPrefab(commandList[1]);
					output += "\n[robot]> I've cleared what you'd wanted me to.";
					commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
					return 0;
				}
				else {
					matchRemains(hitColliderName(commandList[1]),commandList[1]);
					commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
					return 0;
				}
			}
				
		}
		else { //jesli nie bylo kierunku musial napisac co zniszczyc

			if (containsNietBud(commandList[1]) ){
				output += "\n[robot]> You mustn't destroy city's property!";
				commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
				return 0;
			}
			string N="";
			string S="";
			string E="";
			string W="";
			x=0;
			y=0; 

		    
            if ( hitCollider("north") && hitColliderName("north").Contains(commandList[1]) ) { //jesli jest na polnocy to co chce skasowac
				N="north";
			}
            if ( hitCollider("south") && hitColliderName("south").Contains(commandList[1]) ) {
				S="south";
			}
            if ( hitCollider("east") && hitColliderName("east").Contains(commandList[1]) ) {
				E="east";
			}
			if ( hitCollider("west") && hitColliderName("west").Contains(commandList[1]) ) {
				W="west";
			}
			
			if ( String.Equals(N,"") && String.Equals(E,"") && String.Equals(W,"") && String.Equals(S,"") ){ //nic nie jest prawdziwe
				output += "\n[robot]> The thing you are trying to destroy is not here.";
				commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
				return 0;
			}
		    else if ( (N.Length + S.Length + W.Length + E.Length) < 6 ){ //tylko jedno prawdziwe
				if ( String.Equals(N,"north") ) {//jesli cos jest na polnocy
					y=i;
					//clear
					if ( containsRemains(commandList[1]) ){ //sprawdza czy to pieńki/popioły/ruiny
						destroyPrefab("north");
						output += "\n[robot]> I've cleared "+commandList[1]+".";
						commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
						return 0;
					}
					else {
						matchRemains( commandList[1],"north" );
                        commandList.RemoveRange(0,2);
                        return 0;  
                    }
				}
				if ( String.Equals(S,"south") ) {//jesli cos jest na polnocy
					y=-i;
					//clear
					if ( containsRemains(commandList[1]) ){ //sprawdza czy to pieńki/popioły/ruiny
						destroyPrefab("south");
						output += "\n[robot]> I've cleared "+commandList[1]+".";
						commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
						return 0;
					}
					else {
						matchRemains( commandList[1],"south" );
                        commandList.RemoveRange(0,2);
                        return 0;  
                    }
				}
				if ( String.Equals(E,"east") ) {//jesli cos jest na polnocy
					x=i;
					//clear
					if ( containsRemains(commandList[1]) ){ //sprawdza czy to pieńki/popioły/ruiny
						destroyPrefab("east");
						output += "\n[robot]> I've cleared "+commandList[1]+".";
						commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
						return 0;
					}
					else {
						matchRemains( commandList[1],"east" );
                        commandList.RemoveRange(0,2);
                        return 0;  
                    }
				}
				if ( String.Equals(W,"west") ) {//jesli cos jest na polnocy
					x=-i;
					//clear
					if ( containsRemains(commandList[1]) ){ //sprawdza czy to pieńki/popioły/ruiny
						destroyPrefab("west");
						output += "\n[robot]> I've cleared "+commandList[1]+".";
						commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
						return 0;
					}
					else {
						matchRemains( commandList[1],"west" );
                        commandList.RemoveRange(0,2);
                        return 0;    
                    } 
                }
            }
			else { //jesli wiecej niz 1 jest prawdziwe

				output += "\n[robot]> You can destroy a "+ commandList[1] +" in the "+N+" "+S+" "+E+" "+W+". Which one do you choose?";
				

				//specjalna komenda
				specialCommand = true;
				return 0;

			    //z odpowiedzi uzytkownika wyciaga kierunek i wciska go na 2 miejsce commandList[1]
				//potem wykonuje stage_1 od nowa posiadajac juz cala komende
				//jeśli nie podał kierunku to stage_1 znowu to zweryfikuje

			}
			return 0;	
		}
		

	}	


}




