
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FGMath
{

public class PlaySpaceUI : CardGroup
{

    public Vector2 _playSpaceBounds = new(20.0f, 10.0f);
	[SerializeField] private HandUI _handUI;

    void OnEnable()
    {
        GameManager.OnEndTurn += EndTurn;
    }

    void OnDisable()
    {
        GameManager.OnEndTurn -= EndTurn;
    }

	public void ReturnCardsTo(HandUI hand)
	{
		hand.ReturnCards(_cards);
	}

    public void EndTurn(float duration)
    {
		_handUI.ReturnCards(_cards);
        float delay = duration * Global.Data.EndTurnLagFactor;
        for(int i = 0; i < _cards.Count; ++i)
        {
            var card = _cards[i];
            var d = delay*i;
            card.GoToDiscard(duration, d);
        }
        Invoke(nameof(ClearCards), duration);
    }

    private void ClearCards() => _cards.Clear();

    public void TakeCard(CardUI cardUI)
    {
        cardUI.MoveToNewGroup(this);
        RepositionCards();
    }



    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_playSpaceBounds.x, 1.0f, _playSpaceBounds.y ));
    }
    #endif

    public override PseudoTransform GetPseudoTransform(CardUI cardUI, bool isSelected)
    {
        var idx = _cards.FindIndex(0, _cards.Count, (value) => cardUI == value);
        return GetPseudoTransform(idx, isSelected);
    }

    public override PseudoTransform GetPseudoTransform(int idx, bool isSelected)
    {
        Assignment3.Input input;
        input.gridCellIdx = idx;
        input.playAreaCenterPosition = transform.position;
        input.playAreaDimensions = _playSpaceBounds;
        input.cardDimensions = new(2.5f, 3.5f);
        return Assignment3.GetGridCellPosition(input);
    }

    public override void OnPointerClick(PointerEventData eventData, CardUI cardUI){}


}
}