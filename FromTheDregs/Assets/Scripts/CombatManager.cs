﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

	[SerializeField] DungeonManager dungeonManager;

	// Use this for initialization
	void Awake () {
		_turnQueue = new TurnQueue();
	}

	TurnQueue _turnQueue;


	public TurnQueue turnQueue {
		get {return _turnQueue;}
	}

	public void BeginCombat() {
		Debug.Log("Begin Combat");
		
		StartCoroutine(ECombatCycle());
	}
	IEnumerator ECombatCycle() {
		while(true) {
			if(_turnQueue.Length > 0) {
				BaseUnit baseUnit = _turnQueue.queue[0].baseUnit;
				Debug.Log(baseUnit.spritePreset.ToString() + "'s turn...");
				if(!baseUnit.playerControlled) {
					// Do AI stuff

					// Choose spell (melee for this case)

					// Target nearest with melee
					BaseUnit target = GetNearestUnit(baseUnit, false, 1, false);
					if(target != null) {
						List<PathNode> path = baseUnit.FindPath(baseUnit.tile.position, target.tile.position, false, 1);

						// Move / Attack
						if(path.Count > 1) {
							Debug.Log("Moving");
							baseUnit.Move(path[path.Count-2].position.x - baseUnit.tile.position.x, path[path.Count-2].position.y - baseUnit.tile.position.y);
							EndTurn(baseUnit);
						} else {
							
						}
					}
					yield return new WaitForSeconds(0.1f);
				} else {
					yield return new WaitForSeconds(0.1f);
				}
			}
			//yield return new WaitForSeconds(1f);
		}
		yield return new WaitForSeconds(0.5f);
	}

	void EndTurn(BaseUnit b) {
		turnQueue.EndTurn();
		turnQueue.NextTurn();
		turnQueue.Add(new Turn(b, b.modSpeed));
	}

	public bool ValidateMoveTile(Tile tile) {
		if(tile != null) {
			if(tile.baseTerrain != null) {
				if(tile.baseTerrain.walkable) {
					if(tile.baseUnit == null) {
						return true;
					}
				}
			}
		}
		return false;
	}

	public void CheckCombatStatus() {
		// Flag all units within range of another unit
		List<BaseUnit> baseUnits = GetAllBaseUnits();
		foreach(BaseUnit baseUnit in baseUnits) {
			//if(baseUnit.inCombat) {
				bool[,] visitedTiles = new bool[DungeonManager.dimension, DungeonManager.dimension];
				AggroNearbyUnits(baseUnit.tile.position.x, baseUnit.tile.position.y, visitedTiles, baseUnit.tile.position.x, baseUnit.tile.position.y, 4);
			//}
		}

		//_turnQueue.SortTurns();
		Debug.Log("Turn Queue (" + _turnQueue.Length + "):");
		_turnQueue.PrintTurns();
	}

	List<BaseUnit> GetAllBaseUnits() {
		List<BaseUnit> allBaseUnits = new List<BaseUnit>();
		for(int y = 0; y < DungeonManager.dimension; y++) {
			for(int x = 0; x < DungeonManager.dimension; x++) {
				Tile tile = dungeonManager.tiles[x, y];
				if(tile != null) {
					if(tile.baseUnit != null) {
						allBaseUnits.Add(tile.baseUnit);
					}
				}
			}	
		}
		return allBaseUnits;
	}

	BaseUnit GetNearestUnit(BaseUnit b, bool ignoreUnits, int excludeNodesFromEnd, bool targetSameAlliance) {
		List<BaseUnit> allUnits = GetAllBaseUnits();
		BaseUnit nearestUnit = null;
		int shortestPathLength = int.MaxValue;
		
		foreach(BaseUnit t in allUnits) {
			if(t != b) {
				if(targetSameAlliance) {
					if(t.combatAlliance != b.combatAlliance) {continue;}
				} else {
					if(t.combatAlliance == b.combatAlliance) {continue;}
				}

				List<PathNode> path = b.FindPath(b.tile.position, t.tile.position, ignoreUnits, excludeNodesFromEnd);
				if(path.Count > 0 && path.Count < shortestPathLength) {
					nearestUnit = t;
					shortestPathLength = path.Count;
				}
			}
		}
		return nearestUnit;
	}

	void AggroNearbyUnits(int x, int y, bool[,] visited, int ox, int oy, int radius) {

		// Check map bounds
		if(	x < 0 ||
		 	y < 0 ||
		 	x >= (DungeonManager.dimension)-1 || 
			y >= (DungeonManager.dimension)-1) { 
			return;
		}

		// Check casting bounds
		if(	x > ox+radius || y > oy+radius || 
			x < ox-radius || y <  oy-radius) {
			return;
		}

		if(visited[x, y] == true) {
			return;
		}

		visited[x, y] = true;


		if(CheckManhattanDistance(ox, oy, x, y) > radius) {
			return;
		}

		Tile tile = dungeonManager.tiles[x, y];

		// Check wall collision
		if(tile.baseTerrain != null) {
			if(!tile.baseTerrain.walkable) {
				return;
			}
		}

		bool aggroUnit = false;

		// Don't aggro self
		if(!(x == ox && y == oy)) {
			// Require line of sight
			if(tile.baseTerrain.walkable) {
				aggroUnit = RayTrace(ox, oy, x, y);
			}
		}

		// Aggro unit
		if(aggroUnit) {
			if(tile.baseUnit != null) {

				// Put unit in combat
				BaseUnit b1 = dungeonManager.tiles[ox, oy].baseUnit;
				BaseUnit b2 = tile.baseUnit;
				
				if(b1.combatAlliance != b2.combatAlliance) {
					b1.inCombat = true;
					if(!_turnQueue.UnitInQueue(b1)) {
						_turnQueue.Add(new Turn(b1, b1.modSpeed));
					}

					b2.inCombat = true;
						if(!_turnQueue.UnitInQueue(b2)) {
						_turnQueue.Add(new Turn(b2, b2.modSpeed));
					}
				} else {

					// Chain onto nearby units
					if(b1.inCombat || b2.inCombat) {
						b1.inCombat = true;
						if(!_turnQueue.UnitInQueue(b1)) {
							_turnQueue.Add(new Turn(b1, b1.modSpeed));
						}

						b2.inCombat = true;
							if(!_turnQueue.UnitInQueue(b2)) {
							_turnQueue.Add(new Turn(b2, b2.modSpeed));
						}
					}
				}

			}
		} 
		
		AggroNearbyUnits(x+1, y  , visited, ox, oy, radius);
		AggroNearbyUnits(x  , y-1, visited, ox, oy, radius);
		AggroNearbyUnits(x-1, y  , visited, ox, oy, radius);
		AggroNearbyUnits(x  , y+1, visited, ox, oy, radius);
	}

	int CheckManhattanDistance(int x1, int y1, int x2, int y2) {
		return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
	}

	bool RayTrace(int x1, int y1, int x2, int y2) {
		int dx = Mathf.Abs(x2 - x1);
		int sx = -1;

		if(x1 < x2) {
			sx = 1;
		}

		int dy = Mathf.Abs(y2 - y1);
		int sy = -1;
		
		if(y1 < y2) {
			sy = 1;
		}

		int err = -dy/2;

		if(dx > dy) {
			err = dx/2;
		}

		while(true) {

			if(!dungeonManager.tiles[x1, y1].baseTerrain.walkable) {
				return false;
			}

			if(x1 == x2 && y1 == y2) {
				return true;
			}

			int e2 = err;

			if(e2 > -dx) {
				err -= dy;
				x1 += sx;
			}

			if(e2 < dy) {
				err += dx;
				y1 += sy;
			}
		}
	}

}
