using UnityEngine;
using System.Collections;

public class wander : MonoBehaviour {
	// determines if the enemy is moving already
	bool isMoving = false;
	// temp value for distance enemy moves
	Vector2 moveDistance;
	// speed of enemy
	public int speed = 5;
	// enemy position
	int enemyPosx;
	int enemyPosy;
	// the longest an enemy will wait after he walks, until he walks again
	public int waitTime = 3;
	// the map builder object
	public MapBuilding map;
	
	// Use this for initialization
	void Start () {
		// the starting position of the enemy is where it is placed
		enemyPosx = (int)transform.position.x;
		enemyPosy = (int)transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		if (!isMoving){
			isMoving = true;
			if (Random.value < 0.25f) {
				if (map.mapGrid[enemyPosx,enemyPosy + 1].traversable){
					isMoving = true;
					enemyPosy = enemyPosy + 1;
					StartCoroutine (MoveToSpace(enemyPosx, enemyPosy));
				} else {isMoving = false;}
			} else if (Random.value < 0.5f) {
				if (map.mapGrid[enemyPosx,enemyPosy - 1].traversable){
					isMoving = true;
					enemyPosy = enemyPosy - 1;
					StartCoroutine (MoveToSpace(enemyPosx, enemyPosy));
				} else {isMoving = false;}
			} else if (Random.value < 0.75f) {
				if (map.mapGrid[enemyPosx + 1,enemyPosy].traversable){
					isMoving = true;
					enemyPosx = enemyPosx + 1;
					StartCoroutine (MoveToSpace(enemyPosx, enemyPosy));
				} else {isMoving = false;}
			} else {
				if (map.mapGrid[enemyPosx - 1,enemyPosy].traversable){
					isMoving = true;
					enemyPosx = enemyPosx - 1;
					StartCoroutine (MoveToSpace(enemyPosx, enemyPosy));
				} else {isMoving = false;}
			}
		}
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
		map.BuildMap ();
		// we want the bug wander slowly, so he will pause after each move
		yield return new WaitForSeconds((int)Random.Range(0, waitTime));
		// set so that player can move with new input again
		isMoving = false;
	}
}
