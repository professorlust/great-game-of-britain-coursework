﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    #region Fields

    private GameObject selected;                                // Currently selected object
    private Canvas mainCanvas;                                  // Main canvas in scene for UI
    private Vector3 idlePos = new Vector3(0.0f, 0.5f, 0.0f);    // On a stop what local position will the player be located at
    public bool controllerEnabled { get; set; }

    #endregion Contains fields for this class

    // Use this for initialization
    void Start ()
    {
        controllerEnabled = false;

        // Fetch main canvas from scene
        mainCanvas = GameObject.FindGameObjectWithTag("Main Canvas").GetComponent<Canvas>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        // If left-mouse is pressed and currently not hovering over a UI element
        if (Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)
        {
            Debug.Log("Mouse is down");

            // Send a raycast
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

            // if it hit something
            if (hit)
            {
                // if it hit a station
                if (hitInfo.transform.parent.gameObject.tag == "Station")
                {
                    // set selected to equal that station
                    selected = hitInfo.transform.parent.gameObject;
                    Debug.Log("Selected Station" + selected.GetComponent<Stop>().stopName);

                    //update UI text on main canvas to display selected stops name
                    mainCanvas.transform.Find("Text_Selection").GetComponent<Text>().text = ("Selected: " + selected.GetComponent<Stop>().stopName);
                }
                // else if it's not a station
                else
                {
                    Debug.Log("Didn't select a valid object");
                }
            }
            // else if it didn't hit any coliider
            else
            {
                Debug.Log("No hit");
            }
        }

	}

    /// <summary>
    /// Moves player to selected station
    /// </summary>
    public void moveToSelectedStation()
    {
        // if selected is not empty
        if (selected != null)
        {
            if (selected.transform.childCount > 1)
            {
                // change player parent to new station and update position
                gameObject.transform.parent = selected.transform;

                // will adjust depending on how many children are attached to the station
                gameObject.transform.localPosition = idlePos + ((new Vector3(1.5f, 0f, 0f)) * (selected.transform.childCount - 2));
            }
            else
            {
                // change player parent to new station and update position
                gameObject.transform.parent = selected.transform;
                gameObject.transform.localPosition = idlePos;
            }
        }
        else
        {
            Debug.Log("Selected is empty");
        }
    }
}
