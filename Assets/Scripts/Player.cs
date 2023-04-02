using Microsoft.MixedReality.Toolkit.Boundary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public bool isPlayingWhite;
    public dynamic bot;
    public Player(bool isPlayingWhite)
    {
        this.isPlayingWhite = isPlayingWhite;
    }
}
