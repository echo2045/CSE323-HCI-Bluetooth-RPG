using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;

public class BluetoothReceiver : MonoBehaviour
{
    private SerialPort serialPort;
    public string portName = "CNCA0"; // Adjust COM port as needed
    public int baudRate = 115200; // Adjust baud rate as needed
    private string receivedMessage;

    private Rigidbody2D rb;
    private Animator anim;
    public float MovementSpeed;
    private Vector2 MovementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        // Initialize the serial port
        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = 1000;

        // Open the serial port
        try
        {
            serialPort.Open();
            Debug.Log("Serial port opened successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    void Update()
    {

    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    void FixedUpdate()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                receivedMessage = serialPort.ReadLine();
                Move();
                Animate();
            }
            catch (System.TimeoutException)
            {
                // Handle timeout exception
            }
        }


        
    }

    private void Move()
    {
        float Horizontal = 0;
        float Vertical = 0;
        if (receivedMessage == "stop moving") { Horizontal = 0; Vertical = 0; }
        else if (receivedMessage == "move forward") { Horizontal = 0; Vertical = 1; }
        else if (receivedMessage == "move backward") { Horizontal = 0; Vertical = -1; }
        else if (receivedMessage == "move right") { Horizontal = 1; Vertical = 0; }
        else if (receivedMessage == "move left") { Horizontal = -1; Vertical = 0; }

        //= Input.GetAxis("Horizontal");
        //= Input.GetAxis("Vertical");


        if (Horizontal == 0 && Vertical == 0)
            {
                rb.velocity = Vector2.zero;
                return;
            }

        MovementInput = new Vector2(Horizontal, Vertical);
        rb.velocity = MovementInput * MovementSpeed * Time.fixedDeltaTime;
    }

    private void Animate()
    {
        anim.SetFloat("MovementX", MovementInput.x);
        anim.SetFloat("MovementY", MovementInput.y);
    }
}
