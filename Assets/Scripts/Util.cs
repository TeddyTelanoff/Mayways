using UnityEngine;

public class Util
{
    public static bool HasComponent<T>(this Component obj) =>
        obj.TryGetComponent(out T unused);
}