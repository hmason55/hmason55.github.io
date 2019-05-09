using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncementManager : MonoBehaviour {

    //[SerializeField] GameObject _prefab;
    static AnnouncementManager _instance;
    public static int messageCapacity = 3;

    public static AnnouncementManager instance {
        get {
            if(_instance == null) {
                GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/UI/AnnouncementManager")) as GameObject;
                go.name = "AnnouncementManager";

                Transform canvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
                go.transform.SetParent(canvas);
                go.transform.SetSiblingIndex(canvas.childCount-2);

                RectTransform rt = go.GetComponent<RectTransform>();
                rt.localPosition = new Vector3(0f, 832f, 0f);
                rt.sizeDelta = new Vector2(-1024f, 256f);
                rt.anchoredPosition = new Vector2(0f, -128f);
                rt.localScale = new Vector3(1f, 1f, 1f);
                _instance = go.GetComponent<AnnouncementManager>();
            }
            return _instance;
        }
    }

    public static void Display(string text, Color color, float duration = 3f) {
        instance.IDisplay(text, color, duration);
    }

    #region Internal Methods
    void IDisplay(string text, Color color, float duration = 3f) {
        Transform t = (GameObject.Instantiate(Resources.Load("Prefabs/UI/AnnouncementText")) as GameObject).transform;
        t.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        AnnouncementText announcementText = t.GetComponent<AnnouncementText>();

        if(transform.childCount >= messageCapacity) {
            Transform child = transform.GetChild(messageCapacity-1);
            if(child != null) {Destroy(child.gameObject);}
        }

        t.SetParent(transform);
        t.SetAsLastSibling();
        
        announcementText.Initialize(text, color, duration);
    }
    #endregion
}
