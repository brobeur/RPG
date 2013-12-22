using UnityEngine;
using System.Collections;

public class MapBuilding : MonoBehaviour {

	// the dimension of the map (must be square)
	public static int mapSize = 14;
	
	public class tile
	{
		public string tileType;
		public bool traversable;
		
		public tile (string type, bool trav){
			tileType = type;
			traversable = trav;
		}
	}
	
	// tile types for this map
	public tile Grass = new tile ("Grass", true);
	public tile Water = new tile ("Water", false);
	public tile Floor = new tile ("Floor", true);
	public tile Walls = new tile ("Walls", false);
	public tile Path = new tile ("Path", true);
	public tile WallBase = new tile ("WallBase", false);
	public tile NPCs = new tile ("NPCs", false);
	public tile Enemies = new tile ("Enemies", false);
	public tile Players = new tile ("Players", false);
	
	// block is used to find blocks that the play is not allowed to move onto
	public GameObject[] block;
	public GameObject[] saveWalls;
	// mapGrid keeps track of the map tiles
	public tile[,] mapGrid = new tile[mapSize,mapSize];
	// placeholders for finding coordinates of objects
	public int posx;
	public int posy;

	
	// Use this for initialization
	void Start () {
		BuildMap ();
	}
	
	// Makes the map, just adds the blocks that shouldnt be passable to an array
	// blocks such as water and other characters will be in here
	public void PlaceMapObjects (tile target){
		block = GameObject.FindGameObjectsWithTag (target.tileType);
		for (int i = 0; i < block.Length; i++) {
			posx = (int) block[i].transform.position.x;
			posy = (int) block[i].transform.position.y;
			mapGrid[posx,posy] = target;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int GetMapSize (){
		return mapSize;
	}

	public void BuildMap(){
		// we need to call this function for each object
		PlaceMapObjects (Grass);
		PlaceMapObjects (Water);
		PlaceMapObjects (Floor);
		PlaceMapObjects (Walls);
		PlaceMapObjects (WallBase);
		PlaceMapObjects (Path);
		PlaceMapObjects (NPCs);
		PlaceMapObjects (Enemies);
		PlaceMapObjects (Players);
	}
}
