using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseItem {

	public enum ID {
		
		Cotton_Breeches,
		Cotton_Gloves,
		Cotton_Hood,
		Cotton_Shoes,
		Cotton_Tunic,

		Gold,

		Iron_Chain_Coif,
		Iron_Chain_Gloves,
		Iron_Chain_Leggings,
		Iron_Chain_Shoes,
		Iron_Chain_Tunic,

		Leather_Boots,
		Leather_Bracers,
		Leather_Breeches,
		Leather_Jack,
		Leather_Helm,

		Small_Key,

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
		Unknown_Consumable,
		Healing_Consumable,
		Poisonous_Consumable,
		Trophy,
		Currency,
		Key
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
	int _tier;
	int _quantity;
	string _description;
	int _value;
	string _keycode = "XXXXXX";

	#region Attributes
	// Equipment Attributes
	int _attack;
	int _defense;

	int _healthTotal;
	int _healthRecovery;

	int _essenceTotal;
	int _essenceRecovery;

	int _movementSpeed;
	//int _physicalAttack;
	//int _magicalAttack;
	//int _physicalDefense;
	//int _magicalDefense;
	ArmorWeight _armorWeight;

	// Consumable Attributes
	int _cooldownDuration;
	Sprite _sprite;
	#endregion

	#region Accessors

	public ID id {
		get {return _id;}
	}

	public string keycode {
		get {return _keycode;}
		set {_keycode = value;}
	}

	public int tier {
		get {return _tier;}
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

	public int attack {
		get {return _attack;}
	}

	public int defense {
		get {return _defense;}
	}

	public int healthTotal {
		get {return _healthTotal;}
	}

	public int healthRecovery {
		get {return _healthRecovery;}
	}
	
	public int essenceTotal {
		get {return _essenceTotal;}
	}

	public int essenceRecovery {
		get {return _essenceRecovery;}
	}

	public int movementSpeed {
		get {return _movementSpeed;}
	}

	public int value {
		get {return _value;}
	}
	#endregion

	// Any
	public BaseItem(Category c, int t) {
		_category = c;
		_tier = t;
		_value = 1;

		switch(_category) {
			case Category.Neck_Jewelry:
				_attack = 1 * _tier;
				//_physicalAttack = 2 * _tier;
				//_magicalAttack =  2 * _tier;
			break;

			case Category.Finger_Jewelry:
				_attack = 1 * _tier;
				//_physicalAttack = 1 * _tier;
				//_magicalAttack =  1 * _tier;
			break;

			case Category.Body_Armor:
			case Category.Foot_Armor:
			case Category.Hand_Armor:
			case Category.Head_Armor:
			case Category.Leg_Armor:
				RollArmorWeight();

				_attack = (2 - ((int)_armorWeight)) * (_tier);
				//_physicalAttack = (3 - ((int)_armorWeight)) * (_tier);
				//_magicalAttack =  (3 - ((int)_armorWeight)) * (_tier);

				_defense = ((int)_armorWeight) * (_tier);
				//_physicalDefense = ((int)_armorWeight + 1) * (_tier);
				//_magicalDefense =  ((int)_armorWeight + 1) * (_tier);
			break;
		}
	}

	// Armor
	public BaseItem(Category c, ArmorWeight w, int t) {
		_category = c;
		_armorWeight = w;
		_tier = t;
		_value = 1;

		Init();
	}

	void Init() {
		switch(_category) {
			case Category.Neck_Jewelry:
				_attack = 1 * _tier;
				//_physicalAttack = 2 * _tier;
				//_magicalAttack =  2 * _tier;
			break;

			case Category.Finger_Jewelry:
				_attack = 1 * _tier;
				//_physicalAttack = 1 * _tier;
				//_magicalAttack =  1 * _tier;
			break;

			case Category.Body_Armor:
			case Category.Foot_Armor:
			case Category.Hand_Armor:
			case Category.Head_Armor:
			case Category.Leg_Armor:
				
				_attack = (2 - ((int)_armorWeight)) * (_tier);
				//_physicalAttack = (3 - ((int)_armorWeight)) * (_tier);
				//_magicalAttack =  (3 - ((int)_armorWeight)) * (_tier);

				_defense = ((int)_armorWeight) * (_tier);
				//_physicalDefense = ((int)_armorWeight + 1) * (_tier);
				//_magicalDefense =  ((int)_armorWeight + 1) * (_tier);
			break;
		}
	}

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


			case ID.Gold:
				_sprite = spriteManager.items.currency[0];
			break;


			case ID.Iron_Chain_Coif:
				_sprite = spriteManager.items.heavyArmorHead[0];
			break;

			case ID.Iron_Chain_Gloves:
				_sprite = spriteManager.items.heavyArmorHands[0];
			break;

			case ID.Iron_Chain_Leggings:
				_sprite = spriteManager.items.heavyArmorLegs[0];
			break;

			case ID.Iron_Chain_Shoes:
				_sprite = spriteManager.items.heavyArmorFeet[0];
			break;

			case ID.Iron_Chain_Tunic:
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


			case ID.Small_Key:
				_sprite = GenerateKeycodeSprite(_keycode, spriteManager.items.keys[0], spriteManager);
			break;
		}
	}

	void RollArmorWeight() {
		int ndx = UnityEngine.Random.Range(0, Enum.GetValues(typeof(ArmorWeight)).Length);
		_armorWeight = (ArmorWeight)ndx;
	}

	void RollCategory() {
		int ndx = UnityEngine.Random.Range(3, 8);
		_category = (Category)ndx;
	}

	public string NameToString() {
		string name = "";
		string type = "";
		string piece = "";
		string suffix = "";
		// Prefix

		// Type
		switch(_category) {
			case Category.Body_Armor:
			case Category.Foot_Armor:
			case Category.Hand_Armor:
			case Category.Head_Armor:
			case Category.Leg_Armor:
				switch(_armorWeight) {
					case ArmorWeight.Light:
						type = ((LightArmorTier)_tier - 1).ToString();
					break;

					case ArmorWeight.Medium:
						type = ((MediumArmorTier)_tier - 1).ToString();
					break;

					case ArmorWeight.Heavy:
						type = ((HeavyArmorTier)_tier - 1).ToString();
					break;
				}
			break;

			case Category.Finger_Jewelry:
			case Category.Neck_Jewelry:
				type = ((JewelryTier)_tier - 1).ToString();
			break;
		}

		// Piece
		switch(_category) {
			case Category.Body_Armor:
				switch(_armorWeight) {
					case ArmorWeight.Light:
						piece = "Tunic";
					break;

					case ArmorWeight.Medium:
						piece = "Jack";
					break;

					case ArmorWeight.Heavy:
						piece = "Cuirass";
					break;
				}
			break;
			
			case Category.Foot_Armor:
				switch(_armorWeight) {
					case ArmorWeight.Light:
						piece = "Shoes";
					break;

					case ArmorWeight.Medium:
						piece = "Boots";
					break;

					case ArmorWeight.Heavy:
						piece = "Greaves";
					break;
				}
			break;

			case Category.Hand_Armor:
				switch(_armorWeight) {
					case ArmorWeight.Light:
						piece = "Gloves";
					break;

					case ArmorWeight.Medium:
						piece = "Bracers";
					break;

					case ArmorWeight.Heavy:
						piece = "Gauntlets";
					break;
				}
			break;

			case Category.Head_Armor:
				switch(_armorWeight) {
					case ArmorWeight.Light:
						piece = "Hood";
					break;

					case ArmorWeight.Medium:
						piece = "Helm";
					break;

					case ArmorWeight.Heavy:
						piece = "Helm";
					break;
				}
			break;

			case Category.Leg_Armor:
				switch(_armorWeight) {
					case ArmorWeight.Light:
						piece = "Breeches";
					break;

					case ArmorWeight.Medium:
						piece = "Breeches";
					break;

					case ArmorWeight.Heavy:
						piece = "Leggings";
					break;
				}
			break;

			case Category.Neck_Jewelry:
				piece = "Necklace";
			break;

			case Category.Finger_Jewelry:
				piece = "Ring";
			break;
		}

		if(type != "") {
			
			name += type;

			if(piece != "") {
				name += " " + piece;
			}
		}

		name = name.Replace("_", " ");

		// Suffix
		return name;
	}

	public string CategoryToString() {
		return _category.ToString().Replace("_", " ");
	}

	public string AttributeCategoriesToString() {
		string attributeString = "";

		if(_attack > 0) {
			attributeString += "<b><color=cyan>Attack</color></b>\n";
		} else if(_attack < 0) {
			attributeString += "<b><color=red>Attack</color></b>\n";
		} else {
			attributeString += "Attack\n";
		}

		if(_defense > 0) {
			attributeString += "<b><color=cyan>Defense</color></b>\n";
		} else if(_defense < 0) {
			attributeString += "<b><color=red>Defense</color></b>\n";
		} else {
			attributeString += "Defense\n";
		}

		attributeString += "\n";
		attributeString += "Health Total\n";
		attributeString += "Health Recovery\n";
		attributeString += "\n";
		attributeString += "Essence Total\n";
		attributeString += "Essence Recovery\n";
		attributeString += "\n";
		attributeString += "Movement Speed\n";

		return attributeString;
	}

	public string AttributeValuesToString() {
		string attributeString = "";

		if(_attack > 0) {
			attributeString += "<b><color=cyan>+ " + (_attack) + "</color></b>\n";
		} else if(_attack < 0) {
			attributeString += "<b><color=red>- " + Mathf.Abs(_attack) + "</color></b>\n";
		} else {
			attributeString += _attack + "\n";
		}

		if(_defense > 0) {
			attributeString += "<b><color=cyan>+ " + _defense + "</color></b>\n";
		} else if(_defense < 0) {
			attributeString += "<b><color=red>- " + Mathf.Abs(_defense) + "</color></b>\n";
		} else {
			attributeString += _defense + "\n";
		}

		attributeString += "\n";

		attributeString += _healthTotal + "\n";
		attributeString += _healthRecovery + "\n";
		attributeString += "\n";
		attributeString += _essenceTotal + "\n";
		attributeString += _essenceRecovery + "\n";
		attributeString += "\n";
		attributeString += _movementSpeed + "\n";

		return attributeString;
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

		switch(item) {

			case ID.Cotton_Breeches:
				_name = "Cotton Breeches";
				_category = Category.Leg_Armor;
				_description = "Breeches made of cotton.";
				_value = 20;
			break;

			case ID.Cotton_Gloves:
				_name = "Cotton Gloves";
				_category = Category.Hand_Armor;
				_description = "Gloves made of cotton.";
				_value = 20;
			break;

			case ID.Cotton_Hood:
				_name = "Cotton Hood";
				_category = Category.Head_Armor;
				_description = "A hood made of cotton.";
				_value = 20;
			break;

			case ID.Cotton_Shoes:
				_name = "Cotton Shoes";
				_category = Category.Foot_Armor;
				_description = "Shoes made of cotton.";
				_value = 20;
			break;

			case ID.Cotton_Tunic:
				_name = "Cotton Tunic";
				_category = Category.Body_Armor;
				_description = "A tunic made of cotton.";
				_value = 20;
			break;


			case ID.Gold:
				_name = "Gold";
				_category = Category.Currency;
				_description = "Item description.";
				_value = 1;
			break;


			case ID.Iron_Chain_Coif:
				_name = "Iron Chain Coif";
				_category = Category.Head_Armor;
				_description = "A foot soldier's coif.";
				_value = 20;
			break;

			case ID.Iron_Chain_Gloves:
				_name = "Iron Chain Gloves";
				_category = Category.Hand_Armor;
				_description = "Gloves made of iron chainmail.";
				_value = 20;
			break;

			case ID.Iron_Chain_Leggings:
				_name = "Iron Chain Leggings";
				_category = Category.Leg_Armor;
				_description = "Leggings made of chainmail.";
				_value = 20;
			break;

			case ID.Iron_Chain_Shoes:
				_name = "Iron Chain Shoes";
				_category = Category.Foot_Armor;
				_description = "Shoes made of chainmail.";
				_value = 20;
			break;

			case ID.Iron_Chain_Tunic:
				_name = "Iron Chain Tunic";
				_category = Category.Body_Armor;
				_description = "A tunic made of chainmail.";
				_value = 20;
			break;


			case ID.Leather_Boots:
				_name = "Leather Boots";
				_category = Category.Foot_Armor;
				_description = "Boots made of leather.";
				_value = 20;
			break;

			case ID.Leather_Bracers:
				_name = "Leather Boots";
				_category = Category.Foot_Armor;
				_description = "Boots made of leather.";
				_value = 20;
			break;

			case ID.Leather_Breeches:
				_name = "Leather Breeches";
				_category = Category.Leg_Armor;
				_description = "Breeches made of leather.";
				_value = 20;
			break;

			case ID.Leather_Jack:
				_name = "Leather Jack";
				_category = Category.Body_Armor;
				_description = "A jack made of leather.";
				_value = 20;
			break;

			case ID.Leather_Helm:
				_name = "Leather Helm";
				_category = Category.Head_Armor;
				_description = "A helm made of leather.";
				_value = 20;
			break;


			case ID.Small_Key:
				_name = "Small Key";
				_category = Category.Key;
				_keycode = GenerateKeycode();
				_description = "A small key with a unique color and the engraving: \"" + _keycode + "\"";
				_value = 0;
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
