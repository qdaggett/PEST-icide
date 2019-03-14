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

    // Player character prefabs
    public GameObject birdPrefab;
    public GameObject ratPrefab;
    public GameObject snakePrefab;
    public GameObject wombatPrefab;

    // List of players connected
    public List<GameObject> playerList;

    // List of controllers connected
    public string[] controllers;

    // Slots to hold instantiated players
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;

    // Player spawnpoints
    public GameObject[] spawnPoints;

    // Player resources
    public int player1DResource;
    public int player2DResource;
    public int player3DResource;
    public int player4DResource;

    // For playing with fewer than 4 players
    public bool player1Active;
    public bool player2Active;
    public bool player3Active;
    public bool player4Active;

    Scene currentScene;

    public int[] charSelections;

    public GameObject GasTrap;
    public GameObject MouseTrap;
    public string winner;

    public bool gameSceneInitialized;

    public float startTime = 10; // Sixty seconds times five
    public float timer;

	// Use this for initialization
	void Start ()
    {
        gameSceneInitialized = false;
        charSelections = new int[4];
        player1Active = false;
        player2Active = false;
        player3Active = false;
        player4Active = false;
        timer = startTime;
        spawnPoints = new GameObject[4];


    }

    // Update is called once per frame
    void Update() {

        // Debugging controllers
        DebugControllers();

        currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Main Quinn Version")
        {
            if(gameSceneInitialized == false)
            {
                spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
                SpawnPlayers();
                InitializePlayers();
                SetCameras();

                gameSceneInitialized = true;
            }

            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                GameOver();
            }
            
        }

    }

    // Function that will switch game scenes once the time has run out
    void GameOver()
    {
        if (CheckWinner() != null)
        {
            Debug.Log("Player " + CheckWinner().GetComponent<Player>().playerNum + "is the winner!");
            winner = CheckWinner().GetComponent<Player>().playerNum.ToString();
            SceneManager.LoadScene("Victory");
        
            //end our scene 
            //in the next scene we read the data from the game manager and then we display it
            //When we are done displaying the data, we reset it back to default values when they hit play again or we destory everything if they go back to main menu
        }
        else
        {
            Debug.Log("No winner");
            //end our scene
        }

    }

    public GameObject CheckWinner()
    {
        if (player1DResource > player2DResource && player1DResource > player3DResource && player1DResource > player4DResource)
            return Player1;
        else if (player2DResource > player1DResource && player2DResource > player3DResource && player2DResource > player4DResource)
            return Player2;
        else if (player3DResource > player1DResource && player3DResource > player2DResource && player3DResource > player4DResource)
            return Player3;
        else if (player4DResource > player1DResource && player4DResource > player2DResource && player4DResource > player3DResource)
            return Player4;
        else
            return null;
    }

    // Dividing up the screen
    void SetCameras()
    {
        // Temporary rect transform
        Rect temp = Rect.zero;

        // 1 player currently active
        if(player1Active && !player2Active && !player3Active && !player4Active)
        {
            // Player 1
            temp.Set(0.0f, 0.0f, 1.0f, 1.0f);
            Player1.GetComponentInChildren<Camera>().rect = temp;
        }

        // 2 players currently active
        else if(player1Active && player2Active && !player3Active && !player4Active)
        {
            // Player 1
            temp.Set(0.0f, 0.0f, 0.5f, 1.0f);
            Player1.GetComponentInChildren<Camera>().rect = temp;

            // Player 2
            temp.Set(0.5f, 0.0f, 0.5f, 1.0f);
            Player2.GetComponentInChildren<Camera>().rect = temp;

        }

        // 3 players currently active
        else if(player1Active && player2Active && player3Active && !player4Active)
        {
            // Player 1
            temp.Set(0.0f, 0.0f, 0.5f, 0.5f);
            Player1.GetComponentInChildren<Camera>().rect = temp;

            // Player 2
            temp.Set(0.5f, 0.0f, 0.5f, 0.5f);
            Player2.GetComponentInChildren<Camera>().rect = temp;

            // Player 3
            temp.Set(0.0f, 0.0f, 1.0f, 0.5f);
            Player3.GetComponentInChildren<Camera>().rect = temp;
        }

        // 4 players currently active
        else if(player1Active && player2Active && player3Active && player4Active)
        {
            // Player 1
            temp.Set(0.0f, 0.5f, 0.5f, 0.5f);
            Player1.GetComponentInChildren<Camera>().rect = temp;

            // Player 2
            temp.Set(0.5f, 0.5f, 0.5f, 0.5f);
            Player2.GetComponentInChildren<Camera>().rect = temp;

            // Player 3
            temp.Set(0.0f, 0.0f, 0.5f, 0.5f);
            Player3.GetComponentInChildren<Camera>().rect = temp;

            // Player 4
            temp.Set(0.5f, 0.0f, 0.5f, 0.5f);
            Player4.GetComponentInChildren<Camera>().rect = temp;
        }

        // Resetting temp back to zero
        temp = Rect.zero;

    }

    void SpawnPlayers()
    {
        for(int i = 0; i < controllers.Length; i++)
        {
            if (charSelections[i].Equals(1))
            {
                int tempRand;
                tempRand = Random.Range(2,6);
                print(tempRand);

                if(tempRand == 2)
                {
                    playerList.Add(Instantiate(ratPrefab, spawnPoints[i].transform.position, Quaternion.identity));
                    playerList[i].name = "Player " + (i + 1);
                    playerList[i].GetComponent<Player>().spawnPoint = spawnPoints[i].transform.position;
                    playerList[i].GetComponent<Player>().maxHealth = 25f;
                    playerList[i].GetComponent<Player>().health = 25f;
                    playerList[i].GetComponent<Player>().playerNum = i + 1;
                }
                else if(tempRand == 3)
                {
                    playerList.Add(Instantiate(birdPrefab, spawnPoints[i].transform.position, Quaternion.identity));
                    playerList[i].name = "Player " + (i + 1);
                    playerList[i].GetComponent<Player>().spawnPoint = spawnPoints[i].transform.position;
                    playerList[i].GetComponent<Player>().maxHealth = 40f;
                    playerList[i].GetComponent<Player>().health = 40f;
                    playerList[i].GetComponent<Player>().playerNum = i + 1;
                }
                else if(tempRand == 4)
                {
                    playerList.Add(Instantiate(snakePrefab, spawnPoints[i].transform.position, Quaternion.identity));
                    playerList[i].name = "Player " + (i + 1);
                    playerList[i].GetComponent<Player>().spawnPoint = spawnPoints[i].transform.position;
                    playerList[i].GetComponent<Player>().maxHealth = 50f;
                    playerList[i].GetComponent<Player>().health = 50f;
                    playerList[i].GetComponent<Player>().playerNum = i + 1;
                }
                else if(tempRand == 5)
                {
                    playerList.Add(Instantiate(wombatPrefab, spawnPoints[i].transform.position, Quaternion.identity));
                    playerList[i].name = "Player " + (i + 1);
                    playerList[i].GetComponent<Player>().spawnPoint = spawnPoints[i].transform.position;
                    playerList[i].GetComponent<Player>().maxHealth = 70f;
                    playerList[i].GetComponent<Player>().health = 70f;
                    playerList[i].GetComponent<Player>().playerNum = i + 1;
                }
                
            }

            else if(charSelections[i].Equals(2))
            {
                playerList.Add(Instantiate(ratPrefab, spawnPoints[i].transform.position, Quaternion.identity));
                playerList[i].name = "Player " + (i + 1);
                playerList[i].GetComponent<Player>().spawnPoint = spawnPoints[i].transform.position;
                playerList[i].GetComponent<Player>().maxHealth = 25f;
                playerList[i].GetComponent<Player>().health = 25f;
                playerList[i].GetComponent<Player>().playerNum = i + 1;
            }

            else if(charSelections[i].Equals(3))
            {
                playerList.Add(Instantiate(birdPrefab, spawnPoints[i].transform.position, Quaternion.identity));
                playerList[i].name = "Player " + (i + 1);
                playerList[i].GetComponent<Player>().spawnPoint = spawnPoints[i].transform.position;
                playerList[i].GetComponent<Player>().maxHealth = 40f;
                playerList[i].GetComponent<Player>().health = 40f;
                playerList[i].GetComponent<Player>().playerNum = i + 1;
            }

            else if(charSelections[i].Equals(4))
            {
                playerList.Add(Instantiate(snakePrefab, spawnPoints[i].transform.position, Quaternion.identity));
                playerList[i].name = "Player " + (i + 1);
                playerList[i].GetComponent<Player>().spawnPoint = spawnPoints[i].transform.position;
                playerList[i].GetComponent<Player>().maxHealth = 50f;
                playerList[i].GetComponent<Player>().health = 50f;
                playerList[i].GetComponent<Player>().playerNum = i + 1;
            }

            else if (charSelections[i].Equals(5))
            {
                playerList.Add(Instantiate(wombatPrefab, spawnPoints[i].transform.position, Quaternion.identity));
                playerList[i].name = "Player " + (i + 1);
                playerList[i].GetComponent<Player>().spawnPoint = spawnPoints[i].transform.position;
                playerList[i].GetComponent<Player>().maxHealth = 70f;
                playerList[i].GetComponent<Player>().health = 70f;
                playerList[i].GetComponent<Player>().playerNum = i + 1;
            }
        }
    }

    void InitializePlayers()
    {

        for(int i = 0; i < playerList.Count; i++)
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

    // Debug function for checking how many controllers are connected
    void DebugControllers()
    {
        // Checking for connected controllers
        controllers = Input.GetJoystickNames();

        // If any controllers are, or have been previously, connected
        if (controllers.Length > 0)
        {
            // For each controller entry
            for (int i = 0; i < controllers.Length; i++)
            {
                // Check if string entry is empty or not
                if (!string.IsNullOrEmpty(controllers[i]))
                {
                    // String is not empty, controller is connected
                    Debug.Log("Controller " + i + " currently connected using: " + controllers[i]);
                }

                else
                {
                    // String is empty, controller is not connected
                    Debug.Log("Controller " + i + " is not connected.");
                }

                // Enabling players for number of controllers plugged in
                if(controllers.Length >= 1 && !string.IsNullOrEmpty(controllers[0]))
                {
                    player1Active = true;
                }

                if(controllers.Length >= 2 && !string.IsNullOrEmpty(controllers[1]))
                {
                    player2Active = true;
                }

                if(controllers.Length >= 3 && !string.IsNullOrEmpty(controllers[2]))
                {
                    player3Active = true;
                }

                if(controllers.Length >= 4 && !string.IsNullOrEmpty(controllers[3]))
                {
                    player4Active = true;
                }
                

            }
        }
    }
}
