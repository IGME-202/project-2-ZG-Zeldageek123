using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public Camera cameraObject;
    [SerializeField]
    float totalCamHeight;
    [SerializeField]
    float totalCamWidth;

    //Two Renderer fields - one for each type of game object
    SpriteRenderer asteroidRender;

    SpriteRenderer shipRender;
    float asteroidRadius;
    float shipRadius;
    Vector3 distance;
    bool shipCollided;

    enum CheckType
    {
        AABB,
        Circle
    }

    [SerializeField]
    CheckType checktype;

    [SerializeField]
    GameObject spaceShip;

    [SerializeField]
    GameObject asteroid;
    float xPos;
    float yPos;

    [SerializeField]
    GameObject[] asteroidArray;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.main;

        //Find total height and width of the view
        totalCamHeight = 2f * cameraObject.orthographicSize;
        totalCamWidth = totalCamHeight * cameraObject.aspect;

        //Making 10 random asteroids
        asteroidArray = new GameObject[10];
        //Random gen their x and y pos. Must be within range of viewport
        for (int i = 0; i < 10; i++)
        {
            xPos = Random.Range((-totalCamWidth / 2) + 1, (totalCamWidth / 2) - 1);
            yPos = Random.Range((-totalCamHeight / 2) + 1, (totalCamHeight / 2) - 1);

            //Create the game object, add to array
            GameObject ast = Instantiate(asteroid, new Vector3(xPos, yPos, 0), Quaternion.identity);
            asteroidArray[i] = ast;
        }

        //Start with AABB
        checktype = CheckType.AABB;
    }

    // Update is called once per frame
    void Update()
    {
        //Change checking type based on last number pressed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            checktype = CheckType.AABB;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            checktype = CheckType.Circle;
        }

        switch (checktype)
        {
            case CheckType.AABB:
                for (int i = 0; i < asteroidArray.Length; i++)
                {
                    if (AABBDetection(spaceShip, asteroidArray[i]))
                    {
                        shipRender.color = Color.red;
                        asteroidRender.color = Color.red;
                        shipCollided = true;
                    }
                    else
                    {
                        asteroidRender.color = Color.white;
                        if (!shipCollided)
                        {
                            //If the ship never collided, THEN change its color back to white
                            //Necessary because ship will be turned white again despite being in contact
                            //  with one asteroid because it isn't in contact with another. 
                            shipRender.color = Color.white;
                        }
                    }
                }
                shipCollided = false;
                break;
            case CheckType.Circle:
                for (int i = 0; i < asteroidArray.Length; i++)
                {
                   if (CircleCollision(spaceShip, asteroidArray[i]))
                   {
                        shipRender.color = Color.red;
                        asteroidRender.color = Color.red;
                        shipCollided = true;
                   }
                   else
                   {
                       asteroidRender.color = Color.white;
                        if (!shipCollided)
                        {
                            //If the ship never collided, THEN change its color back to white
                            //Necessary because ship will be turned white again despite being in contact
                            //  with one asteroid because it isn't in contact with another. 
                            shipRender.color = Color.white;
                        }
                    }
                }
                shipCollided = false;
                break;
        }
    }

    /// <summary>
    /// Checks collision using AABB
    /// </summary>
    /// <returns>True if spaceship collides with a meteor</returns>
    bool AABBDetection(GameObject ship, GameObject asteroid)
    {
        //get both sprite renderers
        //This won't always refer to the same asteroid
        asteroidRender = asteroid.GetComponent<SpriteRenderer>();
        shipRender = ship.GetComponent<SpriteRenderer>();

        //Collision conditions ----------------------------------------
        if (shipRender.bounds.min.x < asteroidRender.bounds.max.x && 
            shipRender.bounds.max.x > asteroidRender.bounds.min.x &&
            shipRender.bounds.max.y > asteroidRender.bounds.min.y &&
            shipRender.bounds.min.y < asteroidRender.bounds.max.y)
        {
            //Only a collision if all these are met
            return true;
        }
        //no collision possible
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Checks collision using bounding circles
    /// </summary>
    /// <returns></returns>
    bool CircleCollision(GameObject ship, GameObject asteroid)
    {
        //get both sprite renderers
        //This won't always refer to the same asteroid
        asteroidRender = asteroid.GetComponent<SpriteRenderer>();
        shipRender = ship.GetComponent<SpriteRenderer>();

        //Find the radius of both using their bounds
        asteroidRadius = (asteroidRender.bounds.size.x) / 2;
        shipRadius = (shipRender.bounds.size.x) / 2;

        //Find the distance between the two points, and store that data in a new vector3
        distance = new Vector3((asteroidRender.bounds.center.x - shipRender.bounds.center.x),
            (asteroidRender.bounds.center.y - shipRender.bounds.center.y));

        //If the sum of the radii is less than the distance between the points
        if ((shipRadius + asteroidRadius) * (shipRadius + asteroidRadius) < distance.sqrMagnitude)
        {
            //No collision
            return false;
        }
        else
        {
            //There must be a collision
            return true;
        }
    }
}
