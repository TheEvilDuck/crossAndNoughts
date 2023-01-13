using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Cell/Create a new cell")]
public class CellVisual : ScriptableObject
{
    public string cellName;
    public CellType cellType;
    public Sprite sprite;
}
