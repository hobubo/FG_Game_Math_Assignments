
using UnityEditor;

namespace FGMath
{

class GlobalSetter : EditorWindow
{

    [MenuItem("FG Card Game/Global Setter")]
    public static void ShowWindow()
    {
        GetWindow<GlobalSetter>("Globals");
    }
    
    void OnGUI()
    {
        Global.Data.LerpSmoothDecay = EditorGUILayout.Slider(
            "Card Interp Speed", Global.Data.LerpSmoothDecay, 1.0f, 30.0f);
    }
}
}