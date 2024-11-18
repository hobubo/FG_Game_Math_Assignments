using System.Collections.Generic;
using UnityEngine;

namespace FGMath
{
    [RequireComponent(typeof(Camera))]
    public class AutoFOV : MonoBehaviour
    {
        
        private Camera _camera;
        private readonly List<Transform> visibles = new();
        [SerializeField] float _margin = 2.0f;

        void Awake()
        {
            _camera = GetComponent<Camera>();
            PopulateVisibles<DeckUI>();
            PopulateVisibles<CardGroup>();
            PopulateVisibles<DiscardUI>();
        }

        void PopulateVisibles<T>() where T : MonoBehaviour
        {
            foreach(var obj in FindObjectsByType<T>(FindObjectsSortMode.None))
            {
                visibles.Add(obj.transform);
            }
        }

        void LateUpdate()
        {
            if(visibles.Count == 0) return;
            float maxTheta = float.MinValue;
            foreach (var visible in visibles)
            {
                var camSpacePos = _camera.transform.InverseTransformPoint(visible.position);
                float theta = Mathf.Acos(Vector3.Dot(Vector3.forward, camSpacePos.normalized));
                theta += Mathf.Asin( _margin / camSpacePos.z);
                maxTheta = Mathf.Max(theta, maxTheta);
            }

            _camera.fieldOfView = maxTheta*Mathf.Rad2Deg;
        }
    }
}