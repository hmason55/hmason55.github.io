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
		Trap,
		CavernDoor,
		CryptDoor,
		HedgeDoor,
		DungeonDoor,
		HubShop,
		Shop,
	}

	Sprite _sprite;
	Sprite _highlightSprite;
	Sprite _lockSprite;
	Sprite _lockHighlightSprite;
	Biome _biome;
	Usage _usage = Usage.None;
	DecorationType _decorationType;
	Bag _bag;

	bool _traversable = false;
	bool _locked = false;
	string _lockcode = "XXXXXX";

	public Sprite sprite {
		get {return _sprite;}
	}

	public Sprite highlightSprite {
		get {return _highlightSprite;}
	}

	public Sprite lockSprite {
		get {return _lockSprite;}
	}

	public Sprite lockHighlightSprite {
		get {return _lockHighlightSprite;}
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

	public bool traversable {
		get {return _traversable;}
		set {_traversable = value;}
	}

	public bool locked {
		get {return _locked;}
		set {_locked = value;}
	}

	public string lockcode {
		get {return _lockcode;}
		set {_lockcode = value;}
	}

	public BaseDecoration(Biome b, DecorationType d, SpriteManager s) {
		_biome = b;
		_decorationType = d;
		
		if(_decorationType == DecorationType.Container) {
			

			//_items.Add(new BaseItem(BaseItem.ID.Gold, Random.Range(1, 99)));
			//_items.Add(new BaseItem(BaseItem.ID.Small_Key));
			//_items.Add(new BaseItem((BaseItem.ID)Random.Range(0, 16)));
			
			if(PlayerData.current.retrievalMode == true) {
				PlayerData.current.retrievalMode = false;
				_bag = new Bag(Bag.BagType.Container, PlayerData.current.retrievalBag.items);
			} else {
				List<BaseItem> _items = new List<BaseItem>();
				_items.Add(new BaseItem(BaseItem.ID.Gold, Random.Range(1, 99)));
				_items.Add(new BaseItem((BaseItem.ID)Random.Range(0, 16)));
				_bag = new Bag(Bag.BagType.Container, _items);
				
			}
			
		}

		if(_decorationType == DecorationType.Exit) {
			_traversable = false;
			_locked = true;
			_lockcode = BaseItem.GenerateKeycode();
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
						highlights = spriteManager.biomeCavern.exitHighlights;

						if(_lockcode != "XXXXXX") {
							_lockSprite = BaseItem.GenerateKeycodeSprite(_lockcode, spriteManager.biomeCavern.locks[0], spriteManager);
							_lockHighlightSprite = BaseItem.GenerateKeycodeSprite(_lockcode, spriteManager.biomeCavern.lockHighlights[0], spriteManager);
						}
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
						highlights = spriteManager.biomeCrypt.exitHighlights;

						if(_lockcode != "XXXXXX") {
							_lockSprite = BaseItem.GenerateKeycodeSprite(_lockcode, spriteManager.biomeCrypt.locks[0], spriteManager);
							_lockHighlightSprite = BaseItem.GenerateKeycodeSprite(_lockcode, spriteManager.biomeCrypt.lockHighlights[0], spriteManager);
						}
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
						highlights = spriteManager.biomeDungeon.exitHighlights;

						if(_lockcode != "XXXXXX") {
							_lockSprite = BaseItem.GenerateKeycodeSprite(_lockcode, spriteManager.biomeDungeon.locks[0], spriteManager);
							_lockHighlightSprite = BaseItem.GenerateKeycodeSprite(_lockcode, spriteManager.biomeDungeon.lockHighlights[0], spriteManager);
						}
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
						highlights = spriteManager.biomeForsaken.exitHighlights;

						if(_lockcode != "XXXXXX") {
							_lockSprite = BaseItem.GenerateKeycodeSprite(_lockcode, spriteManager.biomeForsaken.locks[0], spriteManager);
							_lockHighlightSprite = BaseItem.GenerateKeycodeSprite(_lockcode, spriteManager.biomeForsaken.lockHighlights[0], spriteManager);
						}
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
						highlights = spriteManager.biomeRuins.exitHighlights;

						if(_lockcode != "XXXXXX") {
							_lockSprite = BaseItem.GenerateKeycodeSprite(_lockcode, spriteManager.biomeRuins.locks[0], spriteManager);
							_lockHighlightSprite = BaseItem.GenerateKeycodeSprite(_lockcode, spriteManager.biomeRuins.lockHighlights[0], spriteManager);
						}
					break;
				}
			break;
		}

		int variation = Random.Range(0, sprites.Count-1);

		switch(_decorationType) {
			case DecorationType.Container:
				_sprite = sprites[variation];
				_highlightSprite = highlights[variation];
			break;

			case DecorationType.Exit:
				if(_traversable) {
					_sprite = sprites[1];
					_highlightSprite = highlights[1];
				} else {
					_sprite = sprites[0];
					_highlightSprite = highlights[0];
				}
			break;

			case DecorationType.CavernDoor:
				_sprite = spriteManager.cavernDoor[0];
				_highlightSprite = spriteManager.cavernDoor[1];
			break;

			case DecorationType.CryptDoor:
				_sprite = spriteManager.cryptDoor[0];
				_highlightSprite = spriteManager.cryptDoor[1];
			break;

			case DecorationType.HedgeDoor:
				_sprite = spriteManager.hedgeDoor[0];
				_highlightSprite = spriteManager.hedgeDoor[1];
			break;

			case DecorationType.DungeonDoor:
				_sprite = spriteManager.dungeonDoor[0];
				_highlightSprite = spriteManager.dungeonDoor[1];
			break;

			default:
				_sprite = sprites[variation];
			break;
		}
	}

	public bool Unlock(Bag bag) {
		int ndx = bag.FindKey(_lockcode);
		if(ndx > -1) {
			if(bag.RemoveAt(ndx)) {
				_locked = false;
				_lockcode = "XXXXXX";
				return true;
			}
		}
		return false;
	}

}
