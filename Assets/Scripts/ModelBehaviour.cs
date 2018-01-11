using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModelBehaviour : MonoBehaviour, IInputClickHandler {

    public float rotationSpeed;

    private String poi_name;
    private bool isDetailState = false;
    private bool textFadeInDone = false;
    private bool textFadeOutDone = false;
    private bool shouldRotate = false;

    public void OnInputClicked(InputClickedEventData eventData) {
        if(this.isDetailState) {
            this.isDetailState = false;
            this.gameObject.GetComponent<Animator>().SetTrigger("model_end");
            GameObject.Find("ClickHandlerObject").GetComponent<POIBehaviuor>().removeModelFromActiveList(this.poi_name);
            // Detail state has been exited - vuforia and models need to be activated again
            Invoke("enableVuforiaAndModels", 3.0f);
            // Start the marker fade in animation 3 seconds later - model end animation takes 3 seconds to terminate
            Invoke("markerFadeIn", 3.5f);
            
        } else {
            GameObject.Find("QR_Vuforia").GetComponent<DistanceTrackingHandler>().changeVuforiaElementsState(false, this.poi_name);
            this.gameObject.GetComponent<Animator>().SetTrigger("model_detail");
            this.isDetailState = true;
            this.shouldRotate = true;
        }
    }

    private void markerFadeIn() {
        GameObject.Find("ClickHandlerObject").GetComponent<POIBehaviuor>().activateMarker(this.poi_name);
    }

    private void enableVuforiaAndModels() {
        this.shouldRotate = false;
        GameObject.Find("QR_Vuforia").GetComponent<DistanceTrackingHandler>().changeVuforiaElementsState(true, this.poi_name);
    }

    private void destroyThisObject() {
        Destroy(this.gameObject);
    }

    public void setPOIName(String poi_name) {
        this.poi_name = poi_name;
    }

    public String getPOIName() {
        return this.poi_name;
    }

    // Use this for initialization
    void Start () {

    }

	// Update is called once per frame
	void Update () {
		if (GazeManager.Instance.HitObject.Equals(this.gameObject)) {
            InputManager.Instance.PushModalInputHandler(this.gameObject);
        }
        this.gameObject.transform.Find("Informations").LookAt(Camera.main.transform.position);
        this.gameObject.transform.Find("Informations").Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
        // If model is in detail animator state - should rotate
        if (this.shouldRotate) {
            this.gameObject.transform.Rotate(0, this.rotationSpeed, 0);
        }
	}
}
