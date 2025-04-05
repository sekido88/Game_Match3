using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TouchController : BaseComponetMatch3
{
    private Vector2Int _firstTouchPosition;
    private Vector2Int _lastTouchPosition;

    private IEnumerator MoveDot(BaseDot a, BaseDot b)
    {
        // ! Lay gia tri cua pos dot hien tai roi di chuyen 
        Vector2Int aPos = a.GetCurrentPos();
        Vector2Int bPos = b.GetCurrentPos();

        a.transform.DOMove(new Vector3(bPos.x, bPos.y, 0), 0.15f).SetEase(Ease.Linear);
        b.transform.DOMove(new Vector3(aPos.x, aPos.y, 0), 0.15f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.2f);

        match3.DotTiles[aPos.x, aPos.y].SetCurrentPos(bPos.x, bPos.y);
        match3.DotTiles[bPos.x, bPos.y].SetCurrentPos(aPos.x, aPos.y);

        CommonUtils.Swap(ref match3.DotTiles[aPos.x, aPos.y], ref match3.DotTiles[bPos.x, bPos.y]);
    }


    public IEnumerator HandleTouchMove()
    {
        if (!match3.GetCurrentState().Equals(Match3.State.Full))
        {
            yield break;
        }

        float angle = GetAngleFromVector(_firstTouchPosition, _lastTouchPosition);
        Direction direction = GetDirectionMove(angle);
        Vector2Int dirVector2 = CommonUtils.directionToVector2Int[direction];

        Vector2Int firstDotPos = new Vector2Int(_firstTouchPosition.x, _firstTouchPosition.y);
        Vector2Int targetDotPos = new Vector2Int(firstDotPos.x + dirVector2.x, firstDotPos.y + dirVector2.y);

        BaseDot firstDot = match3.DotTiles[firstDotPos.x, firstDotPos.y];

        if (!match3.IsValidPosition(targetDotPos.x, targetDotPos.y)) yield break;
        BaseDot targetDot = match3.DotTiles[targetDotPos.x, targetDotPos.y];
        if (!match3.IsValidDot(firstDot) || !match3.IsValidDot(targetDot)) yield break;

        yield return StartCoroutine(MoveDot(firstDot, targetDot));

        if (!match3.CanMatch(firstDotPos.x, firstDotPos.y) && !match3.CanMatch(targetDotPos.x, targetDotPos.y))
        {
             yield return StartCoroutine(MoveDot(firstDot, targetDot));
        }
        else
        {
            // match3.HandleTouchMatch(firstDot, targetDot);
            match3.HandleMatch();
        }
    }

    
    private Direction GetDirectionMove(float angle)
    {
        if (45f < angle && angle <= 135f)
        {
            return Direction.Up;
        }
        else if (135f < angle && angle <= 225f)
        {
            return Direction.Left;
        }
        else if (225f < angle && angle <= 315f)
        {
            return Direction.Down;
        }

        return Direction.Right;
    }
    private float GetAngleFromVector(Vector2 a, Vector2 b)
    {
        float angle = Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
        while (angle < 0) angle += 360f;
        return angle;
    }

    public void SetFirstPosition(Vector2Int firstTouchPosition)
    {
        _firstTouchPosition = firstTouchPosition;
    }

    public void SetLastTouchPosition(Vector2Int lastTouchPosition)
    {
        _lastTouchPosition = lastTouchPosition;
    }
}