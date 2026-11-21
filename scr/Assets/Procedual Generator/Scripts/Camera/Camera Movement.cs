using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject m_followTarget;
    [SerializeField] private float m_velociy;
    [SerializeField] private Vector3 m_angle;
    [SerializeField] private Vector3 m_targetOffset;

    [SerializeField] private List<Vector3> m_flightPerimeter;// [0] = Center Pos; [1] = Size;

    private GameInput gameInput;
    private InputAction move;
    private InputAction swap;
    private Vector2 moveValue;
    private List<Vector3> perimeter;
    private bool fixedCamera;


    private void Awake()
    {
        transform.rotation = Quaternion.Euler(m_angle);
        SetPerimeter();
        gameInput = new GameInput();
        fixedCamera = true;
    }

    private void Update()
    {
        if (fixedCamera) transform.position = m_followTarget.transform.position + m_targetOffset;
        else Movement();
    }

    private void SetPerimeter()
    {
        perimeter = new(4);
        var center = m_flightPerimeter[0];
        var size = m_flightPerimeter[1];
        perimeter.Add(new Vector3(size.x / 2, 0, size.z / 2) + center);   // (+) X; (+) Z
        perimeter.Add(new Vector3(size.x / 2, 0, size.z / -2) + center);  // (+) X; (-) Z
        perimeter.Add(new Vector3(size.x / -2, 0, size.z / -2) + center); // (-) X; (-) Z
        perimeter.Add(new Vector3(size.x / -2, 0, size.z / 2) + center);  // (-) X; (+) Z
    }

    private void OnEnable()
    {
        move = gameInput.Camera.Move;
        move.Enable();
        move.performed += OnMoveInput;
        move.canceled += OnMoveInput;

        swap = gameInput.Camera.Swap;
        swap.Enable();
        swap.performed += OnSwapInput;
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void OnSwapInput(InputAction.CallbackContext context)
    {
        fixedCamera = !fixedCamera;
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        moveValue = new Vector2(value.x, value.y) * m_velociy * Time.fixedDeltaTime;
    }


    private void Movement()
    {
        var newPos = new Vector3(transform.position.x + moveValue.x, transform.position.y, transform.position.z + moveValue.y);
        var inside = (newPos.z < perimeter[0].z && newPos.x < perimeter[0].x &&
                          newPos.z > perimeter[2].z && newPos.x > perimeter[2].x);
        if (inside) transform.position = newPos;
    }

    private void OnDrawGizmos()
    {
        SetPerimeter();
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(m_flightPerimeter[0], m_flightPerimeter[1]);
        Gizmos.DrawSphere(perimeter[0], 2);
        Gizmos.DrawSphere(perimeter[1], 2);
        Gizmos.DrawSphere(perimeter[2], 2);
        Gizmos.DrawSphere(perimeter[3], 2);
    }
}
