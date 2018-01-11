using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HoloToolkit.Unity.InputModule;

public class CubeBehaviour : MonoBehaviour, IInputClickHandler {

    public GameObject prefab;
    public float forceMultiplier;
    public float cubeSpawnDistance;
    public List<GameObject> cubes;

    public void OnInputClicked(InputClickedEventData eventData) {
        bool newPlace = true;

        foreach (GameObject cube in this.cubes) {
            if (GazeManager.Instance.HitObject.Equals(cube)) {
                Rigidbody rb = cube.GetComponent<Rigidbody>();
                rb.AddForce(Camera.main.transform.forward * this.forceMultiplier);
                newPlace = false;
            }
        }
        if (newPlace) {
            GameObject newCube = Instantiate(this.prefab) as GameObject;
            newCube.transform.position = Camera.main.transform.position + Camera.main.transform.forward * this.cubeSpawnDistance;
            this.cubes.Add(newCube);
        }
    }


    // Use this for initialization
    void Start () {
        this.cubes = new List<GameObject>();
        InputManager.Instance.PushModalInputHandler(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (!GazeManager.Instance.HitObject.Equals(GameObject.Find("ResetButton")) &&
            !GazeManager.Instance.HitObject.Equals(GameObject.Find("SwitchButton"))) {
            InputManager.Instance.PushModalInputHandler(this.gameObject);
        }
    }
}
