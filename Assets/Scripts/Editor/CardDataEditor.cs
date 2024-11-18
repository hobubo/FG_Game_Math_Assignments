using UnityEngine;
using UnityEditor;

namespace FGMath
{

[CustomEditor(typeof(CardDataSO))]
[CanEditMultipleObjects]
public class CardDataSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();

        var shopWeightProp = serializedObject.FindProperty("_shopWeight");
        EditorGUILayout.PropertyField(shopWeightProp);

        EditorGUILayout.BeginHorizontal();

        var qualities = GameManager.CardSet.Qualities.ConvertAll(quality => quality.name).ToArray();
        var qualityProp = serializedObject.FindProperty("_quality");
        var quality = qualityProp.objectReferenceValue as CardQualitySO;
        
        int selectedQuality = quality == null ? -1 : System.Array.IndexOf(qualities, quality.name);
        EditorGUILayout.PrefixLabel("Quality");
        int newQuality = EditorGUILayout.Popup(selectedQuality, qualities);

        if(newQuality != selectedQuality)
        {
            qualityProp.objectReferenceValue = GameManager.CardSet.Qualities[newQuality];
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        var resources = GameManager.CardSet.Resources.ConvertAll(resource => resource.name).ToArray();
        var resourceTypeProp = serializedObject.FindProperty("_costType");
        var resource = resourceTypeProp.objectReferenceValue as ResourceSO;


        int selectedResource = resource == null ? -1 : System.Array.IndexOf(resources, resource.name);

        EditorGUILayout.PrefixLabel("Cost", EditorStyles.label);
        var costAmountProp = serializedObject.FindProperty("_costAmount");
        costAmountProp.intValue = EditorGUILayout.IntField(costAmountProp.intValue);

        int newResource = EditorGUILayout.Popup(selectedResource, resources);
        if(newResource != selectedResource)
        {
            resourceTypeProp.objectReferenceValue = GameManager.CardSet.Resources[newResource];
        }

        EditorGUILayout.EndHorizontal();

        if(quality != null)
        {
            var textBoxes = serializedObject.FindProperty("_descriptions");
            if(textBoxes != null)
            {
                EditorGUILayout.PrefixLabel("Effect Descriptions");
                for (int i = 0; i < quality._effectChoices; i++)
                {
                    textBoxes.GetArrayElementAtIndex(i).stringValue = EditorGUILayout.TextArea(textBoxes.GetArrayElementAtIndex(i).stringValue, GUILayout.Height(80));
                }
            }
        }

        var imageProp = serializedObject.FindProperty("_image");
        EditorGUILayout.PropertyField(imageProp);
        
        var sprite = imageProp.objectReferenceValue as Sprite;
        var tex = AssetPreview.GetAssetPreview(sprite);
        if(tex != null)
        {
            GUI.DrawTexture(
                GUILayoutUtility.GetAspectRect((float)tex.width/tex.height),
                tex);
        }

        serializedObject.ApplyModifiedProperties();
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var sprite = serializedObject.FindProperty("_image").objectReferenceValue as Sprite;
        if(sprite == null)
        {
            return base.RenderStaticPreview(assetPath, subAssets, width, height);
        }
        return AssetPreview.GetAssetPreview(sprite);

    }
}
}