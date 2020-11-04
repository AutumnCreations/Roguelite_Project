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
        [SerializeField] Material hoverMaterial = null;
        [SerializeField] Material castingMaterial = null;

        Material currentMaterial = null;
        Material lastMaterial = null;

        private void Start()
        {
            currentMaterial = gameObject.GetComponent<MeshRenderer>().material;
            lastMaterial = currentMaterial;
        }

        private void UpdateMaterial(Material newMaterial)
        {
            currentMaterial = newMaterial;
            gameObject.GetComponent<MeshRenderer>().material = currentMaterial;
        }

        public void SetDefaultMaterial()
        {
            lastMaterial = defaultMaterial;
            UpdateMaterial(defaultMaterial);
        }

        public void ResetTileMaterial()
        {
            UpdateMaterial(lastMaterial);
        }

        public void SetHoverMaterial()
        {
            UpdateMaterial(hoverMaterial);
        }

        public void SetCastingMaterial()
        {
            lastMaterial = castingMaterial;
            UpdateMaterial(castingMaterial);
        }

    }
}
