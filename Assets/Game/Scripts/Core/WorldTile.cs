using UnityEngine;
using TMPro;

namespace Roguelite.Core
{
    public class WorldTile : MonoBehaviour
    {
        [SerializeField] Color defaultColor;
        [SerializeField] Color hoverColor;
        [SerializeField] Color castingColor;

        [SerializeField] public TextMeshProUGUI xCoordinate;
        [SerializeField] public TextMeshProUGUI yCoordinate;
        [SerializeField] public TextMeshProUGUI zCoordinate;

        Material tileMaterial = null;

        Color currentColor;
        Color lastColor;

        MeshRenderer child = null;
        public HexTile Hex { get; set; }

        private void Start()
        {
            child = gameObject.GetComponentInChildren<MeshRenderer>();

            tileMaterial = child.material;

            currentColor = tileMaterial.color;
            lastColor = currentColor;
        }

        #region Colors
        public void UpdateColor(Color newColor)
        {
            currentColor = newColor;
            tileMaterial.color = currentColor;
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
