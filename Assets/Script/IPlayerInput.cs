using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput
{
    public static IPlayerInput Instance;
    void ControllerInput(int player, string button, string[] values)
    {
        return;
    }
}

public static class IPlayerObj
{
    public static IPlayerInput instance;
}
