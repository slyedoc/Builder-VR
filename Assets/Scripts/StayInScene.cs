using UnityEngine;
using System.Collections;

public class StayInScene : MonoBehaviour {

    // Use this for initialization
    public bool stayInScene = true;
	void Start () {
        if( stayInScene) {
#if UNITY_EDITOR
         
        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
#endif
        }

    }

}
