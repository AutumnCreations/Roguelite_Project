using TMPro;
using UnityEngine;

namespace Scripts.Worlds
{
    public class WorldTile : MonoBehaviour
    {
        [SerializeField] Color defaultColor;
        [SerializeField] Color hoverColor;
        [SerializeField] Color castingColor;

        [SerializeField] public TextMeshProUGUI xCoordinate;
        [SerializeField] public TextMeshProUGUI yCoordinate;
        [SerializeField] public TextMeshProUGUI zCoordinate;

        [SerializeField] public GameObject occupyingObject;

        Material tileMaterial = null;

        Color currentColor;
        Color lastColor;

        MeshRenderer child = null;

        private HexTile _hex;
        public HexTile Hex
        {
            get => _hex;
            set
            {
                _hex = value;
                SetTileName(value);
            }
        }

        private void Start()
        {
            child = gameObject.GetComponentInChildren<MeshRenderer>();

            tileMaterial = child.material;

            currentColor = tileMaterial.color;
            lastColor = currentColor;
        }

        private void SetTileName(HexTile hex)
        {
            name = hex.ToString();
            xCoordinate.text = $"{hex.Q}";
            yCoordinate.text = $"{hex.R}";
            zCoordinate.text = $"{hex.S}";
        }

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
    }
}
