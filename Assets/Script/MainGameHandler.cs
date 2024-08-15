using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Windows;
using WebSocketSharp;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MainGameHandler : MonoBehaviour
{
    public static MainGameHandler instance;
    public bool InLobby;
    public static string RoomCode;
    public List<string> Players;
    public List<int> UpdatedPlayers;
    public List<string> Inputs;
    public int MinPlayers;
    public int MaxPlayers;
    private WebSocket webSocket;

    public bool RoomUpdate;
    public bool PlayerUpdate;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        Application.runInBackground = true;
        instance = this;


        webSocket = new WebSocket("ws://54.67.79.113:8080");
        webSocket.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket connection established.");
        };
        webSocket.OnMessage += (sender, e) =>
        {
            MainGameHandler mainGameHandler = this;
            // Parse the incoming message as JSON
            var message = JsonUtility.FromJson<Message>(e.Data);

            try
            {
                // Check if the message type is "room"
                switch (message.type)
                {
                    case "room":
                        RoomCode = message.data;
                        RoomUpdate = true;
                        Debug.Log("Received room code: " + RoomCode);
                        break;
                    case "player":
                        if (!InLobby) return;
                        Debug.Log("Player Joined: " + message.data);
                        if(Players.Count < MaxPlayers)
                        {
                            PlayerUpdate = true;
                            AddPlayer(message.data);
                        }
                        else
                        {
                            //Send message that there is no more room.
                            //webSocket.Send();
                        }
                        break;
                    case "playSend":
                        Debug.Log("Hello from " + message.data);
                        Inputs.Add(message.data);
                        break;
                    default:
                        Debug.Log("Message received from " + ((WebSocket)sender).Url + ": " + e.Data);
                        break;

                }
            }
            catch(Exception err)
            {
                Debug.LogError(err);
            }
        };
        webSocket.Connect();
    }
    void Update()
    {
        if (RoomUpdate)
        {
            RoomUpdate = false;
            UpdateRoomCode();
        }
        if (PlayerUpdate)
        {
            PlayerUpdate = false;
            PlayerUpdateUI();
            Lobby lobby = GameObject.Find("Lobby").GetComponent<Lobby>();
            lobby.UpdateLobby(UpdatedPlayers, Players);
            UpdatedPlayers.Clear();
        }
        if (Inputs.Count > 0)
        {
            foreach(string input in Inputs)
            {
                SendInput(input);
            }
            Inputs.Clear();
        }
    }

    public void UpdateRoomCode()
    {
        Lobby lobby = GameObject.Find("Lobby").GetComponent<Lobby>();
        lobby.UpdateRoomCode(RoomCode);
        Debug.Log("Room code updated successfully.");
    }

    void AddPlayer(string player)
    {
        PlayerUpdate = true;
        print(player);

        // Initialize the number to append
        int dupNum = 0;
        string uniquePlayerName = player;

        // Check if the player name already exists in the list
        while (Players.Contains(uniquePlayerName))
        {
            // Append the number to the player name if it already exists
            dupNum++;
            uniquePlayerName = player + dupNum;
        }

        // Add the unique player name to the list
        Players.Add(uniquePlayerName);
        UpdatedPlayers.Add(Players.Count - 1);
    }


    void PlayerUpdateUI()
    {
        Lobby lobby = GameObject.Find("Lobby").GetComponent<Lobby>();
        lobby.UpdatePlayerNames(Players);
    }

    public void SendToPhone(string Player, string Content)
    {
        // Escape double quotes in the content
        string escapedContent = Content.Replace("\"", "\\\"");

        // Create JSON string with escaped content
        string jsonContent = $"{{\"method\": \"content\", \"data\": {{\"room\": \"{RoomCode}\", \"player\": \"{Player}\", \"content\": \"{escapedContent}\"}}}}";

        print("Sending content to " + Player);
        webSocket.Send(jsonContent);
    }



    void SendInput(string Data)
    {
        var player = JsonUtility.FromJson<PlayControls>(Data);
        print(player.values[0]);

        // Find all MonoBehaviour objects in the scene
        MonoBehaviour[] allObjects = FindObjectsOfType<MonoBehaviour>();

        // Access the singleton instance of IPlayerObj
        IPlayerInput playerObject = IPlayerObj.instance;

        // Check if the singleton instance is not null
        if (playerObject != null)
        {
            Debug.Log("Sending values: " + player.button + ", " + player.values + " from Player " + Players.IndexOf(player.player) + " to " + playerObject);

            // Call the ControllerInput method on the IPlayerInput object
            playerObject.ControllerInput(Players.IndexOf(player.player), player.button, player.values);
        }
        else
        {
            Debug.LogError("No object implementing IPlayerInput found in the scene.");
        }
    }



    // Define a class to represent the message structure
    [System.Serializable]
    public class Message
    {
        public string type;
        public string data;
    }

    [System.Serializable]
    public class PlayControls
    {
        public string player;
        public string button;
        public string[] values;
    }
}