using UnityEngine.UI;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Text))]
public class ReadingText : MonoBehaviour {
    Text m_text = null;
    int count = 0;
    string completeText = "";
    bool completed = false;
    public int step = 1;
    public bool blinkWhenCompletedWithTriangle = true;

    public string text {
        get { return completeText; }
        set { 
            completeText = value;
            count = 0;
            completed = false;
        }
    }

    void Awake() {
        m_text = this.GetComponent<Text>();
        text = m_text.text;
    }

    void Update() {
        count++;
        if(completed) {
            if(!blinkWhenCompletedWithTriangle)
                return;
            m_text.text = completeText + (count / 20 % 2 == 0 ? "▼" : "");
            return;
        }
        var read = count / step;
        m_text.text = completeText.Substring(0, read);	
        if(read == completeText.Length)
            completed = true;
    }
}
