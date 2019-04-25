﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


	Spell _spell;
	Vector2 _velocity;
	Vector2Int _end;

	public Spell spell {
		get {return _spell;}
		set {_spell = value;}
	}

	public Vector2 velocity {
		get {return _velocity;}
		set {_velocity = value;}
	}

	public Vector2Int end {
		get {return _end;}
		set {_end = value;}
	}

	RectTransform _rectTransform;

	// Use this for initialization
	void Start () {
		_rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_rectTransform != null) {
			_rectTransform.anchoredPosition += new Vector2(_velocity.x, _velocity.y) * Time.deltaTime;
		}
	}

	void OnDestroy() {
		if(_spell.caster.tile.unit != null) {
			_spell.caster.tile.unit.SpawnSpellEffect(_spell);
		}
	}
}
