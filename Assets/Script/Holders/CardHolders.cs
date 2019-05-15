using UnityEngine;
using System.Collections;
using GH.GameElements;

namespace GH
{
    [CreateAssetMenu(menuName ="Holders/Card Holder")]
    public class CardHolders : ScriptableObject
    {
        public GH.TransformVariable handGrid;
        public GH.TransformVariable[] fieldGrid;
        public GH.TransformVariable battleLine;

        public PlayerHolder thisPlayer;


        public void SetCardOnBattleLine(CardInstance card)
        {
            Vector3 position = card.viz.gameObject.transform.position;
            Setting.SetParentForCard(card.transform, battleLine.value.transform);

            position.z = card.viz.gameObject.transform.position.z;
            position.y = card.viz.gameObject.transform.position.y;
            card.viz.gameObject.transform.position = position;
            card.SetIsOnAttackTrue();

            
        }

        public void SetCardDown(CardInstance card)
        {

            Setting.SetParentForCard(card.transform, card.GetOriginFieldLocation()); 
            //Replace card to original place from battle line or whereever
            //I think I need to find FieldArea gameobject and make it as parent of card. Should check this.
            
                                   
        }
        public void SetUpPlayer(PlayerHolder p)
        {
            thisPlayer = p;
        }


        /// <summary>
        /// LoadPlayer
        /// Call player's card on hand, on field and stats
        /// And move this card holder's object to dest
        /// </summary>
        /// <param name="targetPlayer"></param>
        /// <param name="playerStatsUI"></param>
        public void LoadPlayer(PlayerHolder targetPlayer, PlayerStatsUI playerStatsUI)
        {
            int i = 0;
           
            if (targetPlayer == null || playerStatsUI ==null)
                return;

            foreach (CardInstance c in targetPlayer.fieldCard)
            {
                GameObject cardParentObj;
                GameObject headObj;
                //Card is on attack
                if (c.gameObject.GetComponentInParent<Area>() == null || c.transform.parent.name.Contains("BattleLine"))
                {

                    //It detects card on battle line. position should be changed to another battleLine
                    //string str = null;
                    cardParentObj = c.transform.parent.gameObject; //BattleLine
                    headObj = cardParentObj.transform.parent.gameObject;// BattleLineObjs
                    
                    //if(str!=null)
                    //Setting.SetParentForCard(c.viz.gameObject.transform, headObj.transform.FindChild(str).transform);

                }
                    
                else
                {

                    //Below codes find current card's field area and relocate to same area of other side.
                    
                    cardParentObj = c.gameObject.GetComponentInParent<Area>().gameObject;     

                    headObj = cardParentObj.transform.parent.gameObject;
                    for (int t = 0; t < headObj.transform.childCount; t++)
                    {
                        if (cardParentObj == headObj.transform.GetChild(t).gameObject)
                            i = t;

                    }
                    Setting.SetParentForCard(c.viz.gameObject.transform, fieldGrid[i].value.transform);
                }

            
            }

            ///This function is to arrange cards on player's handgrid.
            ///However I have 'CardH andOrder' function for card arrangement.
            foreach (CardInstance c in targetPlayer.handCards)
            {
                Setting.SetParentForCard(c.viz.gameObject.transform, handGrid.value.transform);
            }

            /// Do I really need this function?
            /// Below functions are for changing transform position of user info
            /// however I think it is unnecessary, I won't finish it.
            /// In future if I need, develop below codes.
            ///change location of player informations
            //player.statsUI = otherPlayerStats;
            //player.LoadPlayerOnStatsUI();

            //Debug.Log("current player ui transform : " + otherPlayer.statsUI.transform.localPosition);
            //Transform currentUIObject = player.statsUI.transform;
            //GameObject opponentUIObject = otherPlayerStats.gameObject;

            foreach(CardInstance c in targetPlayer.attackingCards)
            {
                //Debug.Log("attackingCardsCheck");
                SetCardOnBattleLine(c);
            }

        }
    }
}
