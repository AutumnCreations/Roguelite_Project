using System.Collections.Generic;
using UnityEngine;

namespace Roguelite.Core
{
    public class HexTile
    {
        static readonly float widthMultiplier = Mathf.Sqrt(3) / 2;

        public readonly int Q; // Column
        public readonly int R; // Row
        public readonly int S;

        public readonly Vector3 Position;

        public HexTile(int q, int r)
        {
            this.Q = q;
            this.R = r;
            this.S = -(q + r);

            this.Position = GetPosition();
        }

        public override string ToString()
        {
            return $"{nameof(Q)}: {Q}, {nameof(R)}: {R}, {nameof(S)}: {S}";
        }

        private Vector3 GetPosition()
        {
            float radius = .5f;
            float height = radius * 2;
            float width = widthMultiplier * height;

            float verSpacing = height * .75f;
            float horSpacing = width;

            return new Vector3(horSpacing * (this.Q + this.R / 2f), 0, verSpacing * this.R);
        }
    }
}
