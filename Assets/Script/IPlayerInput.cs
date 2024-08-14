using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput
{
    void ControllerInput(int player, string button, string[] values)
    {
        return;
    }
}
