using UnityEngine;



namespace GH.GameCard.CardLogics
{
    public class MoveCardInstance
    {
        private static GameController gameController = Setting.gameController;
        public static void DropCreatureCard(Transform cardTransform, Transform fieldTransform, CreatureCard card)
        {
            Debug.LogFormat("DropCreatureCard: Field Transform is {0}", fieldTransform);
            SetParentForCard(cardTransform, fieldTransform);
            card.CardCondition.CanUse = false;
            card.PhysicalCondition.gameObject.SetActive(true);
            card.PhysicalCondition.SetOriginFieldLocation(fieldTransform.transform);
            gameController.CurrentPlayer.InGameData.ManaManager.UseMana(card.Data.ManaCost);
            gameController.CurrentPlayer.CardManager.DropCardOnField(card);

        }
         
        public static void SetParentForCard(Transform target, Transform dest)
        {
            target.SetParent(dest);
            if (dest.name != "BattleLine")
                target.rotation = dest.rotation;
            target.localPosition = Vector3.zero;
            target.localScale = dest.localScale;

        }
        /// <summary>
        /// This is for placing defending cards to attacking card
        /// </summary>
        /// <param name="defendCard"></param>
        /// <param name="attackingCard"></param>
        /// <param name="newLocalPosition"></param>
        /// <param name="newEuler"></param>
        public static void SetParentForCard(Transform defendCard, Transform attackingCard, Vector3 newLocalPosition, Transform defendLine)
        {
            defendCard.SetParent(defendLine);
            defendCard.rotation = attackingCard.rotation;
            defendCard.position = newLocalPosition;
            defendCard.localScale = attackingCard.localScale;
        }

        public static void SetCardsForAttack(Transform card, Transform battleLine)
        {
            Debug.LogFormat("SetCardsForAttack: {0} is moved to {1}", card.name, battleLine.name);
            card.SetParent(battleLine);
            card.position = new Vector3(card.position.x, card.position.y, card.position.z + ((battleLine.position.z - card.position.z) * 0.7f));
            card.localScale = battleLine.localScale;
        }
        /// <summary>
        /// This function only changes gameobject's location.
        /// Move 'defendingCard' position to near 'attackingCard'.
        /// </summary>
        /// <param name="defendCard"></param>
        /// <param name="attackingCard"></param>
        /// <param name="count"></param>
        public static void SetCardsForBlock(Card defendCardInst, Card attackCardInst, int count)
        {
            Transform defendCardTransform = defendCardInst.PhysicalCondition.transform;
            Transform attackCardTransform = attackCardInst.PhysicalCondition.transform;
            Vector3 blockPosition = attackCardTransform.position;
            Transform defendLine = defendCardInst.User.CardTransform.DefendingLine.value;


            blockPosition.x += 3 * count;
            blockPosition.z -= 3;
            Debug.LogFormat("Attacking card transform: {0}, Defending card transform: {1}, Blocking position: {2}"
                , attackCardTransform.position, defendCardTransform.position, blockPosition);
            SetParentForCard(defendCardTransform, attackCardTransform, blockPosition, defendLine);
        }
        
        public static void SetCardToGrave(Card inst)
        {
            GraveLogic grave = new GraveLogic();
            //card die
            Debug.LogFormat("SetCardToGrave: {0}'s {1} died", inst.User.PlayerProfile.Name, inst.Data.Name);
            inst.CardCondition.CanUse = false;
            grave.SetCardToGrave(inst);
        }


    }

}
