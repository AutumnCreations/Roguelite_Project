using System.Collections;

namespace Scripts.Turns.Actions
{
    public class NoAction : TurnAction
    {
        protected override IEnumerator AnimationInternal()
        {
            yield break;
        }
    }
}
