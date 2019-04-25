using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class EssenceUI : MonoBehaviour {

	[SerializeField] GameObject _essencePrefabEmpty;
	[SerializeField] GameObject _essencePrefabFilled;
	Image[] _images;

	BaseUnit _baseUnit;

	public Image[] images {
		get {return _images;}
	}

	public BaseUnit baseUnit {
		get {return _baseUnit;}
		set {_baseUnit = value;}
	}

	void Start () {
		UpdateUI();
	}

	public void UpdateUI() {
		_images = new Image[transform.childCount];
		for(int i = 0; i < _images.Length; i++) {
			_images[i] = transform.GetChild(i).GetComponent<Image>();
		}
		
		if(_baseUnit != null) {
			SetFilledEssence(_baseUnit.attributes.currentEssence);
		}
		
	}

	public void PreviewUsage(int filled, int usage) {
		for(int i = 0; i < filled; i++) {
			if(i >= filled-usage) {
				_images[i].color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
			}
		}
	}

	public void SetFilledEssence(int count) {
		for(int i = 0; i < _images.Length; i++) {
			if(i < count) {
				_images[i].sprite = _essencePrefabFilled.GetComponent<Image>().sprite;	
			} else {
				_images[i].sprite = _essencePrefabEmpty.GetComponent<Image>().sprite;
			}
		}
		ResetAll();
	}

	public void AddEssence(int count, bool empty = true) {
		for(int i = 0; i < count; i++) {
			if(empty) {
				GameObject essence = GameObject.Instantiate(_essencePrefabEmpty);
				essence.transform.SetParent(transform);
			} else {
				GameObject essence = GameObject.Instantiate(_essencePrefabFilled);
				essence.transform.SetParent(transform);
			}

		}
		UpdateUI();
	}

	public void RemoveEssence(int count, bool empty) {
		for(int i = _images.Length-1; i >= _images.Length-count-1; i--) {
			Destroy(transform.GetChild(i).gameObject);
		}
		UpdateUI();
	}

	public void ResetAll() {
		foreach(Image image in _images) {
			image.color = new Color(1f, 1f, 1f, 1f);
		}
	}
}
