//using UnityEngine;
//using UnityEditor;


//namespace GH
//{
//    public class LoadPlayerUI : ScriptableObject
//    {
//        GameController gc = Setting.gameController;
//        public void LoadPlayerOnActive(PlayerHolder loadedPlayer)
//        {
//            //At first run, bottomcardholder is player1, topCardHolder is player2
//            PlayerHolder prevPlayer = gc.TopCardHolder.thisPlayer;


//            if (loadedPlayer == gc.TopCardHolder.thisPlayer)
//            //It is run when player turn(or position) is changed
//            {
//                //playerStats[0] = UIs at bottom / playerStats[1] = UIs at top
//                prevPlayer = gc.BottomCardHolder.thisPlayer;

//                LoadPlayerOnHolder(prevPlayer, gc.allPlayers[1].currentCardHolder, playerStats[0]);              
//                LoadPlayerOnHolder(loadedPlayer, gc.allPlayers[0].currentCardHolder, playerStats[1]);

//                //LoadPlayerOnHolder(prevPlayer, gc.allPlayers[1].currentCardHolder, playerStats[0]);              
//                //LoadPlayerOnHolder(loadedPlayer, gc.allPlayers[0].currentCardHolder, playerStats[1]);
//                if (turns[turnIndex].TurnIndex != 2) 
//                {
//                    topCardHolder = prevPlayer.currentCardHolder;
//                    bottomCardHolder = loadedPlayer.currentCardHolder;
//                }
//            }
//            else if (loadedPlayer == bottomCardHolder.thisPlayer && loadedPlayer.player == "Player2")
//            {
//                Debug.Log("Loop2 + loadedPlayer is " + loadedPlayer);
//                prevPlayer = topCardHolder.thisPlayer;
//                LoadPlayerOnHolder(prevPlayer, bottomCardHolder, playerStats[1]);
//                LoadPlayerOnHolder(loadedPlayer, topCardHolder, playerStats[0]);

//            }
//            else if (loadedPlayer != topCardHolder.thisPlayer && loadedPlayer != bottomCardHolder.thisPlayer)
//            {
//                Debug.LogError("loaded player isn't at bottom nor top");
//            }
//        }
//        public void LoadPlayerOnHolder(PlayerHolder targetPlayer, CardHolders destCardHolder, PlayerStatsUI targetUI)
//        {
//            destCardHolder.LoadPlayer(targetPlayer, targetUI);
//        }

//        public void SwitchUI()
//        {

//        }
//    }
//}