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

    public static int GetClearTime(int index) {
        return PlayerPrefs.GetInt("tatami" + index, 0);
    }

    public static void UpdateClearTime(int index, int time) {
        var preTime = GetClearTime(index);
        if(preTime != 0) {
            time = Mathf.Min(time, preTime);
        }
        PlayerPrefs.SetInt("tatami" + index, time);
        PlayerPrefs.Save();
    }

    public static void DeleteClearTimeAll() {
        PlayerPrefs.DeleteAll();
    }

    public readonly static List<StageData> StageDatas = new List<StageData>();

    public static void ReadAll() {
        Action<string> ConstructDatas = (text) => {
            var lines = text.Replace("\r\n", "\n").Split('\n');
            var data = lines.TakeWhile(_ => !_.StartsWith("#")).Aggregate((a, b) => a + "\n" + b);
            var name = lines.SkipWhile(_ => !_.StartsWith("#")).ElementAt(1);
            var explanation = lines.SkipWhile(_ => !_.StartsWith("#")).Skip(2).Aggregate((a, b) => a + "\n" + b);
            StageDatas.Add(new StageData(name, explanation, data));
        };
        var folder = "TatamiStageData/";
        var extention = "txt";
        Func<string,int> filename2Index = 
            (_) => int.Parse(_.Replace(folder, "").Replace("." + extention, ""));
        StageDatas.Clear();
        if(Directory.Exists(folder)) { // try to read from folder 
            folder = "./" + folder;
            var files = Directory.GetFiles(folder).Where(_ => new Regex(folder + @"\d+\." + extention + "$").Match(_).Success).ToArray();
            Array.Sort(files, (a, b) => filename2Index(a) - filename2Index(b));
            foreach(var f in files) {
                using(var sr = new StreamReader(f)) {
                    ConstructDatas(sr.ReadToEnd());
                }
            }
        } else { // try to read from Resources (For Android etc..)  
            Debug.Log("From Resources");
            var textObjects = Resources.LoadAll(folder);
            Array.Sort(textObjects, (a, b) => filename2Index(a.name) - filename2Index(b.name));
            foreach(var to in textObjects) {
                var ta = to as TextAsset;
                ConstructDatas(ta.text);
            }
        }           
    }

    static PlayingData() {        
        ReadAll();
    }
}