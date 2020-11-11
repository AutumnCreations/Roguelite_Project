using System.Collections;

namespace Scripts.Core
{
    public class TurnAction
    {
        private readonly IEnumerator _action;

        public TurnAction(IEnumerator action)
        {
            _action = action;
        }

        public IEnumerator Act()
        {
            return _action;
        }
    }
}
