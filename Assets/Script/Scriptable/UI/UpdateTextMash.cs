using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;
using UnityEngine.UI;

namespace GH.UI
{
    public class UpdateTextMash : UIPropertyUpdater
    {
        public StringVariable targetString;
        public TextMesh targetText;
        
        /// <summary>
        /// Use this to update a text UI element based on the target string variable
        /// </summary>
        public override void Raise()
        {
            targetText.text = targetString.value;
        }
        
        public void Raise(string target)
        {
            targetText.text = target;
        }
    }
}
