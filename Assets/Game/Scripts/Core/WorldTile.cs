using UnityEngine;

namespace Roguelite.Core
{
    public class WorldTile : MonoBehaviour
    {
        [HideInInspector]
        public int X;
        [HideInInspector]
        public int Z;

        [SerializeField] Material defaultMaterial = null;
        [SerializeField] Material highlightedMaterial = null;


        private void UpdateMaterial(Material newMaterial)
        {
            gameObject.GetComponent<MeshRenderer>().material = newMaterial;

        }

        public void SetDefaultMaterial()
        {
            UpdateMaterial(defaultMaterial);
        }

        public void SetHighlightedMaterial()
        {
            UpdateMaterial(highlightedMaterial);
        }

    }
}
