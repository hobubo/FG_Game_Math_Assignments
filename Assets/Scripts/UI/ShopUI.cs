

using UnityEngine;
using UnityEngine.EventSystems;

namespace FGMath
{

public class ShopUI : CardGroup
{
    void OnEnable()
    {
        GameManager.OnRefreshShop += RefreshShop;
    }

    void OnDisable()
    {
        GameManager.OnRefreshShop -= RefreshShop;
    }

    public void TestShop()
    {
        GameManager.RefreshShop();
    }

    void RefreshShop()
    {
        UpdateCards(GameManager.ShopContents);
    }

    public override void OnPointerClick(PointerEventData eventData, CardUI cardUI)
    {
        GameManager.BuyFromShop(cardUI.CardData);
        cardUI.CardData = null;
    }
}
}