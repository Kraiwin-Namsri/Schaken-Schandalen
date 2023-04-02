using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadStockFishScene()
    {
        SceneManager.LoadScene("VS_Stockfish");
    }

    public static void Backwards()
    {

    }

    public static void Forwards()
    {

    }

    public static void PlayAsWhite() 
    {

    }

    public static void PlayAsBlack()
    {

    }

    public static void StockfishBestMove()
    {

    }

    public static void EnterFenstring()
    {
    }
}
