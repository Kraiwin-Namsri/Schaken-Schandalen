using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager
{
    public int counter = 0;
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadStockFishScene()
    {
        SceneManager.LoadScene("VS_Stockfish");
    }

    public void Backwards(Board.Intern internboard)
    {
        Debug.Log("backwards");
        counter++;
        Legal legal = internboard.legal;
        Debug.Log(legal.gamePositions.Count);
        internboard.fenManager.Apply(legal.gamePositions[(legal.gamePositions.Count-1) - counter]);
    }

    public void Forwards(Board.Intern internboard)
    {
        Debug.Log("forewards");
        counter--;
        Legal legal = internboard.legal;
        internboard.fenManager.Apply(legal.gamePositions[(legal.gamePositions.Count - 1) - counter]);
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
