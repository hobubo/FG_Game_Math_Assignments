using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FGMath
{

[Serializable]
public abstract class CardGroup : MonoBehaviour
{
    [SerializeField] protected GameObject _prefab;
    [SerializeField] Vector3 _selectOffset;
    protected int _count;
    public List<CardUI> _cards = new();

    public void UpdateCards(List<CardDataSO> newContents)
    {
        _count = newContents.Count;
        ResizeGroup();
        for(int i = 0; i < _count; ++i)
        {
            _cards[i].CardData = newContents[i];
        }
    }
    public void ResizeGroup()
    {
        for(int i = _count; i < _cards.Count; ++i)
        {
            Destroy(_cards[i].gameObject);
            _cards.RemoveAt(i);
        }
        for(int i = _cards.Count; i < _count; ++i)
        {
            var newCard = Instantiate(_prefab, transform);
            newCard.name = "Card "+(i+1);
            _cards.Add(newCard.GetComponent<CardUI>());
        }
        RepositionCards();
    }

    public void RepositionCards()
    {
        for(int i = 0;i < _cards.Count; ++i)
        {
            _cards[i].SetTargetPseudoTransform(GetPseudoTransform(i, false));
        }
    }

    public virtual PseudoTransform GetPseudoTransform(CardUI cardUI, bool isSelected)
    {
        int idx = _cards.FindIndex(0, _cards.Count, (value) => cardUI == value);
        return GetPseudoTransform(idx, isSelected);
    }

    public virtual PseudoTransform GetPseudoTransform(int idx, bool isSelected)
    {
        Assignment1.Input input;
        input.handPosition = transform.position;
        input.handRotation = transform.rotation;
        input.cardCount = _cards.Count;
        input.cardIdx = idx;
        input.cardDimensions = new Vector2(2.5f, 3.5f);
        input.selectedOffset = _selectOffset;

        return Assignment1.GetCardPosition(input, isSelected);
    }
    
    public abstract void OnPointerClick(PointerEventData eventData, CardUI cardUI);
}
}