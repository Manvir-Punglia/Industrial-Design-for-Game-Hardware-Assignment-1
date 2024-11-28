using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoPlayerController : MonoBehaviour
{
    public SerialController serialController;   
    public GameObject playerCapsule;           
    public float rotateSpeed = 1000.0f;        
    public float moveSpeed = 10.0f;            
    public float jumpForce = 5.0f;             
    public Rigidbody rb;                       
    private bool isGrounded = true;            

    int horizontalValue = 512;
    int verticalValue = 512;

    void Start()
    {
        // Ensure Rigidbody is assigned
        if (rb == null)
        {
            rb = playerCapsule.GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        string message = serialController.ReadSerialMessage();

        if (message != null)
        {
            string[] data = message.Split(',');

            foreach (string div in data)
            {
                if (div.StartsWith("H:"))
                {
                    int.TryParse(div.Substring(2), out horizontalValue);
                }
                if (div.StartsWith("V:"))
                {
                    int.TryParse(div.Substring(2), out verticalValue);
                }
            }

            float normalizedHorizontal = (verticalValue - 512) / 512f;
            float normalizedVertical =  (horizontalValue - 512) / 512f;     

            // Rotate the Rigidbody

            if (verticalValue > 25 || horizontalValue < 25)
            {
                Quaternion targetRotation = rb.rotation * Quaternion.Euler(-normalizedVertical * rotateSpeed * Time.deltaTime, -normalizedHorizontal * rotateSpeed * Time.deltaTime, 0);
                rb.MoveRotation(targetRotation);
            }
            

            // Process button presses
            foreach (string div in data)
            {
                if (div.Contains(":1"))  // Button pressed true
                {
                    Vector3 movement = Vector3.zero;

                    if (div.StartsWith("W:"))
                    {
                        movement += playerCapsule.transform.forward * moveSpeed * Time.deltaTime;
                    }
                    if (div.StartsWith("A:"))
                    {
                        movement -= playerCapsule.transform.right * moveSpeed * Time.deltaTime;
                    }
                    if (div.StartsWith("S:"))
                    {
                        movement -= playerCapsule.transform.forward * moveSpeed * Time.deltaTime;
                    }
                    if (div.StartsWith("D:"))
                    {
                        movement += playerCapsule.transform.right * moveSpeed * Time.deltaTime;
                    }

                    rb.MovePosition(rb.position + movement);

                    if (div.StartsWith("Space:") && isGrounded)
                    {
                        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                        isGrounded = false;
                    }
                }
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Check if grounded
        isGrounded = true;
    }
}
