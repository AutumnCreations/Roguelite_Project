using System;

namespace Scripts.Core
{
    public readonly struct HexTile : IEquatable<HexTile>
    {
        public int Q { get; }
        public int R { get; }
        public int S { get; }

        public HexTile(int q, int r)
        {
            Q = q;
            R = r;
            S = -(q + r);
        }

        public override string ToString()
        {
            return $"{nameof(Q)}: {Q}, {nameof(R)}: {R}, {nameof(S)}: {S}";
        }

        public bool Equals(HexTile other)
        {
            return Q == other.Q && R == other.R && S == other.S;
        }

        public override bool Equals(object obj)
        {
            return obj is HexTile other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Q;
                hashCode = (hashCode * 397) ^ R;
                hashCode = (hashCode * 397) ^ S;
                return hashCode;
            }
        }
    }
}
