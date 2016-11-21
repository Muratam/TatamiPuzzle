using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public Point2D Pos{ get; private set; }

    Point2D targetPos;

    void Start() {
    }

    void Update() {
        //transform.
    }

    public void Move(Point2D pos) {
        var delta = pos - this.Pos;
        this.Pos = pos;
        transform.position = new Vector3(pos.x, 1, -pos.y);
        Debug.Log(-Mathf.Rad2Deg * Mathf.Atan2(delta.x, delta.y));
        transform.localRotation = Quaternion.Euler(0, -Mathf.Rad2Deg * Mathf.Atan2(delta.x, delta.y), 0);
    }

}
