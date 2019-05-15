﻿using UnityEngine;
using System.Collections;
namespace GH
{

    [CreateAssetMenu(menuName = "Card Type/Creature")]
    public class Creature : CardType
    {
        public override void OnSetType(CardViz viz)
        {
            base.OnSetType(viz);

            viz.statsHolder.SetActive(true);
        } 
    }

}