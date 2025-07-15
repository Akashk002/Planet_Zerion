using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IInteractable
{
    [SerializeField] private BuildingType buildingType;
    [SerializeField] private GameObject roomPanel;

    public void Interact()
    {

    }
    public string GetName()
    {
        return SplitCamelCase(buildingType.ToString());
    }

    public static string SplitCamelCase(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(
            input,
            "(\\B[A-Z])",
            " $1"
        );
    }

    public void TakeDamage(float damage)
    {
        GameService.Instance.buildingManager.AddDamage((int)damage);
    }
}
