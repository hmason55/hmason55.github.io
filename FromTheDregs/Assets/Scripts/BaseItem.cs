using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseItem {

	public enum ID {
		Chainmail_Coif,
		Chainmail_Gloves,
		Chainmail_Leggings,
		Chainmail_Shoes,
		Chainmail_Tunic,

		Cotton_Breeches,
		Cotton_Gloves,
		Cotton_Hood,
		Cotton_Shoes,
		Cotton_Tunic,

		Dagger,

		Gladius,

		Gold,

		Leather_Boots,
		Leather_Bracers,
		Leather_Breeches,
		Leather_Jack,
		Leather_Helm,


		Novice_Tome,
		Parma,
		Potion_of_Clotting,
		Potion_of_Curing,
		Potion_of_Return,
		

		Rune_of_Constitution,
		Rune_of_Dexterity,
		Rune_of_Intelligence,
		Rune_of_Strength,

		Small_Key,

		Staff_of_Flame,
		Throwing_Knives,
	}

	public enum Category {
		Unknown,
		Primary_Weapon,
		Secondary_Weapon,
		Body_Armor,
		Foot_Armor,
		Hand_Armor,
		Head_Armor,
		Leg_Armor,
		Neck_Jewelry,
		Finger_Jewelry,
		Consumable,
		Unknown_Consumable,
		Healing_Consumable,
		Poisonous_Consumable,
		Trophy,
		Currency,
		Key,
		Runestone,
	}

	public enum ArmorWeight {
		Light,
		Medium,
		Heavy
	}

	public enum LightArmorTier {
		Cotton,
		Trimmed_Cotton,
		Silk,
		Trimmed_Silk
	}

	public enum MediumArmorTier {
		Leather,
		Studded_Leather
	}

	public enum HeavyArmorTier {
		Iron_Chain,
		Iron_Plate
	}

	public enum JewelryTier {
		Copper,
		Silver,
		Gold
	}

	Category _category;
	string _name;
	ID _id;
	int _quantity;
	string _description;
	int _value;
	string _keycode = "XXXXXX";

	#region Attributes
	// Equipment Attributes
	int _physicalDamage;
	int _spellDamage;

	int _blockModifier;
	int _physicalArmor;
	int _spellArmor;

	int _healthTotal;
	int _healthRecovery;

	int _essenceTotal;
	int _essenceRecovery;

	ArmorWeight _armorWeight;
	List<Spell.Preset> _spells;

	// Consumable Attributes
	int _cooldownDuration;
	[System.NonSerialized] Sprite _sprite;
	#endregion

	#region Accessors

	public ID id {
		get {return _id;}
	}

	public string keycode {
		get {return _keycode;}
		set {_keycode = value;}
	}

	public string name {
		get {return _name;}
	}

	public int quantity {
		get {return _quantity;}
		set {_quantity = value;}
	}

	public string description {
		get {return _description;}
	}

	public Category category {
		get {return _category;}
	}

	public Sprite sprite {
		get {return _sprite;}
	}

	public int physicalDamage {
		get {return _physicalDamage;}
	}

	public int spellDamage {
		get {return _spellDamage;}
	}

	public int blockModifier {
		get {return _blockModifier;}
	}

	public List<Spell.Preset> spells {
		get {return _spells;}
	}

	public int value {
		get {
			if(_category == Category.Runestone) {
				_value = PlayerData.current.attributes.ToNextLevel();
			}
			return _value;
		}
		set {_value = value;}
	}
	#endregion

	public void LoadSprite(SpriteManager spriteManager) {
		if(spriteManager == null) {return;}

		switch(_id) {

			case ID.Cotton_Breeches:
				_sprite = spriteManager.items.lightArmorLegs[0];
			break;

			case ID.Cotton_Gloves:
				_sprite = spriteManager.items.lightArmorHands[0];
			break;

			case ID.Cotton_Hood:
				_sprite = spriteManager.items.lightArmorHead[0];
			break;

			case ID.Cotton_Shoes:
				_sprite = spriteManager.items.lightArmorFeet[0];
			break;

			case ID.Cotton_Tunic:
				_sprite = spriteManager.items.lightArmorBody[0];
			break;

			case ID.Dagger:
				_sprite = spriteManager.items.daggers[0];
			break;

			case ID.Gladius:
				_sprite = spriteManager.items.swords[0];
			break;

			case ID.Gold:
				_sprite = spriteManager.items.currency[0];
			break;


			case ID.Chainmail_Coif:
				_sprite = spriteManager.items.heavyArmorHead[0];
			break;

			case ID.Chainmail_Gloves:
				_sprite = spriteManager.items.heavyArmorHands[0];
			break;

			case ID.Chainmail_Leggings:
				_sprite = spriteManager.items.heavyArmorLegs[0];
			break;

			case ID.Chainmail_Shoes:
				_sprite = spriteManager.items.heavyArmorFeet[0];
			break;

			case ID.Chainmail_Tunic:
				_sprite = spriteManager.items.heavyArmorBody[0];
			break;


			case ID.Leather_Boots:
				_sprite = spriteManager.items.mediumArmorFeet[0];
			break;

			case ID.Leather_Bracers:
				_sprite = spriteManager.items.mediumArmorHands[0];
			break;

			case ID.Leather_Breeches:
				_sprite = spriteManager.items.mediumArmorLegs[0];
			break;

			case ID.Leather_Helm:
				_sprite = spriteManager.items.mediumArmorHead[0];
			break;

			case ID.Leather_Jack:
				_sprite = spriteManager.items.mediumArmorBody[0];
			break;

			case ID.Novice_Tome:
				_sprite = spriteManager.items.tomes[0];
			break;

			case ID.Parma:
				_sprite = spriteManager.items.shields[0];
			break;

			case ID.Potion_of_Return:
				_sprite = spriteManager.items.potions[0];
			break;

			case ID.Potion_of_Clotting:
				_sprite = spriteManager.items.potions[2];
			break;

			case ID.Potion_of_Curing:
				_sprite = spriteManager.items.potions[1];
			break;

			case ID.Rune_of_Constitution:
				_sprite = spriteManager.items.runestones[3];
			break;

			case ID.Rune_of_Dexterity:
				_sprite = spriteManager.items.runestones[1];
			break;

			case ID.Rune_of_Intelligence:
				_sprite = spriteManager.items.runestones[2];
			break;

			case ID.Rune_of_Strength:
				_sprite = spriteManager.items.runestones[0];
			break;

			case ID.Small_Key:
				_sprite = GenerateKeycodeSprite(_keycode, spriteManager.items.keys[0], spriteManager);
			break;

			case ID.Staff_of_Flame:
				_sprite = spriteManager.items.staves[0];
			break;
			
		}
	}

	public string CategoryToString() {
		return _category.ToString().Replace("_", " ");
	}

	public string AttributeCategoriesToString() {
		string attributeString = "";

		if(_physicalDamage > 0) {
			attributeString += "<b><color=cyan>Physical Damage</color></b>\n";
		} else if(_physicalDamage < 0) {
			attributeString += "<b><color=red>Physical Damage</color></b>\n";
		}

		if(_spellDamage > 0) {
			attributeString += "<b><color=cyan>Spell Damage</color></b>\n";
		} else if(_spellDamage < 0) {
			attributeString += "<b><color=red>Spell Damage</color></b>\n";
		}

		if(_blockModifier > 0) {
			attributeString += "<b><color=cyan>Blocking</color></b>\n";
		} else if(_blockModifier < 0) {
			attributeString += "<b><color=red>Blocking</color></b>\n";
		}

		if(_physicalArmor > 0) {
			attributeString += "<b><color=cyan>Physical Armor</color></b>\n";
		} else if(_physicalArmor < 0) {
			attributeString += "<b><color=red>Physical Armor</color></b>\n";
		}

		if(_spellArmor > 0) {
			attributeString += "<b><color=cyan>Spell Armor</color></b>\n";
		} else if(_spellArmor < 0) {
			attributeString += "<b><color=red>Spell Armor</color></b>\n";
		}

		return attributeString;
	}

	public string AttributeValuesToString() {
		string attributeString = "";

		if(_physicalDamage > 0) {
			attributeString += "<b><color=cyan>+ " + (_physicalDamage) + "</color></b>\n";
		} else if(_physicalDamage < 0) {
			attributeString += "<b><color=red>- " + Mathf.Abs(_physicalDamage) + "</color></b>\n";
		} else {
			attributeString += _physicalDamage + "\n";
		}

		if(_spellDamage > 0) {
			attributeString += "<b><color=cyan>+" + (_spellDamage) + "</color></b>\n";
		} else if(_spellDamage < 0) {
			attributeString += "<b><color=red>-" + Mathf.Abs(_spellDamage) + "</color></b>\n";
		}

		if(_blockModifier > 0) {
			attributeString += "<b><color=cyan>+" + (_blockModifier) + "</color></b>\n";
		} else if(_blockModifier < 0) {
			attributeString += "<b><color=red>-" + Mathf.Abs(_blockModifier) + "</color></b>\n";
		}

		if(_physicalArmor > 0) {
			attributeString += "<b><color=cyan>+ " + _physicalArmor + "</color></b>\n";
		} else if(_physicalArmor < 0) {
			attributeString += "<b><color=red>- " + Mathf.Abs(_physicalArmor) + "</color></b>\n";
		}

		return attributeString;
	}

	public string AttributesToString() {
		string attributeString = "";
		if(_physicalDamage > 0) {
			attributeString += "+" + _physicalDamage + " Phys ATK\n";
		} else if(_physicalDamage < 0) {
			attributeString += _physicalDamage + " Phys ATK\n";
		}

		if(_spellDamage > 0) {
			attributeString += "+" + _spellDamage + " Spell ATK\n";
		} else if(_spellDamage < 0) {
			attributeString += _spellDamage + " Spell ATK\n";
		}

		if(_blockModifier > 0) {
			attributeString += "+" + _blockModifier + " Block\n";
		} else if(_blockModifier < 0) {
			attributeString += _blockModifier + " Block\n";
		}

		if(_physicalArmor > 0) {
			attributeString += "+" + _physicalArmor + " Phys DEF\n";
		} else if(_physicalArmor < 0) {
			attributeString += _physicalArmor + " Phys DEF\n";
		}
		return attributeString;
	}

	public string SpellsToString() {
		string str = "";
		if(_spells == null) {return str;}
		if(_spells.Count == 0) {return str;}

		foreach(Spell.Preset spell in _spells) {
			str += spell.ToString().Replace("_", " ") + "\n";
		}
		
		return str;
	}

	public bool IsEquipment() {
		switch(_category) {
			case BaseItem.Category.Body_Armor:
			case BaseItem.Category.Finger_Jewelry:
			case BaseItem.Category.Foot_Armor:
			case BaseItem.Category.Hand_Armor:
			case BaseItem.Category.Head_Armor:
			case BaseItem.Category.Leg_Armor:
			case BaseItem.Category.Neck_Jewelry:
			case BaseItem.Category.Primary_Weapon:
			case BaseItem.Category.Secondary_Weapon:
			return true;
		}
		return false;
	}

	public bool IsConsumable() {
		switch(_category) {
			case BaseItem.Category.Consumable:
			case BaseItem.Category.Healing_Consumable:
			case BaseItem.Category.Poisonous_Consumable:
			case BaseItem.Category.Unknown_Consumable:
			return true;
		}
		return false;
	}

	public bool IsStackable() {
		switch(_category) {
			case BaseItem.Category.Currency:
			case BaseItem.Category.Trophy:
			return true;
		}
		return false;
	}

	public BaseItem(ID item, int q = 1) {
		_id = item;
		_quantity = q;
		_spells = new List<Spell.Preset>();
	
		switch(item) {

			case ID.Cotton_Breeches:
				_name = "Cotton Breeches";
				_category = Category.Leg_Armor;
				_description = "Breeches made of cotton.";
				_value = 1;
			break;

			case ID.Cotton_Gloves:
				_name = "Cotton Gloves";
				_category = Category.Hand_Armor;
				_description = "Gloves made of cotton.";
				_value = 1;
			break;

			case ID.Cotton_Hood:
				_name = "Cotton Hood";
				_category = Category.Head_Armor;
				_description = "A hood made of cotton.";
				_value = 1;
			break;

			case ID.Cotton_Shoes:
				_name = "Cotton Shoes";
				_category = Category.Foot_Armor;
				_description = "Shoes made of cotton.";
				_value = 1;
			break;

			case ID.Cotton_Tunic:
				_name = "Cotton Tunic";
				_category = Category.Body_Armor;
				_description = "A tunic made of cotton.";
				_value = 1;
			break;

			case ID.Chainmail_Coif:
				_name = "Iron Chain Coif";
				_category = Category.Head_Armor;
				_description = "A foot soldier's coif.";
				_value = 1;
			break;

			case ID.Chainmail_Gloves:
				_name = "Iron Chain Gloves";
				_category = Category.Hand_Armor;
				_description = "Gloves made of iron chainmail.";
				_value = 1;
			break;

			case ID.Chainmail_Leggings:
				_name = "Iron Chain Leggings";
				_category = Category.Leg_Armor;
				_description = "Leggings made of chainmail.";
				_value = 1;
			break;

			case ID.Chainmail_Shoes:
				_name = "Iron Chain Shoes";
				_category = Category.Foot_Armor;
				_description = "Shoes made of chainmail.";
				_value = 1;
			break;

			case ID.Chainmail_Tunic:
				_name = "Iron Chain Tunic";
				_category = Category.Body_Armor;
				_description = "A tunic made of chainmail.";
				_value = 1;
			break;


			case ID.Dagger:
				_name = "Dagger";
				_category = Category.Secondary_Weapon;
				_description = "A short blade, often used as a side-arm.";
				_physicalDamage = 5;
				_spells.Add(Spell.Preset.Severing_Strike);
				_spells.Add(Spell.Preset.Poison_Fang);
				_value = 10;
			break;

			case ID.Gladius:
				_name = "Gladius";
				_category = Category.Primary_Weapon;
				_description = "A short light-weight blade made of steel with a wooden hilt. Used by foot soldiers.";
				_physicalDamage = 10;
				_spells.Add(Spell.Preset.Slash);
				_spells.Add(Spell.Preset.Severing_Strike);
				_value = 10;
			break;

			case ID.Gold:
				_name = "Gold";
				_category = Category.Currency;
				_description = "Item description.";
				_value = 1;
			break;


			case ID.Leather_Boots:
				_name = "Leather Boots";
				_category = Category.Foot_Armor;
				_description = "Boots made of leather.";
				_value = 1;
			break;

			case ID.Leather_Bracers:
				_name = "Leather Bracers";
				_category = Category.Hand_Armor;
				_description = "Bracers made of leather.";
				_value = 1;
			break;

			case ID.Leather_Breeches:
				_name = "Leather Breeches";
				_category = Category.Leg_Armor;
				_description = "Breeches made of leather.";
				_value = 1;
			break;

			case ID.Leather_Jack:
				_name = "Leather Jack";
				_category = Category.Body_Armor;
				_description = "A jack made of leather.";
				_value = 1;
			break;

			case ID.Leather_Helm:
				_name = "Leather Helm";
				_category = Category.Head_Armor;
				_description = "A helm made of leather.";
				_value = 1;
			break;


			case ID.Novice_Tome:
				_name = "Novice's Tome";
				_category = Category.Secondary_Weapon;
				_description = "A tome that contains spells fit for a novice.";
				_spellDamage = 5;
				_spells.Add(Spell.Preset.Lightning_Strike);
				//_spells.Add(Spell.Preset.SummonMinorUndead);
				_value = 10;
			break;
			
			case ID.Parma:
				_name = "Parma";
				_category = Category.Secondary_Weapon;
				_description = "A round wooden shield with an iron frame.";
				_blockModifier = 3;
				_spells.Add(Spell.Preset.Block);
				_value = 10;
			break;
			
			case ID.Potion_of_Clotting:
				_name = "Potion of Clotting";
				_category = Category.Consumable;
				_description = "Reduces the effects of <b><color=#C00000>Bleed</color></b> by 5.";
				_value = 1;
			break;

			case ID.Potion_of_Curing:
				_name = "Potion of Curing";
				_category = Category.Consumable;
				_description = "Reduces the effects of <b><color=#40C000>Poison</color></b> by 5.";
				_value = 1;
			break;

			case ID.Potion_of_Return:
				_name = "Potion of Return";
				_category = Category.Consumable;
				_description = "A dark cloudy potion. Can be used to return to a safe place.";
				_value = 5;
			break;

			
			case ID.Rune_of_Constitution:
				_name = "Rune of Constitution";
				_category = Category.Runestone;
				_description = "Increases constitution.";
				_value = 0;
			break;

			case ID.Rune_of_Dexterity:
				_name = "Rune of Dexterity";
				_category = Category.Runestone;
				_description = "Increases dexterity.";
				_value = 0;
			break;

			case ID.Rune_of_Intelligence:
				_name = "Rune of Intelligence";
				_category = Category.Runestone;
				_description = "Increases intelligence.";
				_value = 0;
			break;

			case ID.Rune_of_Strength:
				_name = "Rune of Strength";
				_category = Category.Runestone;
				_description = "Increases strength.";
				_value = 0;
			break;


			case ID.Small_Key:
				_name = "Small Key";
				_category = Category.Key;
				_keycode = GenerateKeycode();
				_description = "A small key with a unique color.";
				_value = 0;
			break;

			case ID.Staff_of_Flame:
				_name = "Staff of Flame";
				_category = Category.Primary_Weapon;
				_description = "A worn staff, used for casting basic fire spells.";
				_spellDamage = 10;
				_spells.Add(Spell.Preset.Fireball);
				_value = 10;
			break;

		}

		if(!IsStackable()) {
			_quantity = 1;
		}
	}

	public static Sprite GenerateKeycodeSprite(string code, Sprite template, SpriteManager spriteManager) {
		Color baseColor = Color.gray;
		if(ColorUtility.TryParseHtmlString("#"+code, out baseColor)) {
			float h, s, v = 1f;
			Color.RGBToHSV(baseColor, out h, out s, out v);
			Color[] swatch = {Color.HSVToRGB(h, s, v), Color.HSVToRGB(h, s-0.10f, v-0.25f), Color.HSVToRGB(h, s-0.20f, v-0.50f)};
			
			Texture2D texture = new Texture2D(48, 48);
			Color[] colors = template.texture.GetPixels();

			for(int i = 0; i < colors.Length; i++) {
				if(colors[i].a > 0f) {
					colors[i] = Swatch.SwapToColor(colors[i], swatch);
				}	
			}
			
			texture.SetPixels(colors);
			texture.Apply();

			Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 48, 48), new Vector2(0.5f, 0.5f), 48f);
			sprite.texture.filterMode = FilterMode.Point;
			return sprite;
		} else {
			return template;
		}
	}

	public static string GenerateKeycode() {
		float h = UnityEngine.Random.Range(0f, 1f);
		float s = UnityEngine.Random.Range(0.30f, 0.90f);
		float v = UnityEngine.Random.Range(0.70f, 1f);
		return ColorUtility.ToHtmlStringRGB(Color.HSVToRGB(h, s, v));
	}
}
