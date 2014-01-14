using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	// The map that we want to spawn enemies on
	Grid map;
	// used for the spawning of the Enemy type bug
	GameObject[] bugSpawns;
	GameObject bug;
	int maxNumOfBugs = 2;
	public int currNumOfBugs;
	float bugCooldown = 10f;
	bool bugSpawnCooldown;

	// Use this for initialization
	void Start () {
		// Find the grid of the map that we are on
		map = GameObject.FindGameObjectWithTag ("GridBuilder").GetComponent<Grid> ();
		// Find all the spawn points
		bugSpawns = GameObject.FindGameObjectsWithTag ("SpawnBug");
		// Get the orignal bug and we will use it for cloning
		bug = GameObject.FindGameObjectWithTag ("Bug");
		// set it to inactive because we are just using it as a blueprint for other bugs
		bug.SetActive (false);
		currNumOfBugs = 0;
		bugSpawnCooldown = true;
		StartCoroutine (SpawnCooldown("bug"));

	} 

	// Update is called once per frame
	void Update () {
		if (!bugSpawnCooldown & currNumOfBugs < maxNumOfBugs) {
			// flip the cooldown boolean
			bugSpawnCooldown = true;
			// start counting the cooldown
			StartCoroutine (SpawnCooldown("bug"));
			MakeClone ("bug");
		}
	}

	void MakeClone (string orignal) {
		// update the right enemy counter
		switch (orignal) {
			case ("bug"):
				currNumOfBugs++;
				// randomly generate a possible spawn point
				int spawnPoint = Random.Range(0, bugSpawns.Length);
				// save the position to spawn in
				int tempX = (int) bugSpawns[spawnPoint].transform.position.x;
				int tempY = (int) bugSpawns[spawnPoint].transform.position.y;
				// if we cant spawn it here re-find a spawn point
				// Game will go into infinte loop if objects are frozen on all the spawnPoints
				while (!map.mapGrid[tempX, tempY].CanIMoveHere()){
					spawnPoint = Random.Range(0, bugSpawns.Length);
					tempX = (int) bugSpawns[spawnPoint].transform.position.x;
					tempY = (int) bugSpawns[spawnPoint].transform.position.y;
				}
				// make a clone
			GameObject clone = (GameObject) Instantiate (bug, new Vector2 	(tempX, tempY), Quaternion.identity);
				clone.SetActive (true);
				break;
		}
	}

	IEnumerator SpawnCooldown(string cooldownObj) {
		switch (cooldownObj) {
			case ("bug"): 
			yield return new WaitForSeconds(bugCooldown);
			bugSpawnCooldown = false;
			break;
		}
	}
}
