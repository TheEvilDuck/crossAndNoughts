using UnityEngine;
using System.Collections.Generic;
using System;
[CreateAssetMenu(menuName ="Bots/Tsar Gnidonid bot")]
public class TsarGnidonid : Bot
{   
    Vector2Int _lastDecision;
    CellType _botCellType = CellType.Empty;
    int _fieldSize;
    List<CellType>_cellTypeBeforeBotTurn = new List<CellType>();
    List<CellType>_cellTypeAfterBotTurn = new List<CellType>();
    Array AllCellTypes = Enum.GetValues(typeof(CellType));
    Vector2Int _rowDirection;
    
    public override Vector2Int GetBotDecision(CellType[,] field,int inARowToWin)
    {
        
        List<Vector2Int>possibleDesicions = GetPossibleDecisions(field);
        List<Vector2Int>mainDecisions = new List<Vector2Int>();
        List<Vector2Int>cancelNextPlayerWinDecisions = new List<Vector2Int>();
        List<Vector2Int>cancelPreviosPlayerWinDecisions = new List<Vector2Int>();
        List<Vector2Int>turnNearEnemyDecisions = new List<Vector2Int>();
        List<Vector2Int>tryToMakeARowDecisions = new List<Vector2Int>();
        
        _fieldSize = field.GetLength(0);


        if (_lastDecision==null)
            {
                for (int x = 0;x<_fieldSize;x++)
                {
                    for (int y = 0;y<_fieldSize;y++)
                    {
                        if (!_cellTypeBeforeBotTurn.Contains(field[x,y])&&field[x,y]!=CellType.Empty)
                        {
                            _cellTypeBeforeBotTurn.Add(field[x,y]);
                            foreach (Vector2Int possibleDecision in possibleDesicions)
                            {
                                if ((possibleDecision-new Vector2Int(x,y)).magnitude<=1)
                                    turnNearEnemyDecisions.Add(possibleDecision);
                            }
                        }
                    }
                }
            }

        else
            {
                if (_botCellType==CellType.Empty)
                    _botCellType = field[_lastDecision.x,_lastDecision.y];
                for (int x = 0;x<_fieldSize;x++)
                {
                    for (int y = 0;y<_fieldSize;y++)
                    {
                        if (!_cellTypeAfterBotTurn.Contains(field[x,y])
                        &&!_cellTypeBeforeBotTurn.Contains(field[x,y])
                        &&field[x,y]!=CellType.Empty
                        &&field[x,y]!=_botCellType)
                            _cellTypeAfterBotTurn.Add(field[x,y]);
                    }
                }
                foreach (CellType cellType in AllCellTypes)
                {
                    foreach (Vector2Int possibleDecision in possibleDesicions)
                    {
                        CellType[,]fieldCopy = field.Clone() as CellType[,];
                        fieldCopy[possibleDecision.x,possibleDecision.y] = cellType;
                        if (CheckRow(possibleDecision.x,possibleDecision.y,cellType,fieldCopy,inARowToWin))
                        {
                            if (cellType==_botCellType)
                                mainDecisions.Add(possibleDecision);
                            else
                            {
                                if (_cellTypeAfterBotTurn.Contains(cellType))
                                    cancelNextPlayerWinDecisions.Add(possibleDecision);
                                if (_cellTypeBeforeBotTurn.Contains(cellType))
                                    cancelNextPlayerWinDecisions.Add(possibleDecision);
                            }
                        }
                        else if ((possibleDecision-_lastDecision).magnitude<=1)
                            {
                                if (cellType==_botCellType)
                                {
                                    Vector2Int direction = possibleDecision-_lastDecision;
                                    if (possibleDesicions.Contains(possibleDecision+direction))
                                        tryToMakeARowDecisions.Add(possibleDecision);
                                }
                            }
                    }
                }
            }
        if (mainDecisions.Count!=0)
            _lastDecision = mainDecisions[UnityEngine.Random.Range(0,mainDecisions.Count-1)];
        else if (cancelNextPlayerWinDecisions.Count!=0)
            _lastDecision = cancelNextPlayerWinDecisions[UnityEngine.Random.Range(0,cancelNextPlayerWinDecisions.Count-1)];
        else if (cancelPreviosPlayerWinDecisions.Count!=0)
            _lastDecision = cancelPreviosPlayerWinDecisions[UnityEngine.Random.Range(0,cancelPreviosPlayerWinDecisions.Count-1)];
        else if (tryToMakeARowDecisions.Count!=0)
            {
                Vector2Int decision = tryToMakeARowDecisions[UnityEngine.Random.Range(0,tryToMakeARowDecisions.Count-1)];
                if (_rowDirection!=null)
                {
                    if (possibleDesicions.Contains(_lastDecision+_rowDirection))
                        decision = _lastDecision+_rowDirection;
                }
                _rowDirection = decision-_lastDecision;
                _lastDecision = decision;
            }
        else if (turnNearEnemyDecisions.Count!=0)
            _lastDecision = turnNearEnemyDecisions[UnityEngine.Random.Range(0,turnNearEnemyDecisions.Count-1)];
        else if (possibleDesicions.Count>0)
            _lastDecision = possibleDesicions[UnityEngine.Random.Range(0,possibleDesicions.Count-1)];
        
        return _lastDecision;
    }
}
