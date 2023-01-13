using UnityEngine;
using System.Collections.Generic;
using System;
[CreateAssetMenu(menuName ="Bots/Gnida bot")]
public class Gnida : Bot
{
    public override Vector2Int GetBotDecision(CellType[,] field,int inARowToWin)
    {
        int fieldSize = field.GetLength(0);
        List<Vector2Int>possibleDecisions = new List<Vector2Int>();
        List<Vector2Int>mainDecisions = new List<Vector2Int>();
        List<Vector2Int>notTooGoodDecisions = new List<Vector2Int>();
        for (int x = 0;x<fieldSize;x++)
        {
            for (int y = 0;y<fieldSize;y++)
            {
                if (field[x,y]==CellType.Empty)
                    possibleDecisions.Add(new Vector2Int(x,y));
            }
        }
        foreach (Vector2Int possibleDecision in possibleDecisions)
            {
                Array AllCellTypes = Enum.GetValues(typeof(CellType));
                foreach (CellType cellType in AllCellTypes)
                {
                    if (cellType == CellType.Empty)continue;
                    CellType[,]fieldCopy = new CellType[fieldSize,fieldSize];
                    fieldCopy = field.Clone() as CellType[,];
                    fieldCopy[possibleDecision.x,possibleDecision.y] = cellType;
                    if (CheckRow(possibleDecision.x,possibleDecision.y,cellType,fieldCopy,inARowToWin))
                    {
                        mainDecisions.Add(possibleDecision);
                    }
                    else if(CheckRow(possibleDecision.x,possibleDecision.y,cellType,fieldCopy,inARowToWin-1))
                    {
                        notTooGoodDecisions.Add(possibleDecision);
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
        return possibleDecisions[UnityEngine.Random.Range(0,possibleDecisions.Count-1)];
    }
}
