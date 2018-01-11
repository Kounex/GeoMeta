using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTextBehaviour : MonoBehaviour {

    public bool debugMode;

    public void debugMessage(string text) {
        this.GetComponent<Text>().text = this.GetComponent<Text>().text + "\n" + text;
    }

	// Use this for initialization
	void Start () {
        if(!this.debugMode) {          
            this.GetComponent<Text>().color = new Color(0f, 0f, 0f, 0f);
        }
        this.GetComponent<Text>().text = "no debug message";
    }
	
	// Update is called once per frame
	void Update () {
     
	}
}
