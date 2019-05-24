using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH.GameCard;

namespace GH.GameTurn
{
    [CreateAssetMenu(menuName ="Turns/Battle Resolve")]
    public class BattleResolvePhase : Phase
    {
        public Element elementAttack;
        public Element elementHealth;


        public override bool IsComplete()
        {
            //Debug.Log("BattleResolvePhase IsComplete");
            PlayerHolder p = Setting.gameController.currentPlayer;
            PlayerHolder e = Setting.gameController.GetOpponentOf(p);


            if (p == e)
                Debug.LogError("p = = e ");
            if (p.attackingCards.Count == 0)
            {
                //Debug.Log("BattleResolvePhase forceExit");
                return true;

            }

            Dictionary<CardInstance, BlockInstance> defDic = Setting.gameController.GetBlockInstances();


            for (int i = 0; i < p.attackingCards.Count; i++)
            {
                //Debug.Log("BattleResolvePhase Check attackingCards");
                CardInstance inst = p.attackingCards[i];
                Card c = inst.viz.card;
                CardProperties attack = c.GetProperties(elementAttack);
                if (attack == null)
                {
                    Setting.RegisterLog("Attack of the " + c.name + "is null. This card can't attack", Color.red);
                    continue;
                }

                int attackValue  = attack.intValue;


                BlockInstance bi = GetBlockInstanceOfAttacker(inst, defDic);
                if(bi!=null)
                {
                    for(int defenders =0; defenders < bi.defenders.Count; defenders++)
                    {
                        CardProperties def = c.GetProperties(elementHealth);
                        if (def==null)
                        {
                            Debug.LogWarning("You are trying to block with a card with no def element");
                            continue;
                        }

                        attackValue -= def.intValue;

                        if(def.intValue <= attackValue)
                        {
                            Debug.Log("defendcard dead");
                            //defend card Die
                            bi.defenders[i].CardInstanceToGrave();
                        }
                    }
                }

                if(attackValue <= 0)
                {
                    //Attack Card Die
                    attackValue = 0;
                    inst.CardInstanceToGrave();
                }
                else
                {
                    p.DropCardOnField(inst, false);
                    p.currentCardHolder.SetCardDown(inst);
                    inst.ColorCard(true);

                }
                ////////
                Setting.RegisterLog("Attack damage is "+ attackValue, Color.red);
                e.DoDamage(attackValue);
            }
            Dictionary <CardInstance, BlockInstance> blockInstances = Setting.gameController.GetBlockInstances();


            foreach (CardInstance c in blockInstances.Keys)
            {
                //Debug.Log("Blocked card :" + c.viz.card);
                if (c.dead)
                {
                    Debug.Log("this card is dead");
                    break;
                }
                c.ColorCard(false);
                Setting.SetParentForCard(c.transform, c.GetOriginFieldLocation());
            }

            foreach (CardInstance c in e.fieldCard)
            {
                //Bug: Defender's card should go into blockdic.
                Debug.Log("enemy player's attacking card :" + c.viz.card);
                Setting.SetParentForCard(c.transform, c.GetOriginFieldLocation());
            }

            Setting.gameController.ClearBlockInstances();
            p.attackingCards.Clear();
            return true;

        }

        BlockInstance GetBlockInstanceOfAttacker(CardInstance attck, Dictionary<CardInstance, BlockInstance> blockInst)
        {
            BlockInstance r = null;
            blockInst.TryGetValue(attck, out r);
            return r;

        }

        public override void OnEndPhase()
        {

        }

        public override void OnStartPhase()
        {
          
        }
    }
}
