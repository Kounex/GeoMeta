using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

public class DistanceTextBehaviour : MonoBehaviour {

    public bool showDistance;

	// Use this for initialization
	void Start () {
        if(!this.showDistance) {
            this.GetComponent<Text>().color = new Color(0f, 0f, 0f, 0f);
        }
	}
	
	// Update is called once per frame
	void Update () {
        // this.transform.position = Camera.main.transform.position + new Vector3(-0.1f, 0.1f, 1.0f);
        float distance = (GazeManager.Instance.HitPosition - Camera.main.transform.position).magnitude;
        float distanceRounded = (int)(distance * 100f);
        this.GetComponent<Text>().text = (distanceRounded / 100f).ToString() + " m";
    }
}
