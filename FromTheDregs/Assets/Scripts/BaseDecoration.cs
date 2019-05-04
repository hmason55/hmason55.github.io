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
		Loot,
	}

	Tile _tile;
	Sprite _sprite;
	Sprite _highlightSprite;
	Sprite _lockSprite;
	Sprite _lockHighlightSprite;
	Biome _biome;
	Usage _usage = Usage.None;
	DecorationType _decorationType;
	Bag _bag;

	int _animationFrame;
	Sprite[] _animation;
	Sprite[] _highlightAnimation;
	bool _animated = false;


	bool _traversable = false;
	bool _locked = false;
	string _lockcode = "XXXXXX";

	public Tile tile {
		get {return _tile;}
	}
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

	public bool animated {
		get {return _animated;}
	}

	public BaseDecoration(DecorationType d, Tile t, bool a = false) {
		_decorationType = d;
		_tile = t;
		_animated = a;

		switch(_decorationType) {
			case DecorationType.Loot:
				_bag = new Bag(Bag.BagType.Container);
			break;
		}

		LoadTexture();
	}

	public BaseDecoration(Biome b, DecorationType d, Tile t, SpriteManager s, bool a = false, bool retrieval = false) {
		_biome = b;
		_decorationType = d;
		_tile = t;
		_animated = a;
		List<BaseItem> _items = new List<BaseItem>();

		switch(_decorationType) {
			
			case DecorationType.Container:
				if(PlayerData.current.retrievalMode == true && retrieval) {
					_bag = new Bag(Bag.BagType.Container, PlayerData.current.retrievalBag.items);
					PlayerData.current.retrievalMode = false;
					SaveLoadData.Save();
					AnnouncementManager.Display("Something here feels familiar...", Color.white);
				} else {
					int numItems = Random.Range(0, 5);
					for(int i = 0; i < numItems; i++) {
						_items.Add(new BaseItem(BaseItem.ID.Gold, Random.Range(2, 8)));
					}

					//_items.Add(new BaseItem((BaseItem.ID)Random.Range(0, 16)));
					_bag = new Bag(Bag.BagType.Container, _items);
				}
			break;

			case DecorationType.Exit:
				_traversable = false;
				_locked = true;
				_lockcode = BaseItem.GenerateKeycode();
			break;

			case DecorationType.HubShop:
				_items.Add(new BaseItem(BaseItem.ID.Rune_of_Strength));
				_items.Add(new BaseItem(BaseItem.ID.Rune_of_Dexterity));
				_items.Add(new BaseItem(BaseItem.ID.Rune_of_Intelligence));
				_items.Add(new BaseItem(BaseItem.ID.Rune_of_Constitution));
				//_items.Add(new BaseItem(BaseItem.ID.Gladius));
				//_items.Add(new BaseItem(BaseItem.ID.Parma));
				//_items.Add(new BaseItem(BaseItem.ID.Dagger));
				_items.Add(new BaseItem(BaseItem.ID.Potion_of_Return));
				_items.Add(new BaseItem(BaseItem.ID.Potion_of_Clotting));
				_items.Add(new BaseItem(BaseItem.ID.Potion_of_Curing));
				_items.Add(new BaseItem(BaseItem.ID.Cotton_Tunic));
				_items.Add(new BaseItem(BaseItem.ID.Leather_Jack));
				_items.Add(new BaseItem(BaseItem.ID.Chainmail_Tunic));
				_items.Add(new BaseItem(BaseItem.ID.Gladius));
				_items.Add(new BaseItem(BaseItem.ID.Staff_of_Flame));
				_items.Add(new BaseItem(BaseItem.ID.Parma));
				_items.Add(new BaseItem(BaseItem.ID.Novice_Tome));
				_bag = new Bag(Bag.BagType.Shop, _items);
			break;

		
			
		}

		LoadTexture(s);
	}

	public void LoadTexture() {
		if(_animated) {
			_animation = AssetReference.sprites.decorations.merchant.animation.ToArray();
			_highlightAnimation = AssetReference.sprites.decorations.merchant.highlightAnimation.ToArray();
			//for(int i = 0; i < _)
			return;
		}

		switch(_decorationType) {
			case DecorationType.Loot:
				_animation = AssetReference.sprites.decorations.loot.animation.ToArray();
				_highlightAnimation = AssetReference.sprites.decorations.loot.highlightAnimation.ToArray();
			break;
		}

		_sprite = _animation[0];
		_highlightSprite = _highlightAnimation[0];
	}

	public void LoadTexture(SpriteManager spriteManager) {
		List<Sprite> sprites = new List<Sprite>();
		List<Sprite> highlights = new List<Sprite>();
		if(_animated) {
			_animation = AssetReference.sprites.decorations.merchant.animation.ToArray();
			_highlightAnimation = AssetReference.sprites.decorations.merchant.highlightAnimation.ToArray();
			//for(int i = 0; i < _)
			return;
		} else {
			_animation = new Sprite[1];
			_highlightAnimation = new Sprite[1];
		}

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
				_animation[0] = sprites[variation];
				_sprite = _animation[0];
				_highlightAnimation[0] = highlights[variation];
				_highlightSprite = _highlightAnimation[0];
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

	public void IncrementAnimation(int frame = -1) {
		if(_tile != null) {
			_animationFrame = _tile.animationController.animationFrame;
		} else {
			_animationFrame = frame;
		}

		if(_animation != null) {
			if(_animationFrame < 10) {
				_sprite = _animation[0];
				if(_highlightAnimation.Length > 0) {_highlightSprite = _highlightAnimation[0];}
			} else if(_animationFrame < 11) {
				if(_animation.Length > 1) {_sprite = _animation[1];}
				if(_highlightAnimation.Length > 1) {_highlightSprite = _highlightAnimation[1];}
			} else if(_animationFrame < 22) {
				if(_animation.Length > 2) {_sprite = _animation[2];}
				if(_highlightAnimation.Length > 2) {_highlightSprite = _highlightAnimation[2];}
			} else {
				if(_animation.Length > 3) {_sprite = _animation[3];}
				if(_highlightAnimation.Length > 3) {_highlightSprite = _highlightAnimation[3];}
			}
		}
	}

}
