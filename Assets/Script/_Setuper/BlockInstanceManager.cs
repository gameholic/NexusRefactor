using UnityEngine;
using System.Collections.Generic;
using GH.GameCard;
using GH;
using System;

namespace GH.Setup
{
    public class BlockInstanceManager 
    {

        private Dictionary<CardInstance, BlockInstance> _BlockInstDic;
        public Dictionary<CardInstance,BlockInstance> BlockInstDict
        {
            set { _BlockInstDic = value; }
            get { return _BlockInstDic; }
        }

        public void ClearBlockInst()
        {
            BlockInstDict.Clear();
        }
        public void AddBlockInstance(CardInstance attk, CardInstance def, ref int count)
        {

            BlockInstance b = null;
            b = GetBlockInstanceOfAttacker(attk);
            if (b == null)
            {
                b = new BlockInstance();
                b.attacker = attk;

                BlockInstDict.Add(attk, b);
            }

            if (!b.defenders.Contains(def))
            {
                b.defenders.Add(def);
            }
            count = b.defenders.Count;
        }
        public BlockInstance GetBlockInstanceOfAttacker(CardInstance attck)
        {
            BlockInstance r = null;
            BlockInstDict.TryGetValue(attck, out r);
            return r;
        }
        public BlockInstance GetBlockInstanceOfAttacker(CardInstance attck, Dictionary<CardInstance, BlockInstance> blockInst)
        {
            BlockInstance r = null;
            blockInst.TryGetValue(attck, out r);
            return r;
        }

        public static implicit operator Dictionary<object, object>(BlockInstanceManager v)
        {
            throw new NotImplementedException();
        }
    }
}