using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ElphiBehaviour : MonoBehaviour {

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		if(GazeManager.Instance.HitObject.Equals(this.gameObject)) {
            this.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 0.5f);
        }
	}
}
