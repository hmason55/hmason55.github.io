using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {
	Vector2Int _position;
	bool _walkable;
	int _distanceFromStart;
	int _trueDistanceFromEnd;
	int _totalDistance;
	PathNode _parentNode;

	public Vector2Int position {
		get {return _position;}
		set {_position = value;}
	}

	public bool walkable {
		get {return _walkable;}
		set {_walkable = value;}
	}

	public int distanceFromStart {
		get {return _distanceFromStart;}
		set {_distanceFromStart = value;}
	}

	public int trueDistanceFromEnd {
		get {return _trueDistanceFromEnd;}
		set {_trueDistanceFromEnd = value;}
	}

	public int totalDistance {
		get {return _totalDistance;}
		set {_totalDistance = value;}
	}
	
	public PathNode parentNode {
		get {return _parentNode;}
		set {_parentNode = value;}
	}

	public PathNode(Vector2Int p) {
		_position = p;
	}
}
