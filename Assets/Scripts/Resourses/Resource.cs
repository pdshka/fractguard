using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "Scriptable Objects/Resource")]
public class Resource : ScriptableObject
{
    public string resourceName;
    public Sprite sprite;
}
