using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
	Sprite _highlightSprite;
	Biome _biome;
	Usage _usage = Usage.None;
	DecorationType _decorationType;
	Bag _bag;

	public Sprite sprite {
		get {return _sprite;}
	}

	public Sprite highlightSprite {
		get {return _highlightSprite;}
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

	public Bag bag {
		get {return _bag;}
		set {_bag = value;}
	}

	public BaseDecoration(Biome b, DecorationType d, SpriteManager s) {
		_biome = b;
		_decorationType = d;
		
		if(_decorationType == DecorationType.Container) {
			List<BaseItem> _items = new List<BaseItem>();

			_items.Add(new BaseItem(BaseItem.ID.Gold, Random.Range(1, 99)));
			_bag = new Bag(Bag.BagType.Container, _items);
		}

		LoadTexture(s);
	}

	public void LoadTexture(SpriteManager spriteManager) {
		List<Sprite> sprites = new List<Sprite>();
		List<Sprite> highlights = new List<Sprite>();
		switch(_biome.biomeType) {
			
			// Cavern
			case Biome.BiomeType.cavern:
				switch(_decorationType) {
					case DecorationType.Container:
						sprites = spriteManager.biomeCavern.container;
						highlights = spriteManager.biomeCavern.containerHighlights;
					break;

					case DecorationType.Small:
						sprites = spriteManager.biomeCavern.decorationSmall;
					break;

					case DecorationType.Entrance:
						sprites = spriteManager.biomeCavern.entrance;
					break;

					case DecorationType.Exit:
						sprites = spriteManager.biomeCavern.exit;
					break;
				}
			break;

			// Crypt
			case Biome.BiomeType.crypt:
				switch(_decorationType) {
					case DecorationType.Container:
						sprites = spriteManager.biomeCrypt.container;
						highlights = spriteManager.biomeCrypt.containerHighlights;
					break;

					case DecorationType.Small:
						sprites = spriteManager.biomeCrypt.decorationSmall;
					break;

					case DecorationType.Entrance:
						sprites = spriteManager.biomeCrypt.entrance;
					break;

					case DecorationType.Exit:
						sprites = spriteManager.biomeCrypt.exit;
					break;
				}
			break;

			// Dungeon
			case Biome.BiomeType.dungeon:
				switch(_decorationType) {
					case DecorationType.Container:
						sprites = spriteManager.biomeDungeon.container;
						highlights = spriteManager.biomeDungeon.containerHighlights;
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

			// Forsaken
			case Biome.BiomeType.forsaken:
				switch(_decorationType) {
					case DecorationType.Container:
						sprites = spriteManager.biomeForsaken.container;
						highlights = spriteManager.biomeForsaken.containerHighlights;
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

			// Ruins
			case Biome.BiomeType.ruins:
				switch(_decorationType) {
					case DecorationType.Container:
						sprites = spriteManager.biomeRuins.container;
						highlights = spriteManager.biomeRuins.containerHighlights;
					break;

					case DecorationType.Small:
						sprites = spriteManager.biomeRuins.decorationSmall;
					break;

					case DecorationType.Entrance:
						sprites = spriteManager.biomeRuins.entrance;
					break;

					case DecorationType.Exit:
						sprites = spriteManager.biomeRuins.exit;
					break;
				}
			break;
		}
		int variation = Random.Range(0, sprites.Count-1);
		_sprite = sprites[variation];

		if(_decorationType == DecorationType.Container) {
			_highlightSprite = highlights[variation];
		}
	}

}
