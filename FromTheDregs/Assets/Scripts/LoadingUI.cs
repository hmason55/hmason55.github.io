using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LoadingUI : MonoBehaviour {

    [SerializeField] Image _backgroundImage;
    [SerializeField] Text _loadingText;
    [SerializeField] Text _helpText;
    string[] _tips = {
        "This is a simple tip.",
        "Please read me."
    };

    Coroutine fadeInCoroutine;
    Coroutine fadeOutCoroutine;

    void RandomizeHelpText() {
        _helpText.text = _tips[Random.Range(0, _tips.Length-1)];
    }

    public void FadeIn(float duration = 1f) {
        if(fadeInCoroutine == null) {
            fadeInCoroutine = StartCoroutine(EFadeIn(duration));
        }
    }

    public void FadeOut(float duration = 1f) {
        if(fadeOutCoroutine == null) {
            fadeOutCoroutine = StartCoroutine(EFadeOut(duration));
        }
    }

    public void ShowUI() {
        _backgroundImage.enabled = true;
        _loadingText.enabled = true;
        _helpText.enabled = true;

        RandomizeHelpText();
        _backgroundImage.color = new Color(0f, 0f, 0f, 1f);
        _loadingText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        _helpText.color = new Color(0.8f, 0.8f, 0.8f, 1f);

    }

    public void HideUI() {
        _backgroundImage.color = new Color(0f, 0f, 0f, 0f);
        _loadingText.color = new Color(0.5f, 0.5f, 0.5f, 0f);
        _helpText.color = new Color(0.8f, 0.8f, 0.8f, 0f);

        _backgroundImage.enabled = false;
        _loadingText.enabled = false;
        _helpText.enabled = false;
    }

    IEnumerator EFadeIn(float fadeTime) {
        _backgroundImage.enabled = true;
        //_loadingText.enabled = true;
        //_helpText.enabled = true;
        //RandomizeHelpText();

        for(float i = 0.0f; i < 1.0f; i += Time.deltaTime/fadeTime) {
            _backgroundImage.color = new Color(0f, 0f, 0f, i);
            //_loadingText.color = new Color(0.5f, 0.5f, 0.5f, i);
            //_helpText.color = new Color(0.8f, 0.8f, 0.8f, i);
            yield return new WaitForSeconds(Time.deltaTime/fadeTime);
        }

        fadeInCoroutine = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield break;
    }

    IEnumerator EFadeOut(float fadeTime) {
        _backgroundImage.enabled = true;
        _loadingText.enabled = true;
        _helpText.enabled = true;

        float alpha = 1.0f;
        for(float i = 0.0f; i < 1.0f; i += Time.deltaTime/fadeTime) {
            _backgroundImage.color = new Color(0f, 0f, 0f, alpha-i);
            _loadingText.color = new Color(0.5f, 0.5f, 0.5f, alpha-i);
            _helpText.color = new Color(0.8f, 0.8f, 0.8f, alpha-i);
            yield return new WaitForSeconds(Time.deltaTime/fadeTime);
        }

        _backgroundImage.enabled = false;
        _loadingText.enabled = false;
        _helpText.enabled = false;

        fadeOutCoroutine = null;
        yield break;
    }

}
