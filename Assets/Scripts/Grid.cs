using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	// Dimensions of the grid/map
	public static int mapSizeX = 28;
	public static int mapSizeY = 13;
	
	// Placeholder used for finding GameObjects
	public GameObject[] TileObjects;

	// The actual grid used for that map
	public Tile[,] mapGrid = new Tile[mapSizeX,mapSizeY];

	// A list of all the traversable tiles, This will be used for checking
	// to see if a Game Object is on one of these tiles
	List<Tile> TraversableTiles = new List<Tile>();
	List<GameObject> tempList = new List<GameObject>();

	// Use this for initialization
	void Start () {
		MapTheGrid ("Grass");
		MapTheGrid ("Water");
		MapTheGrid ("Path");
		MapTheGrid ("BuildingFloor");
		MapTheGrid ("BuildingWall");
		MapTheGrid ("BuildingRoof");
		MapTheGrid ("BuildingBase");
	}

	// Update is called once per frame
	void Update (){
		// This is probably inneficent but i think its easist to just have the
		// grid constantly checking for gameobjects and placing them where they belong.
		// And it should work for when we have multiple players as well
		PlaceGameObjects ();
	}

	// Helper function for mapping all the gameObjects (players, enemies, NPCs) on this map
	void PlaceGameObjects() {
		// Make sure the temp list of active game objects is cleared
		tempList.Clear ();
		// add all the game objects that are on this map to the list
		tempList.AddRange (GameObject.FindGameObjectsWithTag("Player"));
		tempList.AddRange (GameObject.FindGameObjectsWithTag("Bug"));
		// reset all tiles
		foreach (Tile tile in TraversableTiles) {
			tile.ObjectHere = null; 
		}
		// add the ones in the list to the grid
		foreach (GameObject obj in tempList) {
			int posX = (int) obj.transform.position.x;
			int posY = (int) obj.transform.position.y;
			mapGrid[posX, posY].ObjectHere = obj;
		}
	}

	// Used as a helper function to map the grid at the beggning of each Map
	void MapTheGrid(string target){
		TileObjects = GameObject.FindGameObjectsWithTag (target);
		for (int i = 0; i < TileObjects.Length; i++) {
			int posX = (int) TileObjects[i].transform.position.x;
			int posY = (int) TileObjects[i].transform.position.y;
			mapGrid[posX, posY] = new Tile (target);
			if (mapGrid[posX, posY].traversable){
				TraversableTiles.Add(mapGrid[posX, posY]);
			}
		}
	}
	
}
