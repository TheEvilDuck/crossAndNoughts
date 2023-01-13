using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Visuals : MonoBehaviour
{
    [SerializeField]float _cellSize = 0.1f;
    [SerializeField]float _scalePerFieldSize = 0.9f;
    [SerializeField]float _cellSpacing = 0.1f;
    [SerializeField]GameObject _cellPrefab;
    [SerializeField]Image _currentPlayerImage;
    [SerializeField]TextMeshProUGUI  _text;
    [SerializeField]CellVisual[] _visualPull;
    private Cell[,]_fieldVisual;
    private Dictionary<CellType,CellVisual>_playersCellVisuals;

    public Cell[,] InitVisuals(int fieldSize)
    {
        ClearField();
        _fieldVisual = new Cell[fieldSize,fieldSize];
        Vector3 size = Vector3.zero;
        for (int x = 0;x<fieldSize;x++)
        {
            for (int y = 0;y<fieldSize;y++)
            {
                Vector3 position = new Vector3(0,0,1);
                GameObject obj = Instantiate(_cellPrefab,position,Quaternion.identity,transform);
                size = obj.GetComponent<SpriteRenderer>().bounds.size;
                obj.transform.localScale*=_cellSize/(_scalePerFieldSize*fieldSize);
                Vector3 translate = new Vector3(x*size.x/(_scalePerFieldSize*fieldSize)*_cellSize,y*size.y/(_scalePerFieldSize*fieldSize)*_cellSize,0);
                obj.transform.Translate(translate);
                _fieldVisual[x,y] = obj.GetComponent<Cell>();
                _fieldVisual[x,y].SetCoordinates(x,y);

            }
        }
        //Camera.main.transform.position = new Vector3(size.x*fieldSize/2f/(_scalePerFieldSize*fieldSize)*_cellSize,size.y*fieldSize/2f/(_scalePerFieldSize*fieldSize)*_cellSize,0);
        return _fieldVisual;

    }
    public void DefinePlayersVisuals(Player[] players)
    {
        int playersCount = players.GetLength(0);
        _playersCellVisuals?.Clear();
        _playersCellVisuals = new Dictionary<CellType, CellVisual>(playersCount);
        for (int i = 0;i<playersCount;i++)
        {
            for (int j = 0;j<_visualPull.GetLength(0);j++)
            {
                if (_visualPull[j].cellType==players[i]._cellType)
                {
                    _playersCellVisuals.Add(players[i]._cellType,_visualPull[j]);
                    continue;
                }
            }
        }
    }
    public void UpdateVisuals(int x,int y,CellType cellType)
    {
        _fieldVisual[x,y]?.ChangeSprite(_playersCellVisuals[cellType]?.sprite);
    }
    public void UpdateText(string text)
    {
        _text.text = text;
    }
    public void UpdateCurrentPlayerImage(CellType cellType)
    {
        _currentPlayerImage.sprite = _playersCellVisuals[cellType].sprite;
    }
    public void ClearField()
    {
        if (_fieldVisual == null)
            return;
        for (int x = 0;x<_fieldVisual.GetLength(0);x++)
        {
            for (int y = 0;y<_fieldVisual.GetLength(1);y++)
            {
                _fieldVisual[x,y]._cellClicked.RemoveAllListeners();
                Destroy(_fieldVisual[x,y].gameObject);
            }
        }
    }
}
