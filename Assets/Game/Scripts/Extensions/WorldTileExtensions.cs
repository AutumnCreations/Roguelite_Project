using System;
using Roguelite.Core;
using UnityEngine;

namespace Assets.Game.Scripts.Extensions
{
    public static class WorldTileExtensions
    {
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
