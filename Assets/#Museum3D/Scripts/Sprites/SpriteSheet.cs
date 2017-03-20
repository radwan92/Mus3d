using System;
using UnityEngine;

namespace Mus3d
{
    public class SpriteSheet : ScriptableObject
    {
        public enum Layout
        {
            Horizontal,
            Vertical
        }

        public Texture2D Sheet;
        [Space]
        public int      Rows;
        public int      Columns;
        [Space]
        public int      SpriteWidth;
        public int      SpriteHeight;
        [Space]
        public int      PixelsPerUnit;
        public Vector2  Padding;
        public Vector2  Offset;
        public Vector2  Pivot       = new Vector2 (0.5f, 0.5f);

        [NonSerialized] bool m_isInitialized;

        Sprite[][] m_sprites;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Initialize ()
        {
            m_sprites = new Sprite[Columns][];

            for (int i = 0; i < Columns; i++)
                m_sprites[i] = new Sprite[Rows];

            for (int y = 0; y < Rows; y++)
                for (int x = 0; x < Columns; x++)
                    m_sprites[x][y] = CreateSprite (x, y);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public Sprite[] GetSpriteSequence (int x, int y, int count, Layout layout)
        {
            Sprite[] sprites = new Sprite[count];

            for (int i = 0; i < count; i++)
            {
                sprites[i] = GetSprite (x, y);

                if (layout == Layout.Horizontal)
                    x++;
                else
                    y++;
            }

            return sprites;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public Sprite[] GetSpriteSequence (params Vector2[] spritePositions)
        {
            Sprite[] sprites = new Sprite[spritePositions.Length];

            for (int i = 0; i < spritePositions.Length; i++)
            {
                int spriteX = (int)spritePositions[i].x;
                int spriteY = (int)spritePositions[i].y;

                sprites[i] = GetSprite (spriteX, spriteY);
            }

            return sprites;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public Sprite GetSprite (int x, int y)
        {
            return m_sprites[x][y];
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        Sprite CreateSprite (int x, int y)
        {
            var xPosition = x * (SpriteWidth + (int)Padding.x) + Offset.x;
            var yPosition = Sheet.height - ((y + 1) * SpriteHeight + y * (int)Padding.y + Offset.y);

            var spriteRect = new Rect (xPosition, yPosition, SpriteWidth, SpriteHeight);

            return Sprite.Create (Sheet, spriteRect, Pivot, PixelsPerUnit);
        }
    }
}