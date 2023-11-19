using System;
using UnityEngine;

[DisallowMultipleComponent]
public class DestroyedEvent : MonoBehaviour
{
    public event Action<DestroyedEvent, DestroyedEventArgs> OnDestroyed;

    public void CallDestroyedEvent(int exp)
    {
        OnDestroyed?.Invoke(
            this,
            new DestroyedEventArgs()
            {
                experience = exp
            });
    }
}

public class DestroyedEventArgs : EventArgs
{
    public int experience; // here goes something given for killing enemy
}
