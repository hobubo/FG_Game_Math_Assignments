using TMPro;
using UnityEngine;

namespace FGMath
{
    public class DiscardUI : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _countDisplay;

        void OnEnable()
        {
            GameManager.OnDrawNewHand += UpdateDisplay;
            GameManager.OnShuffleDiscardIntoDeck += UpdateDisplay;
            GameManager.OnCardBought += UpdateDisplay;
            GameManager.discardUI = this;

            UpdateDisplay();
        }

        void OnDisable()
        {
            GameManager.OnDrawNewHand -= UpdateDisplay;
            GameManager.OnShuffleDiscardIntoDeck -= UpdateDisplay;
            GameManager.OnCardBought -= UpdateDisplay;
            if(GameManager.discardUI == this) GameManager.discardUI = null;

        }

        private void UpdateDisplay()
        {
            _countDisplay.text = GameManager.Discard.Count.ToString();
        }
    }
}