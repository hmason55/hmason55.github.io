using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DamageText : MonoBehaviour {

	Vector2 _velocity;
	Vector2 _gravity;

	float _lifetime = 1.25f;
	float _spawnScaling = 1.5f;
	
	float _scalingDuration = 0.15f;

	RectTransform _rectTransform;
	Vector3 _originalScale;

	float _spawntime;

	public RectTransform rectTransform {
		get {return _rectTransform;}
	}

	void Awake() {
		transform.SetParent(GameObject.FindObjectOfType<CameraController>().transform);
		_rectTransform = GetComponent<RectTransform>();
		_velocity = new Vector2(0f, 20f);
		_spawntime = Time.realtimeSinceStartup;
		Destroy(gameObject, _lifetime);
	}

	public void Init(Vector2 position, string text, Color color) {
		_rectTransform.anchoredPosition = new Vector2(position.x + DungeonManager.TileWidth/2, position.y + DungeonManager.TileWidth/2);
		_originalScale = _rectTransform.localScale;
		Text t = GetComponent<Text>();
		t.text = text;
		t.color = color;
	}

	void Update () {
		if(Time.realtimeSinceStartup < _spawntime+(_scalingDuration*_lifetime)) {
			_rectTransform.localScale = _originalScale*_spawnScaling;
		} else {
			_rectTransform.localScale = _originalScale;
		}
		_rectTransform.anchoredPosition += _velocity*Time.deltaTime;
	}
}
