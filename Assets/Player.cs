using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public CellType _cellType = CellType.Cross;
    public bool _isHuman = true;

    public Player(CellType cellType)
    {
        _cellType = cellType;
    }
}
