using System.Collections.Generic;
using MyLibrary;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FillBoard : BaseComponetMatch3
{
    private float _gravityAcceleration = 9.8f;
    private float _maxVeclocity = 15f;

    private class FallingTileInfo
    {
        public BaseDot PDot;
        public int SourceX, SourceY;
        public int TargetX, TargetY;
        public float Velocity;
        public bool IsLanded;
    }

    private List<FallingTileInfo> _fallingDots = new();

    void Update()
    {
        switch (match3.GetCurrentState())
        {
            case Match3.State.Full :
                match3.HandleMatch();
                break;

            case Match3.State.Falling:
                if (IsDoneFalling())
                    match3.SetCurrentState(Match3.State.Filling);
                else
                    ApplyGravityEffect();
                break;

            case Match3.State.Filling:
                FillFullBoard();
                break;
        }
    }
    private void ApplyGravityEffect()
    {
        IdentifyFallingPieces();
        UpdateFallingPieces();
    }
    private void IdentifyFallingPieces()
    {
        _fallingDots.RemoveAll(tile => tile.IsLanded);

        for (int i = 0; i < match3.Width; i++)
        {
            for (int j = 1; j < match3.Height; j++)
            {
                if (match3.DotTiles[i, j] == null) continue;

                int lowestEmptyY = j;
                while (lowestEmptyY >= 0 && match3.IsValidPosition(i, lowestEmptyY - 1) && match3.DotTiles[i, lowestEmptyY - 1] == null)
                {
                    lowestEmptyY--;
                }


                if (lowestEmptyY < j && !_fallingDots.Exists(t => t.PDot == match3.DotTiles[i, j]))
                {
                    CommonUtils.Swap(ref match3.DotTiles[i, j], ref match3.DotTiles[i, lowestEmptyY]);

                    var newFalling = new FallingTileInfo
                    {
                        PDot = match3.DotTiles[i, lowestEmptyY],
                        SourceX = i,
                        SourceY = j,
                        TargetX = i,
                        TargetY = lowestEmptyY,
                        Velocity = 0f,
                        IsLanded = false
                    };

                    _fallingDots.Add(newFalling);
                }
            }
        }
    }
    private void UpdateFallingPieces()
    {
        foreach (var dotInfo in _fallingDots)
        {
            if (dotInfo.IsLanded) continue;

            dotInfo.Velocity += _gravityAcceleration * Time.deltaTime;
            dotInfo.Velocity = Mathf.Min(dotInfo.Velocity, _maxVeclocity);

            Vector3 currentPos = dotInfo.PDot.transform.position;
            float newY = currentPos.y - dotInfo.Velocity * Time.deltaTime;

            float targetY = dotInfo.TargetY;

            if (Mathf.Abs(targetY - newY) <= 0.1f)
            {
                dotInfo.PDot.transform.position = new Vector3(dotInfo.TargetX, dotInfo.TargetY, 0);

                dotInfo.PDot.SetCurrentPos(dotInfo.TargetX, dotInfo.TargetY);

                dotInfo.IsLanded = true;
                dotInfo.Velocity = 0f;
            }
            else
            {
                dotInfo.PDot.transform.position = new Vector3(dotInfo.TargetX, newY, 0);
            }

        }
    }
    private bool IsDoneFalling()
    {
        if (_fallingDots.Count > 0 && _fallingDots.Exists(t => !t.IsLanded))
            return false;

        for (int i = 0; i < match3.Width; i++)
        {
            for (int j = match3.Height - 1; j >= 0; j--)
            {
                if (match3.DotTiles[i, j] == null) continue;
                if (match3.IsValidPosition(i, j - 1) && match3.DotTiles[i, j] && match3.DotTiles[i, j - 1] == null) return false;
            }
        }

        return true;
    }
    private void FillFullBoard()
    {
        bool hasEmptySpaces = false;

        for (int i = 0; i < match3.Width; i++) {
            for (int j = 0; j < match3.Height; j++) {
                if (match3.DotTiles[i, j] == null) {
                    hasEmptySpaces = true;

                    Vector3 startPos = new Vector3(i, match3.Height, 0);

                    BaseDot newDot = match3.CreateRandomDot(startPos);
                    match3.DotTiles[i, j] = newDot;
                    
                    var newFalling = new FallingTileInfo {
                        PDot = newDot,
                        SourceX = i,
                        SourceY = match3.Height,
                        TargetX = i,
                        TargetY = j,
                        Velocity = 0f,
                        IsLanded = false,
                    };

                    _fallingDots.Add(newFalling);
                }
            }
        }

        if (hasEmptySpaces) {
            match3.SetCurrentState(Match3.State.Falling);
        }
        else {
            match3.SetCurrentState(Match3.State.Full);
        }
    }

}