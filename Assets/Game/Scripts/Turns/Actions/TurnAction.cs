using System.Collections;

namespace Scripts.Turns.Actions
{
    public abstract class TurnAction
    {
        public bool Completed { get; set; }

        public IEnumerator Act()
        {
            var enumerator = ActInternal();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            Completed = true;
        }

        protected abstract IEnumerator ActInternal();
    }
}
