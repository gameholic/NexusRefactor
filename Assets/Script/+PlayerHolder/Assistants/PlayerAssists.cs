using UnityEngine;
using UnityEditor;



namespace GH.Player.Assists
{

    public abstract class PlayerAssists 
    {
        protected PlayerHolder player;


        public abstract void Init(PlayerHolder p);
    }

}