﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSprites {

	public Sprite unknown;
	public Sprite unknownNeck;
	public Sprite unknownHead;
	public Sprite unknownFinger;
	public Sprite unknownPrimary;
	public Sprite unknownBody;
	public Sprite unknownSecondary;
	public Sprite unknownHands;
	public Sprite unknownLegs;
	public Sprite unknownFeet;

	#region Armor
	public List<Sprite> lightArmorBody;
	public List<Sprite> lightArmorFeet;
	public List<Sprite> lightArmorHands;
	public List<Sprite> lightArmorHead;
	public List<Sprite> lightArmorLegs;

	public List<Sprite> mediumArmorBody;
	public List<Sprite> mediumArmorFeet;
	public List<Sprite> mediumArmorHands;
	public List<Sprite> mediumArmorHead;
	public List<Sprite> mediumArmorLegs;

	public List<Sprite> heavyArmorBody;
	public List<Sprite> heavyArmorFeet;
	public List<Sprite> heavyArmorHands;
	public List<Sprite> heavyArmorHead;
	public List<Sprite> heavyArmorLegs;
	#endregion

	#region Jewelry
	public List<Sprite> jewelryNeck;
	public List<Sprite> jewelryFinger;
	#endregion

	#region Weapons
	public List<Sprite> daggers;
	public List<Sprite> shields;
	public List<Sprite> staves;
	public List<Sprite> swords;
	public List<Sprite> tomes;
	#endregion

	#region Consumables
	public List<Sprite> potions;
	#endregion

	#region Currency
	public List<Sprite> currency;
	#endregion

	public List<Sprite> keys;
	public List<Sprite> runestones;
}