using System;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public FollowAxis Axis;
    public Transform Object;
    public Vector3 Offset;
    public FollowBounds Bounds;
    public float Smooth = 0.75f;

    private void FixedUpdate()
    {
        if (Object is null)
            return;

        var pos = transform.position + Offset;
        if (Axis.HasFlag(FollowAxis.X))
        {
            pos.x = Object.position.x + Offset.x;
            pos.x = Mathf.Clamp(pos.x, Bounds.Min.x, Bounds.Max.x);
        }
        if (Axis.HasFlag(FollowAxis.Y))
        {
            pos.y = Object.position.y + Offset.y;
            pos.y = Mathf.Clamp(pos.y, Bounds.Min.y, Bounds.Max.y);
        }
        if (Axis.HasFlag(FollowAxis.Z))
        {
            pos.z = Object.position.z + Offset.z;
            pos.z = Mathf.Clamp(pos.z, Bounds.Min.z, Bounds.Max.z);
        }

        transform.position = Vector3.Lerp(transform.position, pos, Smooth);
    }
}

[Flags]
public enum FollowAxis
{
    None = 0,
    X = 1 << 0,
    Y = 1 << 1,
    Z = 1 << 3,
}

[Serializable]
public class FollowBounds
{
    public Vector3 Min = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
    public Vector3 Max = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
}