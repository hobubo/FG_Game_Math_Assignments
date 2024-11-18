
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FGMath
{

public class HandUI : CardGroup
{

    [SerializeField] PlaySpaceUI _playSpaceUI;
    [SerializeField] DeckUI _deckUI;
    void OnEnable()
    {
        GameManager.OnEndTurn += EndTurn;
        GameManager.OnDrawNewHand += DrawNewHand;
    }

    void OnDisable()
    {
        GameManager.OnEndTurn -= EndTurn;
        GameManager.OnDrawNewHand -= DrawNewHand;
    }

    public void EndTurn(float duration)
    {
		_playSpaceUI.ReturnCardsTo(this);
        float delay = duration * Global.Data.EndTurnLagFactor;
        for(int i = 0; i < _cards.Count; ++i)
        {
            var card = _cards[i];
            var d = delay*i;
            card.GoToDiscard(duration, d);
        }
    }

    private void DrawNewHand()
    {
		foreach (var card in _cards)
		{
			card.transform.SetPositionAndRotation(_deckUI.transform.position, _deckUI.transform.rotation);
			card.transform.localScale = Vector3.zero;

			// card.SetTargetPseudoTransform(new(_deckUI.transform));
		}
        UpdateCards(GameManager.Hand);
    }

    public override void OnPointerClick(PointerEventData eventData, CardUI cardUI)
    {
        _playSpaceUI.TakeCard(cardUI);
        RepositionCards();
    }

	internal void ReturnCards(List<CardUI> cards)
	{
		//@BUG: We don't do any checks for duplicates.
		var cardsCopy = new List<CardUI>(cards);
		foreach(var card in cardsCopy)
		{
			card.MoveToNewGroup(this);
		}
	}
}
}