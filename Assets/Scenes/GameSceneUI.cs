using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GameSceneUI : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("startGameBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            Debug.Log("Click Main Menu");
            Loader.Load(Loader.Scene.MainMenu);
        };
    }
}
