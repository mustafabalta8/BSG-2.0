using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Employee Place Shower")]
public class Emp_Place_Points : ScriptableObject
{
    [SerializeField] GameObject pointPrefab;

    public List<Transform> GetPoints()
    {
        var WAyPoints = new List<Transform>();
        foreach (Transform child in pointPrefab.transform)
        {
            WAyPoints.Add(child);
        }

        return WAyPoints;
    }

}
