using System;
using UnityEngine;

public class Follow2D : MonoBehaviour
{
    public FollowAxis2D Axis;
    public Transform Object;
    public Vector2 Offset;
    public FollowBounds2D Bounds;
    public float Smooth = 0.75f;

    private void FixedUpdate()
    {
        if (Object is null)
            return;

        var pos = transform.position + (Vector3)Offset;
        if (Axis.HasFlag(FollowAxis2D.X))
        {
            pos.x = Object.position.x + Offset.x;
            pos.x = Mathf.Clamp(pos.x, Bounds.Min.x, Bounds.Max.x);
        }
        if (Axis.HasFlag(FollowAxis2D.Y))
        {
            pos.y = Object.position.y + Offset.y;
            pos.y = Mathf.Clamp(pos.y, Bounds.Min.y, Bounds.Max.y);
        }

        transform.position = Vector3.Lerp(transform.position, pos, Smooth);
    }
}

[Flags]
public enum FollowAxis2D
{
    None = 0,
    X = 1 << 0,
    Y = 1 << 1,
}

[Serializable]
public class FollowBounds2D
{
    public Vector2 Min = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
    public Vector2 Max = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
}