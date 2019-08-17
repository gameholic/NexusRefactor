using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using GH.Player.Assists;

namespace GH.Player
{

    public class PlayerCardManager : PlayerAssists
    {


        [System.NonSerialized]
        public List<int> handCards = new List<int>();
        [System.NonSerialized]
        public List<int> fieldCard = new List<int>();
        [System.NonSerialized]
        public List<int> attackingCards = new List<int>();
        [System.NonSerialized]
        public List<int> deadCards = new List<int>();
        [System.NonSerialized]
        public List<int> allCards = new List<int>();

        public override void Init(NewPlayerHolder p)
        {
            player = p;
        }
    }
}