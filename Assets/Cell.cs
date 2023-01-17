using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Cell : MonoBehaviour
{
    [SerializeField]SpriteRenderer spriteRenderer;
    public CellClickedEvent _cellClicked;
    private int _x,_y;
    private bool _canClick;
    private void Awake() {
        _cellClicked = new CellClickedEvent();
    }
    private void OnMouseUp() {
        if (_canClick)
        {
            _cellClicked?.Invoke(_x,_y);
        }
    }
    private void OnMouseDown() {
        _canClick = true;
    }
    private void OnMouseOver() {
        if (Input.touchCount!=0)
        {
            if (Input.touches[0].deltaPosition.magnitude>0.1f)
                _canClick = false;
        }
    }
    private void OnMouseExit() {
        _canClick = false;
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
