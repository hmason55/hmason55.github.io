using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class CameraController : MonoBehaviour {

	public Vector2 _target;

	[SerializeField] float _deadzone = 1.0f;

	[SerializeField] float _panSpeed = 25f;
	float _aspectRatio = 1.778f;
	float _canvasWidth = 1080f;
	float _canvasHeight = 1920f;

	float _cachedWidth;
	float _cachedHeight;
	
	Vector3 _trueTarget;

	public Vector2 target {
		get {return _target;}
		set {_target = value;}
	}

	void Awake() {
		_target = new Vector2(0, 0);
		UpdateResolution();
	}

	void Update() {
		if(_cachedWidth != Screen.width || _cachedHeight != Screen.height) {
			SnapToTarget();
		}
	}

	void LateUpdate() {
		transform.position = Vector3.Lerp(transform.position, new Vector3(_trueTarget.x, _trueTarget.y, -10f), _panSpeed * Time.deltaTime);
	}

	void MoveLayer(RectTransform layer) {
		Vector2 distanceVector = new Vector2(_trueTarget.x - layer.anchoredPosition.x, _trueTarget.y - layer.anchoredPosition.y);
		if(distanceVector.magnitude > _deadzone) {
			layer.anchoredPosition = Vector2.LerpUnclamped(layer.anchoredPosition, _trueTarget, _panSpeed * Time.deltaTime);
		} else if(distanceVector.magnitude != 0f){
			SnapToTarget();
		}
	}

	float CalcDistance(float x1, float y1, float x2, float y2) {
		return (x2 - x1) + (y2 - y1);
	}

	public void UpdateResolution() {
		_cachedWidth = Screen.width;
		_cachedHeight = Screen.height;
		float referenceWidth = 1080f;
		float referenceHeight = 1920f;

	}

	public void MoveToTarget() {
		UpdateResolution();

		float targetTrueX = (_target.x * DungeonManager.TileWidth + DungeonManager.TileWidth/2);
		float targetTrueY = (_target.y * DungeonManager.TileHeight + DungeonManager.TileWidth/2 - 24f);
		_trueTarget = new Vector2(targetTrueX, targetTrueY);
	}

	public void SnapToTarget() {
		MoveToTarget();
		transform.position = new Vector3(_trueTarget.x, _trueTarget.y, -10f);
	}
}
