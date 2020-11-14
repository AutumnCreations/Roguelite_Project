using System;
using Scripts.Worlds;
using UnityEngine;

namespace Scripts.Extensions
{
    public static class WorldTileExtensions
    {
        public static Vector3 GetWorldPosition(this HexTile hex)
        {
            // sqrt(3) / 2
            const float widthMultiplier = 0.866025403784439f;

            const float radius = 0.5f;
            const float height = radius * 2;
            const float width = widthMultiplier * height;

            const float verticalSpacing = height * .75f;
            const float horizontalSpacing = width;

            return new Vector3(horizontalSpacing * (hex.Q + hex.R / 2f), 0, verticalSpacing * hex.R);
        }

        public static int DistanceTo(this HexTile origin, HexTile target)
        {
            return origin.DistanceTo(target.Q, target.R);
        }

        public static int DistanceTo(this HexTile origin, int q, int r)
        {
            var s = -(q + r);
            return (Math.Abs(origin.Q - q) + Math.Abs(origin.R - r) + Math.Abs(origin.S - s)) / 2;
        }
    }
}
