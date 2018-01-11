using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetButtonBehaviour : MonoBehaviour, IInputClickHandler {

    private TextMesh resetText;

    public void OnInputClicked(InputClickedEventData eventData) {
        List<GameObject> cubes = GameObject.Find("CubeManager").GetComponent<CubeBehaviour>().cubes;
        foreach (GameObject cube in cubes) {
            Destroy(cube);
        }
    }

    // Use this for initialization
    void Start () {
        this.resetText = GameObject.Find("ResetText").GetComponent<TextMesh>();
        this.resetText.transform.position = this.transform.position - new Vector3(0.0f, 0.015f, 0.0f);
        this.resetText.transform.LookAt(Camera.main.transform.position);
        this.resetText.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
	}
	
	// Update is called once per frame
	void Update () {
        if (GazeManager.Instance.HitObject.Equals(this.gameObject)) {
            InputManager.Instance.PushModalInputHandler(this.gameObject);
        }
        this.resetText.transform.position = this.transform.position - new Vector3(0.0f, 0.015f, 0.0f);
        this.resetText.transform.LookAt(Camera.main.transform.position);
        this.resetText.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
    }
}
