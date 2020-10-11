using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //Camera fields
    public Camera cameraObject;
    [SerializeField]
    float totalCamHeight;
    [SerializeField]
    float totalCamWidth;

    //The asteroid's fields
    [SerializeField]
    Vector3 asteroidPosition;
    [SerializeField]
    public Vector3 direction;
    float xDir;
    float yDir;
    float speed = .025f; //less than speed of the vehicle

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.main;
        //Find total height and width of the view
        totalCamHeight = 2f * cameraObject.orthographicSize;
        totalCamWidth = totalCamHeight * cameraObject.aspect;

        //Direction should be randomized upon start
        //Don't want either of the values to be 0, bc if both are 0 no movement will occur
        xDir = Random.Range(-1f, 2f);
        while (xDir == 0) //In the case that 0 keeps getting rolled
        {
            xDir = Random.Range(-1f, 2f);
        }
        yDir = Random.Range(-1f, 2f);

        while (yDir == 0) //In the case the 0 keeps getting rolled
        {
            yDir = Random.Range(-1f, 2f);
        }

        //Set this asteroid's direction
        direction = new Vector3(xDir, yDir);
        direction.Normalize();

        //The position is where this asset is
        asteroidPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //On each frame, the position should update by the amount specified
        asteroidPosition += direction * speed;

        //Update the position of this asteroid accordingly
        this.transform.localPosition = asteroidPosition;

        // Check if not on viewport
        // Note - width / height must be divided by two because origin is centered.
        if (asteroidPosition.x < ((-totalCamWidth / 2)))
        {
            asteroidPosition = new Vector3((totalCamWidth / 2), asteroidPosition.y, 0f);
        }
        if (asteroidPosition.x > (totalCamWidth / 2))
        {
            asteroidPosition = new Vector3((-totalCamWidth / 2), asteroidPosition.y, 0f);
        }
        if (asteroidPosition.y < ((-totalCamHeight / 2)))
        {
            asteroidPosition = new Vector3(asteroidPosition.x, (totalCamHeight / 2), 0);
        }
        if (asteroidPosition.y > (totalCamHeight / 2))
        {
            asteroidPosition = new Vector3(asteroidPosition.x, (-totalCamHeight / 2), 0);
        }
    }
}
