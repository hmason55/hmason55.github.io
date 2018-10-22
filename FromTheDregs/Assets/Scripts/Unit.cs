using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Unit : MonoBehaviour {
	
	BaseUnit _baseUnit;
	Sprite[] _idleAnimation;
	static int IdleAnimationLength = 4;
	int _animationFrame = 0;

	Sprite _sprite;

	[SerializeField] Image _image;
	
	[SerializeField] Tile _tile;

	public Tile tile {
		set {_tile = value;}
		get {return _tile;}
	}


	public Sprite[] idleAnimation {
		get {return _idleAnimation;}
		set {_idleAnimation = value;}
	}

	public int animationFrame {
		get {return _animationFrame;}
		set {_animationFrame = value;}
	}

	public Image image {
		get {return _image;}
	}

	public Sprite sprite {
		set {_sprite = value;}
		get {return _sprite;}
	}

	public BaseUnit baseUnit {
		set {_baseUnit = value;}
		get {return _baseUnit;}
	}


	public void LoadSprites() {
		if(_baseUnit != null) {
			_idleAnimation = new Sprite[IdleAnimationLength];
			_idleAnimation = _tile.spriteManager.unitHumanWizard1.idle.ToArray();
		}
	}

	public void IncrementAnimation() {
		_animationFrame = _baseUnit.tile.animationController.animationFrame;

		// Idle animation
		if(_idleAnimation != null) {
			if(_animationFrame < 10) {
				_sprite = _idleAnimation[0];
			} else if(_animationFrame < 11) {
				_sprite = _idleAnimation[1];
			} else if(_animationFrame < 22) {
				_sprite = _idleAnimation[2];
			} else {
				_sprite = _idleAnimation[3];
			}
		}

	}

	public void UpdateSprite() {
		GetComponent<Image>().sprite = _sprite;
	}

	public void TransferUnit(Unit nextUnit) {
		nextUnit.idleAnimation = _idleAnimation;
		nextUnit.animationFrame = _animationFrame;
		nextUnit.sprite = _sprite;
		nextUnit.UpdateSprite();
		nextUnit.GetComponent<Image>().enabled = true;
		ClearAssets();
	}

	public void ClearAssets() {
		_idleAnimation = null;
		_animationFrame = 0;
		_sprite = null;
		GetComponent<Image>().sprite = null;
		GetComponent<Image>().enabled = false;
	}
}
