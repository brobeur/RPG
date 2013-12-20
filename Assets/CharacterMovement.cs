// I think i need to figure out how to slow movement down. so when you press w,a,s, or d you
// just move once forward and stop. It happens to quick right now

// then maybe have the npc have a chat bubble or something interesting happen when you click on him

// have your inventory/character screen come up when you click on yourself

// expand the map and see if i get errors.


using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	KeyCode moveUp = KeyCode.W;
	KeyCode moveDown = KeyCode.S;
	KeyCode moveLeft = KeyCode.A;
	KeyCode moveRight = KeyCode.D;

	// the dimension of the map (must be square)
	static int mapSize = 14;

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
	tile Grass = new tile ("Grass", true);
	tile Water = new tile ("Water", false);
	tile Floor = new tile ("Floor", true);
	tile Walls = new tile ("Walls", false);
	tile Path = new tile ("Path", true);
	tile Player = new tile ("Player", false);
	tile WallBase = new tile ("WallBase", false);

	// block is used to find blocks that the play is not allowed to move onto
	GameObject[] block;
	GameObject[] saveWalls;
	// mapGrid keeps track of the map tiles
	tile[,] mapGrid = new tile[mapSize,mapSize];
	// placeholders for finding coordinates of objects
	int posx;
	int posy;
	// players position on the map
	int playerPosx;
	int playerPosy;

	// Use this for initialization
	void Start () {
		// the starting position of your character.
		playerPosx = 0;
		playerPosy = 0;

		// we need to call this function for each object that you dont want to be able to walk on
		PlaceMapObjects (Grass);
		PlaceMapObjects (Water);
		PlaceMapObjects (Floor);
		PlaceMapObjects (Walls);
		PlaceMapObjects (WallBase);
		PlaceMapObjects (Path);
		PlaceMapObjects (Player);
	}

	// Makes the map, just adds the blocks that shouldnt be passable to an array
	// blocks such as water and other characters will be in here
	void PlaceMapObjects (tile target){
		block = GameObject.FindGameObjectsWithTag (target.tileType);
		for (int i = 0; i < block.Length; i++) {
			posx = (int) block[i].transform.position.x;
			posy = (int) block[i].transform.position.y;
			mapGrid[posx,posy] = target;
		}
	}

	// When this object is clicked on
	void OnMouseDown(){
		print ("clicked on yourself");
	}

	// Update is called once per frame
	void Update () {
		// Move your character when you press w,a,s,d. We must stay inside the map
		// and we cant move into blocks that don't allow it
		// allowable blocks are currently grass, path and floor
		if (Input.GetKey (moveUp) & playerPosy < mapSize - 1) {
			if (mapGrid[playerPosx,playerPosy+1].traversable){
				playerPosy = playerPosy + 1;
				transform.position = new Vector2(playerPosx, playerPosy);
				// If your characters current position is on a floor (as in inside)
				// and where we came from was not floor, then we need to hide the walls
				if (mapGrid [playerPosx, playerPosy-1] != Floor &
				    mapGrid [playerPosx, playerPosy] == Floor) {
					BringWallsDown();
				} else if (mapGrid [playerPosx, playerPosy-1] == Floor &
				           mapGrid [playerPosx, playerPosy] != Floor) {
					BringWallsUp();
				}
			}
		} else if (Input.GetKey (moveDown) & playerPosy > 0) {
			if (mapGrid[playerPosx,playerPosy-1].traversable) {
				playerPosy = playerPosy - 1;
				transform.position = new Vector2(playerPosx, playerPosy);
				// If your characters current position is on a floor (as in inside)
				// and where we came from was not floor, then we need to hide the walls
				if (mapGrid [playerPosx, playerPosy+1] != Floor &
				    mapGrid [playerPosx, playerPosy] == Floor) {
					BringWallsDown();
				} else if (mapGrid [playerPosx, playerPosy+1] == Floor &
				          mapGrid [playerPosx, playerPosy] != Floor) {
					BringWallsUp();
				}
			}
		} else if (Input.GetKey (moveLeft) & playerPosx > 0) {
			if (mapGrid[playerPosx-1,playerPosy].traversable) {
				playerPosx = playerPosx - 1;
				transform.position = new Vector2(playerPosx, playerPosy);
				// If your characters current position is on a floor (as in inside)
				// and where we came from was not floor, then we need to hide the walls
				if (mapGrid [playerPosx+1, playerPosy] != Floor &
				    mapGrid [playerPosx, playerPosy] == Floor) {
					BringWallsDown();
				} else if (mapGrid [playerPosx+1, playerPosy] == Floor &
				           mapGrid [playerPosx, playerPosy] != Floor) {
					BringWallsUp();
				}
			}
		} else if (Input.GetKey (moveRight) & playerPosx < mapSize - 1) {
			if (mapGrid[playerPosx+1,playerPosy].traversable) {
				playerPosx = playerPosx + 1;
				transform.position = new Vector2(playerPosx, playerPosy);
				// If your characters current position is on a floor (as in inside)
				// and where we came from was not floor, then we need to hide the walls
				if (mapGrid [playerPosx-1, playerPosy] != Floor &
				    mapGrid [playerPosx, playerPosy] == Floor) {
					BringWallsDown();
				} else if (mapGrid [playerPosx-1, playerPosy] == Floor &
				          mapGrid [playerPosx, playerPosy] != Floor) {
					BringWallsUp();
				}
			}
		}
	}

	public void BringWallsDown(){
		// find all the walls on the whole map (yes, it brings down all the walls.. whatevs)
		saveWalls = GameObject.FindGameObjectsWithTag ("Walls");
		for (int i = 0; i < saveWalls.Length; i++) {
			saveWalls[i].SetActive(false);
		}
		// Remap the floor square so we can walk on them :)
		PlaceMapObjects (Floor);
	}
	
	public void BringWallsUp(){
		for (int i = 0; i < saveWalls.Length; i++) {
			saveWalls[i].SetActive(true);
		}
		// Remap the walls so we are restricted again
		PlaceMapObjects (Walls);
	}
	
}
