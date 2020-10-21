using Assets.Game.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRoamTarget : MonoBehaviour
{
    [SerializeField] float speed = 10f;


    private CharacterController controller;
    private CameraController cam;
    private Transform playerTarget = null;

    void Start()
    {
        cam = FindObjectOfType<CameraController>();
        playerTarget = cam.playerTarget;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (cam.followPlayer) return;

        HandleBaseMovement();
    }

    private void HandleBaseMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        if (direction.magnitude >= 0.1f)
        {
            //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(direction.normalized * speed * Time.deltaTime);
        }
    }
}
