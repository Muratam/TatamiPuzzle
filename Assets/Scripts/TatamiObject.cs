using UnityEngine;
using System.Collections;
using System;

public class TatamiObject : MonoBehaviour {
    
    int color = 0;

    public int x{ get; private set; }

    public int y{ get; private set; }

    public int w{ get; private set; }

    public int h{ get; private set; }


    public int Color {
        get{ return color; }
        set{ ChangeColor(value); }
    }

    float rotationTime = 0f;
    const float maxRotationTime = 0.3f;
    Point2D rotationDirection = new Point2D(0, 0);

    void Update() {
        Animate();
    }

    void Animate() {
        if(rotationTime <= 0f)
            return;
        if(rotationTime - Time.deltaTime < 0) {
            rotationTime = 0f;        
        } else {
            rotationTime -= Time.deltaTime;
        }
        var rotVal = 180f * (1f - rotationTime / maxRotationTime);
        if(rotationDirection.x != 0) {
            transform.localRotation = Quaternion.Euler(0, 0, -rotVal * Mathf.Sign(rotationDirection.x));
        } else {
            transform.localRotation = Quaternion.Euler(rotVal * Mathf.Sign(rotationDirection.y), 0, 0);
        }
    }

    public void ChangeColor(int color) {
        ChangeColor(color, new Point2D(0, 0));
    }

    public void ChangeColor(int color, Point2D rotationDirection) {
        if(this.color != color) {
            this.color = color;
            GetComponent<Renderer>().material.color = enumColor(color);        
            rotationTime = maxRotationTime;
            this.rotationDirection = rotationDirection;
        }
    }

    public static void PlaceObject(GameObject go, Transform parent, int x, int y, int w, int h) {
        go.transform.SetParent(parent);
        go.transform.position = new Vector3(x + w / 2f - 0.5f, 0, -1f * (y + h / 2f - 0.5f));    
    }

    public void Place(Transform parent, int x, int y, int w, int h, int c) {
        PlaceObject(this.gameObject, parent, x, y, w, h);
        transform.localScale = new Vector3(w, 0.3f, h) * 0.98f;
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
        ChangeColor(c);
    }

    Color enumColor(int i) {
        /*
        var del = 0.82f;
        var colors = new List<Color>() {
            new Color(1, del, del),
            new Color(del, 1, del),
            new Color(del, del, 1),
            new Color(1, 1, del),
            new Color(del, 1, 1),
            new Color(1, del, 1),
            new Color(1, 1, 1),
        };*/
        //return colors[i % colors.Count];
        float val = 1f - (float)i / 2f;
        return new Color(val, val, val);
    }

}
