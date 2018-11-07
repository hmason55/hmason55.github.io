using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class Decoration : MonoBehaviour {
	[SerializeField] Tile _tile;

	[SerializeField] Image _image;
	Sprite _sprite;
	Biome _biome;
	Usage _usage = Usage.None;

	DecorationType _decorationType;

	public enum DecorationType {
		none,
		container,
		trap,
		small,
		puddle,
		exit,
		entrance
	}

	public enum Usage {
		None
	}
	public Biome biome {
		get {return _biome;}
		set {_biome = value;}
	}

	public DecorationType decorationType {
		get {return _decorationType;}
		set {_decorationType = value;}
	}

	public Sprite sprite {
		get {return _sprite;}
	}
	
	public void Init(bool walkable) {
		
		if(!walkable) {
			return;
		}

		GetComponent<Image>().enabled = true;
		LoadTexture();
	}

	public void LoadTexture() {
		//DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/Sprites/biomes/" + _biome.ToString() + "/");
 		//FileInfo[] info = dir.GetFiles(_biome.ToString() + "_" + _decorationType.ToString() + "_" + "*.png");
		List<Sprite> sprites = new List<Sprite>();
		switch(_biome.biomeType) {
			case Biome.BiomeType.dungeon:
				switch(_decorationType) {
					case DecorationType.container:
						sprites = _tile.spriteManager.biomeDungeon.container;
					break;

					case DecorationType.small:
						sprites = _tile.spriteManager.biomeDungeon.decorationSmall;
					break;

					case DecorationType.entrance:
						sprites = _tile.spriteManager.biomeDungeon.entrance;
					break;

					case DecorationType.exit:
						sprites = _tile.spriteManager.biomeDungeon.exit;
					break;
				}
			break;

			case Biome.BiomeType.forsaken:
				switch(_decorationType) {
					case DecorationType.container:
						sprites = _tile.spriteManager.biomeForsaken.container;
					break;

					case DecorationType.small:
						sprites = _tile.spriteManager.biomeForsaken.decorationSmall;
					break;

					case DecorationType.entrance:
						sprites = _tile.spriteManager.biomeForsaken.entrance;
					break;

					case DecorationType.exit:
						sprites = _tile.spriteManager.biomeForsaken.exit;
					break;
				}
			break;
		}
		int variation = Random.Range(0, sprites.Count-1);
		_sprite = sprites[variation];
		//_sprite = Resources.Load<Sprite>("Sprites/biomes/" + _biome.ToString() + "/" + _biome.ToString() + "_" + _decorationType.ToString() + "_" + variation);
		UpdateSprite();
	}

	void UpdateSprite() {
		GetComponent<Image>().sprite = _sprite;
	}
}
