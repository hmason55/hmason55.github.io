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
		Cloth,
		Silk
	}

	public enum MediumArmorTier {
		Leather,
		Studded_Leather
	}

	public enum HeavyArmorTier {
		Iron_Chain,
		Iron_Plate
	}

	Category _category;
	int _id;
	int _tier;
	int _value;

	#region Attributes
	// Equipment Attributes
	int _physicalAttack;
	int _magicalAttack;
	int _physicalDefense;
	int _magicalDefense;
	ArmorWeight _armorWeight;

	// Consumable Attributes
	int _cooldownDuration;
	Sprite _sprite;
	#endregion

	public int tier {
		get {return _tier;}
	}

	public Category category {
		get {return _category;}
	}

	public Sprite sprite {
		get {return _sprite;}
	}

	public BaseItem(Category c, int t) {
		_category = c;
		_tier = t;
		RollCategory();

		switch(_category) {
			case Category.Body_Armor:
			case Category.Foot_Armor:
			case Category.Hand_Armor:
			case Category.Head_Armor:
			case Category.Leg_Armor:
				RollArmorWeight();
				
				_physicalAttack = (3 - ((int)_armorWeight)) * (_tier);
				_magicalAttack =  (3 - ((int)_armorWeight)) * (_tier);

				_physicalDefense = ((int)_armorWeight + 1) * (_tier);
				_magicalDefense =  ((int)_armorWeight + 1) * (_tier);
			break;
		}

		Debug.Log(NameToString());
	}

	public void LoadSprite(SpriteManager spriteManager) {
		if(spriteManager == null) {return;}

		switch(_category) {
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
		int ndx = UnityEngine.Random.Range(3, 7);
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

	public string AttributesToString() {
		string attributeString = "";


		if(_physicalAttack != 0) {
			attributeString += _physicalAttack + " Physical Attack\n";
		}

		if(_magicalAttack != 0) {
			attributeString += _magicalAttack + " Magical Attack\n";
		}
		
		attributeString += "\n";

		if(_physicalDefense != 0) {
			attributeString += _physicalDefense + " Physical Defense\n";
		}

		if(_magicalDefense != 0) {
			attributeString += _magicalDefense + " Magical Defense\n";
		}
		
		return attributeString;
	}
}
