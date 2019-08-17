using UnityEngine;
using UnityEditor;



namespace GH.Player.Assists
{

    public abstract class PlayerAssists 
    {
        protected NewPlayerHolder player;


        public abstract void Init(NewPlayerHolder p);
    }

}