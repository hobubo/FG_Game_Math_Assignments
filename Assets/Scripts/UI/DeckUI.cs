using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FGMath
{
    public class DeckUI : MonoBehaviour
                , IPointerClickHandler
    {
        [SerializeField] private TextMeshPro _countDisplay;

        void OnEnable()
        {
            GameManager.OnDrawNewHand += UpdateDisplay;
            GameManager.OnShuffleDiscardIntoDeck += UpdateDisplay;
            GameManager.OnCardBought += UpdateDisplay;
            UpdateDisplay();
        }

        void OnDisable()
        {
            GameManager.OnDrawNewHand -= UpdateDisplay;
            GameManager.OnShuffleDiscardIntoDeck -= UpdateDisplay;
            GameManager.OnCardBought -= UpdateDisplay;
        }

        private void UpdateDisplay()
        {
            _countDisplay.text = GameManager.Deck.Count.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GameManager.DrawNewHand();
        }
    }
}