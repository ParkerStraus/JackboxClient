using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriviaGameHandler : MonoBehaviour, IPlayerInput
{
    public int[] pointAmt = new int[8];
    public List<int> previousQuestions = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        IPlayerObj.instance=this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ControllerInput(int player, string button, string[] values)
    {
        return;
    }

    public void BroadcastQuestionPrompt(TriviaQuestion question)
    {

    }

    public void NewQuestion()
    {
        
    }
}

[Serializable]
public class TriviaQuestion
{
    public string question;
    public string[] answers;
    public int CorrectChoice;
}
