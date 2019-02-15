﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    // Awake() runs before any Start() calls
    // Enforces the singleton pattern
    private void Awake()
    {
        // Check if instance exists
        if(instance == null)
        {
            // If not, set the game manager to this
            instance = this;
        }

        else if(instance != this)
        {
            Destroy(gameObject);
        }

        // Ensures that this persists between scenes
        DontDestroyOnLoad(gameObject);
    }

    // For internal use
    private float timeRemaining;
    private uint player1Food;
    private uint player2Food;
    private uint player3Food;
    private uint player4Food;

    //The sources the players depositied
    private uint player1DepositedRes;
    private uint player2DepositedRes;
    private uint player3DepositedRes;
    private uint player4DepositedRes;

    // Bird prefab
    public GameObject birdPrefab;

    // List of players connected
    public GameObject[] playerList;

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;

    Scene currentScene;

    public GameObject GasTrap;
    public GameObject MouseTrap;
    public string winner;

    public bool gameSceneInitialized;

    // Quick way to do get and set functions for variables
    public float TimeRemaining
    {
        get { return timeRemaining; }
        set { timeRemaining = value; }
    }

    public uint Player1Food
    {
        get { return player1Food; }
        set { player1Food = value; }
    }

    public uint Player2Food
    {
        get { return player2Food; }
        set { player2Food = value; }
    }

    public uint Player3Food
    {
        get { return player3Food; }
        set { player3Food = value; }
    }

    public uint Player4Food
    {
        get { return player4Food; }
        set { player4Food = value; }
    }

    public uint Player1DepositedRes
    {
        get { return player1DepositedRes;}
     
    }
    public uint Player2DepositedRes
    {
        get { return player2DepositedRes; }

    }
    public uint Player3DepositedRes
    {
        get { return player3DepositedRes; }

    }
    public uint Player4DepositedRes
    {
        get { return player4DepositedRes; }

    }

    private float startTime = 120; // Sixty seconds times five

	// Use this for initialization
	void Start ()
    {
        gameSceneInitialized = false;
    }

    // Update is called once per frame
    void Update() {
        //if (Player1 != null) //if the game is actually running and players exist
        //{
        //    TimeRemaining -= Time.deltaTime;
        //    Player1Food = Player1.GetComponent<Player>().Resources;
        //
        //    player1DepositedRes = Player1.GetComponent<Player>().depositedResources;
        //
        //    if (TimeRemaining <= 0.0f)
        //    {
        //        GameOver();
        //    }
        //
        //
        //
        //}

        currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Main Quinn Version" && gameSceneInitialized == false)
        {
            playerList = GameObject.FindGameObjectsWithTag("Player");

            InitializePlayers();
            SetCameras();

            TimeRemaining = startTime;
            gameSceneInitialized = true;
        }

    }

    // Function that will switch game scenes once the time has run out
    void GameOver()
    {
        //if (CheckWinner() != null)
        //{
        //    Debug.Log("Player " + CheckWinner().GetComponent<Player>().playerNumber + "is the winner!");
        //    winner = CheckWinner().GetComponent<Player>().playerNumber.ToString();
        //    SceneManager.LoadScene("Victory");
        //
        //    //end our scene 
        //    //in the next scene we read the data from the game manager and then we display it
        //    //When we are done displaying the data, we reset it back to default values when they hit play again or we destory everything if they go back to main menu
        //}
        //else
        //{
        //    Debug.Log("No winner");
        //    //end our scene
        //}

    }

    public GameObject CheckWinner()
    {
        if (player1DepositedRes > player2DepositedRes && player1DepositedRes > player3DepositedRes && player1DepositedRes > player4DepositedRes)
            return Player1;
        else if (player2DepositedRes > player1DepositedRes && player2DepositedRes > player3DepositedRes && player2DepositedRes > player4DepositedRes)
            return Player2;
        else if (player3DepositedRes > player1DepositedRes && player3DepositedRes > player2DepositedRes && player3DepositedRes > player4DepositedRes)
            return Player3;
        else if (player4DepositedRes > player1DepositedRes && player4DepositedRes > player2DepositedRes && player4DepositedRes > player3DepositedRes)
            return Player4;
        else
            return null;
    }

    // Dividing up the screen
    void SetCameras()
    {
        // Temporary rect transform
        Rect temp = Rect.zero;

        // Top left
        temp.Set(0.0f, 0.5f, 0.5f, 0.5f);
        Player1.GetComponentInChildren<Camera>().rect = temp;

        // Top right
        temp.Set(0.5f, 0.5f, 0.5f, 0.5f);
        Player2.GetComponentInChildren<Camera>().rect = temp;

        // Bottom left
        temp.Set(0.0f, 0.0f, 0.5f, 0.5f);
        Player3.GetComponentInChildren<Camera>().rect = temp;

        // Bottom right
        temp.Set(0.5f, 0.0f, 0.5f, 0.5f);
        Player4.GetComponentInChildren<Camera>().rect = temp;

        // Resetting temp back to zero
        temp = Rect.zero;

    }

    void SpawnPlayers()
    {
        for(int i = 0; i < playerList.Length; i++)
        {
            Player1 = Instantiate(birdPrefab);
            Player1.GetComponent<Player>().spawnPoint = new Vector3(0, 0, 0);
        }
    }

    void InitializePlayers()
    {

        for(int i = 0; i < playerList.Length; i++)
        {
            if(playerList[i].GetComponent<Player>().playerNum == 1)
            {
                Player1 = playerList[i];
            }
            else if(playerList[i].GetComponent<Player>().playerNum == 2)
            {
                Player2 = playerList[i];
            }
            else if(playerList[i].GetComponent<Player>().playerNum == 3)
            {
                Player3 = playerList[i];
            }
            else if(playerList[i].GetComponent<Player>().playerNum == 4)
            {
                Player4 = playerList[i];
            }
            else
            {
                Debug.LogError("Something broke when initializing players");
            }
        }
    }
}
