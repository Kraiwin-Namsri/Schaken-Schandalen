using Microsoft.MixedReality.Toolkit.Boundary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public bool isPlayingWhite;
    public dynamic bot;
    public Action<Player, Move> callback;
    public Player(Action<Player, Move> callback, bool isPlayingWhite)
    {
        this.callback = callback;
        this.isPlayingWhite = isPlayingWhite;
    }

    public void SubmitMove(Move move)
    {
        callback(this, move);
    }
}
