using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GH
{
    [CreateAssetMenu(menuName ="Cards/Weapon")]
    public class WeaponCard : CardType
    {
        public override void OnSetType(CardViz viz)
        {
            base.OnSetType(viz);

            viz.statsHolder.SetActive(true);
            viz.weaponHolder.SetActive(false);

        }
    }
}
