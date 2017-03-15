using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mus3d
{
    //public class EnemySprites
    //{
    //    Dictionary<Direction, Sprite[]>   m_idleSpritesByDirection  = new Dictionary<Direction, Sprite[]> ();
    //    Dictionary<Direction, Sprite[]>   m_walkSpritesByDirection  = new Dictionary<Direction, Sprite[]> ();
    //    Dictionary<EnemyAnimator.State, Sprite[]>   m_spritesByState          = new Dictionary<EnemyAnimator.State, Sprite[]> ();

    //    Texture2D m_spriteSheet;

    //    Vector2 m_pivot         = new Vector2 (0.5f, 0.5f);
    //    float   m_pixelsPerUnit = 32f;

    //    /* ---------------------------------------------------------------------------------------------------------------------------------- */
    //    public EnemySprites (Texture2D spriteSheet)
    //    {
    //        m_spriteSheet = spriteSheet;

    //        LoadIdleSprites ();
    //        LoadWalkSprites ();
    //        LoadDeathSprites ();
    //        LoadAttackSprites ();
    //        LoadBodySprite ();
    //        LoadHitSprite ();
    //    }

    //    /* ---------------------------------------------------------------------------------------------------------------------------------- */
    //    public Sprite[] GetForState (EnemyAnimator.State state, Direction direction = Direction.N)
    //    {
    //        switch (state)
    //        {
    //            case EnemyAnimator.State.Idle:
    //                return m_idleSpritesByDirection[direction];
    //            case EnemyAnimator.State.Walk:
    //                return m_walkSpritesByDirection[direction];
    //            case EnemyAnimator.State.Die:
    //            case EnemyAnimator.State.Hit:
    //            case EnemyAnimator.State.Attack:
    //            case EnemyAnimator.State.Body:
    //                return m_spritesByState[state];
    //        }

    //        return null;
    //    }

    //    /* ---------------------------------------------------------------------------------------------------------------------------------- */
    //    void LoadHitSprite ()
    //    {
    //        var hitSprite = new Sprite[1]
    //        {
    //            Sprite.Create (m_spriteSheet, new Rect (7 * 65, 1 * 65, 64, 64), m_pivot, m_pixelsPerUnit)
    //        };

    //        m_spritesByState.Add (EnemyAnimator.State.Hit, hitSprite);
    //    }

    //    /* ---------------------------------------------------------------------------------------------------------------------------------- */
    //    void LoadBodySprite ()
    //    {
    //        var bodySprite = new Sprite[1]
    //        {
    //            Sprite.Create (m_spriteSheet, new Rect (4 * 65, 1 * 65, 64, 64), m_pivot, m_pixelsPerUnit)
    //        };

    //        m_spritesByState.Add (EnemyAnimator.State.Body, bodySprite);
    //    }

    //    /* ---------------------------------------------------------------------------------------------------------------------------------- */
    //    void LoadAttackSprites ()
    //    {
    //        var attackSprites = new Sprite[3];
    //        for (int i = 0; i < 3; i++)
    //        {
    //            var spriteRect = new Rect (i * 65, 0, 64, 64);
    //            attackSprites[i] = Sprite.Create (m_spriteSheet, spriteRect, m_pivot, m_pixelsPerUnit);
    //        }

    //        m_spritesByState.Add (EnemyAnimator.State.Attack, attackSprites);
    //    }

    //    /* ---------------------------------------------------------------------------------------------------------------------------------- */
    //    void LoadDeathSprites ()
    //    {
    //        var dieSprites = new Sprite[4];
    //        for (int i = 0; i < 4; i++)
    //        {
    //            var spriteRect = new Rect (i * 65, 1 * 65, 64, 64);
    //            dieSprites[i] = Sprite.Create (m_spriteSheet, spriteRect, m_pivot, m_pixelsPerUnit);
    //        }

    //        m_spritesByState.Add (EnemyAnimator.State.Die, dieSprites);
    //    }

    //    /* ---------------------------------------------------------------------------------------------------------------------------------- */
    //    void LoadIdleSprites ()
    //    {
    //        foreach (Direction direction in Enum.GetValues (typeof (Direction)))
    //        {
    //            var spriteArray = new Sprite[1];

    //            var spriteRect = new Rect (((int)direction) * 65, 6 * 65, 64, 64);
    //            spriteArray[0] = Sprite.Create (m_spriteSheet, spriteRect, m_pivot, m_pixelsPerUnit);

    //            m_idleSpritesByDirection.Add (direction, spriteArray);
    //        }
    //    }

    //    /* ---------------------------------------------------------------------------------------------------------------------------------- */
    //    void LoadWalkSprites ()
    //    {
    //        foreach (Direction direction in Enum.GetValues (typeof (Direction)))
    //        {
    //            var spriteArray = new Sprite[4];

    //            int startRowIndex = 2;
    //            int endRowIndex   = startRowIndex + spriteArray.Length;

    //            for (int rowIndex = startRowIndex; rowIndex < endRowIndex; rowIndex++)
    //            {
    //                var spriteRect = new Rect (((int)direction) * 65, rowIndex * 65, 64, 64);
    //                spriteArray[spriteArray.Length - 1 - (rowIndex - startRowIndex)] = Sprite.Create (m_spriteSheet, spriteRect, m_pivot, m_pixelsPerUnit);
    //            }

    //            m_walkSpritesByDirection.Add (direction, spriteArray);
    //        }
    //    }
    //}
}