using Scripts.Control;
using UnityEngine;

namespace Scripts.Worlds
{
    public class EnemyContainer : MonoBehaviour
    {
        public Enemy[] Characters;

        private void Awake()
        {
            Characters = gameObject.GetComponentsInChildren<Enemy>();
        }
    }
}
