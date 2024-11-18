﻿// AUTOGENERATED CODE //
using UnityEditor;

namespace FGMath
{

public static class GeneratedMenuItems
{

    static readonly string[] guids = new string[]
    {
        "Assets/CardSets/Coffee Shop (Example)/Coffee Shop.asset",
    };

    [MenuItem("FG Card Game/Choose Card Set/Coffee Shop")]
    private static void MenuItemCoffeeShop()
    {
        CardSetMenu.SetAddressableLabelToActive("Assets/CardSets/Coffee Shop (Example)/Coffee Shop.asset", guids);
        CardSetMenu.LoadCardSetFromPath("Assets/CardSets/Coffee Shop (Example)/Coffee Shop.asset");
    }


}
}
