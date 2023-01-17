using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Bots/Random bot")]
public class FullRandom : Bot
{
    public override Vector2Int GetBotDecision(CellType[,] field, int inARowToWin)
    {
        List<Vector2Int>possibleDecisions = GetPossibleDecisions(field);
        int randomIndex = UnityEngine.Random.Range(0,possibleDecisions.Count-1);
        return possibleDecisions[randomIndex];
    }
}
