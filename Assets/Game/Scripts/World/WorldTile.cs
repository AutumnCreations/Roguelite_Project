using UnityEngine;

namespace Roguelite.Core
{
    public class WorldTile : MonoBehaviour
    {
        public int X;
        public int Z;

        Player player;

        //private void Start()
        //{
        //    //Using tag reference to find player to avoid conflicts with enemy test dummies
        //    player = GameObject.FindWithTag("Player").GetComponent<Player>();
        //}

        //private void OnMouseEnter()
        //{
        //    player.targetTile = this;
        //}

        //private void OnMouseExit()
        //{
        //    player.targetTile = null;
        //}
    }
}
