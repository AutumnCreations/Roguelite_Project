using Roguelite.Combat;
using Roguelite.Core;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Roguelite.UI
{
    public class TooltipPopup : MonoBehaviour
    {
        [SerializeField] private GameObject popupCanvasObject;
        [SerializeField] private RectTransform popupObject;
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float padding;

        private Canvas popupCanvas;

        private void Awake()
        {
            popupCanvas = popupCanvasObject.GetComponent<Canvas>();
            HideInfo();
        }

        private void Update()
        {
            FollowCursor();
        }

        private void FollowCursor()
        {
            if (!popupCanvasObject.activeSelf) { return; }

            Vector3 newPos = Input.mousePosition + offset;
            newPos.z = 0f;
            float rightEdgeToScreenEdgeDistance = Screen.width - (newPos.x + popupObject.rect.width * popupCanvas.scaleFactor / 2) - padding;
            if (rightEdgeToScreenEdgeDistance < 0)
            {
                newPos.x += rightEdgeToScreenEdgeDistance;
            }
            float leftEdgeToScreenEdgeDistance = 0 - (newPos.x - popupObject.rect.width * popupCanvas.scaleFactor / 2) + padding;
            if (leftEdgeToScreenEdgeDistance > 0)
            {
                newPos.x += leftEdgeToScreenEdgeDistance;
            }
            float topEdgeToScreenEdgeDistance = Screen.height - (newPos.y + popupObject.rect.height * popupCanvas.scaleFactor) - padding;
            if (topEdgeToScreenEdgeDistance < 0)
            {
                newPos.y += topEdgeToScreenEdgeDistance;
            }
            popupObject.transform.position = newPos;
        }

        public void DisplayItemInfo(Item item)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<size=35>").Append(item.ColoredName).Append("</size>").AppendLine();
            builder.Append(item.GetTooltipInfoText());
            infoText.text = builder.ToString();

            popupCanvasObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
        }

        //Change this to Enemy information, such as enemy name, type, etc., after Proof of Concept
        public void DisplayCharacterInfo(Enemy enemy)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<size=35><color=#8f0d2b>").Append(enemy.name).Append("</color></size>").AppendLine();
            builder.Append("<color=#8f0d2b>").Append(enemy.Health).Append("</color> Health").AppendLine();
            infoText.text = builder.ToString();

            popupCanvasObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);

        }

        public void HideInfo()
        {
            popupCanvasObject.SetActive(false);
        }
    }
}