using UnityEngine;

public class ArmyPatternBuilder : MonoBehaviour
{
    [SerializeField] ArmyPatternSO armyPattern;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            armyPattern.positions.Add(HelperUtilities.GetMouseWorldPosition());
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.X))
        {
            armyPattern.positions.RemoveAt(armyPattern.positions.Count - 1);
        }
    }

    private void OnDrawGizmos()
    {
        if (armyPattern.positions.Count > 1)
        {
            for (int i = 0; i < armyPattern.positions.Count - 1; i++)
            {
                Gizmos.DrawLine(armyPattern.positions[i], armyPattern.positions[i + 1]);
            }
        }
    }
}
