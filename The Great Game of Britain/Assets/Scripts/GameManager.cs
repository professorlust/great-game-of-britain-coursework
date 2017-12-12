﻿    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ------------------------------------------------------------------------------------
/// 
/// Authors : Harman Uppal, Tariq
/// 
/// ------------------------------------------------------------------------------------
/// </summary>

public class GameManager : MonoBehaviour
    {

    #region Fields
    private GameObject[] players = new GameObject[4];
    private GameObject start;
    private Vector3 idlePos = new Vector3(0.0f, 0.5f, 0.0f);    // On a stop what local position will the player be located at
    private Canvas mainCanvas;
    public GameObject playerPrefab;

    public Text playerDisplay;
    public Text turnDisplay;
    public Text travelsDisplay;

    private float countdown = 1.0f;
    private int currentplayer = 0;
    private int turnCounter = 1;
    private int numTravels = 0;
    private bool rolled;

    private bool opsCompleted;

    // Managers
    private GameObject cardManager;
    #endregion


    // Use this for initialization
    void Start()
    {
        // Find managers
        cardManager = GameObject.Find("Card Manager");


        rolled = false;

        int numOfPlayers = 2;
        if (GameObject.Find("Game Setup Manager") != null)
        {
            numOfPlayers = GameObject.Find("Game Setup Manager").GetComponent<GameSetUp>().getNumOfPlayers();
        }

        Debug.Log("Number of players set: " + numOfPlayers);

        for (int i = 0; i < numOfPlayers; i++)
        {
            GameObject newPlayer = Instantiate(playerPrefab);
        }

        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Station"))
        {
            if (g.GetComponent<Stop>().checkIsStart())
            {
                start = g.gameObject;
            }
        }

        playerDisplay.text = ("Player " + (currentplayer + 1) + "'s turn");
        turnDisplay.text = ("Turn: " + turnCounter);

        players[currentplayer].GetComponent<PlayerController>().setEnableAnimation(true);
        players[currentplayer].GetComponent<PlayerController>().controllerEnabled = true;
        players[currentplayer].GetComponent<PlayerController>().setTravels(numTravels);

        mainCanvas = GameObject.FindGameObjectWithTag("Main Canvas").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        // buffer to allow stations to spawn
        countdown -= 1.0f * Time.deltaTime;
        if (countdown <= 0.0f && !opsCompleted)
        {
            Debug.Log("Assigning Roles");
            playersToStart();
            // set true if all operations are complete
            opsCompleted = true;
        }
        else if (countdown <= 0.0f && opsCompleted)
        {
            // end countdown
            countdown = 0.0f;
        }

        playerDisplay.text = ("Player " + (currentplayer + 1) + "'s turn");
        turnDisplay.text = ("Turn: " + turnCounter);
        travelsDisplay.text = ("Travels Remaining: " + numTravels);
    }

    /// <summary>
    /// moves all the players to the start position.
    /// </summary>
    private void playersToStart()
    {
        // find all players
        foreach (GameObject p in players)
        {
            if (start.transform.childCount > 1)
            {
                // change player parent to new station and update position
                p.transform.parent = start.transform;
                p.transform.localPosition = idlePos + (new Vector3(1.5f, 0.0f, 0.0f) * (start.transform.childCount - 2));
            }
            else
            {
                // change player parent to new station and update position
                p.transform.parent = start.transform;
                p.transform.localPosition = idlePos;
            }
        }
    }


    public void moveCurrentPlayer()
    {
        players[currentplayer].GetComponent<PlayerController>().moveToSelectedStation();
        numTravels = players[currentplayer].GetComponent<PlayerController>().getTravels();
    }

    private void nextPlayerTurn()
    {
        
        players[currentplayer].GetComponent<PlayerController>().setEnableAnimation(false); //disables current player
        players[currentplayer].GetComponent<PlayerController>().controllerEnabled = false;
        currentplayer++;
        turnCounter++;
        if (currentplayer>=players.Length)
        {
            currentplayer = 0;
           
        }

        players[currentplayer].GetComponent<PlayerController>().setEnableAnimation(true); //enables the next player in turn 
        players[currentplayer].GetComponent<PlayerController>().controllerEnabled = true;       
        rolled =false;
        mainCanvas.transform.Find("Text_Selection").GetComponent<Text>().text = ("Selected: " );
        mainCanvas.transform.Find("Text_MovePermission").GetComponent<Text>().text = ("Accessible: ");
        numTravels = 0;
    }

        public void rollDice()
        {
            if (!rolled)
            {
                var randomInt = Random.Range(1, 7);
                numTravels = randomInt;
                players[currentplayer].GetComponent<PlayerController>().setTravels(numTravels);
            }

            rolled = true;
        }

        public void endGame()
        {
            SceneManager.LoadScene("EndGame_Scene");
        }

        public void drawHazardCard()
        {
            //cardManager.GetComponent<Card>().drawCard();
        }
    }