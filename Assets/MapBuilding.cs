using UnityEngine;
using System.Collections;

public class MapBuilding : MonoBehaviour {

	// the dimension of the map (must be square)
	public static int mapSize = 14;

	public class characters {
		public string characterType;
		public GameObject gameObject;

		public characters (string type, GameObject obj){
			characterType = type;
			gameObject = obj;
		}
	}

	public class gameItem {
		public string itemType;
		
		public gameItem (string type){
			itemType = type;
		}
	}


	public class tile
	{
		public string tileType;
		public bool traversable;
		public characters character;
		public gameItem items;
		
		public tile (string type){
			tileType = type;
			if (type == "Grass" | type == "Floor" | type == "Path") {
				traversable = true;
			} else { 
				traversable = false;
			}
			items = null;
		}

		public bool CanIMoveHere(){
			if (!traversable | character != null){
				return false;
			} else {
				return true;
			}
		}
	}
	// block is used to find blocks
	public GameObject[] block;
	// gamechar is used to find characters
	public GameObject[] gameChar;
	// mapGrid keeps track of the map tiles
	public tile[,] mapGrid = new tile[mapSize,mapSize];
	// placeholders for finding coordinates of objects
	public int posx;
	public int posy;

	
	// Use this for initialization
	void Start () {
		BuildMap ();

	}

	//finds the character objects on the map and puts them into a tile
	public void PlaceCharacterObjects (string target){
		int positionX, positionY;
		gameChar = GameObject.FindGameObjectsWithTag (target);
		for (int i = 0; i < gameChar.Length; i++) {
			positionX = (int) gameChar[i].transform.position.x;
			positionY = (int) gameChar[i].transform.position.y;
			mapGrid[positionX,positionY].character = new characters (target, gameChar[i]);
		}
	}

	// Makes the map, just adds the blocks that shouldnt be passable to an array
	// blocks such as water and other characters will be in here
	public void PlaceMapObjects (string target){
		block = GameObject.FindGameObjectsWithTag (target);
		for (int i = 0; i < block.Length; i++) {
			posx = (int) block[i].transform.position.x;
			posy = (int) block[i].transform.position.y;
			mapGrid[posx,posy] = new tile (target);
		}
	}

	public void BuildMap(){
		// we need to call this function for each object
		PlaceMapObjects ("Grass");
		PlaceMapObjects ("Water");
		PlaceMapObjects ("Floor");
		PlaceMapObjects ("Walls");
		PlaceMapObjects ("WallBase");
		PlaceMapObjects ("Path");
		PlaceCharacterObjects ("NPC");
		PlaceCharacterObjects ("EnemyBug");
		PlaceCharacterObjects ("Players");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
