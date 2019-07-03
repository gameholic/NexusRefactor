using UnityEngine;
using UnityEditor;


namespace GH.Setup
{
    public class LoadPlayerUI
    {
        
        public void LoadPlayerOnActive(PlayerHolder loadedPlayer)
        {

            GameController gc = Setting.gameController;
            PlayerHolder prevPlayer = gc.TopCardHolder.thisPlayer;
            if (loadedPlayer == gc.TopCardHolder.thisPlayer)
            {
                prevPlayer = gc.BottomCardHolder.thisPlayer;

                LoadPlayerOnHolder(prevPlayer, gc.GetPlayer(1)._CardHolder, gc.GetPlayerUIInfo(0));
                LoadPlayerOnHolder(loadedPlayer, gc.GetPlayer(0)._CardHolder, gc.GetPlayerUIInfo(1));

                if (gc.GetTurns(gc.turnIndex).PhaseIndex != 2)
                {
                    gc.TopCardHolder = prevPlayer._CardHolder;
                    gc.BottomCardHolder = loadedPlayer._CardHolder;
                }
            }
            else if (loadedPlayer == gc.BottomCardHolder.thisPlayer && loadedPlayer.player == "Player2")
            {
                Debug.Log("Loop2 + loadedPlayer is " + loadedPlayer);
                prevPlayer = gc.TopCardHolder.thisPlayer;
                LoadPlayerOnHolder(prevPlayer, gc.BottomCardHolder, gc.GetPlayerUIInfo(1));
                LoadPlayerOnHolder(loadedPlayer, gc.TopCardHolder, gc.GetPlayerUIInfo(0));

            }
            else if (loadedPlayer != gc.TopCardHolder.thisPlayer && loadedPlayer != gc.BottomCardHolder.thisPlayer)
            {
                Debug.LogError("loaded player isn't at bottom nor top");
            }
        }
        public void LoadPlayerOnHolder(PlayerHolder targetPlayer, CardHolders destCardHolder, PlayerStatsUI targetUI)
        {
            destCardHolder.LoadPlayer(targetPlayer, targetUI);
        }

        public void SwitchUI()
        {

        }
    }
}