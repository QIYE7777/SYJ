using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public float speed = 6f;
    public float turnSpeed = 20f;
    public Vector3 movement { get; private set; }
    Animator anim;
    Rigidbody playerRigidbody;
    public int floorMask;
    float camRayLength = 100f;
    public Vector3 playerToMouse;

    Quaternion m_Rotation = Quaternion.identity;

    bool _mouseButtonDown;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            _mouseButtonDown = true;
        if (Input.GetMouseButtonUp(0))
            _mouseButtonDown = false;

        float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        float v = CrossPlatformInputManager.GetAxisRaw("Vertical");
        Move(h, v);
        Animating(h, v);

        if (_mouseButtonDown)
            TurningMouse();
        else
            //TurningKeyboard();
            TurningMouse();
    }

    void Move(float h, float v)
    {
        movement = new Vector3(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    void TurningKeyboard()
    {
        //Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);
        //m_Rotation = Quaternion.LookRotation(desiredForward);
        var rotateDir = movement;
        rotateDir.y = 0;
        if (rotateDir.magnitude == 0)
            return;

        m_Rotation = Quaternion.LookRotation(movement);
        playerRigidbody.MoveRotation(m_Rotation);
    }

    void TurningMouse()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, (1 << floorMask)))
        {
            //Debug.Log(floorHit.point);
            playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotatation);
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}