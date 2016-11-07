using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

public struct StageData {
    public readonly string Name;
    public readonly string Explanation;
    public readonly string Data;

    public StageData(string Name, string Explanation, string Data) {
        this.Name = Name;
        this.Explanation = Explanation;
        this.Data = Data;
    }
}

public static class PlayingData {
    static int stageIndex = 0;

    public static int StageIndex {
        get{ return stageIndex; }
        set{ stageIndex = Mathf.Clamp(value, 0, StageMax - 1); }
    }

    public static int StageMax {
        get {
            return StageDatas.Count;
        }
    }

    public readonly static List<StageData> StageDatas = new List<StageData>();

    static PlayingData() {
        var folder = "./TatamiStageData/";
        var extention = "txt";
        var files = Directory.GetFiles(folder)
            .Where(_ => new Regex(folder + @"\d+\." + extention).Match(_).Success).ToArray();
        Func<string,int> filename2Index = 
            (_) => int.Parse(_.Replace(folder, "").Replace("." + extention, ""));
        Array.Sort(files, (a, b) => filename2Index(a) - filename2Index(b));
        foreach(var f in files) {
            using(var sr = new StreamReader(f)) {
                var lines = sr.ReadToEnd().Split('\n');
                var data = lines.TakeWhile(_ => !_.StartsWith("#")).Aggregate((a, b) => a + "\n" + b);
                var name = lines.SkipWhile(_ => !_.StartsWith("#")).ElementAt(1);
                var explanation = lines.SkipWhile(_ => !_.StartsWith("#")).Skip(2).Aggregate((a, b) => a + "\n" + b);
                StageDatas.Add(new StageData(name, explanation, data));
            }
        }
    }
}