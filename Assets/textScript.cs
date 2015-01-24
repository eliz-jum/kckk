

/*
WCZYTUJE INPUT
	TWORZY CZYSTA KOMENDE W LISCIE
		NP Dear Robot please build me a beautidul house, a shed in the west and distroy this stupid tree! 
		BUILD HOUSE BUILD WEST SHED DISTROY TREE
		slowo_1 = build / distroy
		slowo_2 = north / west itp
		slowo_3 = shed / house / villa
		
		
		IF FOR EACH =="?"
		JEDNA Z ILUS UNIWERSALNYCH ODPOOWIEDZI
		ELSE
		stage_1();
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class textScript : MonoBehaviour {
	public InputField userInput;
	public Text roboTalk;
	public string output = "Czekam na polecenia.";
	private string commandText, roboText = "";
	private int processState;
	public List<string> wholeCommand = new List<string>();
	private List<string> commandList = new List<string>();
	private static Hashtable commands = new Hashtable();
	public float i = 0.8f;
	public float x, y = 0;
	public LayerMask myLayerMask;
	public Transform parposition;
	
	void OnSubmit(string line) {
		string commandLog;
		//Debug.Log ("OnSubmit("+line+")");
		commandText = DateTime.Now.ToString("h:mm:ss tt") +  "\nUzytkownik: \n   "+ line + "\n" ;
		//		roboText = Parser (line);
		output = line;
		commandLog = Parser (line);
		if ( String.Equals(commandLog,"pusta") ) {
			output = "Wpisałeś niepoprawną komendę.";
		}	
		else 
			stage_1 ();
		//output += commandText + "Robot:\n" + "   "+roboText; 
	}
	void Awake () { 
		userInput = GameObject.Find ("userInput").GetComponent<InputField>();
		userInput.onEndEdit.AddListener (((value) => OnSubmit(value)));
		//userInput.validation = InputField.Validation.Alphanumeric;	
		//tworzenie tablicy komend
		commands.Add("build",1);//drugi argument w nawiasie to będzie np. jakiś wskaźnik na funkcje
		commands.Add("destroy",1);
		commands.Add("clear",1);
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
		commands.Add("west",2);
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
				if (String.Equals(commands[word],3) || String.Equals(commands[word],4)) {
					tempCommand.Add (word);
				}
			}
		} 
		//ZAPYTAJ O DRUGIE SŁOWO JEŚLI NIE GO NIE MA
		commandList.InsertRange(commandList.Count, tempCommand); // wstawia na koniec
		//build house north
		swap(commandList);
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
	void stage_1() {
		Debug.Log ("jestem w stage_1");
		Debug.Log("slowo 1 = "+commandList[0]);
        if ( commandList.Count > 0 && String.Equals(commandList[0], "go") ) {
  			go();
  		}
  		/*

  		if commandList[0]==build{
  			stage_2();
  		}
  		*/
        if ( commandList.Count > 0 && String.Equals(commandList[0], "destroy") ){
  		    destroy();
  		}
        if ( commandList.Count > 0 && String.Equals(commandList[0], "clear") ){
  			destroy();
  		}
		if ( commandList.Count > 0 )
			stage_1 ();
	}
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
				output =  "You can't go in this direction. The "+(hitColliderName("north"))+" is there." ;
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
					output = "You can't go in this direction. The "+hitColliderName("south")+" is there." ;
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
				output = "You can't go in this direction. The "+hitColliderName("east")+" is there." ;
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
				output = "You can't go in this direction. The "+hitColliderName("west")+" is there." ;
			}
			commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
			return 0;
		}


		//jesli nie podal kierunku
		if (isDirection==0) {
			output = "You didn't say where!";
            commandList.RemoveRange(0,1);
            return 0;
		}
        return 0;
	}
	
	/*GDZIE BUDUJE
		
	void stage_2(){
		if commandList[1]== slowo_2   //jesli podal kierunek
		{
			//USTALANIE WSPOLRZEDNYCH
				float i = 0.8f;				TO MUSZA BYC ZMIENNE PUBLICZE ALBO PRZEKAZYWANE DO FUNKCJI
				float x=0;
			float y=0;
			if commandList[1]=="north"
				y=i;
			if commandList[1]=="south"
				y=-i;
			if commandList[1]=="west"
				x=-i;
			if commandList[1]=="east" //prawo
				x=i;
			
			
			if raycast na kierunek == grass 
				stage_3();
			
			
			else
				if raycast == tree
					"First You have to cut the tree"
						"Do U want to do it now?"
						if imput == "yes"
							destroyPrefab( ?, tree); //usowa drzewao
			addPrefab ( x, y, trunks) //pokazuje pienki
				stage_2(); //bo zostaly pienki wiec wykona sie jeszcze raz i wejdzie do kolejnego if'a
			//jesli nie to na koncu poda mozliwosc innych lokalizacji
			else 
				offer_directions();
			if  raycast == house/villa/shed
				prefab_name = raycast
					"First You have to distroy the"+prefab_name
					"Do U want to do it now(yes/no)?"
					if input == "yes"
						destroyPrefab( direction ?, prefab name) //usowa ville / dom / szalas
							if prefab_name == shed
								addPrefab ( x, y, ashes) //pokazuje popioly
									else
										addPrefab ( x, y, ruins) //pokazuje ruiny
											
											stage_2(); //bo zostaly popioly / ruiny
			else 
				offer_directions();
			
			if raycast == trunks || ruins || ashes
				prefab_name = raycast
					"U have to clear the "+prefab_name
					"Do U want to do it now?"
					if input == "yes"
						destroyPrefab( direction ?, prefab_name) //usowa pienki / ruiny / popioly
							stage_3(); //bo moze budowac
			else 
				offer_directions();
			if raycast == pegaz || zamek || fontanna || tory ||okraglak    //TRZEBA ZMIENIC NAZWY NA ANGIELSKIE!!!
				prefab_name = raycast //to w co uderzyl raycast
					"This is"+prefab_name+"! You can't build here!"
					offer_directions();
			
			
		}
		
		else { //jesli nie podal kierunku
			offer_directions();
			
		}
		
	}
	
	PROPONOWANIE KIERUNKOW
	void offer_directions(){
		if raycast(up)!= grass && (down) && (left) && (right) //jesli nigdzie nie moze budowac
			"There is no ground u can build on. U have to either move or clear the area."
				
				CZYSCI CALA LISTE/komendę
				
				else
					if raycast(up) == grass
						"You can build in the north."
							if raycast(down) == grass
								" You can build in the south."
									if raycast(left) == grass
										" You can build in the west."
											if raycast(right) == grass
												" You can build in the east."
													
													
													"Do u want to choose another direction?"
													if input == no
														BREAK ;
		else
			zapisuje zdanie
				szuka slowa_2 //np z ospowiedzi   Yes I want to build it in the north   wyciagnie north
				if nie ma słowa_2
					"Name your direction."
						zapisuje zdanie
						szuka slowa_2 //np z ospowiedzi   Yes I want to build it in the north   wyciagnie north	
						wpycha je do listy jako lista(2)
						stage_2();
		else 
			wpycha je do listy jako lista(2)
				stage_2();
		//wykonuje jeszcze raz stage_2 zeby sprawdzic czy nie wybral niedozwolonego kierunku
		
	}
	
	
	
	
	
	CO BUDUJE
		
	void Stage_3(){
		if commandList[2] != slowo_3{ //jesli nie podal co ma zbudowac
			sprawdz ile ma pieniedzy
				if money < shed_money
					"You don't have money for any construction. You have to go to the bank and withdraw the money."
						else
							offer_house();
			
			
			else { //jesli podal
				string prefab_name = commandList[2];
				if money < prefab_name.money //jesli mu nie starczy pieniedzy na to co chce zbudowac
					offer_house();
				
				else
					addPrefab(x, y, prefab_name) //funkcja budujaca
						USUN Z LISTY 3 pierwsze
						
						JESLI COS JESZCZE ZOSTALO W LISCIE TO 
						stage_1();
			}
			
			
			
		}
		
		
		
		void offer_house(){
			"You don't have enough money"
				"Do u want to build a cheaper house?"
					if money >= house_money
						"You can build a house or a shed."
							else
								"You can build only a shed."
									
									if input=="no"
										BREAK???
											USUN Z LISTY
											else
												input np "Yes, a shed" albo "Yes."
													zapisz zdanie
													for each szukaj slowa nr 2
														slowo_2 = znalezione slowo       -----zmienna publiczna!
															wcisnij ja do listy jako commandList[1]
															stage_3();
		}

*/
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
		if ( (String.Equals(slowo, "opera")) || (String.Equals(slowo, "opera(Clone)")) )
			return true;
		if ( (String.Equals(slowo, "railway")) || (String.Equals(slowo, "railway(Clone)")) )
			return true;
		if ( (String.Equals(slowo, "fountain")) || (String.Equals(slowo, "fountain(Clone)")) )
			return true;
		if ( (String.Equals(slowo, "castle")) || (String.Equals(slowo, "castle(Clone)")) )
			return true;
		if ( (String.Equals(slowo, "okraglak")) || (String.Equals(slowo, "okraglak(Clone)")) )
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
		}
		else if ( (String.Equals(prefab_name, "shed")) || (String.Equals(prefab_name, "shed(Clone)")) ){
				destroyPrefab(direction);
				addPrefab(x, y, "ashes");
		}
		else {
			destroyPrefab(direction);
			addPrefab(x, y, "ruins");
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
            Debug.Log(commandList.Count);
            if ( commandList.Count > 2 && ((containsBuilding(commandList[2])) || (String.Equals(commandList[2],"tree"))) )  { //jesli podal budynek lub drzewo
    		    if ( (containsBuilding(hitColliderName(commandList[1]))) || (String.Equals(hitColliderName(commandList[1]),"tree")) ){//czy w kierunku ktory podal jest budynek/drzewo ktory podal

    				matchRemains( commandList[2], commandList[1]);
    				commandList.RemoveRange(0,3); //USUN jedną KOMENDĘ
    				return 0;
    			}
    			else { //w tym kierunku jest cos innego
    				// np na gorze jest home a on pisze "Destroy shed in the north."
    				Debug.Log ("You can't destroy the "+commandList[2]); //You can't destroy a shed
    				Debug.Log ("In the "+commandList[1]+" there is/are the "+hitColliderName(commandList[1]) ); //in the north there is a home.
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
    				Debug.Log ("You can't clear "+commandList[2]); 
    				Debug.Log ("In the "+commandList[1]+" there is the " + hitColliderName(commandList[1]));
    				commandList.RemoveRange(0,3); //USUN jedną KOMENDĘ
    				return 0;
    			}
    		}
			
			else { //jesli nie podal co usunac tylko kierunek
				if (String.Equals ((hitColliderName(commandList[1])), "grass")) {
					Debug.Log("There is nothing to destroy.");
					commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
					return 0;
				}
				if (containsNietBud(hitColliderName(commandList[1])) ){
					Debug.Log("You mustn't destroy city's property!");
					commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
					return 0;
				}
				if ( containsRemains(hitColliderName(commandList[1])) ){ //TO DO clear
					destroyPrefab(commandList[1]);
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
			if (containsNietBud(hitColliderName(commandList[1])) ){
				Debug.Log("You mustn't destroy city's property!");
				commandList.RemoveRange(0,2); //USUN jedną KOMENDĘ
				return 0;
			}
			string N="";
			string S="";
			string E="";
			string W="";
			x=0;
			y=0; 

		    
            if ( hitCollider("north") && hitColliderName("west").Contains(commandList[1]) ) { //jesli jest na polnocy to co chce skasowac
				N="north";
			}
            if ( hitCollider("south") && hitColliderName("west").Contains(commandList[1]) ) {
				S="south";
			}
            if ( hitCollider("east") && hitColliderName("west").Contains(commandList[1]) ) {
				E="east";
			}
			if ( hitCollider("west") && hitColliderName("west").Contains(commandList[1]) ) {
				W="west";
			}
			
			if ( String.Equals(N,"") && String.Equals(E,"") && String.Equals(W,"") && String.Equals(S,"") ){ //nic nie jest prawdziwe
				Debug.Log ("The thing you are trying to destroy is not here.");
			}
		    else if ( (N.Length + S.Length + W.Length + E.Length) < 6 ){ //tylko jedno prawdziwe
				if ( String.Equals(N,"north") ) {//jesli cos jest na polnocy
					y=i;
					//clear
					if ( containsRemains(commandList[1]) ){ //sprawdza czy to pieńki/popioły/ruiny
						destroyPrefab("north");
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
					//int oldLength = commmandList.Count;
					Debug.Log ("U can destroy a "+ commandList[1] +" in the "+N+" "+S+" "+E+" "+W+". Which one do u choose?");

					 //z odpowiedzi uzytkownika tworzy czysta komende i wciska ja na 3 miejsce commandList
					//potem wykonuje stage_1 od nowa posiadajac juz cala komende
					//musi wiedziec ze to nie jest noemalna komenda ktora dostal tylko kierunek ktory ma wyjac i wcisnac do pierwszej

					//
				}
			return 0;	
		}
		

	}	


}




