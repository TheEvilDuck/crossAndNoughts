using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(menuName ="Bots/Random difficulty 2 bot")]

public class RandomButTryToWin : Bot
{
    Array AllCellTypes = Enum.GetValues(typeof(CellType));
    public override Vector2Int GetBotDecision(CellType[,] field, int inARowToWin)
    {
        List<Vector2Int>possibleDecisions = GetPossibleDecisions(field);
        List<Vector2Int>mainDecisions = new List<Vector2Int>();
        foreach (Vector2Int possibleDecision in possibleDecisions)
        {
            foreach (CellType cellType in AllCellTypes)
            {
                CellType[,]fieldCopy = field.Clone() as CellType[,];
                fieldCopy[possibleDecision.x,possibleDecision.y] = cellType;
                if (CheckRow(possibleDecision.x,possibleDecision.y,cellType,fieldCopy,inARowToWin))
                    mainDecisions.Add(possibleDecision);
            }
        }
        if (mainDecisions.Count>0)
        {
            return mainDecisions[UnityEngine.Random.Range(0,mainDecisions.Count-1)];
        }
        return possibleDecisions[UnityEngine.Random.Range(0,possibleDecisions.Count-1)];
    }
}
