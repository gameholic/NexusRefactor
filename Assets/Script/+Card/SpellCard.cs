using UnityEngine;
using UnityEditor;


namespace GH.GameCard
{
    public class SpellCard : Card
    {
        public override void Init(GameObject go)
        {

        }
        public override bool CanDropCard()
        {
            Debug.Log("This is Spell Card. Can't be dropped");
            return false;
        }

        public override bool CanUseCard()
        {
            bool ret = false;
            return ret;
        }


        public override bool UseCard()
        {

            bool ret = false;

            return ret;
        }
    }
}