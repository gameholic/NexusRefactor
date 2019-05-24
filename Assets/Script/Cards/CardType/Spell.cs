using UnityEngine;
using System.Collections;


namespace GH.GameCard
{

    [CreateAssetMenu(menuName = "Card Type/Spell")]
    public class Spell : CardType
    {
        public override void OnSetType(CardViz viz)
        {
            base.OnSetType(viz);

            viz.GetStatsHolder().SetActive(false);
        }
    }
}