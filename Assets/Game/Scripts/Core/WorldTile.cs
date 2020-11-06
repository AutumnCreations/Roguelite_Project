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

        [SerializeField] Color defaultColor;
        [SerializeField] Color hoverColor;
        [SerializeField] Color castingColor;

        Material currentMaterial = null;
        Material lastMaterial = null;

        Color currentColor;
        Color lastColor;

        MeshRenderer child = null;

        private void Start()
        {
            child = gameObject.GetComponentInChildren<MeshRenderer>();

            currentMaterial = child.material;
            lastMaterial = currentMaterial;

            currentColor = currentMaterial.color;
            lastColor = currentColor;
        }

        #region Materials
        private void UpdateMaterial(Material newMaterial)
        {
            currentMaterial = newMaterial;
            child.material = currentMaterial;
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
        #endregion

        #region Colors
        public void UpdateColor(Color newColor)
        {
            currentColor = newColor;
            currentMaterial.color = currentColor;
        }

        public void SetDefaultColor()
        {
            lastColor = defaultColor;
            UpdateColor(defaultColor);
        }

        public void ResetTileColor()
        {
            UpdateColor(lastColor);
        }

        public void SetHoverColor()
        {
            UpdateColor(hoverColor);
        }

        public void SetCastingColor()
        {
            lastColor = castingColor;
            UpdateColor(castingColor);
        }
        #endregion
    }
}
