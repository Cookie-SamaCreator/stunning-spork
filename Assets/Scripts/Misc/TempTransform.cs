using UnityEngine;

/// <summary>
/// Utility class for creating and automatically destroying a temporary Transform.
/// </summary>
public class TempTransform : System.IDisposable
{
    public Transform Transform { get; }
    private readonly GameObject _gameObject;

    public TempTransform(Vector3 position)
    {
        _gameObject = new GameObject("MagicAttackTarget");
        _gameObject.transform.position = position;
        Transform = _gameObject.transform;
    }

    public void Dispose()
    {
        Object.Destroy(_gameObject);
    }
}