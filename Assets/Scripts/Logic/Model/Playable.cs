using System;
using System.Collections.Generic;

[Serializable]
public class Playable
{
    public List<PlayableCard> playableCards;
    public List<int> playableTargets;
    public bool playableContinue;
}