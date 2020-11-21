using System.Collections;

namespace Scripts.Turns.Actions
{
    public abstract class TurnAction
    {
        public bool Completed { get; set; }

        public virtual void Move() { }
        public virtual void CastSpell() { }

        public IEnumerator Animation()
        {
            var enumerator = AnimationInternal();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            Completed = true;
        }

        protected abstract IEnumerator AnimationInternal();
    }
}
