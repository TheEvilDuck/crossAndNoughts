using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(menuName ="Bots/Corner bot")]
public class CornerLover : Bot
{
    public override Vector2Int GetBotDecision(CellType[,] field, int inARowToWin)
    {
        List<Vector2Int>possibleDecisions = GetPossibleDecisions(field);
        List<Vector2Int>mainDesicions = new List<Vector2Int>();
        Array AllCellTypes = Enum.GetValues(typeof(CellType));
        foreach (Vector2Int decision in possibleDecisions)
        {
            if (decision.x ==decision.y||decision.x==Mathf.Abs(decision.y-decision.x)||decision.y == Mathf.Abs(decision.y-decision.x))
            {
                mainDesicions.Add(decision);
            }
        }
        if (mainDesicions.Count>0)
        {
            foreach (Vector2Int possibleDecision in mainDesicions)
            {
                foreach (CellType cellType in AllCellTypes)
                {
                    if (cellType == CellType.Empty)continue;
                    CellType[,]fieldCopy = field.Clone() as CellType[,];
                    fieldCopy[possibleDecision.x,possibleDecision.y] = cellType;
                    if (CheckRow(possibleDecision.x,possibleDecision.y,cellType,fieldCopy,inARowToWin))
                    {
                        return possibleDecision;
                    }
                }
            }
            return mainDesicions[UnityEngine.Random.Range(0,mainDesicions.Count-1)];
        }
        return possibleDecisions[UnityEngine.Random.Range(0,possibleDecisions.Count-1)];
    }
}
