using UnityEngine;



namespace GH.GameCard.CardLogics
{
    public class MovePosition : MonoBehaviour
    {
        private static GameController gameController = Setting.gameController;

        public static void DropCreatureCard(Transform cardTransform, Transform fieldTransform, Card card)
        {
            Debug.LogFormat("DropCreatureCard: Field Transform is {0}", fieldTransform);
            SetParentForCard(cardTransform, fieldTransform);
            card.Instance.CanUseByViz(false);
            card.Instance.SetAttackable(false);
            card.Instance.gameObject.SetActive(true);
            card.Instance.SetOriginFieldLocation(fieldTransform.transform);
            gameController.CurrentPlayer.PayMana(card);
            gameController.CurrentPlayer.DropCardOnField(card.Instance);

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
        public static void SetCardsForBlock(CardInstance defendCardInst, CardInstance attackCardInst, int count)
        {
            Transform defendCardTransform = defendCardInst.transform;
            Transform attackCardTransform = attackCardInst.transform;
            Vector3 blockPosition = attackCardTransform.position;
            Transform defendLine = defendCardInst.owner._CardHolder.defenceLine.value;


            blockPosition.x += 3 * count;
            blockPosition.z -= 3;
            Debug.LogFormat("Attacking card transform: {0}, Defending card transform: {1}, Blocking position: {2}"
                , attackCardTransform.position, defendCardTransform.position, blockPosition);
            SetParentForCard(defendCardTransform, attackCardTransform, blockPosition, defendLine);
        }

    }

}
