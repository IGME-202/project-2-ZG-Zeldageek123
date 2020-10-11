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

    //Three Renderer fields - one for each type of game object
    SpriteRenderer asteroidRender;
    SpriteRenderer shipRender;
    SpriteRenderer bulletRender;

    //Renderers
    float asteroidRadius;
    float shipRadius;
    Vector3 distance;
    bool shipCollided;

    //Invincibility and timer
    bool invincibility;
    int iFrames;

    //The bullets the ship will fire
    List<GameObject> bullets;
    [SerializeField]
    GameObject bullet;
    Vector3 bulletPos;

    //Lives of the ship and current score
    int score;
    int lives;

    enum CheckType
    {
        AABB,
        Circle
    }
    [SerializeField]
    CheckType checktype;

    [SerializeField]
    GameObject spaceShip;

    float astxPos;
    float astyPos;

    // Red ---
    [SerializeField]
    GameObject redAsteroid;
    [SerializeField]
    GameObject redSmall1;
    [SerializeField]
    GameObject redSmall2;
    bool redDestroyed = false;

    // Blue ---
    [SerializeField]
    GameObject blueAsteroid;
    [SerializeField]
    GameObject blueSmall1;
    [SerializeField]
    GameObject blueSmall2;
    bool blueDestroyed = false;

    // Green ---
    [SerializeField]
    GameObject greenAsteroid;
    [SerializeField]
    GameObject greenSmall1;
    [SerializeField]
    GameObject greenSmall2;
    bool greenDestroyed = false;

    // Yellow ---
    [SerializeField]
    GameObject yellowAsteroid;
    [SerializeField]
    GameObject yellowSmall1;
    [SerializeField]
    GameObject yellowSmall2;
    bool yellowDestroyed = false;

    float xPos;
    float yPos;
    float rotation;

    //Big asteroids
    [SerializeField]
    List <GameObject> asteroids;
    //Small asteroids
    [SerializeField]
    List<GameObject> smallAsteroids;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = Camera.main;

        //Find total height and width of the view
        totalCamHeight = 2f * cameraObject.orthographicSize;
        totalCamWidth = totalCamHeight * cameraObject.aspect;

        //score is 0 initially, lives is 3
        score = 0;
        lives = 3;
        invincibility = false;
        iFrames = 0;

        //Making 4 random asteroids
        asteroids = new List<GameObject>();
        //Random gen their x and y pos. Must be within range of viewport
        for (int i = 0; i < 4; i++)
        {
            xPos = Random.Range((-totalCamWidth / 2) + 1, (totalCamWidth / 2) - 1);
            yPos = Random.Range((-totalCamHeight / 2) + 1, (totalCamHeight / 2) - 1);

            //Find random nums for the rotation of the asteroids
            rotation = Random.Range(0, 360);

            //Create the game object, add to array
            //Create colored asteroid based on which number asteroid this is
            if (i == 3)
            {
                GameObject ast = Instantiate(redAsteroid, new Vector3(xPos, yPos, 0), Quaternion.Euler(0, 0, rotation));
                asteroids.Add(ast);
            }
            else if (i == 2)
            {
                GameObject ast = Instantiate(blueAsteroid, new Vector3(xPos, yPos, 0), Quaternion.Euler(0, 0, rotation));
                asteroids.Add(ast);
            }
            else if (i == 1)
            {
                GameObject ast = Instantiate(greenAsteroid, new Vector3(xPos, yPos, 0), Quaternion.Euler(0, 0, rotation));
                asteroids.Add(ast);
            }
            else
            {
                GameObject ast = Instantiate(yellowAsteroid, new Vector3(xPos, yPos, 0), Quaternion.Euler(0, 0, rotation));
                asteroids.Add(ast);
            }
        }

        //Start with Circle
        checktype = CheckType.Circle;

        //initialize bullet list
        bullets = new List<GameObject>();
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
                for (int i = 0; i < asteroids.Count; i++)
                {
                    if (AABBDetection(spaceShip, asteroids[i]))
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
                    //If the ship collided, lose a life
                    if (shipCollided)
                    {
                        lives--;
                    }
                }
                shipCollided = false;
                break;
            case CheckType.Circle:
                for (int i = 0; i < asteroids.Count; i++)
                {
                   if (CircleCollision(spaceShip, asteroids[i]))
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

                if (invincibility == true)  //While true, up the counter
                {
                    iFrames++;
                    if (iFrames >= 180) //3 seconds at 60fps
                    {
                        iFrames = 0;
                        invincibility = false;
                    }
                }
                //If the ship collided, lose a life
                if (shipCollided && invincibility == false)
                {
                    lives--;
                    invincibility = true;
                }
                shipCollided = false;
                break;
        }

        // Determine if a bullet should be fired
        //Only registers first press so user can't hold / spam bullets
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bulletPos = GameObject.Find("Ship").GetComponent<Vehicle>().vehiclePosition;

            //Instantiate a bullet game object
            GameObject bul = Instantiate(bullet, bulletPos, Quaternion.identity);

            //Cap at 5 bullets at a time
            if (bullets.Count < 5) //Under max value
            {
                //Add this to the list
                bullets.Add(bul);
            }
            else //At max, remove oldest
            {
                Destroy(bullets[0]);
                bullets.RemoveAt(0);

                //Add to the list
                bullets.Add(bul);
            }
        }

        //Check for collision between all bullets and asteroids
        for(int i = 0; i < bullets.Count; i++)
        {
            bulletRender = bullets[i].GetComponent<SpriteRenderer>();

            for (int j = 0; j < asteroids.Count; j++)
            {
                asteroidRender = asteroids[j].GetComponent<SpriteRenderer>();

                if (bulletRender.bounds.min.x < asteroidRender.bounds.max.x &&
                bulletRender.bounds.max.x > asteroidRender.bounds.min.x &&
                bulletRender.bounds.max.y > asteroidRender.bounds.min.y &&
                bulletRender.bounds.min.y < asteroidRender.bounds.max.y)
                {
                    //Determine which color this asteroid was, or if it is one of the smaller asts

                    // Big Asteroids -------------------------------------------------------------------------
                    if(asteroids[j] == GameObject.Find("blueAst1(Clone)"))
                    {
                        //Update score
                        score += 20;

                        //Find the x and the y of the parent asteroid
                        astxPos = GameObject.Find("blueAst1(Clone)").GetComponent<Transform>().position.x;
                        astyPos = GameObject.Find("blueAst1(Clone)").GetComponent<Transform>().position.y;

                        //If either of these = 0 just set pos to 1
                        if (astxPos == 0)
                        {
                            astxPos = 1f;
                        }
                        if (astyPos == 0)
                        {
                            astyPos = 1f;
                        }

                        //Add the two corresponding broken pieces to the list
                        asteroids.Add(Instantiate(blueSmall1, new Vector3(astxPos,astyPos,0), Quaternion.identity));
                        asteroids.Add(Instantiate(blueSmall2, new Vector3(astxPos, astyPos, 0), Quaternion.identity));

                        //Del the og
                        Destroy(GameObject.Find("blueAst1(Clone)"));

                        //remove
                        asteroids.Remove(GameObject.Find("blueAst1(Clone)"));
                    }
                    else if (asteroids[j] == GameObject.Find("greenAst1(Clone)"))
                    {
                        //Update score
                        score += 20;

                        //Find the x and the y of the parent asteroid
                        astxPos = GameObject.Find("greenAst1(Clone)").GetComponent<Transform>().position.x;
                        astyPos = GameObject.Find("greenAst1(Clone)").GetComponent<Transform>().position.y;

                        //If either of these = 0 just set pos to 1
                        if (astxPos == 0)
                        {
                            astxPos = 1f;
                        }
                        if (astyPos == 0)
                        {
                            astyPos = 1f;
                        }

                        //Add the two corresponding broken pieces to the list
                        asteroids.Add(Instantiate(greenSmall1, new Vector3(astxPos, astyPos, 0), Quaternion.identity));
                        asteroids.Add(Instantiate(greenSmall2, new Vector3(astxPos, astyPos, 0), Quaternion.identity));

                        //Del the og
                        Destroy(GameObject.Find("greenAst1(Clone)"));

                        //remove
                        asteroids.Remove(GameObject.Find("greenAst1(Clone)"));
                    }
                    else if (asteroids[j] == GameObject.Find("redAst1(Clone)"))
                    {
                        //Update score
                        score += 20;

                        //Find the x and the y of the parent asteroid
                        astxPos = GameObject.Find("redAst1(Clone)").GetComponent<Transform>().position.x;
                        astyPos = GameObject.Find("redAst1(Clone)").GetComponent<Transform>().position.y;

                        //If either of these = 0 just set pos to 1
                        if (astxPos == 0)
                        {
                            astxPos = 1f;
                        }
                        if (astyPos == 0)
                        {
                            astyPos = 1f;
                        }

                        //Add the two corresponding broken pieces to the list
                        asteroids.Add(Instantiate(redSmall1, new Vector3(astxPos, astyPos, 0), Quaternion.identity));
                        asteroids.Add(Instantiate(redSmall2, new Vector3(astxPos, astyPos, 0), Quaternion.identity));

                        //Del the og
                        Destroy(GameObject.Find("redAst1(Clone)"));

                        //remove
                        asteroids.Remove(GameObject.Find("redAst1(Clone)"));
                    }
                    else if (asteroids[j] == GameObject.Find("yellowAst1(Clone)"))
                    {
                        //Update score
                        score += 20;

                        //Find the x and the y of the parent asteroid
                        astxPos = GameObject.Find("yellowAst1(Clone)").GetComponent<Transform>().position.x;
                        astyPos = GameObject.Find("yellowAst1(Clone)").GetComponent<Transform>().position.y;

                        //If either of these = 0 just set pos to 1
                        if (astxPos == 0)
                        {
                            astxPos = 1f;
                        }
                        if (astyPos == 0)
                        {
                            astyPos = 1f;
                        }

                        //Add the two corresponding broken pieces to the list
                        asteroids.Add(Instantiate(yellowSmall1, new Vector3(astxPos, astyPos, 0), Quaternion.identity));
                        asteroids.Add(Instantiate(yellowSmall2, new Vector3(astxPos, astyPos, 0), Quaternion.identity));

                        //Del the og
                        Destroy(GameObject.Find("yellowAst1(Clone)"));

                        //remove
                        asteroids.Remove(GameObject.Find("yellowAst1(Clone)"));
                    }

                    //Collision with sub asteroids -----------------------------------------------------------------

                    else if (asteroids[j] == GameObject.Find("blueAst2(Clone)"))
                    {
                        //Update score
                        score += 50;

                        //Delete
                        Destroy(GameObject.Find("blueAst2(Clone)"));

                        //remove
                        asteroids.Remove(GameObject.Find("blueAst2(Clone)"));
                    }

                    else if (asteroids[j] == GameObject.Find("blueAst3(Clone)"))
                    {
                        //Update score
                        score += 50;

                        //Delete
                        Destroy(GameObject.Find("blueAst3(Clone)"));

                        //remove
                        asteroids.Remove(GameObject.Find("blueAst3(Clone)"));
                    }

                    else if(asteroids[j] == GameObject.Find("redAst2(Clone)"))
                    {
                        //Update score
                        score += 50;

                        //Delete
                        Destroy(GameObject.Find("redAst2(Clone)"));

                        //remove
                        asteroids.Remove(GameObject.Find("redAst2(Clone)"));
                    }

                    else if (asteroids[j] == GameObject.Find("redAst3(Clone)"))
                    {
                        //Update score
                        score += 50;

                        //Delete
                        Destroy(GameObject.Find("redAst3(Clone)"));

                        //remove
                        asteroids.Remove(GameObject.Find("redAst3(Clone)"));
                    }

                    else if (asteroids[j] == GameObject.Find("greenAst2(Clone)"))
                    {
                        //Update score
                        score += 50;

                        //Delete
                        Destroy(GameObject.Find("greenAst2(Clone)"));

                        //remove
                        asteroids.Remove(GameObject.Find("greenAst2(Clone)"));
                    }

                    else if (asteroids[j] == GameObject.Find("greenAst3(Clone)"))
                    {
                        //Update score
                        score += 50;

                        //Delete
                        Destroy(GameObject.Find("greenAst3(Clone)"));

                        //remove
                        asteroids.Remove(GameObject.Find("greenAst3(Clone)"));
                    }

                    else if (asteroids[j] == GameObject.Find("yellowAst2(Clone)"))
                    {
                        //Update score
                        score += 50;

                        //Delete
                        Destroy(GameObject.Find("yellowAst2(Clone)"));

                        //remove
                        asteroids.Remove(GameObject.Find("yellowAst2(Clone)"));
                    }

                    else if (asteroids[j] == GameObject.Find("yellowAst3(Clone)"))
                    {
                        //Update score
                        score += 50;

                        //Delete
                        Destroy(GameObject.Find("yellowAst3(Clone)"));

                        //remove
                        asteroids.Remove(GameObject.Find("yellowAst3(Clone)"));
                    }
                }
                //No collision possible
            }
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
    /// Checks collision between ship and asteroids using bounding circles
    /// </summary>
    /// <returns>True if they collided</returns>
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

    /// <summary>
    /// OnGUI()
    /// Displays instructions to the user through GUI for changing camera
    /// </summary>
    void OnGUI()
    {
        //Display instructions
        GUI.Box(new Rect(0, 0, 100, 40), "Lives: " + lives +
            "\n Score: " + score);
        //Display current checktype
        GUI.Box(new Rect(450, 0, 220, 70),
            "Current Asteroid / Ship \nDetection Mode: " + checktype +
            "\n Press 1 for AABB, \n2 for Circle Detection");

        //Display win condition
        if (score == 480 && lives > 0)
        {
            GUI.Box(new Rect(0, 40, 100, 20), "YOU WIN!");
            GUI.Box(new Rect(280, 150, 100, 30), "GAME WON!!!");
        }
        
        if (lives <= 0)
        {
            GUI.Box(new Rect(0, 40, 100, 20), "YOU LOSE!");
            GUI.Box(new Rect(280, 150, 100, 30), "GAME OVER!!!");
            GameObject.Destroy(spaceShip);
        }
    }
}
