using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SwitchButtonBehaviour : MonoBehaviour, IInputClickHandler {
    public String sceneNameToSwitchTo;

    private TextMesh switchtext;

    public void OnInputClicked(InputClickedEventData eventData) {
        SceneManager.LoadScene(this.sceneNameToSwitchTo);
    }

    // Use this for initialization
    void Start () {
        this.gameObject.SetActive(false);
        this.switchtext = GameObject.Find("SwitchText").GetComponent<TextMesh>();
        this.switchtext.transform.position = this.transform.position - new Vector3(0.0f, 0.0f, 0.0f);
        this.switchtext.transform.LookAt(Camera.main.transform.position);
        this.switchtext.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
    }
	
	// Update is called once per frame
	void Update () {
        if (GazeManager.Instance.HitObject.Equals(this.gameObject)) {
            InputManager.Instance.PushModalInputHandler(this.gameObject);
        }
        this.switchtext.transform.position = this.transform.position - new Vector3(0.0f, 0.0f, 0.0f);
        this.switchtext.transform.LookAt(Camera.main.transform.position);
        this.switchtext.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 180.0f);
    }
}
