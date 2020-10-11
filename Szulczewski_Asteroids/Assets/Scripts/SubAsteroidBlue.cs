using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubAsteroidBlue : MonoBehaviour
{
    //Camera fields
    public Camera cameraObject;
    [SerializeField]
    float totalCamHeight;
    [SerializeField]
    float totalCamWidth;

    //The sub-asteroid's fields
    [SerializeField]
    Vector3 asteroidPosition;
    [SerializeField]
    Vector3 direction;
    float xDir;
    float yDir;
    float speed = .02f; //less than speed of the vehicle/asteroid

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.main;
        //Find total height and width of the view
        totalCamHeight = 2f * cameraObject.orthographicSize;
        totalCamWidth = totalCamHeight * cameraObject.aspect;

        //Find x and y direction (should be slight variance from parent asteroid)
        xDir = Random.Range(GameObject.Find("blueAst1(Clone)").GetComponent<Transform>().position.x * -.5f,
            GameObject.Find("blueAst1(Clone)").GetComponent<Transform>().position.x + .5f);
        while (xDir == 0)
        {
            xDir = Random.Range(GameObject.Find("blueAst1(Clone)").GetComponent<Transform>().position.x - .5f,
            GameObject.Find("blueAst1(Clone)").GetComponent<Transform>().position.x + .5f);
        }

        yDir = Random.Range(GameObject.Find("blueAst1(Clone)").GetComponent<Transform>().position.y - .5f,
            GameObject.Find("blueAst1(Clone)").GetComponent<Transform>().position.y + .5f);
        while (xDir == 0)
        {
            yDir = Random.Range(GameObject.Find("blueAst1(Clone)").GetComponent<Transform>().position.y - .5f,
            GameObject.Find("blueAst1(Clone)").GetComponent<Transform>().position.y + .5f);
        }

        //Set THIS direction
        direction = new Vector3(-xDir, -yDir);
        direction.Normalize();

        //When the position is 0,0, the asteroid will not move.
        if (this.transform.position == Vector3.zero)
        {
            asteroidPosition = new Vector3(1f, 1f);
        }
        else
        {
            asteroidPosition = this.transform.position;
        }
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
