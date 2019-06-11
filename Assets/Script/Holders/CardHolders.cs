using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GH.GameElements;
using GH.GameCard;
namespace GH
{
    [CreateAssetMenu(menuName ="Holders/Card Holder")]
    public class CardHolders : ScriptableObject
    {
        public GH.TransformVariable handGrid;
        

        [SerializeField]
        private GH.TransformVariable[] _FieldGrid;
        public GH.TransformVariable battleLine;
        public PlayerHolder thisPlayer;

        public TransformVariable GetFieldGrid(int i)
        {
            return _FieldGrid[i];
        }
        public TransformVariable[] GetFieldGrid()
        {
            return _FieldGrid;
        }

        public void SetCardOnBattleLine(CardInstance card)
        {
            Vector3 position = card.viz.gameObject.transform.position;
            Setting.SetParentForCard(card.transform, battleLine.value.transform);

            position.z = card.viz.gameObject.transform.position.z;
            position.y = card.viz.gameObject.transform.position.y;
            card.viz.gameObject.transform.position = position;
           // card.SetIsOnAttack();
            card.IsOnAttack = true;
            
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
            //int i = 0;           
            if (targetPlayer == null || playerStatsUI ==null)
                return;
            foreach (CardInstance c in targetPlayer.fieldCard)
            {
                GameObject cardParentObj;
                GameObject headObj;
                //Card is on attack
                if (c.gameObject.GetComponentInParent<Area>() == null || c.transform.parent.name.Contains("BattleLine"))
                {
                    cardParentObj = c.transform.parent.gameObject; //BattleLine
                    headObj = cardParentObj.transform.parent.gameObject;// BattleLineObjs
                }
                else
                {
                    cardParentObj = c.gameObject.GetComponentInParent<Area>().gameObject;
                    headObj = cardParentObj.transform.parent.gameObject;
                    //for (int t = 0; t < headObj.transform.childCount; t++)
                    //{
                    //    if (cardParentObj == headObj.transform.GetChild(t).gameObject)
                    //        i = t;
                    //}
                    Setting.SetParentForCard(c.viz.gameObject.transform, c.GetOriginFieldLocation().transform);
                }
            }
            ///This function is to arrange cards on player's handgrid.
            ///However I have 'CardH andOrder' function for card arrangement.
            foreach (CardInstance c in targetPlayer.handCards)
            {
                Setting.SetParentForCard(c.viz.gameObject.transform, handGrid.value.transform);
            }
            foreach(CardInstance c in targetPlayer.attackingCards)
            {
                //Debug.Log("attackingCardsCheck");
                SetCardOnBattleLine(c);
            }

        }
    }
}
