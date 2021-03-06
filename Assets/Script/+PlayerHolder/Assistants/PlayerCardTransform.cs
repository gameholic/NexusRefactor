﻿using UnityEngine;
using System.Collections;
using GH.GameElements;
using GH.GameCard;
using GH.GameCard.CardLogics;

namespace GH.Player.Assists
{
    [CreateAssetMenu(menuName ="PlayerData/CardTransform")]
    public class PlayerCardTransform : PlayerAssists
    {
#pragma warning  disable 0649


        [SerializeField]
        private TransformVariable _Graveyard;
        [SerializeField]
        private TransformVariable[] _FieldGrid;
        [SerializeField]
        private TransformVariable _AttackingLine;
        [SerializeField]
        private TransformVariable _DefendingLine;
        [SerializeField]
        private TransformVariable _HandGrid;



#pragma warning restore 0649

        public TransformVariable AttackingLine
        { get { return _AttackingLine; } }
        public TransformVariable DefendingLine
        { get { return _DefendingLine; } }
        public TransformVariable HandGrid
        { get { return _HandGrid; } }
        public override void Init(PlayerHolder p)
        {
            player = p;
        }

        public TransformVariable Graveyard
        {
            get { return _Graveyard; }
        }
        public TransformVariable GetFieldGrid(int i)
        {
            return _FieldGrid[i];
        }
        public TransformVariable[] GetFieldGrid()
        {
            return _FieldGrid;
        }

        /// <summary>
        /// Set card to 'BattleLine' Obj
        /// This need to get changed. There should be a battle line, 
        /// but card should be placed at little towards enemy from original position
        /// </summary>
        /// <param name="card"></param>
        public void SetCardOnBattleLine(CreatureCard card)
        {
            Vector3 position = card.PhysicalCondition.gameObject.transform.position;
            MoveCardInstance.SetCardsForAttack(card.PhysicalCondition.transform, AttackingLine.value);
           
        }

        public void SetCardBackToOrigin(Card card)
        {
            //Debug.LogFormat("{0} is going back to its original field location, {1}", card.Data.Name,
            //    card.GetOriginFieldLocation().transform.gameObject.name);


            MoveCardInstance.SetParentForCard(card.PhysicalCondition.transform, card.PhysicalCondition.GetOriginFieldLocation()); 
            //Replace card to original place
            //I think I need to find FieldArea gameobject and make it as parent of card. Should check this.
            
                                   
        }

        #region RemovedCodes

        ///// <summary>
        ///// LoadPlayer
        ///// Call player's card on hand, on field and stats
        ///// And move this card holder's object to dest
        ///// </summary>
        ///// <param name="targetPlayer"></param>
        ///// <param name="playerStatsUI"></param>
        //public void LoadPlayer(NewPlayerHolder targetPlayer, PlayerStatsUI playerStatsUI)
        //{
        //    //int i = 0;           
        //    if (targetPlayer == null || playerStatsUI ==null)
        //        return;
        //    foreach (CardInstance c in targetPlayer.fieldCard)
        //    {
        //        GameObject cardParentObj;
        //        GameObject headObj;
        //        //Card is on attack
        //        if (c.gameObject.GetComponentInParent<Area>() == null || c.transform.parent.name.Contains("BattleLine"))
        //        {
        //            cardParentObj = c.transform.parent.gameObject; //BattleLine
        //            headObj = cardParentObj.transform.parent.gameObject;// BattleLineObjs
        //        }
        //        else
        //        {
        //            cardParentObj = c.gameObject.GetComponentInParent<Area>().gameObject;
        //            headObj = cardParentObj.transform.parent.gameObject;
        //            //for (int t = 0; t < headObj.transform.childCount; t++)
        //            //{
        //            //    if (cardParentObj == headObj.transform.GetChild(t).gameObject)
        //            //        i = t;
        //            //}
        //            Setting.SetParentForCard(c.viz.gameObject.transform, c.GetOriginFieldLocation().transform);
        //        }
        //    }
        //    ///This function is to arrange cards on player's handgrid.
        //    ///However I have 'CardH andOrder' function for card arrangement.
        //    foreach (CardInstance c in targetPlayer.handCards)
        //    {
        //        Setting.SetParentForCard(c.viz.gameObject.transform, handGrid.value.transform);
        //    }
        //    foreach(CardInstance c in targetPlayer.attackingCards)
        //    {
        //        //Debug.Log("attackingCardsCheck");
        //        SetCardOnBattleLine(c);
        //    }

        //}
        #endregion

    }
}
