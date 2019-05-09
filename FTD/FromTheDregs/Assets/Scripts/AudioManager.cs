using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	AudioSource _audioSource;
	AudioClip _audioClip;

	public AudioSource audioSource {
		get {return _audioSource;}
	}

	void Awake() {
		_audioSource = GetComponent<AudioSource>();
	}

	public void LoadSound(string soundPath) {
		_audioClip = Resources.Load<AudioClip>(soundPath);
		if(_audioClip == null) {
			Debug.LogWarning("Could not find audio clip at " + soundPath);
		}
	}

	public void PlaySound(float volume = 0.5f, float delay = 0f) {
		if(_audioClip != null) {
			StartCoroutine(IPlaySound(volume, delay));
		}
	}

	IEnumerator IPlaySound(float volume = 0.5f, float delay = 0f) {
		yield return new WaitForSeconds(delay);
		_audioSource.PlayOneShot(_audioClip, volume);
	}

	
}
