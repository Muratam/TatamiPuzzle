using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public static class TatamiUtil {
    static string[] kansuuji = new string[] {
        "零", "一", "二", "三", "四", "五", "六", "七", "八", "九"
    };

    public static string Int2Japanese(int i, bool reactZero = true) {
        if(i < 0)
            return "";
        if(i == 0)
            return reactZero ? kansuuji[0] : "";
        if(i < 10)
            return kansuuji[i];
        if(i < 100)
            return (i < 20 ? "" : Int2Japanese(i / 10, false)) + "十" + Int2Japanese(i % 10, false);
        if(i < 1000)
            return (i < 200 ? "" : Int2Japanese(i / 100, false)) + "百" + Int2Japanese(i % 100, false);
        if(i < 10000)
            return (i < 2000 ? "" : Int2Japanese(i / 1000, false)) + "千" + Int2Japanese(i % 1000, false);
        return "すごく多い";
    }

    [ObsoleteAttribute("Use ReadCharaType")]
    public static List<List<int>> ReadRectType(string text) {
        var varslist = text.Trim().Replace("\r\n", "\n").Split('\n')
            .Select(line => line.Split(' ').Select(_ => int.Parse(_)).ToList()).ToList();
        foreach(var vars in varslist) {
            int x = vars[0], y = vars[1], w = vars[2], h = vars[3], c = vars[4];
            if(!(x >= 0 && y >= 0 && w > 0 && h > 0 && c >= 0)) {
                Debug.Log("INVALID STAGE");
                return null;
            }
        }    
        return varslist;
    }

    public static List<List<int>> ReadCharaType(string text) {
        //「0 1 x 半角空白」以外の文字は区切り文字として扱うので好きにしてよいです
        //各長方形の左上のみしか見ない
        var lines = text.Trim().Replace("\r\n", "\n").Split('\n');
        int max_x = lines.Select(_ => (_.Length + 1) / 2).Max();
        int max_y = (lines.Length + 1) / 2;
        var varslist = new List<List<int>>();
        var grid = new char[max_x * 2, max_y * 2];
        foreach(var x in Enumerable.Range(0,grid.GetLength(0))) {
            foreach(var y in Enumerable.Range(0,grid.GetLength(1))) {
                grid[x, y] = (y < lines.Count() && x < lines[y].Count()) ? lines[y][x] : ' ';
            }
        }

        var searched = new bool[max_x, max_y];
        foreach(var x in Enumerable.Range(0,max_x)) {
            foreach(var y in Enumerable.Range(0,max_y)) {
                if(searched[x, y])
                    continue;
                searched[x, y] = true;
                int c = grid[2 * x, 2 * y];
                int h = 1, w = 1;
                while(x + w < max_x && grid[2 * (x + w) - 1, 2 * y] == ' ')
                    w++;
                while(y + h < max_y && grid[2 * x, 2 * (y + h) - 1] == ' ')
                    h++;
                foreach(var dx in Enumerable.Range(0,w)) {
                    foreach(var dy in Enumerable.Range(0,h)) {
                        searched[x + dx, y + dy] = true;
                    }
                }
                if(c != 'x')
                    c -= '0';
                varslist.Add(new List<int>(){ x, y, w, h, c });
            }
        }
        return varslist;

    }

}
