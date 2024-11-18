
using System;
using UnityEngine;

[Serializable]
public struct PseudoTransform
{
    [SerializeField] public Vector3 pos;
    [SerializeField] public Quaternion rot;
    [SerializeField] public Vector3 scale;

    public PseudoTransform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        pos = position;
        rot = rotation;
        this.scale = scale;
    }

    public PseudoTransform(Transform trans)
    {
        pos = trans.position;
        rot = trans.rotation;
        scale = trans.localScale;
    }

    public readonly void ApplyTo(in Transform trans)
    {
        trans.SetPositionAndRotation(pos, rot);
        trans.localScale = scale;
    }

}