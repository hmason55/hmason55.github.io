using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	[SerializeField] DungeonManager _dungeonManager;
	float _animationFrame = 0f;
	float _animationSpeed = 1f;
	public static int AnimationLength = 26;

	public int animationFrame {
		get {return (int)_animationFrame;}
	}

	public float animationSpeed {
		get {return _animationSpeed;}
		set {_animationSpeed = value;}
	}
	
	// Update is called once per frame
	void Update () {
		_animationFrame += (animationSpeed * AnimationLength * Time.deltaTime);
		_animationFrame %= AnimationLength;
	}

	void LateUpdate() {
		for(int y = 0; y < DungeonManager.dimension; y++) {
			for(int x = 0; x < DungeonManager.dimension; x++) {
				if(_dungeonManager.tiles[x, y] != null) {
					_dungeonManager.tiles[x, y].AnimateUnit();
				}
			}
		}
	}
}
