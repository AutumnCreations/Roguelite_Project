using UnityEngine;

namespace Assets.Game.Scripts
{
    public class CameraController : MonoBehaviour
    {

        [Header("Target References")]
        [SerializeField] public Transform playerTarget = null;
        [SerializeField] Transform freeRoamTarget = null;
        [SerializeField] float distanceFromTarget = 2f;
        [SerializeField] float defaultY = 45;
        [SerializeField] Vector2 distanceMinMax = new Vector2(2f, 15f);
        [SerializeField] Vector2 verticalMinMax = new Vector2(-5, 85);

        [Header("Camera Values")]
        [SerializeField] float scrollSpeed = 6f;
        [SerializeField] float rotationSpeed = 3f;
        [SerializeField] float rotationSmoothTime = .12f;
        [SerializeField] public bool followPlayer = true;

        private Transform target = null;
        private Vector3 rotationSmoothVelocity;
        private Vector3 currentRotation;

        float mouseX, mouseY, mouseScroll;

        void Start()
        {
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
            mouseY = defaultY;
            mouseScroll = distanceFromTarget;
            target = playerTarget;
            freeRoamTarget.position.Set(playerTarget.position.x, freeRoamTarget.position.y, playerTarget.position.z);
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //freeRoamTarget.position.Set(playerTarget.position.x, freeRoamTarget.position.y, playerTarget.position.z);
                followPlayer = !followPlayer;
                freeRoamTarget.position = new Vector3(playerTarget.position.x, freeRoamTarget.position.y, playerTarget.position.z);
            }
            CameraFollow();
        }

        private void CameraFollow()
        {
            if (!followPlayer) { target = freeRoamTarget; }
            else { target = playerTarget; }
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
