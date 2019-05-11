using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class Portrait : MonoBehaviour {
	public static int size = 48;
	List<BaseUnit.Preset> _startingClasses;
	int _startingClassIndex = 0;
	[SerializeField] UnitBehaviour _previewUnit;

	[SerializeField] Image backgroundImage;
	[SerializeField] Image faceImage;
	[SerializeField] Image mouthImage;
	[SerializeField] Image beardImage;
	[SerializeField] Image noseImage;
	[SerializeField] Image eyebrowImage;
	[SerializeField] Image hairImage;
	[SerializeField] Image frameImage;

	// Templates
	[SerializeField] List<Sprite> faceTemplates;
	[SerializeField] List<Sprite> mouthTemplates;
	[SerializeField] List<Sprite> beardTemplates;
	[SerializeField] List<Sprite> noseTemplates;
	[SerializeField] List<Sprite> eyebrowTemplates;
	[SerializeField] List<Sprite> hairTemplates;

	// Indices
	[SerializeField] Character.FaceType _faceType = Character.FaceType.Human_0;
	[SerializeField] Character.MouthType _mouthType = Character.MouthType.Human_0;
	[SerializeField] Character.BeardType _beardType = Character.BeardType.None;
	[SerializeField] Character.NoseType _noseType = Character.NoseType.Human_0;
	[SerializeField] Character.EyebrowType _eyebrowType = Character.EyebrowType.Eyebrow_0;
	[SerializeField] Character.HairType _hairType = Character.HairType.Long_2;

	[SerializeField] Character.SkinColor _skinColor = Character.SkinColor.Human_Light;
	[SerializeField] Character.HairColor _hairColor = Character.HairColor.Chestnut;

	[SerializeField] BaseUnit.Preset _startingClass = BaseUnit.Preset.Warrior;

	#region Accessors
	public Character.FaceType faceType {
		get {return _faceType;}
	}

	public Character.MouthType mouthType {
		get {return _mouthType;}
	}

	public Character.BeardType beardType {
		get {return _beardType;}
	}

	public Character.NoseType noseType {
		get {return _noseType;}
	}

	public Character.EyebrowType eyebrowType {
		get {return _eyebrowType;}
	}

	public Character.HairType hairType {
		get {return _hairType;}
	}

	public Character.SkinColor skinColor {
		get {return _skinColor;}
	}

	public Character.HairColor hairColor {
		get {return _hairColor;}
	}

	public BaseUnit.Preset startingClass {
		get {return _startingClass;}
	}

	public UnitBehaviour previewUnit {
		get {return _previewUnit;}
	}
	#endregion

	void Awake() {
		InitStartingClasses();
		ApplySwatches();
	}

	void InitStartingClasses() {
		_startingClasses = new List<BaseUnit.Preset>();
		_startingClasses.Add(BaseUnit.Preset.Warrior);
		//_startingClasses.Add(BaseUnit.Preset.Rogue);
		_startingClasses.Add(BaseUnit.Preset.Mage);
	}


	void ApplySwatches() {

		// Face
		ApplySwatch(faceImage, faceTemplates[(int)_faceType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));

		// Mouth
		ApplySwatch(mouthImage, mouthTemplates[(int)_mouthType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));

		// Beard
		ApplySwatch(beardImage, beardTemplates[(int)_beardType], ParseColor(Swatch.GetHairSwatch(_hairColor)));

		// Nose
		ApplySwatch(noseImage, noseTemplates[(int)_noseType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));

		// Eyebrows
		ApplySwatch(eyebrowImage, eyebrowTemplates[(int)_eyebrowType], ParseColor(Swatch.GetHairSwatch(_hairColor)));

		// Hair
		ApplySwatch(hairImage, hairTemplates[(int)_hairType], ParseColor(Swatch.GetHairSwatch(_hairColor)));
		ApplyToPreviewCharacter();
	}

	void ApplySwatch(Image img, Sprite template, Color[] swatch) {
		if(img == null || template == null || swatch == null) {return;}

		Texture2D texture = new Texture2D(size, size);

		Color[] colors = template.texture.GetPixels();
		for(int i = 0; i < colors.Length; i++) {
			if(colors[i].a > 0f) {
				colors[i] = Swatch.SwapToColor(colors[i], swatch);
			}	
		}
		
		texture.SetPixels(colors);
		texture.Apply();

		Sprite sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 48f);
		sprite.texture.filterMode = FilterMode.Point;
		img.sprite = sprite;
	}

	Color[] ParseColor(string[] arr) {
		Color[] palette = new Color[Swatch.size];

		for(int i = 0; i < Swatch.size; i++) {
			if(i >= arr.Length) {
				ColorUtility.TryParseHtmlString(Swatch.template[i], out palette[i]);
			} else {
				ColorUtility.TryParseHtmlString(arr[i], out palette[i]);
			}
		}
		
		return palette;
	}

	public void LoadCharacter(Character character) {
		_faceType = character.faceType;
		_mouthType = character.mouthType;
		_beardType = character.beardType;
		_noseType = character.noseType;
		_eyebrowType = character.eyebrowType;
		_hairType = character.hairType;
		_skinColor = character.skinColor;
		_hairColor = character.hairColor;
		ShowCharacter();
		ApplySwatches();
	}

	public void ShowUI() {
		ShowCharacter();
	}

	public void HideUI() {
		HideCharacter();
	}

	public void ShowCharacter() {
		faceImage.enabled = true;
		mouthImage.enabled = true;
		beardImage.enabled = true;
		noseImage.enabled = true;
		eyebrowImage.enabled = true;
		hairImage.enabled = true;
	}

	public void HideCharacter() {
		faceImage.enabled = false;
		mouthImage.enabled = false;
		beardImage.enabled = false;
		noseImage.enabled = false;
		eyebrowImage.enabled = false;
		hairImage.enabled = false;
	}

	#region Interface
	void ApplyToPreviewCharacter() {
		if(_previewUnit == null) {return;}

		Attributes attributes = new Attributes(Attributes.Preset.Warrior);
		BaseUnit.Preset preset = BaseUnit.Preset.Warrior;
		switch(_startingClass) {
		
			case BaseUnit.Preset.Mage:
				attributes = new Attributes(Attributes.Preset.Mage);
				preset = BaseUnit.Preset.Mage;
			break;

			case BaseUnit.Preset.Rogue:
				attributes = new Attributes(Attributes.Preset.Rogue);
				preset = BaseUnit.Preset.Rogue;
			break;

			case BaseUnit.Preset.Warrior:
			break;
		}

		_previewUnit.baseUnit = new BaseUnit(true, attributes.preset, preset, null, true, true);
		

		// Unit equipment
		_previewUnit.baseUnit.bag = new Bag(Bag.BagType.Bag);
		_previewUnit.baseUnit.bag.Add(new BaseItem(BaseItem.ID.Gold, 120));
		_previewUnit.baseUnit.bag.Add(new BaseItem(BaseItem.ID.Potion_of_Return));

		switch(_startingClass) {
			case BaseUnit.Preset.Mage:
				_previewUnit.baseUnit.bag.Add(new BaseItem(BaseItem.ID.Staff_of_Flame));
				_previewUnit.baseUnit.bag.Add(new BaseItem(BaseItem.ID.Novice_Tome));

				_previewUnit.baseUnit.bag.Equip(_previewUnit.baseUnit.bag.FindItemWithID(BaseItem.ID.Staff_of_Flame));
				_previewUnit.baseUnit.bag.Equip(_previewUnit.baseUnit.bag.FindItemWithID(BaseItem.ID.Novice_Tome));
			break;

			case BaseUnit.Preset.Rogue:
				_previewUnit.baseUnit.bag.Add(new BaseItem(BaseItem.ID.Dagger));

				_previewUnit.baseUnit.bag.Equip(_previewUnit.baseUnit.bag.FindItemWithID(BaseItem.ID.Dagger));
			break;

			case BaseUnit.Preset.Warrior:
				_previewUnit.baseUnit.bag.Add(new BaseItem(BaseItem.ID.Gladius));
				_previewUnit.baseUnit.bag.Add(new BaseItem(BaseItem.ID.Parma));

				_previewUnit.baseUnit.bag.Equip(_previewUnit.baseUnit.bag.FindItemWithID(BaseItem.ID.Gladius));
				_previewUnit.baseUnit.bag.Equip(_previewUnit.baseUnit.bag.FindItemWithID(BaseItem.ID.Parma));
			break;
		}

		// Customization
		_previewUnit.baseUnit.character = new Character();
		_previewUnit.baseUnit.character.faceType = faceType;
		_previewUnit.baseUnit.character.mouthType = mouthType;
		_previewUnit.baseUnit.character.beardType = beardType;
		_previewUnit.baseUnit.character.noseType = noseType;
		_previewUnit.baseUnit.character.hairType = hairType;
		_previewUnit.baseUnit.character.skinColor = skinColor;
		_previewUnit.baseUnit.character.hairColor = hairColor;

		_previewUnit.LoadSprites();

	}

	public void NextFace() {
		_faceType++;
		if((int)_faceType > Enum.GetValues(typeof(Character.FaceType)).Length-1) {
			_faceType = 0;
		}

		ApplySwatch(faceImage, faceTemplates[(int)_faceType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));
		ApplyToPreviewCharacter();
	}

	public void PrevFace() {
		_faceType--;
		if((int)_faceType < 0) {
			_faceType = (Character.FaceType)Enum.GetValues(typeof(Character.FaceType)).Length-1;
		}

		ApplySwatch(faceImage, faceTemplates[(int)_faceType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));
		ApplyToPreviewCharacter();
	}

	public void NextMouth() {
		_mouthType++;
		if((int)_mouthType > Enum.GetValues(typeof(Character.MouthType)).Length-1) {
			_mouthType = 0;
		}

		ApplySwatch(mouthImage, mouthTemplates[(int)_mouthType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));
		ApplyToPreviewCharacter();
	}

	public void PrevMouth() {
		_mouthType--;
		if((int)_mouthType < 0) {
			_mouthType = (Character.MouthType)Enum.GetValues(typeof(Character.MouthType)).Length-1;
		}

		ApplySwatch(mouthImage, mouthTemplates[(int)_mouthType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));
		ApplyToPreviewCharacter();
	}

	public void NextBeard() {
		_beardType++;
		if((int)_beardType > Enum.GetValues(typeof(Character.BeardType)).Length-1) {
			_beardType = 0;
		}

		ApplySwatch(beardImage, beardTemplates[(int)_beardType], ParseColor(Swatch.GetHairSwatch(_hairColor)));
		ApplyToPreviewCharacter();
	}

	public void PrevBeard() {
		_beardType--;
		if((int)_beardType < 0) {
			_beardType = (Character.BeardType)Enum.GetValues(typeof(Character.BeardType)).Length-1;
		}

		ApplySwatch(beardImage, beardTemplates[(int)_beardType], ParseColor(Swatch.GetHairSwatch(_hairColor)));
		ApplyToPreviewCharacter();
	}

	public void NextNose() {
		_noseType++;
		if((int)_noseType > Enum.GetValues(typeof(Character.NoseType)).Length-1) {
			_noseType = 0;
		}

		ApplySwatch(noseImage, noseTemplates[(int)_noseType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));
		ApplyToPreviewCharacter();
	}

	public void PrevNose() {
		_noseType--;
		if((int)_noseType < 0) {
			_noseType = (Character.NoseType)Enum.GetValues(typeof(Character.NoseType)).Length-1;
		}

		ApplySwatch(noseImage, noseTemplates[(int)_noseType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));
		ApplyToPreviewCharacter();
	}

	public void NextEyebrows() {
		_eyebrowType++;
		if((int)_eyebrowType > Enum.GetValues(typeof(Character.EyebrowType)).Length-1) {
			_eyebrowType = 0;
		}

		ApplySwatch(eyebrowImage, eyebrowTemplates[(int)_eyebrowType], ParseColor(Swatch.GetHairSwatch(_hairColor)));
		ApplyToPreviewCharacter();
	}

	public void PrevEyebrows() {
		_eyebrowType--;
		if((int)_eyebrowType < 0) {
			_eyebrowType = (Character.EyebrowType)Enum.GetValues(typeof(Character.EyebrowType)).Length-1;
		}

		ApplySwatch(eyebrowImage, eyebrowTemplates[(int)_eyebrowType], ParseColor(Swatch.GetHairSwatch(_hairColor)));
		ApplyToPreviewCharacter();
	}

	public void NextHair() {
		_hairType++;
		if((int)_hairType > Enum.GetValues(typeof(Character.HairType)).Length-1) {
			_hairType = 0;
		}

		ApplySwatch(hairImage, hairTemplates[(int)_hairType], ParseColor(Swatch.GetHairSwatch(_hairColor)));
		ApplyToPreviewCharacter();
	}

	public void PrevHair() {
		_hairType--;
		if((int)_hairType < 0) {
			_hairType = (Character.HairType)Enum.GetValues(typeof(Character.HairType)).Length-1;
		}

		ApplySwatch(hairImage, hairTemplates[(int)_hairType], ParseColor(Swatch.GetHairSwatch(_hairColor)));
		ApplyToPreviewCharacter();
	}

	public void NextHairColor() {
		_hairColor++;
		if((int)_hairColor > Enum.GetValues(typeof(Character.HairColor)).Length-1) {
			_hairColor = 0;
		}

		ApplySwatch(beardImage, beardTemplates[(int)_beardType], ParseColor(Swatch.GetHairSwatch(_hairColor)));
		ApplySwatch(eyebrowImage, eyebrowTemplates[(int)_eyebrowType], ParseColor(Swatch.GetHairSwatch(_hairColor)));
		ApplySwatch(hairImage, hairTemplates[(int)_hairType], ParseColor(Swatch.GetHairSwatch(_hairColor)));
		ApplyToPreviewCharacter();
	}

	public void PrevHairColor() {
		_hairColor--;
		if((int)_hairColor < 0) {
			_hairColor = (Character.HairColor)Enum.GetValues(typeof(Character.HairColor)).Length-1;
		}

		ApplySwatch(beardImage, beardTemplates[(int)_beardType], ParseColor(Swatch.GetHairSwatch(_hairColor)));
		ApplySwatch(eyebrowImage, eyebrowTemplates[(int)_eyebrowType],ParseColor(Swatch.GetHairSwatch(_hairColor)));
		ApplySwatch(hairImage, hairTemplates[(int)_hairType], ParseColor(Swatch.GetHairSwatch(_hairColor)));
		ApplyToPreviewCharacter();
	}

	public void NextSkinColor() {
		_skinColor++;
		if((int)_skinColor > Enum.GetValues(typeof(Character.SkinColor)).Length-1) {
			_skinColor = 0;
		}

		ApplySwatch(faceImage, faceTemplates[(int)_faceType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));
		ApplySwatch(mouthImage, mouthTemplates[(int)_mouthType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));
		ApplySwatch(noseImage, noseTemplates[(int)_noseType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));
		ApplyToPreviewCharacter();
	}

	public void PrevSkinColor() {
		_skinColor--;
		if((int)_skinColor < 0) {
			_skinColor = (Character.SkinColor)Enum.GetValues(typeof(Character.SkinColor)).Length-1;
		}

		ApplySwatch(faceImage, faceTemplates[(int)_faceType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));
		ApplySwatch(mouthImage, mouthTemplates[(int)_mouthType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));
		ApplySwatch(noseImage, noseTemplates[(int)_noseType], ParseColor(Swatch.GetSkinSwatch(_skinColor)));
		ApplyToPreviewCharacter();
	}

	public void PreviousStartingClass() {
		_startingClassIndex--;
		if(_startingClassIndex < 0) {
			_startingClassIndex = _startingClasses.Count-1;
		}

		_startingClass = _startingClasses[_startingClassIndex];
		Debug.Log(_startingClass);
		ApplyToPreviewCharacter();
	}

	public void NextStartingClass() {
		_startingClassIndex++;
		if(_startingClassIndex > _startingClasses.Count-1) {
			_startingClassIndex = 0;
		}

		_startingClass = _startingClasses[_startingClassIndex];
		ApplyToPreviewCharacter();
	}
	#endregion
	

}
