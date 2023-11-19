using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmyPattern_", menuName = "Scriptable Objects/ArmyPatterns")]
public class ArmyPatternSO : ScriptableObject
{
    public List<Vector2> positions;    
}
