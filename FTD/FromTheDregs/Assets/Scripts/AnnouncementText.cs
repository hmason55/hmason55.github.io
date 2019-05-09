using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AnnouncementText : MonoBehaviour {

    [SerializeField] Text _messageText;

    public void Initialize(string text, Color color, float duration = 3f) {
        _messageText.text = text;
        StartCoroutine(EScroll(color, duration));
    }

    IEnumerator EScroll(Color color, float duration = 3f) {
        _messageText.color = color;
        yield return new WaitForSeconds(duration);

        float fadeTime = 1.0f;
        for(float i = 0.0f; i < 1.0f; i += Time.deltaTime/fadeTime) {
            _messageText.color = new Color(color.r, color.g, color.b, 1.0f-i);
            yield return new WaitForSeconds(Time.deltaTime/fadeTime);
        }

        GameObject.Destroy(gameObject);
        yield break;
    }
}
