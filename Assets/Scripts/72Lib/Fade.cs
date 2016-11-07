using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Fade : MonoBehaviour {

    float fadingTime;
    bool isFadeIn;
    Action action;
    Color color;

    Image m_image;
    bool fading = false;

    public bool Fading { get { return fading; } }

    float fadingCurrentTime;

    void Awake() {
        if(GetComponent<Image>() == null) {
            gameObject.AddComponent<Image>();	
            GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
        m_image = GetComponent<Image>();
        m_image.enabled = false;
    }

    public void StartFade(
        bool isFadeIn = false,
        Action action = null,
        float fadingTime = 1f,
        Color? color = null) {
        m_image.enabled = true;
        this.fadingTime = fadingTime;
        this.isFadeIn = isFadeIn;
        this.color = color == null ? new Color(0, 0, 0, isFadeIn ? 0 : 1) : color.Value;
        this.action = action;
        fading = true;	
        fadingCurrentTime = fadingTime;
    }

    void Update() {
        if(!fading)
            return;
        var per = isFadeIn ? 1 - (fadingCurrentTime / fadingTime) : fadingCurrentTime / fadingTime;
        m_image.color = new Color(color.r, color.g, color.b, per);
        if(fadingCurrentTime > 0) {
            fadingCurrentTime -= Time.deltaTime;
        } else {
            fading = false;	
            if(!isFadeIn)
                m_image.enabled = false;
            if(action != null)
                action();
        }
    }
}
