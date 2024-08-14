using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using TMPro;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using UnityEngine.Windows;

public class Lobby : MonoBehaviour, IPlayerInput
{
    public TMP_Text RoomCode;
    public bool InCountDown;
    public TMP_Text[] PlayerNames;
    [TextArea(15, 20)]
    public string lobbyContent;
    [TextArea(15, 20)]
    public string lobbyContent_Admin;


    public int PlayersAdded;

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
                FindObjectOfType<MainGameHandler>().SendToPhone(playerNames[updatedPlayers[i]], lobbyContent_Admin);
            }
            else
            {
                FindObjectOfType<MainGameHandler>().SendToPhone(playerNames[updatedPlayers[i]], lobbyContent);
            }
        }
    }

    public void ControllerInput(int player, string button, string[] values)
    {
        Debug.Log("Received input in lobby from player " + player);
        // Handle the input
        if (player == 0)
        {
            if (!InCountDown)
            {
                // Do something specific for player 0
                StartCoroutine(LobbyCountDown());
            }
            else
            {
                StopAllCoroutines();
            }
        }
    }


    public IEnumerator LobbyCountDown()
    {
        //3
        yield return new WaitForSeconds(1);
        //2

        yield return new WaitForSeconds(1);
        //1

        yield return new WaitForSeconds(1);
        //Game Start
    }
}
