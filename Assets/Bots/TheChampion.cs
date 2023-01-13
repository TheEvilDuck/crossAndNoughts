using UnityEngine;
using System.Collections.Generic;
using System;
[CreateAssetMenu(menuName ="Bots/Champion bot")]
public class TheChampion : Bot
{
    private CellType _cellType;
    private Vector2Int _lastDecision;
    public override Vector2Int GetBotDecision(CellType[,] field,int inARowToWin)
    {
        Array AllCellTypes = Enum.GetValues(typeof(CellType));
        int fieldSize = field.GetLength(0);
        List<Vector2Int>possibleDecisions = new List<Vector2Int>();
        List<Vector2Int>mainDecisions = new List<Vector2Int>();
        List<Vector2Int>notTooGoodDecisions = new List<Vector2Int>();
        List<Vector2Int>tryingToStartARowDecisions = new List<Vector2Int>();
        for (int x = 0;x<fieldSize;x++)
        {
            for (int y = 0;y<fieldSize;y++)
            {
                if (field[x,y]==CellType.Empty)
                    possibleDecisions.Add(new Vector2Int(x,y));
            }
        }
        if (_lastDecision!=null)
        {
            _cellType = field[_lastDecision.x,_lastDecision.y];
            foreach (Vector2Int possibleDecision in possibleDecisions)
            {
                foreach (CellType cellType in AllCellTypes)
                {
                    if (cellType == CellType.Empty)continue;
                    CellType[,]fieldCopy = new CellType[fieldSize,fieldSize];
                    fieldCopy = field.Clone() as CellType[,];
                    fieldCopy[possibleDecision.x,possibleDecision.y] = cellType;
                    if (CheckRow(possibleDecision.x,possibleDecision.y,cellType,fieldCopy,inARowToWin))
                    {
                        if (cellType==_cellType)
                            mainDecisions.Add(possibleDecision);
                        else
                            notTooGoodDecisions.Add(possibleDecision);
                    }
                }
                if ((possibleDecision-_lastDecision).magnitude<2)
                {
                    tryingToStartARowDecisions.Add(possibleDecision);
                }
        }
        }
        if (mainDecisions.Count>0)
        {
            return mainDecisions[UnityEngine.Random.Range(0,mainDecisions.Count-1)];
        }
        if (notTooGoodDecisions.Count>0)
        {
            return notTooGoodDecisions[UnityEngine.Random.Range(0,notTooGoodDecisions.Count-1)];
        }
        if (tryingToStartARowDecisions.Count>0)
        {
            return tryingToStartARowDecisions[UnityEngine.Random.Range(0,tryingToStartARowDecisions.Count-1)];
        }
        _lastDecision = possibleDecisions[UnityEngine.Random.Range(0,possibleDecisions.Count-1)];
        return _lastDecision;
    }
}
