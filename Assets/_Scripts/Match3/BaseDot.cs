using UnityEngine;

public abstract class BaseDot : MonoBehaviour
{
    public enum DotColor
    {
        Red,
        Yellow,
        Green,
        Blue,
        Orange,
        Purple
    }

    public enum DotType
    {
        Regualar,
        Spencial
    }
    protected int _currentX;
    protected int _currentY;
    [SerializeField] protected DotColor _DotColor;
    [SerializeField] protected DotType _DotType;
    protected Match3 _match3;

    protected virtual void Awake()
    {
        _match3 = GameObject.FindAnyObjectByType<Match3>();
    }
    public virtual void Init(int x, int y, DotColor dotColor, DotType dotType = DotType.Regualar)
    {
        _currentX = x;
        _currentY = y;
        _DotColor = dotColor;
        _DotType = dotType;
    }
    public virtual void Init(int x, int y, DotColor DotColor,  DotType dotType = DotType.Regualar, bool isHorizontal = false)
    {
        _currentX = x;
        _currentY = y;
        _DotColor = DotColor;
        _DotType = dotType;
    }

    #region Logic
    void OnMouseDown()
    {
        _match3.HandleOnMouseDown(new Vector2Int(_currentX, _currentY));
    }

    void OnMouseUp()
    {
        Vector3 mousePosition3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mousePosition2 = new Vector2Int((int)mousePosition3.x, (int)mousePosition3.y);
        _match3.HandleOnMouseUp(mousePosition2);
    }

    public virtual void OnMatch()
    {
        _match3.DotTiles[_currentX, _currentY] = null;
        ActivateEffect();
    }
    public virtual void ActivateEffect()
    {

    }
    #endregion


    #region Get Set
    public DotColor GetCurrentDotColor()
    {
        return _DotColor;
    }
    public Vector2Int GetCurrentPos()
    {
        return new Vector2Int(_currentX, _currentY);
    }

    public void SetDotColor(DotColor DotColor)
    {
        _DotColor = DotColor;
    }

    public void SetCurrentPos(int x, int y)
    {
        _currentX = x;
        _currentY = y;
    }
    #endregion 




}