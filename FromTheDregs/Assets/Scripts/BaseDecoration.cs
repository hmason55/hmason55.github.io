using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDecoration {

	public enum Usage {
		None
	}

	public enum DecorationType {
		None,
		Container,
		Entrance,
		Exit,
		Puddle,
		Small,
		Trap
	}

	Sprite _sprite;
	Biome _biome;
	Usage _usage = Usage.None;
	DecorationType _decorationType;

	public Sprite sprite {
		get {return _sprite;}
	}
	public Biome biome {
		get {return _biome;}
		set {_biome = value;}
	}

	public Usage usage {
		get {return _usage;}
		set {_usage = value;}
	}

	public DecorationType decorationType {
		get {return _decorationType;}
		set {_decorationType = value;}
	}

	public BaseDecoration(Biome b, DecorationType d, SpriteManager s) {
		_biome = b;
		_decorationType = d;
		LoadTexture(s);
	}

	public void LoadTexture(SpriteManager spriteManager) {
		List<Sprite> sprites = new List<Sprite>();
		switch(_biome.biomeType) {
			case Biome.BiomeType.dungeon:
				switch(_decorationType) {
					case DecorationType.Container:
						sprites = spriteManager.biomeDungeon.container;
					break;

					case DecorationType.Small:
						sprites = spriteManager.biomeDungeon.decorationSmall;
					break;

					case DecorationType.Entrance:
						sprites = spriteManager.biomeDungeon.entrance;
					break;

					case DecorationType.Exit:
						sprites = spriteManager.biomeDungeon.exit;
					break;
				}
			break;

			case Biome.BiomeType.forsaken:
				switch(_decorationType) {
					case DecorationType.Container:
						sprites = spriteManager.biomeForsaken.container;
					break;

					case DecorationType.Small:
						sprites = spriteManager.biomeForsaken.decorationSmall;
					break;

					case DecorationType.Entrance:
						sprites = spriteManager.biomeForsaken.entrance;
					break;

					case DecorationType.Exit:
						sprites = spriteManager.biomeForsaken.exit;
					break;
				}
			break;
		}
		int variation = Random.Range(0, sprites.Count-1);
		_sprite = sprites[variation];
	}

}
