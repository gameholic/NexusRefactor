using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace GH
{
    public class BlockInstance
    {
        public CardInstance attacker;
        public List<CardInstance> defenders = new List<CardInstance>();
        //Multiple units can defend one attacker


    }

}