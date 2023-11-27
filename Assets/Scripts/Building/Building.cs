using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptable Objects/Building")]
public class Building : ScriptableObject
{
    public Sprite sprite;
    public Resource resource;
    public int resourceIncrease;
    public float timeBetweenIncreases;
    public int money;
    public int stone;
    public int wood;
}
