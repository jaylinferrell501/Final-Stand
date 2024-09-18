using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is used to control the mouse movement of the player

public class MouseMovement : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    [SerializeField] private float m_mouseSensitivity = 600f;   // Mouse sensitivity

    private float m_xAxisRotation = 0f;   // Rotation of the player X-axis
    private float m_yAxisRotation = 0f;   // Rotation of the player Y-axis

    [Header("Clamp")]
    [SerializeField] private float m_topClamp = -90f;   // Top clamp of the player rotation
    [SerializeField] private float m_bottomClamp = 90f;   // Bottom clamp of the player rotation

    // Start is called before the first frame update
    private void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        // Get the mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * m_mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * m_mouseSensitivity * Time.deltaTime;

        // Rotate the player on the X-axis (up and down) 
        m_xAxisRotation -= mouseY;

        // Clamp the rotation of the player on the X-axis
        m_xAxisRotation = Mathf.Clamp(m_xAxisRotation, m_topClamp, m_bottomClamp);

        // Rotate the player on the Y-axis (left and right)
        m_yAxisRotation += mouseX;

        // Use the rotation values to rotate the player
        transform.localRotation = Quaternion.Euler(m_xAxisRotation, m_yAxisRotation, 0f);
    }
}
