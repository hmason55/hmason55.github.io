using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class TerrainBehaviour : MonoBehaviour, IPointerClickHandler {

	BaseTerrain _baseTerrain;

	Tile _tile;

	RectTransform _rectTransform;
	Image _image;
	Shadow _shadow;
	 
	bool init = false;
	bool _renderFlag = false;

	public bool readyCast = false;
	public bool confirmCast = false;

	public BaseTerrain baseTerrain {
		get {return _baseTerrain;}
		set {_baseTerrain = value;}
	}

	public bool renderFlag {
		get {return _renderFlag;}
		set {_renderFlag = value;}
	}

	public Tile tile {
		get {return _tile;}
		set {_tile = value;}
	}

	public Image image {
		get {return _image;}
	}

	void Awake() {
		_rectTransform = GetComponent<RectTransform>();
		_shadow = GetComponent<Shadow>();
		_image = GetComponent<Image>();
	}

	public void OnPointerClick(PointerEventData eventData) {
		Hotbar hotbar = GameObject.FindObjectOfType<Hotbar>();
		if(readyCast) {
			hotbar.ReadyCast(_tile.position);
		} else if(confirmCast) {
			hotbar.ConfirmCast();
		}
	}

	void UpdateSprite() {
		
	}

	public void Transfer(Tile t, BaseTerrain b) {
		float x = t.position.x * DungeonManager.dimension;
		float y = t.position.y * DungeonManager.dimension;
		_rectTransform.anchoredPosition = new Vector2(x, y);

		_renderFlag = true;

		_tile = t;
		_tile.terrain = this;

		if(b != null) {
			_baseTerrain = b;
			if(baseTerrain.terrainType == BaseTerrain.TerrainType.wall_side) {
				_shadow.enabled = true;
			}
			_image.enabled = true;
			_image.sprite = _baseTerrain.sprite;
			_image.color = Color.white;
		}
		
	}

	public void Clear() {
		_rectTransform.anchoredPosition = new Vector2(-256f, -256f);

		_renderFlag = false;
		readyCast = false;
		confirmCast = false;
		_tile.terrain = null;
		_tile = null;
		_baseTerrain = null;
		_shadow.enabled = false;
		_image.sprite = null;
		_image.enabled = false;
	}
}
