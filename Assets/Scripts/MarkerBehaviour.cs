using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MarkerBehaviour : MonoBehaviour {

    public String markerText;

    private GameObject markerHeadObject;
    private GameObject markerTextObject;
    private AudioSource audioSource;
    private bool audioPlays = false;

    // Use this for initialization
    void Start () {
        foreach (Transform child in this.transform) {
            if (child.tag.Equals("marker_head")) {
                this.markerHeadObject = child.gameObject;
            } else if (child.tag.Equals("marker_text")) {
                this.markerTextObject = child.gameObject;
            }
        }

        this.audioSource = this.markerHeadObject.GetComponent<AudioSource>();
        this.markerTextObject.GetComponent<TextMesh>().text = this.markerText;
        this.markerTextObject.SetActive(false);
        this.markerHeadObject.transform.LookAt(Camera.main.transform.position);

    }
	
	// Update is called once per frame
	void Update () {
        this.markerHeadObject.transform.LookAt(Camera.main.transform.position);

        if (GazeManager.Instance.HitObject.Equals(this.markerHeadObject)) {
            InputManager.Instance.PushModalInputHandler(GameObject.Find("ClickHandlerObject"));
            if(!this.audioPlays) {
                this.audioSource.Play();
            }
            //this.markerTextObject.transform.position = GazeManager.Instance.HitPosition + new Vector3(0.5f, 0.1f, 0.0f);
            this.markerTextObject.transform.LookAt(Camera.main.transform.position);
            this.markerTextObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
            this.markerTextObject.SetActive(true);
            this.audioPlays = true;
        } else {
            this.markerTextObject.SetActive(false);
            this.audioPlays = false;
        }
    }
}
