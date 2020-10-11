using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Bullet fields
    [SerializeField]
    Vector3 bulletPosition;
    [SerializeField]
    Vector3 direction;
    float speed = .15f; //Faster than the max speed of .1 for the vehicle

    // Start is called before the first frame update
    void Start()
    {
        //Direction should be the same as the ship's direction in this frame
        direction = GameObject.Find("Ship").GetComponent<Vehicle>().direction;

        //Position should be the same as the vehicle in this frame
        bulletPosition = GameObject.Find("Ship").GetComponent<Vehicle>().vehiclePosition;
    }

    // Update is called once per frame
    void Update()
    {
        //On each frame, the position of the bullet should update by the amount specified
        bulletPosition += direction * speed;

        //Update the position of this bullet accordingly
        this.transform.localPosition = bulletPosition;
    }
}
