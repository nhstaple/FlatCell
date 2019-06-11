# Game Basic Information #

## Summary ##

Imagine if life evolved from geometry instead of cells. This is the premace beind the idea of Flat Cell. Pieces of geometry evolve from their most basic components (`dots`) to the most complex (`spheres`). The scope of this project was reduced to deliver a playable prototype with just the first stage of evolution- `dot`.

## Gameplay explanation ##

The current devlopment state of the game is alpha. The player can move, shoot, use their shields, boost, interact with the environment and AI. The goal of the game is to destroy AI, pickup their stats, level up, and evolve. Due to time contraints the game demo is over after the player reaches 20 kills.

# Main Roles #

Below is a template for you to highlight items of your work. These provide the evidence needed for your work to be evaluated. Try to have at least 4 such descriptions. They will be assessed on the quality of the underlying system and how they are linked to course content. 

*Short Description* - Long description of your work item that includes how it is relevant to topics discussed in class. [link to evidence in your repository](https://github.com/dr-jam/ECS189L/edit/project-description/ProjectDocumentTemplate.md)

Here is an example:  
*Procedural Terrain* - The background of the game consists of procedurally-generated terrain that is produced with Perlin noise. This terrain can be modified by the by the game at run-time via a call to its script methods. The intent is to allow the player to modify the terrain. This system is based off the component design pattern and the procedural content generation portions of the course. [The PCG terrain generation script](https://github.com/dr-jam/CameraControlExercise/blob/513b927e87fc686fe627bf7d4ff6ff841cf34e9f/Obscura/Assets/Scripts/TerrainGenerator.cs#L6).

## User Interface

In the User Interface there exists a canvas that contains all the hearts, shield bar, and score that keeps track of the player kills. The hearts in this case keep track of whether the user gets hit by a projectile by the enemies called dots. The player’s projectiles cannot affect the player’s health and therefore the health will not get affected by getting hit by your own projectiles. The shield bar is on the canvas named WorldOverlay and it keeps track of the Fill Percent and is updated on UI.cs where in void Update () the display shows the FillPercent. This is then attached to the Shield Image and therefore we can see the blue bar decreasing when using the shield and increasing if not used. This was implemented by Kyle Catapuscan and helped by me. The last part Score just keeps track of Myscore and increments when player kills an enemy Dot. There is a title screen and pause screen that you can change back and forth and once score = 20 then you win the game. It’s a straight forward UI with visual representation of the player. I also implemented a throttle when updating health similar to that of the Pikmini Spawner in Homework 3.

## Movement/Physics
##### Nick 
Initial projectile, pickup, and geometry collision. I just set things up so they could be debugged and playtested.

##### Brian
The environment is set to have the rotation and transforms frozen. The boundary has a collider attached that detects objects and keepsthem from leaving. 
The floor is set to trigger with a box that detects the player passing over it in order to change colors. 
The Player, NPC's and projectiles all have colliders with no gravity, and the transform axis out from the screen is frozen.
Everything has the rotation axis out from the screen frozen.
The frozen transform and rotation axes prevent the objects from leaving the plane of play.
We experiemnted with adding ASDR curve to the movement, but due to the erratic behavior it caused we stuck to a more generic movement approach.
The player and NPCs are given a vector of movement and we call MoveTowards on the vector to have them move to that location.
The NPC's movement is generated randomly and would be developed further in later iterations of the game.

## Animation and Visuals
##### David
* Obtained heart through Unity asset store.

##### Nick S.
* Programmatically made player, projectiles, and AI assets available through Unity in-engine tools. See **Game Logic** section for more information.
* Made the `Animation` class for thread safety stuff, and stole the `LineDrawer` class for drawing the forward vectors during debugging.
* Added lerping to the plane collision.
* Added lerping and change of color to the shields.

## Input
#### Kyle
#### The default input configuration is as follows:

Fire1 (Shooting) – Left Mouse Click

Fire2 (Shield) – Right Mouse Click

Movement – W for up, A for left, S for down, and D for right

Boost – Space Bar

Pause Menu – Escape Key

#### The second input configuration is as follows:

Fire1 (Shooting) – Left CTRL

Fire2 (Shield) – Right CTRL

Movement – up arrow for up, left arrow for left, down arrow for down, and right arrow for right

Boost – Space Bar

Pause Menu – Escape Key


#### We have also managed to get inputs mapped accordingly for Playstation 4 and Xbox One controllers when they are connected with a cable.

Movement – left joystick of Xbox or Playstation controller

Shield – Joystick Button 4; correlates to L1 on Playstation Controller and LB (left bumper) on Xbox controller

Shooting – Joystick Button 5; correlates to R1 on Playstation Controller and RB (right bumper) on Xbox controller

Boost – Joystick Button 3; correlates to Triangle on Playstation Controller and Y button on Xbox controller


## Game Logic
#### Nick S.

I've documentated quite a lot in the [Dev Wiki](https://github.com/nhstaple/FlatCell/wiki/Dev). I was responsible for the [Interfaces](https://github.com/nhstaple/FlatCell/wiki/Interfaces), [Spawner](https://github.com/nhstaple/FlatCell/wiki/Spawner), AI, shooting, programmatic asset generation, ... Here is a brief overview of what I handled as Game Logician. For more information about the technical details see the [Dev Wiki](https://github.com/nhstaple/FlatCell/wiki/Dev).

I was also the _git master_ for this assignment.

### Asset Generation
#### [Projectiles](https://github.com/nhstaple/FlatCell/blob/c7c58a1c8f4b5e3cf7b76719fe2783dc9fc3c64b/ProjectFiles/FlatCell/Assets/Scripts/Weapons%20%26%20Projectiles/ProjectileObject.cs#L55)
#### [Dots](https://github.com/nhstaple/FlatCell/blob/c7c58a1c8f4b5e3cf7b76719fe2783dc9fc3c64b/ProjectFiles/FlatCell/Assets/Scripts/Geos/DotObject.cs#L28)
I made the use of interfaces that call `GameObject Spawn(Vector3 Loc)` to programmatically generate assets through `new GameObject()`.

### [Inheritance](https://github.com/nhstaple/FlatCell/blob/c7c58a1c8f4b5e3cf7b76719fe2783dc9fc3c64b/ProjectFiles/FlatCell/Assets/Scripts/Geos/GeoObject.cs#L163)
All `geo`s derive from a base `GeoObject` class. This class implmenets the `IGeo` interfaces, which means all `geo` objects can be added to `GameObjects` and references via a `IGeo` interfaces via its methods.

### [Factories](https://github.com/nhstaple/FlatCell/blob/c7c58a1c8f4b5e3cf7b76719fe2783dc9fc3c64b/ProjectFiles/FlatCell/Assets/Scripts/Spawner/DotSpawner.cs#L68)
The `ISpawner` interfaces acts as a factory for spawning AI. We have one spawner that a `DotSpawner.cs` is attached to. The ideas was to have n spawners attached to it to handle the different types of `geo`s, ie `dot`, `line`, `triangle`, etc.

### [AI Stats and Evolution](https://github.com/nhstaple/FlatCell/blob/c7c58a1c8f4b5e3cf7b76719fe2783dc9fc3c64b/ProjectFiles/FlatCell/Assets/Scripts/Controllers/SimpleDotBehaviour.cs#L84)
Each `geo` has a pointer referencing what hit it last. When that AI dies, the pointer is called and the killer's `killHistory<string, int>` hash table is updated. Each from the AI's score is checked and their stats are updated accordingly.

# Sub-Roles

## Audio
#### Megan
#### List of assets:

•	Background Music: “Neon Cars” | MelodyLoops.com | free licensed preview | https://www.melodyloops.com/my-music/longoloops/neon-cars/

•	Hit Sound: “Impact_004” | Freesound.org | Creative Commons licensed sound| https://freesound.org/people/AlienXXX/sounds/196215/

•	Death Sound: “Game Over Arcade” | Freesound.org | Creative Commons licensed sound| https://freesound.org/people/myfox14/sounds/382310/

•	Shoot Sound: “SFX Laser Shoot 02” | Freesound.org | Creative Commons licensed sound| https://freesound.org/people/bolkmar/sounds/421704/

•	Start Menu Music: “8-Bit March” | twinmusicom.org | Creative Commons licensed sound| http://www.twinmusicom.org/

#### Implementation:

•	The background music is attached to the script that makes the colorful grid, makePlaneGrid.cs, and with play on awake.

•	The start menu music is attached to the title screen and is played on awake.

•	The shoot, hit, and death sounds are all attached to the player, so you do not hear extraneous NPC sounds. They have a higher priority and volume than the background music so that they can be distinctly heard. 

#### Sound style:
•	The sounds are inspired by and aim to bring you back to the stand-up retro arcade games. The “Death Sound” is similar to the sound played when the player dies in Galaga. The “Hit Sound” and “Shoot Sound” are akin to those you would hear in shooting type arcade games. In addition, the background music is entitled “Neon Cars” which ties together the retro feel to our colorful geometry game. 


## Gameplay Testing
#### Link for Raw Comments: https://docs.google.com/document/d/1rYI942jcIvSDMTHJXfsBOHByzJLrH4nIpnVnDgs6cdE/edit?usp=sharing

Our gameplay testing and observations on Friday (June 7) yielded some very helpful input on what items we could alter to potentially make our game better. Some people stated that the gameplay was fun and that the colors and movement mechanics help up decently. However, there were also some very noteworthy critiques, including an improvement of the end goals of the game and a connection between the different enemy colors that affected how gameplay was conducted.
The playtesters typically praised some of the simple visuals and basic movement based upon Project 2. They enjoyed how our game demo retained some of the circular-based movement and boosting mechanics previously seen before. Additionally, other players enjoyed the aesthetic of the background tiles changing colors to grey when the player game object overlapped them. In fact, it was so appealing that a few players felt compelled to run over all of the tiles in the game to change them all to grey. However, there were still some significant criticisms regarding what could be added or adjusted later to improve the game.

#### Some of the criticisms of the game are as follows:

•	The projectiles could be more pronounced and improved so that they didn’t have unusual physics or end up killing the player. Someone suggested making them bigger so that players would know where they are. However, some people did enjoy the idea of bullets being able to kill the player themselves.

•	The various colors used in the game (red, green, blue for enemies, multicolor for the environment tiles) could have adjustments to their code so that they could be made more relevant to the gameplay or have a more important effect.

•	While it is understood that the game was still in development, the players complained how there was no definite end goal for the game.
•	The health system (hearts) could be brighter to indicate better the life of the player.

•	The AI dots could use improvements such as more frequent spawning and improved behavior (such as knowing when to rotate toward and turn to the player).

•	The damage system could be improved by tracking player collision and subsequent damage much better.

•	Shielding seemed unnecessary due to the lack of a challenge or NPCs.

•	Since players were usually conducting tests on an attached Xbox One controller, some suggested having a second form of input for shooting, such as allowing for the mouse or a second joystick to determine the direction the player is firing. Alternatively, an arrow or different player shape would be useful for helping players determine what direction would be firing.

#### Some of these suggestions have been taken into consideration now. Particularly, we have adjusted player projectiles to not kill the player themselves and added a pointer to indicate the direction the player and NPCs are firing. Additionally, we adjusted the NPCs to spawn and fire more frequently in the game. However, some have not been fully added in due to time constraints.


## [Narrative Design](https://github.com/nhstaple/FlatCell/wiki/Narrative)
#### Nick S.
Imagine if life evolved with geometry/color instead of biology. This is presented in the game the following ways:

### The Terrain
The terrain is mutable and reacts to the the actions of the world's inhabitants. This reflects how one can influence the real world through their actions.

### AI
The AI are independent (at the moment purely random with no intelligence) and increase their stats the more kills that they acquire. This mimics evolution which allows the AI to improve as the game simulation runs.

### Player Color
The player's color starts off as nil. Only the AI have color and the player changes their color by killing enemy AI, which drop a portion of their stats for the player to pickup and evolve.

### Evolution *Not Implmented*
The player and enemy AI can morph from `dot` -> `line` -> `triangle` -> `square` ... all the way to a `sphere`. Do to the scope, and a bug in Unity's collider scaling, we reduced the scope to just implement the `dot` stage of evolution. However I did [implement Speed and Damage levelling up.](https://github.com/nhstaple/FlatCell/blob/c7c58a1c8f4b5e3cf7b76719fe2783dc9fc3c64b/ProjectFiles/FlatCell/Assets/Scripts/Controllers/SimpleDotBehaviour.cs#L140)

## Press Kit and Trailer

**Include links to your presskit materials and trailer.**

**Describe how you showcased your work. How did you choose what to show in the trailer? Why did you choose your screenshots?*
[Trailer](https://drive.google.com/file/d/1uV2ZAV5FoKvD1dfD0247UjOSvw1lvcT3/view?usp=sharing)

The trailer for the game ended up being a 2 minute video of gameplay. We ran out of time due to finalizing and fixing bugs for the game so there wasn't enough time to edit the video from the final build. 
EDIT: Trailer finished, sorry for ugly watermark.
The trailer included the Title Screen, Player, NPCs, Attack, Shield, Boost, Color change mechanics, and the win screen.
The Press Kit is spread across a couple pages in the github and unfortunately we ran out of time to consolidate it together.

## Game Feel

A lerp change of color on grid was added to the game. This means that the time that the tiles change is random. In addition, I made the heart change color to blue when the shield is pressed and reverted to red once the shield is depleted. There is also a trail that follows the Player to keep track of the player when moving. All this improves the feeling of moving the player and by moving the player over a tile on the grid, the player changes the color of the tile with its respective color. The Dots also have the ability to change the tile of the grid with their respective color and can bring a fun game of cat and mouse as the player tries to color the grid that the Dot changes. A lot of the game feel portion comes from the player interacting with the Dots and the grid. The tile changing color is done in PlaneCollision.cs and the Hearts changing color to blue whenever the shield is turned on is in UI.cs. The camera of the game is kept as Position Lock Lerp to further enhance the game feel of the camera slowly trailing behind the player. If you click Main Camera you would be able to change the LerpDuration and see that the Player is the target. This camera script was taken from Homework 2 of this class. 
