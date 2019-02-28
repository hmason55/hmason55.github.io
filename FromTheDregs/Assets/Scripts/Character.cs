using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character {
    public string name = "Character Name";
	public string location = "Unknown";
    FaceType _faceType;
	MouthType _mouthType;
	BeardType _beardType;
	NoseType _noseType;
	EyebrowType _eyebrowType;
	HairType _hairType;
	SkinColor _skinColor;
	HairColor _hairColor;

	Attributes _attributes;
	Bag _bag;

	#region Accessors
	public FaceType faceType {
		get {return _faceType;}
		set {_faceType = value;}
	}

	public MouthType mouthType {
		get {return _mouthType;}
		set {_mouthType = value;}
	}

	public BeardType beardType {
		get {return _beardType;}
		set {_beardType = value;}
	}

	public NoseType noseType {
		get {return _noseType;}
		set {_noseType = value;}
	}

	public EyebrowType eyebrowType {
		get {return _eyebrowType;}
		set {_eyebrowType = value;}
	}

	public HairType hairType {
		get {return _hairType;}
		set {_hairType = value;}
	}

	public SkinColor skinColor {
		get {return _skinColor;}
		set {_skinColor = value;}
	}

	public HairColor hairColor {
		get {return _hairColor;}
		set {_hairColor = value;}
	}
	#endregion

	#region Constructors
    public Character() {
        this.name = "Character Name";

        // Default Character
        _faceType = FaceType.Human_0;
        _mouthType = MouthType.Human_0;
        _beardType = BeardType.None;
        _noseType = NoseType.Human_0;
        _eyebrowType = EyebrowType.Eyebrow_0;
        _hairType = HairType.Long_2;
        _skinColor = SkinColor.Human_Light;
        _hairColor = HairColor.Chestnut;

    }
	#endregion

    public enum FaceType {
		Elf_0,
		Elf_1,
		Elf_2,
		Human_0,
		Human_1,
		Human_2,
		Orc_0,
		Orc_1,
	}

	public enum MouthType {
		Elf_0,
		Elf_1,
		Human_0,
		Human_1,
		Human_2,
		Human_3,
		Orc_0,
		Orc_1,
	}

	public enum BeardType {
		None,
		Beard_0,
		Beard_1,
		Beard_2,
		Beard_3,
		Beard_4,
		Beard_5,
		Beard_6,
		Beard_7,
		Beard_8,
	}

	public enum NoseType {
		Elf_0,
		Elf_1,
		Human_0,
		Human_1,
		Human_2,
		Human_3,
		Orc_0,
		Orc_1,
	}

	public enum EyebrowType {
		None,
		Eyebrow_0,
		Eyebrow_1,
		Eyebrow_2,
	}

	public enum HairType {
		None,
		Long_0,
		Long_1,
		Long_2,
		Long_3,
		Medium_0,
		Medium_1,
		Medium_2,
		Medium_3,
		Short_0,
		Short_1,
		Short_2,
	}

	public enum SkinColor {
		Elf_Dark,
		Human_Dark,
		Human_Light,
		Human_Medium,
		Orc_Medium,
	}

	public enum HairColor {
		Platinum,
		Blonde,
		Amber,
		Chestnut,
		Chocolate,
		Auburn,
		Red,
		Gray,
		White
	}
}
