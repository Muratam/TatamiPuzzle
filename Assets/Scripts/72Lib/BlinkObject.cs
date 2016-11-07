using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlinkObject : MonoBehaviour {

    public bool isBlinking = true;
    public float blinkTime = 1f;
    public float koku = 2f;

    void Update() {
        bool isOK = isBlinking ? 
            Time.time % blinkTime < blinkTime / Mathf.Clamp(koku, 1f, 100f) :
            true;
        foreach(var c in GetComponentsInChildren<Transform>()) {
            Blink(isOK, c.gameObject);
        }
        Blink(isOK, this.gameObject);
    }

    static void Blink(bool isOK, GameObject go) {
        if(go.GetComponent<Renderer>() != null)
            go.GetComponent<Renderer>().enabled = isOK;
        else if(go.GetComponent<Graphic>() != null) {
            var c = go.GetComponent<Graphic>().color;
            go.GetComponent<Graphic>().color = new Color(c.r, c.g, c.b, isOK ? 0f : 1f);        
        }
    }
}
