using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Cell : MonoBehaviour
{
    [SerializeField]SpriteRenderer spriteRenderer;
    public CellClickedEvent _cellClicked;
    private int _x,_y;
    private void Awake() {
        _cellClicked = new CellClickedEvent();
    }
    void OnMouseDown()
    {
        _cellClicked.Invoke(_x,_y);
    }
    public void ChangeSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
    public void SetCoordinates(int x,int y)
    {
        _x = x;
        _y = y;
    }
}
public class CellClickedEvent:UnityEvent<int,int>{}
