using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticFunctions {
    public static GameControlLogic GetInstance()
    {
        GameControlLogic result = null;
        var gameControl = GameObject.FindGameObjectWithTag("GameController");
        if (gameControl != null)
        {
            result = gameControl.GetComponent<GameControlLogic>();
        }
        if (result == null)
        {
            Debug.Log("does not find GameControl");
        }
        return result;
    }
}
