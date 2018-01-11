using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class DistanceTrackingHandler : MonoBehaviour, ITrackableEventHandler {

    public bool useFixDistance;
    public double minDistanceBetweenMode;
    public double minDistanceFarMode;

    private TrackableBehaviour mTrackableBehaviour;
    private bool trackingFound = false;
    private bool betweenVisible = false;
    private bool closeVisible = false;
    private bool farVisible = false;
    private bool modelState = false;
    private bool detailMode = false;

    void Start() {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour) {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    void Update() {
        if (this.trackingFound && !this.detailMode) {
            if (Math.Abs(this.gameObject.transform.position.y - Camera.main.transform.position.y) >= this.minDistanceBetweenMode &&
                Math.Abs(this.gameObject.transform.position.y - Camera.main.transform.position.y) < this.minDistanceFarMode) {
                if (!this.betweenVisible) {
                    this.betweenVisible = true;
                    this.closeVisible = false;
                    this.farVisible = false;
                    this.OnTrackingLost();
                    this.OnTrackingFound("hh_text");
                    // If some models have been activated as the user has been close enough to see the markers, those models need to be deactivated 
                    // when the user gets in the state where the Hamburg section text is being displayed
                    if (this.modelState) {
                        this.modelState = false;
                        GameObject.Find("ClickHandlerObject").GetComponent<POIBehaviuor>().changeModelState(false);
                    }
                }
            } else if (Math.Abs(this.gameObject.transform.position.y - Camera.main.transform.position.y) < this.minDistanceBetweenMode) {
                if (!this.closeVisible) {
                    this.betweenVisible = false;
                    this.closeVisible = true;
                    this.farVisible = false;
                    this.OnTrackingLost();
                    this.OnTrackingFound("poi_markers");
                    // Activated models need to be displayed again if the user gets close enough again
                    if (!this.modelState) {
                        this.modelState = true;
                        GameObject.Find("ClickHandlerObject").GetComponent<POIBehaviuor>().changeModelState(true);
                    }
                }
            } else {
                if (!this.farVisible) {
                    this.betweenVisible = false;
                    this.closeVisible = false;
                    this.farVisible = true;
                    this.OnTrackingLost();
                    this.OnTrackingFound("general_text");
                }
            }
        }
    }

    /// <summary>
    /// Implementation of the ITrackableEventHandler function called when the
    /// tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus) {
        if ((newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)) {

            // Determine if user has chosen to use fix custom values for distance tracking or default configuration with 15cm distance to each other from
            // start distance 
            if(!this.useFixDistance) {
                double distanceToTarget = Math.Abs(this.gameObject.transform.position.y - Camera.main.transform.position.y);
                this.minDistanceFarMode = distanceToTarget;
                this.minDistanceBetweenMode = distanceToTarget - 0.15;
            }

            // Target has been tracked~
            this.trackingFound = true;
            GameObject.Find("Debug_Text").GetComponent<DebugTextBehaviour>().debugMessage("tracking found");
        } else {
            this.trackingFound = false;
            GameObject.Find("Debug_Text").GetComponent<DebugTextBehaviour>().debugMessage("tracking lost");
            OnTrackingLost();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
        }
    }

    // Used for detail mode - if detail mode has been started for a model, deactivate all other vuforia elements - if detail mode is terminated, activate all vuforia elements again
    public void changeVuforiaElementsState(bool state, String modelCalled) {
        this.detailMode = !state;
        if (!state) {
            this.OnTrackingLost();
            this.modelState = false;
            this.closeVisible = false;
            this.betweenVisible = false;
            this.farVisible = false;
            GameObject.Find("ClickHandlerObject").GetComponent<POIBehaviuor>().changeModelState(false, modelCalled);
        }
        GameObject.Find("Debug_Text").GetComponent<DebugTextBehaviour>().debugMessage("detailMode = " + this.detailMode);
        GameObject.Find("Debug_Text").GetComponent<DebugTextBehaviour>().debugMessage("trackingFound = " + this.trackingFound);
    }

    private void OnTrackingFound(String gameObjectName) {
        Renderer[] rendererComponents = GameObject.Find(gameObjectName).GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GameObject.Find(gameObjectName).GetComponentsInChildren<Collider>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents) {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents) {
            component.enabled = true;
        }
    }

    private void OnTrackingLost() {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Disable rendering:
        foreach (Renderer component in rendererComponents) {
            component.enabled = false;
        }

        // Disable colliders:
        foreach (Collider component in colliderComponents) {
            component.enabled = false;
        }
    }
}
