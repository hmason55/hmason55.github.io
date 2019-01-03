using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem {
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
	int _id;
	int _tier;
	int _value;

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
	public int tier {
		get {return _tier;}
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
	#endregion

	// Any
	public BaseItem(Category c, int t) {
		_category = c;
		_tier = t;

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

		switch(_category) {

			case Category.Neck_Jewelry:
				_sprite = spriteManager.items.jewelryNeck[_tier-1];
			break;
			
			case Category.Finger_Jewelry:
				_sprite = spriteManager.items.jewelryFinger[_tier-1];
			break;

			case Category.Primary_Weapon:
			case Category.Secondary_Weapon:
			break;

			case Category.Body_Armor:
				switch(_armorWeight) {
					case ArmorWeight.Light:
						_sprite = spriteManager.items.lightArmorBody[_tier-1];
					break;

					case ArmorWeight.Medium:
						_sprite = spriteManager.items.mediumArmorBody[_tier-1];
					break;

					case ArmorWeight.Heavy:
						_sprite = spriteManager.items.heavyArmorBody[_tier-1];
					break;
				}
			break;

			case Category.Foot_Armor:
				switch(_armorWeight) {
					case ArmorWeight.Light:
						_sprite = spriteManager.items.lightArmorFeet[_tier-1];
					break;

					case ArmorWeight.Medium:
						_sprite = spriteManager.items.mediumArmorFeet[_tier-1];
					break;

					case ArmorWeight.Heavy:
						_sprite = spriteManager.items.heavyArmorFeet[_tier-1];
					break;
				}
			break;

			case Category.Hand_Armor:
				switch(_armorWeight) {
					case ArmorWeight.Light:
						_sprite = spriteManager.items.lightArmorHands[_tier-1];
					break;

					case ArmorWeight.Medium:
						_sprite = spriteManager.items.mediumArmorHands[_tier-1];
					break;

					case ArmorWeight.Heavy:
						_sprite = spriteManager.items.heavyArmorHands[_tier-1];
					break;
				}
			break;

			case Category.Head_Armor:
				switch(_armorWeight) {
					case ArmorWeight.Light:
						_sprite = spriteManager.items.lightArmorHead[_tier-1];
					break;

					case ArmorWeight.Medium:
						_sprite = spriteManager.items.mediumArmorHead[_tier-1];
					break;

					case ArmorWeight.Heavy:
						_sprite = spriteManager.items.heavyArmorHead[_tier-1];
					break;
				}
			break;

			case Category.Leg_Armor:
				switch(_armorWeight) {
					case ArmorWeight.Light:
						_sprite = spriteManager.items.lightArmorLegs[_tier-1];
					break;

					case ArmorWeight.Medium:
						_sprite = spriteManager.items.mediumArmorLegs[_tier-1];
					break;

					case ArmorWeight.Heavy:
						_sprite = spriteManager.items.heavyArmorLegs[_tier-1];
					break;
				}
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
		return "Tier " + _tier.ToString() + " " + _category.ToString().Replace("_", " ");
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
}
