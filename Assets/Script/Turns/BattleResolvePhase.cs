using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH.GameCard;
using GH.Setup;

namespace GH.GameTurn
{
    [CreateAssetMenu(menuName ="Turns/Battle Resolve")]
    public class BattleResolvePhase : Phase
    {
        public Element elementAttack;
        public Element elementHealth;


        public override bool IsComplete()
        {
            GameController gc = Setting.gameController;
            //Debug.Log("BattleResolvePhase IsComplete");
            PlayerHolder p = Setting.gameController.CurrentPlayer;
            PlayerHolder e = Setting.gameController.GetOpponentOf(p);

            if (p == e)
                Debug.LogError("p = = e ");
            if (p.attackingCards.Count == 0)
            {
                //Debug.Log("BattleResolvePhase forceExit");
                return true;

            }

            Dictionary<CardInstance, BlockInstance> defDic = gc.BlockManager.BlockInstDict;


            for (int i = 0; i < p.attackingCards.Count; i++)
            {
                CardInstance inst = p.attackingCards[i];
                Card c = inst.viz.card;
                CardProperties attack = c.GetProperties(elementAttack);
                if (attack == null)
                {
                    Setting.RegisterLog("Attack of the " + c.name + "is null. This card can't attack", Color.red);
                    continue;
                }
                int attackValue  = attack.intValue;
                BlockInstance bi = gc.BlockManager.GetBlockInstanceOfAttacker(inst, defDic);
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
                            //Debug.Log("defendcard dead");
                            bi.defenders[i].CardInstanceToGrave();
                        }
                    }
                }
                if(attackValue <= 0)
                {
                    //Debug.Log("AttackCard dead");
                    attackValue = 0;
                    inst.CardInstanceToGrave();
                }
                else
                {
                    p.DropCardOnField(inst, false);
                    p._CardHolder.SetCardDown(inst);
                    inst.ColorCard(true);
                }
                ////////
                Setting.RegisterLog("Attack damage is "+ attackValue, Color.red);
                e.DoDamage(attackValue);
            }

            Dictionary <CardInstance, BlockInstance> blockInstances = gc.BlockManager.BlockInstDict;


            foreach (CardInstance c in blockInstances.Keys)
            {
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
                Setting.SetParentForCard(c.transform, c.GetOriginFieldLocation());
            }

            gc.BlockManager.ClearBlockInst();
            p.attackingCards.Clear();
            return true;

        }
        /*
        BlockInstance GetBlockInstanceOfAttacker(CardInstance attck, Dictionary<CardInstance, BlockInstance> blockInst)
        {
            BlockInstance r = null;
            blockInst.TryGetValue(attck, out r);
            return r;

        }*/

        public override void OnEndPhase()
        {

        }

        public override void OnStartPhase()
        {


        }
    }
}
