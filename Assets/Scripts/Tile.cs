using UnityEngine;
using System.Collections;

public class Tile {
	// Each grid position will be represented as a tile
	public string tileType;
	public bool traversable;
	public GameObject ObjectHere;

	public Tile (string type) {
		tileType = type;
		ObjectHere = null;
		// Build the tile depending on the type
		string TypeBuilder = type;
		switch (TypeBuilder) {
			case "Grass":
				traversable = true;
				break;
			case "Water":
				traversable = false;
				break;
			case "Path":
				traversable = true;
				break;
			case "BuildingFloor":
				traversable = true;
				break;
			case "BuildingWall":
			// This is true because we should never actually need to walk on walls
			// because the BuildingBase tile will restrict us when the walls are visible
			// and we want to be able to walk over them when they are visible
				traversable = true;
				break;
			case "BuildingRoof":
			// This is true because we should never actually need to walk on roofs
			// because the BuildingBase tile will restrict us when the roofs are visible
			// and we want to be able to walk over them when they are visible
				traversable = true;
				break;
			case "BuildingBase":
				traversable = false;
				break;
		}
	}

	public bool CanIMoveHere(){
		if (traversable && ObjectHere == null) {
			return true;
		} else {
			return false;
		}
	}

	public bool AmIInside(){
		if (tileType == "BuildingFloor" | tileType == "BuildingWall" | tileType == "BuildingRoof") {
			return true;
		} else {
			return false;
		}
	}
}

