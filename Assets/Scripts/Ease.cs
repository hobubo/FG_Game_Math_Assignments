using UnityEngine;


namespace FGMath
{
public static class Ease
{
    // Yoinked from Freya Holmér's amazing talk, go watch:
    // https://www.youtube.com/watch?v=LSNQuFEDOyQ
    public static PseudoTransform Approach(Transform obj, PseudoTransform target, float decay, float dt)
    {
        float dec = Mathf.Exp(-decay * dt);

        return new PseudoTransform(target.pos + (obj.position - target.pos)*dec,
        new Quaternion(
            target.rot.x + (obj.rotation.x - target.rot.x)*dec,
            target.rot.y + (obj.rotation.y - target.rot.y)*dec,
            target.rot.z + (obj.rotation.z - target.rot.z)*dec,
            target.rot.w + (obj.rotation.w - target.rot.w)*dec),
            target.scale + (obj.localScale - target.scale)*dec
        );
    }

    // From the interpolation lecture, takes the current value and converts it to a t from [0:1]
    public static float InverseLerp(float start, float end, float current)
    {
        return  (current - start) / (end - start);
    }

    // The cheaty way to InvLerp in 3 dimensions. We project onto a line and then lerp in one dimension.
    public static float InverseLerpProjected(Vector3 start, Vector3 end, Vector3 current)
    {
        end -= start;
        current -= start;
        return Vector3.Dot(current, end) / end.sqrMagnitude;
    }
}

}