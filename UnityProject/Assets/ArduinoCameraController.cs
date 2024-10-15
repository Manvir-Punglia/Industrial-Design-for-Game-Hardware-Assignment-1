using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoPlayerController : MonoBehaviour
{
    public SerialController serialController;   
    public GameObject playerCapsule;           
    public float rotateSpeed = 1000.0f;
    int horizontalValue = 512;
    int verticalValue = 512;

    void Update()
    {
        // Read the serial message from Arduino
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

            float normalizedHorizontal = (horizontalValue - 512) / 512f; 
            float normalizedVertical = (verticalValue - 512) / 512f;    

            playerCapsule.transform.Rotate(0, -normalizedHorizontal * rotateSpeed * Time.deltaTime, 0);
            playerCapsule.transform.Rotate(-normalizedVertical * rotateSpeed * Time.deltaTime, 0, 0);

            //button presses 
            foreach (string div in data)
            {
                if (div.Contains(":1"))  // Button pressed true 
                {
                    if (div.StartsWith("W:"))
                    {                 
                        playerCapsule.transform.Translate(Vector3.forward * 100f * Time.deltaTime); 
                    }
                    if (div.StartsWith("A:"))
                    {
                        playerCapsule.transform.Translate(-Vector3.right * 100f * Time.deltaTime);
                    }
                    if (div.StartsWith("S:"))
                    {                     
                        playerCapsule.transform.Translate(-Vector3.forward * 100f * Time.deltaTime);
                    }
                    if (div.StartsWith("D:"))
                    {                       
                        playerCapsule.transform.Translate(Vector3.right * 100f * Time.deltaTime);
                    }
                    if (div.StartsWith("Space:"))
                    {
                       //add jump
                    }
                }
            }
        }
    }
}
