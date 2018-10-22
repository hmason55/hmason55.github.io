using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.IO;


public class Terrain : MonoBehaviour, IPointerClickHandler {

	[SerializeField] Tile _tile;
	 
	bool init = false;

	public bool readyCast = false;
	public bool confirmCast = false;

	[SerializeField] Image _image;

	Sprite _sprite;
	Biome.BiomeType _biome = Biome.BiomeType.forsaken;

	public enum TerrainType {
		wall_top,
		wall_side,
		ground
	}

	TerrainType _terrainType = TerrainType.ground;

	bool _walkable = false;

	public bool walkable {
		get {return _walkable;}
		set {_walkable = value;}
	}

	public TerrainType terrainType {
		get {return _terrainType;}
		set {_terrainType = value;}
	}

	public Biome.BiomeType biome {
		get {return _biome;}
		set {_biome = value;}
	}

	public Sprite sprite {
		get {return _sprite;}
	}

	public Image image {
		get {return _image;}
	}

	public void OnPointerClick(PointerEventData eventData) {
		if(readyCast) {
			Hotbar hotbar = GameObject.FindObjectOfType<Hotbar>();
			hotbar.activeSpell.ResetTiles();
			hotbar.activeSpell.ShowEffectRange(_tile.position);
			Debug.Log(_tile.name);
		} else if(confirmCast) {
			Hotbar hotbar = GameObject.FindObjectOfType<Hotbar>();
			hotbar.activeSpell.ConfirmSpellCast();
		}
		//DetectQuadrant(new Vector2Int((int)Input.mousePosition.x, (int)Input.mousePosition.y));
	}

	public void ParseColor(Color color) {
		string colorAsString = ColorUtility.ToHtmlStringRGB(color);
		switch(colorAsString) {
			case "000000":	// Black
				_walkable = false;
				_terrainType = TerrainType.wall_top;
				LoadTexture();
			break;
			
			case "0000FF":	// Blue
			case "FFFF00":	// Yellow
			case "FF00FF": 	// Magenta
			case "00FF00":	// Green
				_walkable = true;
				_terrainType = TerrainType.ground;
				LoadTexture();
			break;
		}
	}

	public void LoadTexture() {
		init = true;
		List<Sprite> sprites = new List<Sprite>();

		switch(_biome) {
			case Biome.BiomeType.dungeon:
				switch(_terrainType) {
					case TerrainType.ground:
						sprites = _tile.spriteManager.biomeDungeon.ground;
					break;

					case TerrainType.wall_side:
						sprites = _tile.spriteManager.biomeDungeon.wallSide;
					break;

					case TerrainType.wall_top:
						sprites = _tile.spriteManager.biomeDungeon.wallTop;
					break;
				}
			break;

			case Biome.BiomeType.forsaken:
				switch(_terrainType) {
					case TerrainType.ground:
						sprites = _tile.spriteManager.biomeForsaken.ground;
					break;

					case TerrainType.wall_side:
						sprites = _tile.spriteManager.biomeForsaken.wallSide;
					break;

					case TerrainType.wall_top:
						sprites = _tile.spriteManager.biomeForsaken.wallTop;
					break;
				}
			break;
		}

		if(sprites != null) {
			int variation = Random.Range(0, sprites.Count-1);
			_sprite = sprites[variation];
		}

		UpdateSprite();
	}

	void UpdateSprite() {
		Image image = GetComponent<Image>();
		if(init) {
			image.enabled = true;
		}
		image.sprite = _sprite;
	}
}
