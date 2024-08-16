using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using TMPro;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using UnityEngine.Windows;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour, IPlayerInput
{
    public TMP_Text RoomCode;
    public bool InCountdown = false ;
    public TMP_Text[] PlayerNames;
    [TextArea(15, 20)]
    public string lobbyContent;
    [TextArea(15, 20)]
    public string lobbyContent_Admin;
    [TextArea(15, 20)]
    public string lobbyContent_AdminAbort;

    public Coroutine LobbyCountdownObj;


    public int PlayersAdded;

    public void Start()
    {

        IPlayerObj.instance = this;
    }

    public void UpdatePlayerNames(List<string> updatedNames)
    {
        for (int i = 0; i < PlayerNames.Length; i++)
        {
            if(updatedNames.Count > i)
            {
                PlayerNames[i].text = updatedNames[i];
            }
            else
            {
                PlayerNames[i].text = "Waiting for Player";
            }
        }
    } 

    public void UpdateRoomCode(string roomCode)
    {
        Debug.Log("updating Room code");
        RoomCode.text = roomCode.ToUpper();
    }

    public void UpdateLobby(List<int> updatedPlayers, List<string>playerNames)
    {
        for(int i = 0; i < updatedPlayers.Count;i++)
        {
            if(updatedPlayers[i] == 0)
            {
                MainGameHandler.instance.SendToPhone(playerNames[updatedPlayers[i]], lobbyContent_Admin);
            }
            else
            {
                MainGameHandler.instance.SendToPhone(playerNames[updatedPlayers[i]], lobbyContent);
            }
        }
    }

    public void ControllerInput(int player, string button, string[] values)
    {
        Debug.Log("Received input in lobby from player " + player);
        // Handle the input
        if (player == 0)
        {
            if (!InCountdown)
            {
                // Do something specific for player 0
                LobbyCountdownObj = StartCoroutine(LobbyCountDown());
            }
            else
            {
                StopCoroutine(LobbyCountdownObj);
            }
        }
    }


    public IEnumerator LobbyCountDown()
    {
        //Send abort to admin
        MainGameHandler.instance.SendToPhone(MainGameHandler.instance.Players.ToArray()[0], lobbyContent_AdminAbort);
        //3
        print(3);
        yield return new WaitForSeconds(1.2f);
        //2
        print(2);

        yield return new WaitForSeconds(1.2f);
        //1
        print(1);

        yield return new WaitForSeconds(1.2f);
        //Game Start
        print("game Start");
        MainGameHandler.instance.InLobby = false;
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("GameScene");
    }
}
