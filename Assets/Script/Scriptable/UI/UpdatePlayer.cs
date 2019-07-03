using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;
using UnityEngine.UI;

namespace GH.UI
{
    public class UpdatePlayer : MonoBehaviour
    {
        public TextMesh targetText;
       
        public void UpdatePlayerText(PlayerHolder p)
        {
            targetText.text = p.player;  
        }
    }
}
