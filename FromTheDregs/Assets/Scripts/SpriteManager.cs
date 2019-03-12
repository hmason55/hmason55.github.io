using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpriteManager : MonoBehaviour {
	public ChunkSprites chunk;
	public BiomeSprites biomeDungeon;
	public BiomeSprites biomeForsaken;
	public BiomeSprites biomeCavern;
	public BiomeSprites biomeRuins;
	public BiomeSprites biomeCrypt;
	public List<Sprite> borderShaded;

	public Sprite shadowSmall;
	public Sprite shadowMedium;
	public Sprite shadowLarge;
	public UnitSprites unitDireRat1;
	public UnitSprites unitDireRatSmall1;
	public UnitSprites unitHumanWizard1;
	public UnitSprites unitGreenSlime1;
	public UnitSprites unitSandBehemoth1;
	public UnitSprites unitSandworm1;
	public UnitSprites unitSpider1;
	public UnitSprites unitSpiderSmall1;
	public UnitSprites unitWarrior1;
	public UnitSprites unitWidow1;
	public UnitSprites unitWidowSmall1;

	public List<Sprite> cavernDoor;
	public List<Sprite> cryptDoor;
	public List<Sprite> hedgeDoor;
	public List<Sprite> dungeonDoor;

	public ItemSprites items;
}
