using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class CameraController : MonoBehaviour {

	public Vector2 _target;

	RectTransform _rectTransform;

	[SerializeField] DungeonManager _dungeonManager;
	[SerializeField] RectTransform _terrainLayer;
	[SerializeField] RectTransform _decorationLayer;
	[SerializeField] RectTransform _unitLayer;

	TapController _tapController;

	[SerializeField] float _deadzone = 1.0f;

	[SerializeField] float _panSpeed = 25f;
	[SerializeField] float _zoom = 4f;
	float _aspectRatio = 1.778f;
	float _canvasWidth = 1080f;
	float _canvasHeight = 1920f;

	float _cachedWidth;
	float _cachedHeight;
	
	float _scale = 4f;
	
	Vector2 trueTarget;

	public Vector2 target {
		get {return _target;}
		set {_target = value;}
	}

	public RectTransform terrainLayer {
		get {return _terrainLayer;}
		set {_terrainLayer = value;}
	}

	public RectTransform decorationLayer {
		get {return _decorationLayer;}
		set {_decorationLayer = value;}
	}

	public RectTransform unitLayer {
		get {return _unitLayer;}
		set {_unitLayer = value;}
	}

	void Awake() {
		_target = new Vector2(0, 0);
		_rectTransform = GetComponent<RectTransform>();
		_tapController = GameObject.FindObjectOfType<TapController>();
		UpdateResolution();
	}

	void Update() {
		if(_cachedWidth != Screen.width || _cachedHeight != Screen.height) {
			SnapToTarget();
		}
	}

	void LateUpdate() {
		//MoveLayer(_rectTransform);
		MoveLayer(_terrainLayer);
		MoveLayer(_decorationLayer);
		MoveLayer(_unitLayer);
	}

	void MoveLayer(RectTransform layer) {
		Vector2 distanceVector = new Vector2(trueTarget.x - layer.anchoredPosition.x, trueTarget.y - layer.anchoredPosition.y);
		if(distanceVector.magnitude > _deadzone) {
			layer.anchoredPosition += distanceVector.normalized * (distanceVector.magnitude * _panSpeed) * Time.deltaTime;
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

		_aspectRatio = (float)Screen.width / (float)Screen.height;
		_canvasWidth = _aspectRatio * referenceHeight;
		_canvasHeight = referenceHeight;
		_scale = _zoom;

		_rectTransform.localScale = new Vector3(_scale, _scale, _scale);
		_terrainLayer.localScale = new Vector3(_scale, _scale, _scale);
		_decorationLayer.localScale = new Vector3(_scale, _scale, _scale);
		_unitLayer.localScale = new Vector3(_scale, _scale, _scale);
	}

	public void MoveToTarget() {
		UpdateResolution();

		float targetTrueX = (_canvasWidth/2)  - (_target.x * DungeonManager.TileWidth  * _scale + DungeonManager.TileWidth/2  * _scale);
		float targetTrueY = (_canvasHeight/2) - (_target.y * DungeonManager.TileHeight * _scale + DungeonManager.TileHeight/2 * _scale);
		trueTarget = new Vector2(targetTrueX, targetTrueY + 76f);
	}

	public void SnapToTarget() {
		MoveToTarget();

		_rectTransform.anchoredPosition = trueTarget;
		_terrainLayer.anchoredPosition = trueTarget;
		_decorationLayer.anchoredPosition = trueTarget;
		_unitLayer.anchoredPosition = trueTarget;
	}
}
