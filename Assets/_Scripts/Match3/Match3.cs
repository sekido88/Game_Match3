using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}
public class Match3 : MonoBehaviour
{
    // public ArrayLayout DepTraiLayout;
    public static Match3 Instance => instance;
    private static Match3 instance;

    [SerializeField] SpritedDot spritedDot;

    public enum State
    {
        Falling,
        Filling,
        Full
    }

    public int Width = 9;
    public int Height = 10;
    public GameObject TileBackground;
    public List<Dot> prefabDots;

    public GameObject[,] BackGroundTiles;
    public BaseDot[,] DotTiles;
    System.Random random = new System.Random();
    private float _tileSize = 1f;

    [Header("Logic Move")]
    private State _state = State.Full;

    [SerializeField] private TouchController _touchController;
    [SerializeField] private SpecialDotManager _specialDotManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        LoadTouchController();
    }

    void LoadTouchController()
    {
        if (_touchController != null) return;
        _touchController = GameObject.FindFirstObjectByType<TouchController>();
    }

    void Start()
    {
        BackGroundTiles = new GameObject[Width, Height];
        DotTiles = new BaseDot[Width, Height];

        InitializeBoard();
    }
    private void InitializeBoard()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Vector3 pos = new Vector3(i * _tileSize, j * _tileSize, 0);

                BackGroundTiles[i, j] = Instantiate(TileBackground, pos, Quaternion.identity);
                BackGroundTiles[i, j].transform.parent = transform;

                DotTiles[i, j] = CreateRandomDot(pos);
            }
        }
        HandleMatch();
    }

    public BaseDot CreateRandomDot(Vector3 pos)
    {
        //! Tao cac dot binh thuong
        int randomIndexDot = random.Next(0, prefabDots.Count);
        Dot dot = Instantiate(prefabDots[randomIndexDot], pos, Quaternion.identity);
        dot.transform.parent = transform;
        dot.transform.name = "x " + pos.x + "y " + pos.y;

        dot.Init((int)pos.x, (int)pos.y, prefabDots[randomIndexDot].GetCurrentDotColor(), BaseDot.DotType.Regualar);

        return dot;
    }

    private BaseDot CreateSpencial(Vector3 pos) {
        SpritedDot dot = Instantiate(spritedDot, pos, Quaternion.identity);
        dot.transform.parent = transform;
        dot.transform.name = "x " + pos.x + "y " + pos.y;

        dot.Init((int)pos.x, (int)pos.y, spritedDot.GetCurrentDotColor());
        return dot;
    }


    public bool IsValidDot(BaseDot a)
    {
        return a != null;
    }

    private Vector2Int GetIndexMatchInRow(int x, int y)
    {
        BaseDot.DotColor dotType = DotTiles[x, y].GetCurrentDotColor();

        int top = x;
        int bottom = x;

        while (bottom >= 0 && IsValidPosition(bottom - 1, y) && IsValidDot(DotTiles[bottom - 1, y]) && DotTiles[bottom - 1, y].GetCurrentDotColor().Equals(dotType))
        {
            bottom--;
        }

        while (top < Width && IsValidPosition(top + 1, y) && IsValidDot(DotTiles[top + 1, y]) && DotTiles[top + 1, y].GetCurrentDotColor().Equals(dotType))
        {
            top++;
        }

        return new Vector2Int(bottom, top);
    }
    private int CountMatchInRow(int x, int y)
    {
        Vector2Int index = GetIndexMatchInRow(x, y);
        int count = index.y - index.x + 1;

        return count;
    }
    private bool CheckRow(int x, int y)
    {
        return CountMatchInRow(x, y) >= 3;
    }
    private Vector2Int GetIndexMatchInCol(int x, int y)
    {
        BaseDot.DotColor dotType = DotTiles[x, y].GetCurrentDotColor();

        int bottom = y;
        int top = y;

        while (bottom >= 0 && IsValidPosition(x, bottom - 1) && IsValidDot(DotTiles[x, bottom - 1]) && DotTiles[x, bottom - 1].GetCurrentDotColor().Equals(dotType))
        {
            bottom--;
        }

        while (top < Height && IsValidPosition(x, top + 1) && IsValidDot(DotTiles[x, top + 1]) && DotTiles[x, top + 1].GetCurrentDotColor().Equals(dotType))
        {
            top++;
        }

        return new Vector2Int(bottom, top);
    }
    private int CountMatchInCol(int x, int y)
    {
        Vector2Int index = GetIndexMatchInCol(x, y);
        int count = index.y - index.x + 1;
        return count;
    }
    private bool CheckCol(int x, int y)
    {
        return CountMatchInCol(x, y) >= 3;
    }
    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }
    public bool CanMatch(int x, int y)
    {
        if (CheckRow(x, y) || CheckCol(x, y)) return true;
        return false;
    }
    
    public void HandleTouchMatch(BaseDot a, BaseDot b) {
        if (!_state.Equals(State.Full))
        {
            return;
        }

        // bool isMatch = false;
    }

    public void HandleMatch()
    {
        if (!_state.Equals(State.Full))
        {
            return;
        }

        bool isMatch = false;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (!IsValidDot(DotTiles[i, j])) continue;
                if (CheckRow(i, j))
                {
                    isMatch = true;
                    Vector2Int indexRow = GetIndexMatchInRow(i, j);
                    for (int row = indexRow.x; row <= indexRow.y; row++)
                    {
                        if (DotTiles[row, j] != null)
                            DotTiles[row, j].OnMatch();
         
                        EffectManager.Instance.SpawnEffectMatch3(row, j);
                    }

                    DotTiles[i, j] = CreateSpencial(new Vector3(i, j , 0));
                }
                if (!IsValidDot(DotTiles[i, j])) continue;
                if (CheckCol(i, j))
                {
                    isMatch = true;

                    Vector2Int indexCol = GetIndexMatchInCol(i, j);

                    for (int col = indexCol.x; col <= indexCol.y; col++)
                    {
                        if (DotTiles[i, col] != null)
                            DotTiles[i, col].OnMatch();

                        EffectManager.Instance.SpawnEffectMatch3(i, col);
                    }

                    
                    DotTiles[i, j] = CreateSpencial(new Vector3(i, j , 0));

                }
            }
        }

        if (isMatch)
            _state = State.Falling;
    }


    void OnDrawGizmos()
    {
        float size = 1f;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Gizmos.color = Color.white;
                Vector3 pos = transform.position + new Vector3(size * i, size * j, 0);
                Gizmos.DrawWireCube(pos, new Vector3(size, size, 0));
            }
        }
    }
    public void HandleOnMouseDown(Vector2Int firstTouchPosition)
    {
        _touchController.SetFirstPosition(firstTouchPosition);
    }

    public void HandleOnMouseUp(Vector2Int lastTouchPosition)
    {
        _touchController.SetLastTouchPosition(lastTouchPosition);
        StartCoroutine(_touchController.HandleTouchMove());
    }

    public State GetCurrentState()
    {
        return _state;
    }
    public void SetCurrentState(State state)
    {
        _state = state;
    }
}
