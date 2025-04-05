using UnityEngine;

public class SpritedDot : BaseDot
{
    [SerializeField] private bool _isHorizontal = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Init(int x, int y, DotColor dotColor, DotType dotType, bool isHorizontal)
    {
        base.Init(x, y, dotColor, dotType, isHorizontal);
        
    }

    public override void ActivateEffect()
    {
        base.ActivateEffect();
        if (_isHorizontal)
        {
            for (int i = 0; i < _match3.Width; i++)
            {
                if (_match3.DotTiles[i, _currentY] == null) continue;
                if (i == _currentX) continue;

                _match3.DotTiles[i, _currentY].OnMatch();
            }
        }
        else
        {
            for (int j = 0; j < _match3.Height; j++)
            {
                if (_match3.DotTiles[_currentX, j] == null) continue;
                if (j == _currentY) continue;

                _match3.DotTiles[_currentX, j].OnMatch();
            }
        }

        Destroy(this.gameObject);
    }
}