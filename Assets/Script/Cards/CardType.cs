//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//namespace GH.GameCard
//{
//    public abstract class CardType : ScriptableObject
//    {

//        public string typeName;
//        public bool canAttack;
//        //public typeLogic logic
//        public virtual void OnSetType(CardViz viz)
//        {
//            Element t = Setting.GetResourceManager().typeElement;
//            CardVizProperties type = viz.GetProperty(t);
//            type.text.text = typeName;
//        }
//        public bool TypeAllowsAttack(CardInstance inst)
//        {
//            ///Cards with charge ability are allowed to attack even their 'isJustPlaced' is true;
//            ///Sample code (including typelogic above:
//            ///bool r = logic.Execute(inst) 
//            ///if(inst,isJustPlaced)
//            ///     inst.isJustPlaced = false 
//            ///return true;
//            if (canAttack)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//    }

//}