using UnityEngine;

public static class Util
{
    public static bool HasComponent<T, C>(this C obj) where C: Component =>
        obj.TryGetComponent(out T _);
}