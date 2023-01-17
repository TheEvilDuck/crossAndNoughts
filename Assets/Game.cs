using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class Game : MonoBehaviour
{
    [SerializeField]Visuals _visuals;
    [SerializeField]CameraManipulation _cameraManipulation;
    [SerializeField]Bot[] _botsPull;

    private int _playersCount = 2;
    private int _fieldSize = 3;
    private int _inARowToWin = 3;
    private int _botsCount = 0;
    private int _botsDifficulty = 0;
    

    private Player[] _players;
    private Bot[] _bots;
    private int _currentPlayerId = 0;
    private CellType[,]_field;
    private int _cellsLeft;

    public void AdjustGameSettings(int playersCount,int fieldSize,int inARowToWin, int botsCount,int botsDifficulty)
    {
        _playersCount = playersCount;
        _fieldSize = fieldSize;
        _inARowToWin = inARowToWin;
        _botsCount = botsCount;
        _botsDifficulty = botsDifficulty;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void NewRound()
    {
        Debug.Log("<color=red>A new game started!</color>");
        _visuals.ClearField();
        _currentPlayerId = 0;

        _cellsLeft = _fieldSize*_fieldSize;
        _players = new Player[_playersCount];
        _field = new CellType[_fieldSize,_fieldSize];

        _cameraManipulation.ResetCamera();
        
        DefineCellTypesForPlayers();
        AddListenersToOnFieldClicked();
        
        DefineBots();
        _visuals.UpdateCurrentPlayerImage(_players[_currentPlayerId]._cellType);
        string bot = "";
        if (!_players[_currentPlayerId]._isHuman)
            bot = "(Bot)";
        _visuals.UpdateText(string.Format("Now player's {0} {1} turn. ",_currentPlayerId+1,bot));
        
        if (!_players[_currentPlayerId]._isHuman)
            BotTurn();
    }
    private void DefineCellTypesForPlayers()
    {
        Array AllCellTypes = Enum.GetValues(typeof(CellType));
        List<CellType>cellTypesList = new List<CellType>();
        foreach (CellType cellType in AllCellTypes)
        {
            if (cellType!=CellType.Empty)
                cellTypesList.Add(cellType);
            if (cellTypesList.Count>=_playersCount)
                break;
        }
        CellType[] cellTypes = cellTypesList.ToArray();
        var keys = cellTypes.Select(i => new System.Random().NextDouble()).ToArray();
        Array.Sort(keys,cellTypes);
        for (int i = 0;i<_playersCount;i++)
        {
            _players[i] = new Player(cellTypes[i]);
        }
    }
    private void AddListenersToOnFieldClicked()
    {
        Cell[,]cells =  _visuals.InitVisuals(_fieldSize);
        _visuals.DefinePlayersVisuals(_players);
        for (int x = 0;x<_fieldSize;x++)
        {
            for (int y = 0;y<_fieldSize;y++)
            {
                _field[x,y] = CellType.Empty;
                if (cells[x,y]==null)continue;
                cells[x,y]._cellClicked.AddListener((x,y)=>
                {
                    if (_players[_currentPlayerId]._isHuman
                    &&!_cameraManipulation.GetCameraManipulating()
                    )
                        Turn(x,y);
                });
            }
        }
    }
    private void DefineBots()
    {
        if (_botsCount<=0) return;
        _bots = new Bot[_botsCount];
        List<Bot>possibleBots = new List<Bot>();
        foreach (Bot bot in _botsPull)
        {
            if (bot._difficulty==_botsDifficulty)
            {
                possibleBots.Add(bot);
            }
        }
        var keys = _players.Select(e => new System.Random().NextDouble()).ToArray();
        for (int i = 0 ;i<_botsCount;i++)
        {
            int randomBotBehaviorIndex = UnityEngine.Random.Range(0,possibleBots.Count);
            _bots[i] = Instantiate(possibleBots[randomBotBehaviorIndex]);
            _players[i]._isHuman = false;
        }
        Array.Sort(keys,_players);
        int botIndex = 0;
        for (int i = 0;i<_playersCount;i++)
        {
            if (!_players[i]._isHuman)
            {
                _bots[botIndex]._playerId = i;
                Debug.Log(string.Format("Added bot with {0} difficulty (player {1})",_bots[botIndex]._difficulty,_bots[botIndex]._playerId));
                botIndex++;
            }
        }

        
    }
    private void BotTurn()
    {
        StartCoroutine(BotTurnCorotine());
    }
    IEnumerator BotTurnCorotine()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f,2f));
        for (int i = 0;i<_botsCount;i++)
            {
                if (_bots[i]._playerId==_currentPlayerId)
                    {
                        Vector2Int decision = _bots[i].GetBotDecision(_field,_inARowToWin);
                        if (decision == null)
                        yield break;
                        Turn(decision.x,decision.y);
                    }
            }
    }

    private void Turn(int x,int y)
    {
        if (_cellsLeft<=0)return;
        if (_field[x,y]!=CellType.Empty) return;
        Debug.Log(string.Format("Player {0}({2}) turn. Is it a bot: {1}",_currentPlayerId,!_players[_currentPlayerId]._isHuman,_players[_currentPlayerId]._cellType));
        ChangeCell(x,y);
        List<Vector2Int>row = new List<Vector2Int>();
        if (CheckRow(x,y,ref row))
        {
            Debug.Log(string.Format("Player {0} win",_currentPlayerId));
            _visuals.UpdateText(string.Format("Player {0} win!",_currentPlayerId+1));
            _cellsLeft = 0;
            _visuals.WinRowChangeColor(row);
            return;
        }
        else
        {
            _cellsLeft--;
            if (_cellsLeft<=0)
            {
                Debug.Log("Draw!");
                _visuals.UpdateText(string.Format("It's draw!"));
                return;
            }
            _currentPlayerId++;
            if (_currentPlayerId>=_playersCount)
                 _currentPlayerId = 0;
            _visuals.UpdateCurrentPlayerImage(_players[_currentPlayerId]._cellType);
            string bot = "";
            if (!_players[_currentPlayerId]._isHuman)
                bot = "(Bot)";
            _visuals.UpdateText(string.Format("Now player's {0} {1} turn. ",_currentPlayerId+1,bot));
            if (!_players[_currentPlayerId]._isHuman)
                BotTurn();
        }
    }

    private void ChangeCell(int x,int y)
    {
        _field[x,y] = _players[_currentPlayerId]._cellType;
        _visuals.UpdateVisuals(x,y,_players[_currentPlayerId]._cellType);
            
    }
    private bool CheckRow(int xFrom,int yFrom,ref List<Vector2Int> row)
    {
        /*return
        (1+GetInARowOfLineInDirection(xFrom,yFrom,1,0,ref row)+GetInARowOfLineInDirection(xFrom,yFrom,-1,0,ref row)>=_inARowToWin)||
        (1+GetInARowOfLineInDirection(xFrom,yFrom,0,1,ref row)+GetInARowOfLineInDirection(xFrom,yFrom,0,-1,ref row)>=_inARowToWin)||
        (1+GetInARowOfLineInDirection(xFrom,yFrom,1,1,ref row)+GetInARowOfLineInDirection(xFrom,yFrom,-1,-1,ref row)>=_inARowToWin)||
        (1+GetInARowOfLineInDirection(xFrom,yFrom,1,-1,ref row)+GetInARowOfLineInDirection(xFrom,yFrom,-1,1,ref row)>=_inARowToWin);
        */
        Vector2Int[]directions = new Vector2Int[4];
        directions[0] = new Vector2Int(1,0);
        directions[1] = new Vector2Int(0,1);
        directions[2] = new Vector2Int(1,1);
        directions[3] = new Vector2Int(1,-1);

        for (int i = 0;i<4;i++)
        {
            row.Clear();
            row.Add(new Vector2Int(xFrom,yFrom));
            int inARow = 
            GetInARowOfLineInDirection(xFrom,yFrom,directions[i].x,directions[i].y,ref row)+
            GetInARowOfLineInDirection(xFrom,yFrom,-directions[i].x,-directions[i].y,ref row);
            if (1+inARow>=_inARowToWin)
                return true;
        }
        return false;



    }
    private int GetInARowOfLineInDirection(int xFrom,int yFrom,int xDirection,int yDirection,ref List<Vector2Int> row)
    {
        int inARow = 0;

        int x = xFrom+xDirection;
        int y = yFrom+yDirection;

        while (x>=0&&y>=0&&x<_fieldSize&&y<_fieldSize)
        {
            if (_field[x,y] == _players[_currentPlayerId]._cellType)
                    {
                        inARow++;
                        row.Add(new Vector2Int(x,y));
                    }
            else break;
            x+=xDirection;
            y+=yDirection;
        }
        return inARow;
    }
}
