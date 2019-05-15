using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH.UI;
using UnityEngine.UI;

namespace GH
{
    public class UpdateTextMeshFromPhase : UIPropertyUpdater
    {
        public PhaseVariable currentPhase;
        public TextMesh targetText;

        /// <summary>
        /// Use this to update a text UI element based on the target string variable
        /// </summary>
        public override void Raise()
        {
            targetText.text = currentPhase.value.phaseName;
        }

    }

}