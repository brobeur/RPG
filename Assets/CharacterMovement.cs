// then maybe have the npc have a chat bubble or something interesting happen when you click on him

// have your inventory/character screen come up when you click on yourself

using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	KeyCode moveUp = KeyCode.W;
	KeyCode moveDown = KeyCode.S;
	KeyCode moveLeft = KeyCode.A;
	KeyCode moveRight = KeyCode.D;

	// block is used to find blocks that the play is not allowed to move onto
	GameObject[] block;
	GameObject[] saveWalls;

	// players position on the map
	int playerPosx;
	int playerPosy;
	// determines whether player is already moving
	bool isMoving = false;
	// temp value for distance player moves
	Vector2 moveDistance;
	// speed of player
	public int speed = 7;
	public MapBuilding map;

	// Use this for initialization
	void Start () {
		// the starting position of your character is where its placed.
		playerPosx = (int)transform.position.x;
		playerPosy = (int)transform.position.y;
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
		if (Input.GetKey (moveUp)) {
			if (map.mapGrid[playerPosx,playerPosy+1].traversable && isMoving == false){
				isMoving = true;
				playerPosy = playerPosy + 1;
				StartCoroutine (MoveToSpace(playerPosx, playerPosy));
				// If your characters current position is on a floor (as in inside)
				// and where we came from was not floor, then we need to hide the walls
				if (map.mapGrid [playerPosx, playerPosy-1] != map.Floor &
				    map.mapGrid [playerPosx, playerPosy] == map.Floor) {
					BringWallsDown();
				} else if (map.mapGrid [playerPosx, playerPosy-1] == map.Floor &
				           map.mapGrid [playerPosx, playerPosy] != map.Floor) {
					BringWallsUp();
				}
			}
		} else if (Input.GetKey (moveDown)) {
			if (map.mapGrid[playerPosx,playerPosy-1].traversable && isMoving == false){
				isMoving = true;
				playerPosy = playerPosy - 1;
				StartCoroutine (MoveToSpace(playerPosx, playerPosy));
				// If your characters current position is on a floor (as in inside)
				// and where we came from was not floor, then we need to hide the walls
				if (map.mapGrid [playerPosx, playerPosy+1] != map.Floor &
				    map.mapGrid [playerPosx, playerPosy] == map.Floor) {
					BringWallsDown();
				} else if (map.mapGrid [playerPosx, playerPosy+1] == map.Floor &
				           map.mapGrid [playerPosx, playerPosy] != map.Floor) {
					BringWallsUp();
				}
			}
		} else if (Input.GetKey (moveLeft)) {
			if (map.mapGrid[playerPosx-1,playerPosy].traversable && isMoving == false){
				isMoving = true;
				playerPosx = playerPosx - 1;
				StartCoroutine (MoveToSpace(playerPosx, playerPosy));
				// If your characters current position is on a floor (as in inside)
				// and where we came from was not floor, then we need to hide the walls
				if (map.mapGrid [playerPosx+1, playerPosy] != map.Floor &
				    map.mapGrid [playerPosx, playerPosy] == map.Floor) {
					BringWallsDown();
				} else if (map.mapGrid [playerPosx+1, playerPosy] == map.Floor &
				           map.mapGrid [playerPosx, playerPosy] != map.Floor) {
					BringWallsUp();
				}
			}
		} else if (Input.GetKey (moveRight)) {
			if (map.mapGrid[playerPosx+1,playerPosy].traversable && isMoving == false){
				isMoving = true;
				playerPosx = playerPosx + 1;
				StartCoroutine (MoveToSpace(playerPosx, playerPosy));
				// If your characters current position is on a floor (as in inside)
				// and where we came from was not floor, then we need to hide the walls
				if (map.mapGrid [playerPosx-1, playerPosy] != map.Floor &
				    map.mapGrid [playerPosx, playerPosy] == map.Floor) {
					BringWallsDown();
				} else if (map.mapGrid [playerPosx-1, playerPosy] == map.Floor &
				           map.mapGrid [playerPosx, playerPosy] != map.Floor) {
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
		map.PlaceMapObjects (map.Floor);
	}
	
	public void BringWallsUp(){
		for (int i = 0; i < saveWalls.Length; i++) {
			saveWalls[i].SetActive(true);
		}
		// Remap the walls so we are restricted again
		map.PlaceMapObjects (map.Walls);
	}

	IEnumerator MoveToSpace(int x,int y)
	{
		while (transform.position.x != x || transform.position.y != y)
		{
			// moving right
			if (transform.position.x < x)
			{
				moveDistance.x = speed * Time.deltaTime;
				// change move distance to only reach x in the case that it would go past it
				if (moveDistance.x + transform.position.x > x)
				{
					moveDistance.x = x - transform.position.x;
				}
			}
			// moving left
			else if (transform.position.x > x)
			{
				moveDistance.x = -speed * Time.deltaTime;
				// change move distance to only reach x in the case that it would go past it
				if (moveDistance.x + transform.position.x < x)
				{
					moveDistance.x = -(transform.position.x - x);
				}
			}
			else // x is already in position
			{
				moveDistance.x = 0;
			}
			// moving up
			if (transform.position.y < y)
			{
				moveDistance.y = speed * Time.deltaTime;
				// change move distance to only reach y in the case that it would go past it
				if (moveDistance.y + transform.position.y > y)
				{
					moveDistance.y = y - transform.position.y;
				}
			}
			// moving down
			else if (transform.position.y > y)
			{
				moveDistance.y = -speed * Time.deltaTime;
				// change move distance to only reach y in the case that it would go past it
				if (moveDistance.y + transform.position.y < y)
				{
					moveDistance.y = -(transform.position.y - y);
				}
			}
			else // y is already in position
			{
				moveDistance.y = 0;
			}
			transform.Translate(moveDistance);
			yield return 0;
		}
		// set so that player can move with new input again
		isMoving = false;
		// after we move we need to update the map so the grid changes
		// right now the whole map is being updated
		map.BuildMap ();
	}
}
