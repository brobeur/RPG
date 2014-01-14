using UnityEngine;
using System.Collections;

public class Bug : MonoBehaviour {
	// maxHealth is somewhere between 9-11
	public int maxHealth;
	public int currHealth;
	public bool isMoving;
	public int speed;
	// will be the grid of the map that we are on
	Grid map;
	GameObject spawner;
	// a temp values used for movement
	float moveDirection;
	public int bugPosX;
	public int bugPosY;

	// Use this for initialization
	void Start () {
		// Find the grid of the map that we are on
		map = GameObject.FindGameObjectWithTag ("GridBuilder").GetComponent<Grid> ();
		spawner = GameObject.FindGameObjectWithTag ("SpawnPoints");
		// set bug stats
		maxHealth = Random.Range (7, 14);
		currHealth = maxHealth;
		isMoving = false;
		speed = 2;
		bugPosX = (int)transform.position.x;
		bugPosY = (int)transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isMoving) {
			moveDirection = Random.value;
			if (moveDirection < 0.25f && map.mapGrid[bugPosX+1, bugPosY].CanIMoveHere()) {
				isMoving = true;
				bugPosX++;
				StartCoroutine (MoveToSpace(bugPosX, bugPosY));
			} else if (moveDirection < 0.5f  && map.mapGrid[bugPosX, bugPosY+1].CanIMoveHere()) {
				isMoving = true;
				bugPosY++;
				StartCoroutine (MoveToSpace(bugPosX, bugPosY));
			} else if (moveDirection < 0.75f  && map.mapGrid[bugPosX-1,bugPosY].CanIMoveHere()) {
				isMoving = true;
				bugPosX--;
				StartCoroutine (MoveToSpace(bugPosX, bugPosY));
			} else if (map.mapGrid[bugPosX,bugPosY-1].CanIMoveHere()){
				isMoving = true;
				bugPosY--;
				StartCoroutine (MoveToSpace(bugPosX, bugPosY));
			}
		}

		if (currHealth <= 0) {
			Destroy(gameObject);
			spawner.GetComponent<EnemySpawner> ().currNumOfBugs--;
		}
	}

	// This function is used to slow down the movement of the Bug.
	// It makes it so it looks like the bug walks from tile to tile
	// rather then just tranforming to the next tile.
	IEnumerator MoveToSpace(int x,int y)
	{
		// temp value for the distance of bug moves
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
		// we want the bug wander slowly, so he will pause for 1-3 seconds after each move
		yield return new WaitForSeconds((int)Random.Range(1, 3));
		// set so that player can move with new input again
		isMoving = false;
		
	}
}
