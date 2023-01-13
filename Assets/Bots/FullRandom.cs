using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Bots/Random bot")]
public class FullRandom : Bot
{
    public override Vector2Int GetBotDecision(CellType[,] field, int inARowToWin)
    {
        List<Vector2Int>_possibleDecisions = new List<Vector2Int>();
        for (int x = 0;x<field.GetLength(0);x++)
        {
            for (int y = 0;y<field.GetLength(1);y++)
            {
                if (field[x,y]==CellType.Empty)
                    _possibleDecisions.Add(new Vector2Int(x,y));
            }
        }
        int randomIndex = UnityEngine.Random.Range(0,_possibleDecisions.Count-1);
        return _possibleDecisions[randomIndex];
    }
}
