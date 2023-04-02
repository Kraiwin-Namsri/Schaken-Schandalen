using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager
{
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadStockFishScene()
    {
        SceneManager.LoadScene("VS_Stockfish");
    }

    public void Backwards()
    {

    }

    public void Forwards()
    {

    }

    public void SwitchColour(Player player1, Player player2)
    {
        player1.isPlayingWhite = !player1.isPlayingWhite;
        player2.isPlayingWhite = !player2.isPlayingWhite;
    }

    public void StockfishBestMove()
    {

    }

    public void EnterFenstring()
    {

    }
}
