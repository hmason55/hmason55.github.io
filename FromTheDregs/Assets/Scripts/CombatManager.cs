using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

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
			BaseUnit baseUnit = _turnQueue.queue[0].baseUnit;
			List<PathNode> path = baseUnit.FindPath(baseUnit.tile.position, baseUnit.tile.dungeonManager.exitPosition, false);
			if(path.Count > 2) {
				Debug.Log(baseUnit.tile.position);
				Debug.Log(path[path.Count-2].position);
				baseUnit.Move(path[path.Count-2].position.x - baseUnit.tile.position.x, path[path.Count-2].position.y - baseUnit.tile.position.y);
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
}
