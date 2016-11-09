using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class MonochromeCamera : MonoBehaviour {

    [SerializeField] Shader monochromeShader;


    void Update() {
        if(Input.GetKeyDown(KeyCode.D)) {
            DoMonochrome();
        } else if(Input.GetKeyDown(KeyCode.U)) {
            UndoMonochrome();
        }
    }


    public void DoMonochrome() {
        GetComponent<Camera>().SetReplacementShader(monochromeShader, null);
        Debug.Log("D");
    }

    public void UndoMonochrome() {
        GetComponent<Camera>().SetReplacementShader(null, null);
        Debug.Log("U");
    }


}
