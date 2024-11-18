
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FGMath
{
public static class Global
{
    static public GlobalSO Data {get; private set;}
    static Global()
    {
        var async = Addressables.LoadAssetAsync<GlobalSO>("Assets/Globals.asset");
        Data = async.WaitForCompletion();
#if UNITY_INCLUDE_TESTS
        if (Data) Data = ScriptableObject.Instantiate(Data);
#endif
    }

    public static void Set(GlobalSO newData)
    {
        Data = newData;
    }
}
}