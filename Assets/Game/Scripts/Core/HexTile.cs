namespace Roguelite.Core
{
    public readonly struct HexTile
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
    }
}
