using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections.Specialized;
using System.Xml.Linq;

// http://free-texture.net/seamless-pattern/tatami01.html
// 火曜日くらいまでに文書化 & 遊べる感じで
// http://opentype.jp/aoyagireisho.htm
//http://sozonomono.com/illust/scroll/

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

// TODO: 1: ステージセレクト
//       2: 背景
//       3: プレイヤー
using UnityEngine.SceneManagement;

public class TatamiManager : MonoBehaviour {
    
    [SerializeField] TatamiObject tatamiOriginal;
    [SerializeField] Player player;
    [SerializeField] Camera mainCamera;
    [SerializeField] Text moveText;
    [SerializeField] SmoothUIAppearer clearText;
    [SerializeField] Fade fade;

    TatamiObject[,] tatamiGridInfo;
    List<TatamiObject> tatamiObjects;
    Point2D deltaPos = new Point2D();
    int maxColor = 0;
    int moveTime = 0;
    bool moving = false;
    bool cleared = false;

    public void Retry() {
        if(cleared)
            return;
        PlaceTatami(PlayingData.StageIndex);
    }

    void Awake() {
        clearText.WhenFinishedCallBack = ClearAndGotoStageSelect;
    }

    void Start() {
        PlaceTatami(PlayingData.StageIndex);

    }


    public void GotoStageSelect() {
        if(cleared)
            return;
        SceneManager.LoadScene("StageSelect");
    }

    void Update() {
        if(cleared)
            return;                    
        if(Input.GetKeyDown(KeyCode.X)) {
            Retry();
        } else if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Backspace)) {
            GotoStageSelect();
        } else {
            MovePlayer();
        }
    }

    void ClearAndGotoStageSelect() {
        PlayingData.UpdateClearTime(PlayingData.StageIndex, moveTime);
        fade.StartFade(true, () => {
            SceneManager.LoadScene("StageSelect");
        }, 2.5f);
    }

    bool CheckCleared() {
        return tatamiObjects.All(_ => _.Color == 0);
    }

    void MovePlayer() {
        if(deltaPos.SquareLength == 0) {
            deltaPos = new Point2D(); 
            if(Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                    var tatami = hit.collider.GetComponent<TatamiObject>();
                    if(tatami == null)
                        return;
                    if(tatami.x <= player.Pos.x && player.Pos.x <= tatami.x + tatami.w) {
                        deltaPos.y = player.Pos.y > tatami.y ? -1 : 1;
                    } else if(tatami.y <= player.Pos.y && player.Pos.y <= tatami.y + tatami.h) {
                        deltaPos.x = player.Pos.x > tatami.x ? -1 : 1;
                    }
                }
            } else {
                if(Input.GetKeyDown(KeyCode.RightArrow)) {
                    deltaPos.x = 1;
                } else if(Input.GetKeyDown(KeyCode.UpArrow)) {
                    deltaPos.y = -1;
                } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                    deltaPos.x = -1;
                } else if(Input.GetKeyDown(KeyCode.DownArrow)) {
                    deltaPos.y = 1;
                } else {
                    return;
                }
            }
        }
        var pos = player.Pos + deltaPos;
        if(pos.x < 0 || pos.y < 0
           || pos.x >= tatamiGridInfo.GetLength(0)
           || pos.y >= tatamiGridInfo.GetLength(1)
           || tatamiGridInfo[pos.x, pos.y] == null) {   
            //stops
            if(moving) {
                moveText.text = "" + TatamiUtil.Int2Japanese(++moveTime);
            }
            moving = false;
            deltaPos = new Point2D(); 
            if(!cleared && CheckCleared()) {
                cleared = true;
                clearText.gameObject.SetActive(true);
            }            
        } else {
            var currentTatami = tatamiGridInfo[player.Pos.x, player.Pos.y];
            var nextTatami = tatamiGridInfo[pos.x, pos.y];
            if(currentTatami != nextTatami) {
                currentTatami.ChangeColor((currentTatami.Color + 1) % (maxColor + 1), deltaPos);
            }
            player.Move(pos);
            moving = true;
        }
    }

    void Init() {
        deltaPos = new Point2D(0, 0);
        moveTime = 0;
        player.Move(new Point2D(0, 0));
        moveText.text = "" + TatamiUtil.Int2Japanese(moveTime);
        foreach(var child in GetComponentsInChildren<TatamiObject>()) {
            Destroy(child.gameObject);
        }
    }

    void PlaceTatami(int index) {
        var text = PlayingData.StageDatas[PlayingData.StageIndex].Data;
        var varslist = TatamiUtil.ReadCharaType(text);
        if(varslist == null)
            return;
        int min_x = 0, min_y = 0;
        var max_x = varslist.Select(_ => _[0] + _[2]).Max();
        var max_y = varslist.Select(_ => _[1] + _[3]).Max();
        this.maxColor = varslist.Select(_ => _[4]).Max();
        Init();
        tatamiGridInfo = new TatamiObject[max_x - min_x, max_y - min_y];
        tatamiObjects = new List<TatamiObject>();
        foreach(var vars in varslist) {
            int x = vars[0], y = vars[1], w = vars[2], h = vars[3], c = vars[4];
            var tatamiClone = Instantiate(tatamiOriginal) as TatamiObject;
            tatamiObjects.Add(tatamiClone);
            tatamiClone.Place(this.transform, x, y, w, h, c);
            foreach(var xi in Enumerable.Range(x,w)) {
                foreach(var yi in Enumerable.Range(y,h)) {
                    Debug.Assert(tatamiGridInfo[xi, yi] == null);
                    tatamiGridInfo[xi, yi] = tatamiClone;
                }
            }
        }
        mainCamera.transform.position = new Vector3(max_x / 2, 10, -max_y * 2);
    }



}