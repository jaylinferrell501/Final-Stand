using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    private CharacterController m_controller;   // Character m_controller of the player

    [Header("Settings")]
    [SerializeField] private float m_speed = 12f;   // m_speed of the player
    [SerializeField] private float m_gravity = -9.81f * 2;   // m_gravity of the player
    [SerializeField] private float m_jumpHeight = 3f;   // Jump height of the player
    [SerializeField] private Transform m_groundCheck;   // Ground check object
    [SerializeField] private float m_GroundDistance = 0.4f;   // Ground distance
    [SerializeField] private LayerMask m_groundMask;   // Ground mask

    private Vector3 m_velocity;   // Velocity of the player
    private bool m_isGrounded;   // Is the player grounded
    private Vector3 m_lastPosition = new Vector3(0f, 0f, 0f);  // Last position of the player

    // Start is called before the first frame update
    private void Start()
    {
        // Get the character m_controller of the player
        m_controller = GetComponent<CharacterController>();   
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the player is grounded
        m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_GroundDistance, m_groundMask);

        // Reset the m_velocity of the player if the player is grounded
        if (m_isGrounded && m_velocity.y < 0)
            m_velocity.y = -2f;

        // Get inputs for the player movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Create the movement vector
        Vector3 move = transform.right * x + transform.forward * z;

        // Move the player
        m_controller.Move(move * m_speed * Time.deltaTime);

        // Check if the player can jump
        if (Input.GetButtonDown("Jump") && m_isGrounded)
            m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity); // Jump

        // Apply gravity to the player
        m_velocity.y += m_gravity * Time.deltaTime;

        // Execute the jump
        m_controller.Move(m_velocity * Time.deltaTime);

        // Update the last position of the player
        m_lastPosition = transform.position;
    }
}
