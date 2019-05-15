using UnityEngine;
using System.Collections;


namespace GH
{

    [CreateAssetMenu(menuName = "Card Type/Spell")]
    public class Spell : CardType
    {
        public override void OnSetType(CardViz viz)
        {
            base.OnSetType(viz);

            viz.statsHolder.SetActive(false);
        }
    }
}