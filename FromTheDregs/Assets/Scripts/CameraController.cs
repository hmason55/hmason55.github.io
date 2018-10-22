using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

	public Vector2 _target;
	RectTransform _rectTransform;

	[SerializeField] DungeonGenerator _dungeonGenerator;

	public float deadzone = 1.0f;
	public bool limitView = false;
	public int renderDistance = 10;

	public float panSpeed = 1f;

	int _halfScreenWidth = 540;
	int _halfScreenHeight = 960;

	float _scale = 4f;

	public Vector2 target {
		get {return _target;}
		set {_target = value;}
	}

	void Awake() {
		_target = new Vector2(0, 0);
		_rectTransform = GetComponent<RectTransform>();
		_halfScreenWidth = Screen.width / 2;
		_halfScreenHeight = Screen.height / 2;
		_scale = _rectTransform.localScale.x;
	}

	float CalcDistance(float x1, float y1, float x2, float y2) {
		return (x2 - x1) + (y2 - y1);
	}

	public void MoveToTarget() {
		float targetTrueX = _halfScreenWidth - (_target.x * _scale * 48 + _scale * 24);
		float targetTrueY = _halfScreenHeight - (_target.y * _scale * 48 + _scale * 24);
		_rectTransform.anchoredPosition = new Vector2(targetTrueX, targetTrueY);
		
	}
}
