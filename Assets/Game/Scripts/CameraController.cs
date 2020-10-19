using UnityEngine;

namespace Assets.Game.Scripts
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] float rotationSpeed = 3f;
        [SerializeField] float scrollSpeed = 6f;
        [SerializeField] Transform target = null;
        [SerializeField] float distanceFromTarget = 2f;
        [SerializeField] Vector2 distanceMinMax = new Vector2(2f, 15f);
        [SerializeField] Vector2 verticalMinMax = new Vector2(-5, 85);
        [SerializeField] float rotationSmoothTime = .12f;

        Vector3 rotationSmoothVelocity;
        Vector3 currentRotation;

        float mouseX, mouseY, mouseScroll;

        void Start()
        {
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
            mouseY = (verticalMinMax.x + verticalMinMax.y) / 2;
        }

        private void LateUpdate()
        {
            CameraMovement();
        }

        void CameraMovement()
        {
            if (Input.GetMouseButton(2))
            {
                mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
                mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            }
            mouseScroll -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            mouseScroll = Mathf.Clamp(mouseScroll, distanceMinMax.x, distanceMinMax.y);
            mouseY = Mathf.Clamp(mouseY, verticalMinMax.x, verticalMinMax.y);

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(mouseY, mouseX), ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotation;

            distanceFromTarget = mouseScroll;

            transform.position = target.position - transform.forward * distanceFromTarget;
        }
    }
}
