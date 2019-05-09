using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class TerrainBehaviour : MonoBehaviour, IPointerClickHandler {

	BaseTerrain _baseTerrain;

	Tile _tile;

	[SerializeField] RectTransform _rectTransform;
	[SerializeField] Image _image;
	[SerializeField] Shadow _shadow;
	[SerializeField] Image _shadedImage;

	public bool render;
	public bool shaded;
	 
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

	public Image shadedImage {
		get {return _shadedImage;}
	}

	public void OnPointerClick(PointerEventData eventData) {
		if(eventData.button == PointerEventData.InputButton.Right) {return;}
		
		Hotbar hotbar = GameObject.FindObjectOfType<Hotbar>();
		if(readyCast) {
			hotbar.ReadyCast(_tile.position);
		} else if(confirmCast) {
			hotbar.ConfirmCast();
		}
	}

	public void UpdateSprite() {
		if(_renderFlag && _baseTerrain != null) {
			if(_baseTerrain.terrainType == BaseTerrain.TerrainType.wall_side) {
				_shadow.enabled = true;
			}

			_shadedImage.sprite = _baseTerrain.shadedSprite;
			_shadedImage.enabled = _baseTerrain.shaded;

			if(_baseTerrain.render) {
				_image.enabled = true;
				_image.sprite = _baseTerrain.sprite;
				_image.color = Color.white;

				
			} else {
				_image.enabled = false;
				_shadedImage.enabled = false;
			}

			render = _baseTerrain.render;
			shaded = _baseTerrain.shaded;
		}
	}

	public void Transfer(Tile t, BaseTerrain b) {
		float x = t.position.x * DungeonManager.TileWidth;
		float y = t.position.y * DungeonManager.TileWidth;
		_rectTransform.anchoredPosition = new Vector2(x, y);

		_renderFlag = true;

		_tile = t;
		_tile.terrain = this;

		if(b != null) {
			_baseTerrain = b;
			if(_baseTerrain.terrainType == BaseTerrain.TerrainType.wall_side) {
				_shadow.enabled = true;
			}

			_shadedImage.sprite = _baseTerrain.shadedSprite;
			_shadedImage.enabled = _baseTerrain.shaded;

			if(_baseTerrain.render) {
				_image.enabled = true;
				_image.sprite = _baseTerrain.sprite;
				_image.color = Color.white;

				//_shadedImage.sprite = _baseTerrain.shadedSprite;
				//_shadedImage.enabled = _baseTerrain.shaded;
				//shaded = _baseTerrain.shaded;
			} else {
				_image.enabled = false;
				_shadedImage.enabled = false;
			}

			render = _baseTerrain.render;
			shaded = _baseTerrain.shaded;
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
		_shadedImage.sprite = null;
		_shadedImage.enabled = false;
	}
}
