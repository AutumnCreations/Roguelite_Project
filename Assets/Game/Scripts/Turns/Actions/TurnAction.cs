namespace Scripts.Turns.Actions
{
    public abstract class TurnAction
    {
        public virtual void Move() { }
        public virtual void CastSpell() { }
    }
}
