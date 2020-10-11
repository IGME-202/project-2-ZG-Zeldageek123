# Project Documentation

-   Name: Cathryn Szulczewski
-   Section: 05
-   Assignment: Project 2 - Asteroids

## Description

This project exists to showcase my skills in implementing my own collision and vector movement / acceleration logic, as well as logic about game states, GUI, and interaction across many different GameObjects / prefabs. It is a modified version of the arcade classic Asteroids. I used position, velocity, and acceleration vectors for asteroid, bullet, and ship movement, used logic utilizing values from the main camera object to wrap asteroids and the ship around the screen, used both AABB and Circle-based collision logic to detect for collision among my GameObjects utilized random for determining random spawn points for asteroids and also for determining the trajectory of smaller asteroids broken from the bigger ones. I also dabbled in pixel art and used my own assets for this project!
	In my version of Asteroids, 4 asteroids, one of each color that I made, (blue, red, green and yellow) are spawned upon start. GUI is displayed in the upper left and upper right corner of the viewport - to the left are markers indicating current score and current lives, while instructions about changing the collision method between the asteroids / ship are displayed in the upper right.  If the user runs into an asteroid, they lose a life and gain 180 frames of invincibility. When 3 lives are lost and lives = 0, the game is over and the ship is removed from the scene. The “Game Over” and “You Lose!” messages will display to the user in the center and upper left of the viewport, respectively. 
	The user shoots bullets with the spacebar, rotates the ship with the left and right arrow keys, and accelerates to a maximum speed by pressing the up key. When the up key is released after movement, the ship decelerates and comes to a gradual halt. One bullet is shot with every press of the spacebar, with a limit of 5 being active in the scene at once. Bullets do not disappear upon colliding with an asteroid - they can go through and damage others. Bullets do not harm the player. A bullet breaks all asteroids upon collision - The player earns 20 points for destroying one of the first 4 (larger) asteroids and 50 points for destroying one of the smaller asteroids. If a player destroys all of the asteroids, attaining 480 points, they win. “You Win!” displays in the upper right corner and the player is free to move and shoot in the empty space all they want until they close the game. “GAME WON!” also displays in the center of the viewport.
	As a note, the smaller asteroids and the larger asteroids are NOT related by a parent /  child relationship explicitly.
	The Vehicle script is responsible for the spawning and movement of the ship, including logic for spinning, logic for its acceleration and maximum velocity, as well as logic for its screen-wrapping. 
	The Bullet script is responsible for determining the initial direction and position of a bullet that is shot, as well as updating its position using a constant speed.
	The Asteroid script is applied ONLY to the parent (larger) asteroids that are seen in the game at start. It is responsible for randomizing their initial direction vectors and updating their position due to a constant speed. It is also responsible for wrapping the asteroids around the screen.
	The SubAsteroidBlue/Red/Green/Yellow scripts are applied ONLY to the child (smaller) asteroids that are instantiated when a larger asteroid is destroyed. They are responsible for varying the direction of “child” asteroids from the “parent” using a random range, updating their position due to a constant speed, and wrapping child asteroids around the screen. They are implemented as separate scripts because I implemented my original assets to be separate types of prefabs, and I used GameObject.Find(" ”) to find the position of the “parent” asteroid’s position, from which I derived and altered the direction vector.
	The CollisionDetection Script is applied to an empty GameObject called “Manager” and is where most of my logic takes place. It is responsible for:
Creating and placing the 4 original asteroid prefabs somewhere on the viewport, rotating each prefab a random number of degrees to make them look different every time
The logic for being able to switch between two collision check modes between the asteroids and the ship - AABB and Circle Detection.
Helper methods that calculate collisions using AABB and Circle Detection.
Collision logic for the asteroids / ship, with 180 frames of cooldown allotted after an impact and a change in the color of the objects colliding to red.
Keeping track of and displaying the current amount of lives - one is lost with every collision with an asteroid, given that it isn’t within the 180 frames of invincibility.
Keeping track of and displaying the current score - increases by 20 for eliminating an original asteroid, increases by 50 for eliminating a “child” asteroid.
Instantiating a bullet object upon pressing the spacebar and storing them in a list, limiting the list to a count of 5 (and thus deleting the oldest object if a new one is fired).
Collision checking between the bullets and all of the asteroid objects
There are conditions for all of the different asteroids being hit.
When a “parent” asteroid is hit, the score is increased by 20 and two of the smaller asteroids of the proper color are given its position, instantiated, and placed in the asteroid list. Then, the asteroid is destroyed and removed from the list.
When a “child” asteroid is hit, the score is increased by 50. The object is destroyed from the unity scene and removed from the asteroid list.
Displaying all GUI to the user:
The number of lives
The current score
“You Win” or “You Lose!” when score = 480 or lives = 0, respectively.
“GAME OVER!!!” when the player’s lives = 0
“GAME WON!!!” when the player score = 480.

## User Responsibilities

 - Default collision check type between asteroids and the ship is set to Circle Detection - press 1 to toggle to AABB, and 2 to go back to Circles.
 - Players can rotate the ship left and right with the left and right arrow keys, and can accelerate to a maximum velocity with the up arrow key. Upon release of the up arrow          key, the ship decelerates to a halt.
 - Ship shoots bullets with every press of the spacebar in the direction the ship is facing.
 - Upon winning, players can still explore / move in the game space.
 - Upon losing, the player ship is destroyed. You can watch the asteroids float by.


## Above and Beyond <kbd>OPTIONAL</kbd>

What did you add or do in your program to earn above and beyond **bouns** points?

 - I created all of the assets used in the final build of the game (all the asteroids, sub-asteroids, bullets, and the ship) using Piskel. I’m quite proud of them!
 - I included a win condition for the game - destroying all of the asteroid chunks and attaining the score of 480 with at least 1 life left.
 - Included the ability to change detection type between AABB and Circle Detection for the ship and asteroids by pressing 1 or 2 respectively, refactored from a previous                exercise.

## Known Issues

 - With the way I implemented spawning, asteroids can spawn on top of the ship at start, resulting in an immediately lost life. 
 - Not necessarily an issue, but because bullets don’t disappear until they’re deleted (being the oldest in the bullet list when another one is fired), if you aim right, you can        destroy both the big asteroid and the two pieces it breaks into in one shot.


## Requirements not completed

None! I think I got them all.

## Sources

Sprites (Link to my own Piskel Gallery)-
https://www.piskelapp.com/user/6203784403877888


## Notes

 - When the player loses and is destroyed from the scene, the asteroids continue to move in the background and the one that killed the player is still highlighted in red. I           think this is pretty cool.
 - The player can continue moving around and shooting when they’ve won
 - The player can lose a life instantly if an asteroid spawns on top of them.

