using UnityEngine;

public  abstract class Bot:ScriptableObject
{
    public int _playerId = 0;
    public int _difficulty = 0;
    public virtual Vector2Int GetBotDecision(CellType[,] field, int inARowToWin)
    {return new Vector2Int(0,0);}
    protected bool CheckRow(int xFrom,int yFrom,CellType cellType,CellType[,] field,int inARowToWin)
    {
        return
        (1+GetInARowOfLineInDirection(xFrom,yFrom,1,0,cellType,field)+GetInARowOfLineInDirection(xFrom,yFrom,-1,0,cellType,field)>=inARowToWin)||
        (1+GetInARowOfLineInDirection(xFrom,yFrom,0,1,cellType,field)+GetInARowOfLineInDirection(xFrom,yFrom,0,-1,cellType,field)>=inARowToWin)||
        (1+GetInARowOfLineInDirection(xFrom,yFrom,1,1,cellType,field)+GetInARowOfLineInDirection(xFrom,yFrom,-1,-1,cellType,field)>=inARowToWin)||
        (1+GetInARowOfLineInDirection(xFrom,yFrom,1,-1,cellType,field)+GetInARowOfLineInDirection(xFrom,yFrom,-1,1,cellType,field)>=inARowToWin);
    }
    private int GetInARowOfLineInDirection(int xFrom,int yFrom,int xDirection,int yDirection,CellType cellType,CellType[,] field)
    {
        int inARow = 0;

        int x = xFrom+xDirection;
        int y = yFrom+yDirection;

        while (x>=0&&y>=0&&x<field.GetLength(0)&&y<field.GetLength(0))
        {
            if (field[x,y] == cellType)
                    inARow++;
            else break;
            x+=xDirection;
            y+=yDirection;
        }
        return inARow;
    }
}
