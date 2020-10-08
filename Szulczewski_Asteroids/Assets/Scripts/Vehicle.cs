using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour

{
    //Camera fields
    public Camera cameraObject;
    [SerializeField]
    float totalCamHeight;
    [SerializeField]
    float totalCamWidth;

    //Vehicle fields
    [SerializeField]
    Vector3 vehiclePosition = Vector3.zero;
    [SerializeField]
    Vector3 direction = Vector3.right;
    [SerializeField]
    Vector3 velocity = Vector3.zero;

    //Not needed bc speed changes every frame
    //[SerializeField]
    //float speed = 0.1f;

    [SerializeField]
    float maximumSpeed;
    // How fast we want the vehicle to turn
    [SerializeField]
    float turnSpeed;

    // Accel vector will calculate the rate of change per second
    [SerializeField]
    Vector3 acceleration;
    public float accelerationRate;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.main;

        //Find total height and width of the view
        totalCamHeight = 2f * cameraObject.orthographicSize;
        totalCamWidth = totalCamHeight * cameraObject.aspect;

        //Initialize our fields
        maximumSpeed = .1f;
        turnSpeed = 3f;
        acceleration = new Vector3(0, 0, 0);
        accelerationRate = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        // Determine if car should accelerate or decelerate
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //speed up
            //Calculate the acceleration vector
            acceleration = direction * accelerationRate;
            //Add acc to velocity
            velocity += acceleration;
            //Limit the velocity
            velocity = Vector3.ClampMagnitude(velocity, maximumSpeed);
        }
        else
        {
            //slow down
            //Velocity will decrease over time
            velocity = velocity * .95f;
        }

        // Determine if the car should turn
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Move left
            //Rotate 1 degrees each frame
            direction = Quaternion.Euler(0, 0, turnSpeed) * direction;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //Move right
            //Rotate 1 degrees each frame
            direction = Quaternion.Euler(0, 0, -turnSpeed) * direction;
        }

        //Update position, then draw
        vehiclePosition += velocity;
        transform.position = vehiclePosition;
        //Set rotation to match the direction
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);


        // Check if not on viewport
        // Note - width / height must be divided by two because origin is centered.
        if (vehiclePosition.x < ((-totalCamWidth / 2)))
        {
            vehiclePosition = new Vector3((totalCamWidth / 2), vehiclePosition.y, 0f);
        }
        if (vehiclePosition.x > (totalCamWidth / 2))
        {
            vehiclePosition = new Vector3((-totalCamWidth / 2), vehiclePosition.y, 0f);
        }
        if (vehiclePosition.y < ((-totalCamHeight / 2)))
        {
            vehiclePosition = new Vector3(vehiclePosition.x, (totalCamHeight / 2), 0);
        }
        if (vehiclePosition.y > (totalCamHeight / 2))
        {
            vehiclePosition = new Vector3(vehiclePosition.x, (-totalCamHeight / 2), 0);
        }

    }
}
