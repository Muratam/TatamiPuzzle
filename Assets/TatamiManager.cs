using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;

// http://free-texture.net/seamless-pattern/tatami01.html

public class TatamiManager : MonoBehaviour {
    [SerializeField] GameObject goTatami;

    void Start() {
        PlaceTatami("t1");
    }

    void Update() {
	
    }

    Color enumColor(int i) {
        var del = 0.82f;
        var colors = new List<Color>() {
            new Color(1, del, del),
            new Color(del, 1, del),
            new Color(del, del, 1),
            new Color(1, 1, del),
            new Color(del, 1, 1),
            new Color(1, del, 1),
            new Color(1, 1, 1),
        };
        return colors[i % colors.Count];
    }

    void PlaceTatami(string fileName) {
        //畳は全て長方形/空白あり/重ならない/format : x y w h 色 (全て数値)
        var text = (Resources.Load(fileName) as TextAsset).text;
        foreach(var line in text.Trim().Split('\n')) {
            var vars = line.Split(' ').Select(_ => int.Parse(_)).ToList();
            int x = vars[0], y = vars[1], w = vars[2], h = vars[3], c = vars[4];
            var goTatamiClone = Instantiate(goTatami) as GameObject;
            goTatamiClone.transform.position = new Vector3(x + w / 2f - 0.5f, 0, y + h / 2f - 0.5f);
            goTatamiClone.transform.localScale = new Vector3(w, 1, h);
            goTatamiClone.GetComponent<Renderer>().material.color = enumColor(c);
        }
    }

}

/*
001100
002221
221101
*/
