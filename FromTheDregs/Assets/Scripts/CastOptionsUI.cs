using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CastOptionsUI : MonoBehaviour {

	[SerializeField] Image _image;
	[SerializeField] Hotbar _hotbar;
	[SerializeField] Button _cancelCastButton;

	bool _hidden = true;
	public Button cancelCastButton {
		get {return _cancelCastButton;}
	}

	public bool hidden {
		get {return _hidden;}
	}

	public void ShowUI() {
		_hidden = false;
		_hotbar.HideHotkeys();
		_image.enabled = true;
		_cancelCastButton.gameObject.SetActive(true);
	}

	public void HideUI() {
		_hidden = true;
		_image.enabled = false;
		_cancelCastButton.gameObject.SetActive(false);
		_hotbar.ShowHotkeys();
	}

	public void CancelCast() {
		_hotbar.CancelPreview();
	}
}
