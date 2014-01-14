using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Movement keys
	KeyCode moveUp = KeyCode.W;
	KeyCode moveDown = KeyCode.S;
	KeyCode moveLeft = KeyCode.A;
	KeyCode moveRight = KeyCode.D;
	// Combat Keys
	KeyCode attack = KeyCode.Space;
	// walking speed of the player
	public int speed = 3;
	// attacking cooldown of the player in seconds
	public int cooldown = 2;
	// The position of the player on the grid
	int playerPosX;
	int playerPosY;
	// Determines whether the player is already moving or attacking
	bool isMoving = false;
	bool isAttacking = false;
	// The map that this player is on
	Grid map;
	// Used as a temp variable to find and save walls/roofs for hiding them when you enter a building
	GameObject[] walls;
	GameObject[] roofs;
	// !!!!!!!!!!!!!!!!!!!!
	GameObject[] enemies;
	Bug bug;
	// Use this for initialization
	void Start () {
		// Find the grid of the map that we are on
		map = GameObject.FindGameObjectWithTag ("GridBuilder").GetComponent<Grid> ();
		// We know we will need to keep track of all of the walls because when
		// we walk into building we need to make them invisible.
		walls = GameObject.FindGameObjectsWithTag ("BuildingWall");
		roofs = GameObject.FindGameObjectsWithTag ("BuildingRoof");
		// Set the initial position of your player to where its placed in Unity
		playerPosX = (int)transform.position.x;
		playerPosY = (int)transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		// check to see if the user wants to move
		if (!isMoving) {
			if (Input.GetKey (moveUp) && map.mapGrid[playerPosX,playerPosY+1].CanIMoveHere()) {
				isMoving = true;
				playerPosY++;
				StartCoroutine (MoveToSpace(playerPosX, playerPosY));
			} else if (Input.GetKey (moveDown) && map.mapGrid[playerPosX,playerPosY-1].CanIMoveHere()) {
				isMoving = true;
				playerPosY--;
				StartCoroutine (MoveToSpace(playerPosX, playerPosY));
			} else if (Input.GetKey (moveRight) && map.mapGrid[playerPosX+1,playerPosY].CanIMoveHere()) {
				isMoving = true;
				playerPosX++;
				StartCoroutine (MoveToSpace(playerPosX, playerPosY));
			} else if (Input.GetKey (moveLeft) && map.mapGrid[playerPosX-1,playerPosY].CanIMoveHere()) {
				isMoving = true;
				playerPosX--;
				StartCoroutine (MoveToSpace(playerPosX, playerPosY));
			}
		}
		// check to see if the user wants to attack
		if (!isAttacking) {
			if (Input.GetKey (attack)) {
				isAttacking = true;
				StartCoroutine (BasicAttack());
			}
		}
	}

	// This function is used for the basic attack of a player
	// right now this function just gets all the bugs on the map,
	// then checks to see if each of the bugs are within 1 square of me
	// in any direction.
	IEnumerator BasicAttack() {
		enemies = GameObject.FindGameObjectsWithTag ("Bug");
		for (int i = 0; enemies.Length > i; i++) {
			bug = enemies[i].GetComponent<Bug> ();
			if ( (Mathf.Abs(playerPosX - bug.bugPosX) <= 1) && (Mathf.Abs(playerPosY - bug.bugPosY) <= 1) ) {
				bug.currHealth = bug.currHealth - 3;
				print ("Attack Hit");
			} else {
				print ("Attack Miss");
			}
		}
		yield return new WaitForSeconds (cooldown);
		isAttacking = false;
	}

	// This function is used to slow down the movement of the player.
	// It makes it so it looks like the player walks from tile to tile
	// rather then just tranforming to the next tile.
	IEnumerator MoveToSpace(int x,int y) {
		// temp value for the distance of player moves
		Vector2 moveDistance;

		while (transform.position.x != x || transform.position.y != y) {
			if (transform.position.x < x) { 
				// moving right
				moveDistance.x = speed * Time.deltaTime;
				// change move distance to only reach x in the case that it would go past it
				if (moveDistance.x + transform.position.x > x) {
					moveDistance.x = x - transform.position.x;
				}
			} else if (transform.position.x > x) { 
				// moving left
				moveDistance.x = -speed * Time.deltaTime;
				// change move distance to only reach x in the case that it would go past it
				if (moveDistance.x + transform.position.x < x) {
					moveDistance.x = -(transform.position.x - x);
				}
			} else {
				// x is already in position
				moveDistance.x = 0;
			} if (transform.position.y < y) {
				// moving up
				moveDistance.y = speed * Time.deltaTime;
				// change move distance to only reach y in the case that it would go past it
				if (moveDistance.y + transform.position.y > y) {
					moveDistance.y = y - transform.position.y;
				}
			} else if (transform.position.y > y) {
				// moving down
				moveDistance.y = -speed * Time.deltaTime;
				// change move distance to only reach y in the case that it would go past it
				if (moveDistance.y + transform.position.y < y) {
					moveDistance.y = -(transform.position.y - y);
				}
			} else {
				// y is already in position
				moveDistance.y = 0;
			}
			transform.Translate(moveDistance);
			yield return 0;
		}
		// Check to see if we need to bring the walls down or up
		CheckForWalls ();
		// set so that player can move with new input again
		isMoving = false;

	}

	// Checks to see if we are in a building or outside and will build the walls up or down
	void CheckForWalls(){
		if (map.mapGrid [playerPosX, playerPosY].AmIInside()) {
			for (int i = 0; i < walls.Length; i++) {
				walls[i].SetActive(false);
			}
			for (int i = 0; i < roofs.Length; i++) {
				roofs[i].SetActive(false);
			}
		} else {
			for (int i = 0; i < walls.Length; i++) {
				walls[i].SetActive(true);
			}
			for (int i = 0; i < roofs.Length; i++) {
				roofs[i].SetActive(true);
			}
		}
	}
}
