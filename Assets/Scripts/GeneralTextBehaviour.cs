using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralTextBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.LookAt(Camera.main.transform.position);
        this.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
    }
}
