using UnityEngine;
using System.Collections.Generic;
using GH.GameCard;
using GH;
using System;


//This Class needs Explanations and logs
namespace GH.Setup
{
    public class BlockInstanceManager 
    {

        private Dictionary<CardInstance, BlockInstance> _BlockInstDic;
        /// <summary>
        /// BlockInstance has list of defenders with one attackers.
        /// Use 'CardInstance' to get 'BlockInstance'
        /// </summary>
        public Dictionary<CardInstance,BlockInstance> BlockInstDict
        {
            set { _BlockInstDic = value; }
            get { return _BlockInstDic; }
        }

        public void ClearBlockInst()
        {
            BlockInstDict.Clear();
        }
        /// <summary>
        /// Add attacking and defending card instances in dictionary.
        /// This data is used in 'BattlePhaseResolve' phase
        /// </summary>
        /// <param name="attk">Attacking card instance</param>
        /// <param name="def">Defending card instance</param>
        /// <param name="count">Number of 'def' card instances for 'attk' </param>
        public void AddBlockInstance(CardInstance attk, CardInstance def, ref int count)
        {

            BlockInstance b = null;

            //Check if there is same attacking card instance
            //If 'attk' is new attacking card instance, make new 'BlockInstance' for this card.

            b = SearchBlockInstanceOfAttacker(attk);
            if (b == null)
            {
                b = new BlockInstance();
                b.attacker = attk;
                BlockInstDict.Add(attk, b);
                Debug.Log("BlockInstDic added in dictionary.");
            }
            else
            {
                Debug.LogWarningFormat("BlockInst For Attacker, {0} AlreadyExist", attk.viz.card.name);
            }
            //If 'def' isn't in the 'defenders' of the BlockInstance 'b', add it in list
            if (!b.defenders.Contains(def))
            {
                b.defenders.Add(def);                
            }
            count = b.defenders.Count;
            Debug.LogErrorFormat("Current Blocking Card Count for {0}  is {1}",b.attacker.viz.card.name, count);
        }
        public bool SearchBlockInstanceOfDefender(CardInstance defend)
        {
            bool result = false;
            foreach (BlockInstance bi in Setting.gameController.BlockManager.BlockInstDict.Values)
            {
                foreach (CardInstance tmp in bi.defenders)
                {
                    if(tmp == defend)
                    {
                        result = true;
                    }
                }

            }
            return result;
        }

        public BlockInstance SearchBlockInstanceOfAttacker(CardInstance attck)
        {
            BlockInstance r = null;
            BlockInstDict.TryGetValue(attck, out r);
            return r;
        }

        /// <summary>
        /// Search and return BlockInstance in 'blockInst' using 'attck' 
        /// </summary>
        /// <param name="attck">Key to find BlockInstance</param>
        /// <param name="dic">Dictionary for BlockInstances</param>
        /// <returns></returns>
        public BlockInstance GetBlockInstanceByAttacker(CardInstance attck, Dictionary<CardInstance, BlockInstance> dic)
        {
            //Debug.LogFormat("GetBlockInstanceByAttacker: {0} try to search {1} in Block Instance", attck.owner.player, attck.viz.card.name);
            BlockInstance r = null;
            dic.TryGetValue(attck, out r);
            return r;
        }

        //public static implicit operator Dictionary<object, object>(BlockInstanceManager v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}