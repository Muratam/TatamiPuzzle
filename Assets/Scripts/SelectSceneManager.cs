using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class SelectSceneManager : MonoBehaviour {
    [SerializeField] RectTransform rtStageSelectOriginal;
    [SerializeField] ReadingText StageNameText;
    [SerializeField] Text ExplanationText;
    int delegateDelta = 0;
    int preMouseReacted = 0;

    void Start() {
        MakeUI();  
        UpdateExplanations();
    }

    void MakeUI() {
        // 実はTextは最前面しかいらなかった
        foreach(var i in Enumerable.Range(1,8)) {
            var cp = Instantiate(rtStageSelectOriginal) as RectTransform;
            cp.SetParent(rtStageSelectOriginal.parent);
            cp.localScale = rtStageSelectOriginal.localScale;
            cp.anchoredPosition = rtStageSelectOriginal.anchoredPosition;
            cp.anchoredPosition += 
                i < 5 ? new Vector2(3, 18) * i :
                new Vector2(3, -18) * (i - 4);
            cp.name = "Select" + i;
            cp.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            cp.transform.SetAsFirstSibling();
            Destroy(cp.transform.GetChild(0).gameObject);
            Destroy(cp.GetComponent<Button>());
        }
    }

    void UpdateExplanations() {
        var index = PlayingData.StageIndex;
        var stageData = PlayingData.StageDatas[index];
        StageNameText.text = TatamiUtil.Int2Japanese(index) + " " + stageData.Name;   
        ExplanationText.text = stageData.Explanation;
    }

    public void SetDelegateDelta(int delta) {
        delegateDelta = delta;
    }

    void Update() {
        int delta = delegateDelta;
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            delta = -1;
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            delta = 1;
        else if(preMouseReacted == 0) {
            delta = Mathf.RoundToInt(Input.mouseScrollDelta.y);
            if(delta != 0)
                delta = (int)Mathf.Sign(delta);
            preMouseReacted = 3;
        } else {
            if(preMouseReacted > 0)
                preMouseReacted--;
        }
        if(delta != 0) {
            PlayingData.StageIndex += delta;
            UpdateExplanations();
            Vibration.VibrateAll();
        }
        delegateDelta = 0;
    }
}
