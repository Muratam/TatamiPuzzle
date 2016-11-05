using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

// http://free-texture.net/seamless-pattern/tatami01.html
// 火曜日くらいまでに文書化 & 遊べる感じで

/*
public class TatamiGameCore {
    //開始位置は 0 0 (仮)
    //畳は全て長方形/空白あり/重ならない/format : x y w h 色 (全て正の整数値)
    TatamiGridInfo[,] tatamiGridInfo;
    Point2D player = new Point2D(0, 0);
    int maxColor = 0;
    //void override operator[,](){
    // }
        
}*/


public class TatamiManager : MonoBehaviour {
    
    [SerializeField] TatamiObject tatamiOriginal;
    [SerializeField] GameObject goPlayer;
    TatamiObject[,] tatamiGridInfo;
    Point2D playerPos = new Point2D();
    Point2D deltaPos = new Point2D();
    int maxColor = 0;


    void Start() {
        PlaceTatami("t1");
    }

    void Update() {
        MovePlayer();
    }

    void MovePlayer() {
        if(deltaPos.SquareLength == 0) {
            deltaPos = new Point2D(); 
            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                deltaPos.x = 1;
            } else if(Input.GetKeyDown(KeyCode.UpArrow)) {
                deltaPos.y = 1;
            } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                deltaPos.x = -1;
            } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
                deltaPos.y = -1;
            } else {
                return;
            }
        }
        var nextPlayerPos = playerPos + deltaPos;
        bool stopped = false;
        if(nextPlayerPos.x < 0
           || nextPlayerPos.y < 0
           || nextPlayerPos.x > tatamiGridInfo.GetLength(0) - 1
           || nextPlayerPos.y > tatamiGridInfo.GetLength(1) - 1
           || tatamiGridInfo[nextPlayerPos.x, nextPlayerPos.y] == null) {           
            stopped = true;
        }
        if(stopped) {
            deltaPos = new Point2D(); 
        } else {
            var nextTatami = tatamiGridInfo[nextPlayerPos.x, nextPlayerPos.y];
            var currentTatami = tatamiGridInfo[playerPos.x, playerPos.y];
            if(currentTatami != nextTatami) {
                currentTatami.ChangeColor((currentTatami.Color + 1) % (maxColor + 1), deltaPos);
            }
            goPlayer.transform.position = new Vector3(nextPlayerPos.x, 1, nextPlayerPos.y);
            playerPos = nextPlayerPos;
        }
    }


    void PlaceTatami(string fileName) {
        var text = (Resources.Load(fileName) as TextAsset).text;
        var varslist = text.Trim().Split('\n')
            .Select(line => line.Split(' ').Select(_ => int.Parse(_)).ToList());
        int min_x = 0, min_y = 0;
        var max_x = varslist.Select(_ => _[0] + _[2]).Max();
        var max_y = varslist.Select(_ => _[1] + _[3]).Max();
        tatamiGridInfo = new TatamiObject[max_x - min_x, max_y - min_y];
        this.maxColor = varslist.Select(_ => _[4]).Max();
        foreach(var vars in varslist) {
            int x = vars[0], y = vars[1], w = vars[2], h = vars[3], c = vars[4];
            Debug.Assert(x >= 0 && y >= 0 && w > 0 && h > 0 && c >= 0);
            var tatamiClone = Instantiate(tatamiOriginal) as TatamiObject;
            tatamiClone.Place(this.transform, x, y, w, h, c);
            foreach(var xi in Enumerable.Range(x,w)) {
                foreach(var yi in Enumerable.Range(y,h)) {
                    Debug.Assert(tatamiGridInfo[xi, yi] == null);
                    tatamiGridInfo[xi, yi] = tatamiClone;
                }
            }
        }
    }



}