using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractTest : MonoBehaviour, IPlayerInput
{
    [SerializeField] private TMP_Text m_text;
    [TextArea(15, 20)]
    [SerializeField] private string TextEntryContent;
    // Start is called before the first frame update
    void Start()
    {
        IPlayerObj.instance = this;
        for (int i = 0; i < MainGameHandler.instance.Players.Count; i++) {
            MainGameHandler.instance.SendToPhone(MainGameHandler.instance.Players[i], TextEntryContent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ControllerInput(int player, string button, string[] values)
    {
        m_text.text += MainGameHandler.instance.Players.ToArray()[player] + '\n' + values[0] + '\n';
        return;
    }
}
