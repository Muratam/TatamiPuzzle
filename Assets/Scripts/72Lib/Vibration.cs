using UnityEngine;
using System.Collections;

public class Vibration : MonoBehaviour {
    
    float vibrateTime = 0f;
    Vector3 DefaultPos;
    public float MaxVibrateTime = 0.1f;
    public Vector3 VibrateDelta = new Vector3(0, -50, 0);

    void Update() {
        if(vibrateTime <= 0f)
            return;
        if(vibrateTime - Time.deltaTime < 0) {
            vibrateTime = 0f;        
        } else {
            vibrateTime -= Time.deltaTime;
        }
        LocalPos = DefaultPos + vibrateTime * VibrateDelta;
    }

    Vector3 LocalPos {
        get {
            if(GetComponent<RectTransform>() == null)
                return transform.localPosition;
            else
                return GetComponent<RectTransform>().anchoredPosition3D;            
        }
        set { 
            if(GetComponent<RectTransform>() == null)
                transform.localPosition = value;
            else
                GetComponent<RectTransform>().anchoredPosition3D = value;            
            
        }
    }

    void Awake() {
        DefaultPos = LocalPos;
    }

    public void Viblate() {
        if(vibrateTime > 0f)
            LocalPos = DefaultPos;
        vibrateTime = MaxVibrateTime;
        DefaultPos = LocalPos;
    }


    public static void VibrateAll() {
        foreach(var vib in FindObjectsOfType<Vibration>()) {
            vib.Viblate();
        }
    
    }
}
