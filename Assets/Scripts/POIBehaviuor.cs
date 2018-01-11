using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class POIBehaviuor : MonoBehaviour, IInputClickHandler {

    public GameObject elphiPrefab;
    public GameObject planetariumPrefab;
    public GameObject dammtorPrefab;
    public GameObject bunkerPrefab;

    private List<GameObject> deactivatedMarkers = new List<GameObject>();
    private List<GameObject> activatedModels = new List<GameObject>();
    private List<GameObject> allModels = new List<GameObject>();
    private GameObject currentCreatedModel;

    public void OnInputClicked(InputClickedEventData eventData) {
        if (GazeManager.Instance.HitObject != null) {
            // HitObject would be the marker_head in this situation, therefore we need to get the parent GameObject
            // to trigger the animation
            GameObject gazedObject = GazeManager.Instance.HitObject.transform.parent.gameObject;
            // As soon as the user triggers a marker, the model will be shown and the marker will fade out - adding
            // it to an list to be able to activate it from another script (ModelBehaviour) as the model is triggered
            this.deactivatedMarkers.Add(gazedObject);

            String markerName = gazedObject.GetComponent<MarkerBehaviour>().markerText;

            GameObject newPOI = new GameObject();

            bool modelAlreadyCreated = false;
            if (this.allModels.Count > 0) {
                foreach (GameObject model in this.allModels) {
                    if (model.GetComponent<ModelBehaviour>().getPOIName() == markerName) {
                        newPOI = model;

                        if (markerName.Equals("Elbphilharmonie")) {
                            newPOI.transform.rotation = this.elphiPrefab.transform.rotation;
                        }
                        if (markerName.Equals("Planetarium")) {
                            newPOI.transform.rotation = this.planetariumPrefab.transform.rotation;
                        }
                        if (markerName.Equals("Bahnhof Dammtor")) {
                            newPOI.transform.rotation = this.dammtorPrefab.transform.rotation;
                        }
                        if (markerName.Equals("Hamburger Flaktürme")) {
                            newPOI.transform.rotation = this.bunkerPrefab.transform.rotation;
                        }

                        modelAlreadyCreated = true;
                        break;
                    }
                }
            }
            
            if(!modelAlreadyCreated) {
                Vector3 poiPos = gazedObject.transform.position;
                if (markerName.Equals("Elbphilharmonie")) {
                    newPOI = Instantiate(this.elphiPrefab, poiPos, this.elphiPrefab.transform.rotation);
                }
                if (markerName.Equals("Planetarium")) {
                    newPOI = Instantiate(this.planetariumPrefab, poiPos, this.planetariumPrefab.transform.rotation);
                }
                if (markerName.Equals("Bahnhof Dammtor")) {
                    newPOI = Instantiate(this.dammtorPrefab, poiPos, this.dammtorPrefab.transform.rotation);
                }
                if (markerName.Equals("Hamburger Flaktürme")) {
                    newPOI = Instantiate(this.bunkerPrefab, poiPos, this.bunkerPrefab.transform.rotation);
                }
                this.allModels.Add(newPOI);
            }

            newPOI.GetComponent<ModelBehaviour>().setPOIName(gazedObject.GetComponent<MarkerBehaviour>().markerText);

            // Model is active - adding to a list too. If the user gets too far away (Hamburg section text will be displayed)
            // the models need to be deactivated (DistanceTrackingHandler)
            this.activatedModels.Add(newPOI);

            // Set member reference to be accessable in another method
            this.currentCreatedModel = newPOI;
            // Marker has been clicked - gets faded out
            gazedObject.GetComponent<Animator>().SetTrigger("fade_out");
            // Start model_start animation 2 seconds later - fade out animation takes 2 seconds to terminate
            Invoke("modelAnimationStart", 2.0f);
        }
    }

    private void modelAnimationStart() {
        this.currentCreatedModel.GetComponent<Animator>().SetTrigger("model_start");
    }

    // Model has been deactivated -> markwer for model has to be displayed again
    public void activateMarker(String poi_name) {
        foreach (GameObject marker in this.deactivatedMarkers) {
            // Find the marker which belongs to the triggered model via name
            if (marker.GetComponent<MarkerBehaviour>().markerText.Equals(poi_name)) {
                marker.GetComponent<Animator>().SetTrigger("fade_in");
            }
        }
    }

    // Model has been deactivated -> has to be removed from active list
    public void removeModelFromActiveList(String modelName) {
        foreach(GameObject model in this.activatedModels) {
            if(model.GetComponent<ModelBehaviour>().getPOIName() == modelName) {
                this.activatedModels.Remove(model);
                GameObject.Find("Debug_Text").GetComponent<DebugTextBehaviour>().debugMessage(modelName + " removed from list");
                GameObject.Find("Debug_Text").GetComponent<DebugTextBehaviour>().debugMessage("list count: " + this.activatedModels.Count);
                break;
            }
        }
    }

    public void changeModelState(bool state) {
        foreach(GameObject model in this.activatedModels) {
            model.SetActive(state);
            if(state) {
                model.GetComponent<Animator>().SetTrigger("model_start");
            }
            GameObject.Find("Debug_Text").GetComponent<DebugTextBehaviour>().debugMessage(model.GetComponent<ModelBehaviour>().getPOIName() + " was set to " + state);
        }
    }

    public void changeModelState(bool state, String modelKeepUntouched) {
        foreach (GameObject model in this.activatedModels) {
            if(model.GetComponent<ModelBehaviour>().getPOIName() != modelKeepUntouched) {
                model.SetActive(state);
                if(state) {
                    model.GetComponent<Animator>().SetTrigger("model_start");
                }
            }
        }
        GameObject.Find("Debug_Text").GetComponent<DebugTextBehaviour>().debugMessage("All models except " + modelKeepUntouched + " were set to " + state);
    }

    // Use this for initialization
    void Start () {
        InputManager.Instance.PushModalInputHandler(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	}
}
