using UnityEngine;
using UnityEngine.UI;

namespace DeadSurvive.UnitButton
{
    public class ButtonView : MonoBehaviour
    {
        public Button Button => _button;

        public Text Text => _text;
        
        [SerializeField] 
        private Button _button;
        [SerializeField]
        private Text _text;
    }
}