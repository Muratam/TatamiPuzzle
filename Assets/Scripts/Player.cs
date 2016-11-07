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
        this.Pos = pos;
        transform.position = new Vector3(pos.x, 1, -pos.y);
    }

}
