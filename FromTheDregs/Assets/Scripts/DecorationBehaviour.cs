using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DecorationBehaviour : MonoBehaviour {
	BaseDecoration _baseDecoration;
	Tile _tile;
	RectTransform _rectTransform;
	Image _image;
	bool _renderFlag = false;

	public BaseDecoration baseDecoration {
		get {return _baseDecoration;}
		set {_baseDecoration = value;}
	}
	public Tile tile {
		get {return _tile;}
		set {_tile = value;}
	}

	public bool renderFlag {
		get {return _renderFlag;}
		set {_renderFlag = value;}
	}
	
	void Awake() {
		_image = GetComponent<Image>();
		_rectTransform = GetComponent<RectTransform>();
	}

	public void Transfer(Tile t, BaseDecoration b) {
		float x = t.position.x * DungeonManager.dimension;
		float y = t.position.y * DungeonManager.dimension;
		_rectTransform.anchoredPosition = new Vector2(x, y);

		_renderFlag = true;
		_tile = t;
		_tile.decoration = this;

		if(b != null) {
			_baseDecoration = b;
			_image.sprite = _baseDecoration.sprite;
			_image.enabled = true;
		}
		
	}

	public void Clear() {
		_rectTransform.anchoredPosition = new Vector2(-256f, -256f);
		_renderFlag = false;
		_tile = null;
		_baseDecoration = null;
		_image.sprite = null;
		_image.enabled = false;
	}

	void UpdateSprite() {
	//GetComponent<Image>().sprite = _baseDecoration.sprite;
	}
}
