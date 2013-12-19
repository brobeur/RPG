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
	enum square {Grass = 1, Water = 2, Floor = 3, Walls = 4, Path = 5, Player = 6, WallBase = 7};

	// the dimension of the map (must be square)
	static int mapSize = 14;
	// block is used to find blocks that the play is not allowed to move onto
	GameObject[] block;
	GameObject[] saveWalls;
	// squareType keeps track of the map strickly in a sense of open and non-open blocks
	square[,] squareType = new square[mapSize,mapSize];
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
		MakeMapObjects (square.Grass);
		MakeMapObjects (square.Water);
		MakeMapObjects (square.Floor);
		MakeMapObjects (square.Walls);
		MakeMapObjects (square.WallBase);
		MakeMapObjects (square.Path);
		MakeMapObjects (square.Player);
	}

	// Makes the map, just adds the blocks that shouldnt be passable to an array
	// blocks such as water and other characters will be in here
	void MakeMapObjects (square target){
		string tagVal = "unknown";
		if (target == square.Grass) {tagVal = "Grass";}
		else if (target == square.Water) {tagVal = "Water";}
		else if (target == square.Floor) {tagVal = "Floor";}
		else if (target == square.Walls) {tagVal = "Walls";}
		else if (target == square.Path) {tagVal = "Path";}
		else if (target == square.Player) {tagVal = "Player";}
		else if (target == square.WallBase) {tagVal = "WallBase";}

		block = GameObject.FindGameObjectsWithTag (tagVal);
		for (int i = 0; i < block.Length; i++) {
			posx = (int) block[i].transform.position.x;
			posy = (int) block[i].transform.position.y;
			squareType[posx,posy] = target;
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
			if (squareType[playerPosx,playerPosy+1] == square.Grass |
			    squareType[playerPosx,playerPosy+1] == square.Path  |
			    squareType[playerPosx,playerPosy+1] == square.Floor){
				playerPosy = playerPosy + 1;
				transform.position = new Vector2(playerPosx, playerPosy);
				// If your characters current position is on a floor (as in inside)
				// and where we came from was not floor, then we need to hide the walls
				if (squareType [playerPosx, playerPosy-1] != square.Floor &
				    squareType [playerPosx, playerPosy] == square.Floor) {
					saveWalls = GameObject.FindGameObjectsWithTag ("Walls");
					for (int i = 0; i < saveWalls.Length; i++) {
						saveWalls[i].SetActive(false);
					}
					// Remap the floor square so we can walk on them :)
					MakeMapObjects (square.Floor);
				} else if (squareType [playerPosx, playerPosy-1] == square.Floor &
				           squareType [playerPosx, playerPosy] != square.Floor) {
					for (int i = 0; i < saveWalls.Length; i++) {
						saveWalls[i].SetActive(true);
					}
					// Remap the walls so we are restricted again
					MakeMapObjects (square.Walls);
				}
			}
		} else if (Input.GetKey (moveDown) & playerPosy > 0) {
			if (squareType[playerPosx,playerPosy-1] == square.Grass |
			    squareType[playerPosx,playerPosy-1] == square.Path  |
			    squareType[playerPosx,playerPosy-1] == square.Floor) {
				playerPosy = playerPosy - 1;
				transform.position = new Vector2(playerPosx, playerPosy);
				// If your characters current position is on a floor (as in inside)
				// and where we came from was not floor, then we need to hide the walls
				if (squareType [playerPosx, playerPosy+1] != square.Floor &
				    squareType [playerPosx, playerPosy] == square.Floor) {
					saveWalls = GameObject.FindGameObjectsWithTag ("Walls");
					for (int i = 0; i < saveWalls.Length; i++) {
						saveWalls[i].SetActive(false);
					}
					// Remap the floor square so we can walk on them :)
					MakeMapObjects (square.Floor);
				} else if (squareType [playerPosx, playerPosy+1] == square.Floor &
				          squareType [playerPosx, playerPosy] != square.Floor) {
					for (int i = 0; i < saveWalls.Length; i++) {
						saveWalls[i].SetActive(true);
					}
					// Remap the walls so we are restricted again
					MakeMapObjects (square.Walls);
				}
			}
		} else if (Input.GetKey (moveLeft) & playerPosx > 0) {
			if (squareType[playerPosx-1,playerPosy] == square.Grass |
			    squareType[playerPosx-1,playerPosy] == square.Path  |
			    squareType[playerPosx-1,playerPosy] == square.Floor) {
				playerPosx = playerPosx - 1;
				transform.position = new Vector2(playerPosx, playerPosy);
				// If your characters current position is on a floor (as in inside)
				// and where we came from was not floor, then we need to hide the walls
				if (squareType [playerPosx+1, playerPosy] != square.Floor &
				    squareType [playerPosx, playerPosy] == square.Floor) {
					saveWalls = GameObject.FindGameObjectsWithTag ("Walls");
					for (int i = 0; i < saveWalls.Length; i++) {
						saveWalls[i].SetActive(false);
					}
					// Remap the floor square so we can walk on them :)
					MakeMapObjects (square.Floor);
				} else if (squareType [playerPosx+1, playerPosy] == square.Floor &
				           squareType [playerPosx, playerPosy] != square.Floor) {
					for (int i = 0; i < saveWalls.Length; i++) {
						saveWalls[i].SetActive(true);
					}
					// Remap the walls so we are restricted again
					MakeMapObjects (square.Walls);
				}
			}
		} else if (Input.GetKey (moveRight) & playerPosx < mapSize - 1) {
			if (squareType[playerPosx+1,playerPosy] == square.Grass |
			    squareType[playerPosx+1,playerPosy] == square.Path  |
			    squareType[playerPosx+1,playerPosy] == square.Floor) {
				playerPosx = playerPosx + 1;
				transform.position = new Vector2(playerPosx, playerPosy);
				// If your characters current position is on a floor (as in inside)
				// and where we came from was not floor, then we need to hide the walls
				if (squareType [playerPosx-1, playerPosy] != square.Floor &
				    squareType [playerPosx, playerPosy] == square.Floor) {
					saveWalls = GameObject.FindGameObjectsWithTag ("Walls");
					for (int i = 0; i < saveWalls.Length; i++) {
						saveWalls[i].SetActive(false);
					}
					// Remap the floor square so we can walk on them :)
					MakeMapObjects (square.Floor);
				} else if (squareType [playerPosx-1, playerPosy] == square.Floor &
				          squareType [playerPosx, playerPosy] != square.Floor) {
					for (int i = 0; i < saveWalls.Length; i++) {
						saveWalls[i].SetActive(true);
					}
					// Remap the walls so we are restricted again
					MakeMapObjects (square.Walls);
				}
			}
		}
	}

}
