# Game Basic Information #

## Summary ##

**A paragraph-length pitch for your game.**

## Gameplay explanation ##

**In this section, explain how the game should be played. Treat this like a manual within a game. It is encouraged to explain the button mappings and the most optimal gameplay strategy.**




# Main Roles #

Your goal is to relate the work of your role and sub-role in terms of the content of the course. Please look at the role sections below for specific instructions for each role.

Below is a template for you to highlight items of your work. These provide the evidence needed for your work to be evaluated. Try to have at least 4 such descriptions. They will be assessed on the quality of the underlying system and how they are linked to course content. 

*Short Description* - Long description of your work item that includes how it is relevant to topics discussed in class. [link to evidence in your repository](https://github.com/dr-jam/ECS189L/edit/project-description/ProjectDocumentTemplate.md)

Here is an example:  
*Procedural Terrain* - The background of the game consists of procedurally-generated terrain that is produced with Perlin noise. This terrain can be modified by the by the game at run-time via a call to its script methods. The intent is to allow the player to modify the terrain. This system is based off the component design pattern and the procedural content generation portions of the course. [The PCG terrain generation script](https://github.com/dr-jam/CameraControlExercise/blob/513b927e87fc686fe627bf7d4ff6ff841cf34e9f/Obscura/Assets/Scripts/TerrainGenerator.cs#L6).

You should replay any **bold text** with your own relevant information. Liberally use the template when necessary and appropriate.

## User Interface

**Describe your user interface and how it relates to gameplay. This can be done via the template.**

## Movement/Physics -Brian

**Describe the basics of movement and physics in your game. Is it the standard physics model? What did you change or modify? Did you make your own movement scripts that do not use the phyics system?**
The environment is set to have the rotation and transforms frozen. The boundary has a collider attached that detects objects and keepsthem from leaving. 
The floor is set to trigger with a box that detects the player passing over it in order to change colors. 
The Player, NPC's and projectiles all have colliders with no gravity, and the transform axis out from the screen is frozen.
Everything has the rotation axis out from the screen frozen.
The frozen transform and rotation axes prevent the objects from leaving the plane of play.
We experiemnted with adding ASDR curve to the movement, but due to the erratic behavior it caused we stuck to a more generic movement approach.
The player and NPCs are given a vector of movement and we call MoveTowards on the vector to have them move to that location.
The NPC's movement is generated randomly and would be developed further in later iterations of the game.

## Animation and Visuals

**List your assets including their sources, and licenses.**

**Describe how your work intersects with game feel, graphic design, and world-building. Include your visual style guide if one exists.**

## Input


# The default input configuration is as follows:

Fire1 (Shooting) – Left Mouse Click

Fire2 (Shield) – Right Mouse Click

Movement – W for up, A for left, S for down, and D for right

Boost – Space Bar

Pause Menu – Escape Key


# The second input configuration is as follows:

Fire1 (Shooting) – Left CTRL

Fire2 (Shield) – Right CTRL

Movement – up arrow for up, left arrow for left, down arrow for down, and right arrow for right

Boost – Space Bar

Pause Menu – Escape Key


# We have also managed to get inputs mapped accordingly for Playstation 4 and Xbox One controllers when they are connected with a cable.

Movement – left joystick of Xbox or Playstation controller

Shooting – Joystick Button 4; correlates to L1 on Playstation Controller and LB (left bumper) on Xbox controller

Shield – Joystick Button 5; correlates to R1 on Playstation Controller and RB (right bumper) on Xbox controller

Boost – Joystick Button 3; correlates to Triangle on Playstation Controller and Y button on Xbox controller


## Game Logic

**Document what game states and game data you managed and what design patterns you used to complete your task.**

# Sub-Roles

## Audio

**List your assets including their sources, and licenses.**

**Describe the implementation of your audio system.**

**Document the sound style.** 

## Gameplay Testing
Link for Raw Comments: https://docs.google.com/document/d/1rYI942jcIvSDMTHJXfsBOHByzJLrH4nIpnVnDgs6cdE/edit?usp=sharing

Our gameplay testing and observations on Friday (June 7) yielded some very helpful input on what items we could alter to potentially make our game better. Some people stated that the gameplay was fun and that the colors and movement mechanics help up decently. However, there were also some very noteworthy critiques, including an improvement of the end goals of the game and a connection between the different enemy colors that affected how gameplay was conducted.
The playtesters typically praised some of the simple visuals and basic movement based upon Project 2. They enjoyed how our game demo retained some of the circular-based movement and boosting mechanics previously seen before. Additionally, other players enjoyed the aesthetic of the background tiles changing colors to grey when the player game object overlapped them. In fact, it was so appealing that a few players felt compelled to run over all of the tiles in the game to change them all to grey. However, there were still some significant criticisms regarding what could be added or adjusted later to improve the game.
	Some of the criticisms of the game are as follows:
•	The projectiles could be more pronounced and improved so that they didn’t have unusual physics or end up killing the player. Someone suggested making them bigger so that players would know where they are. However, some people did enjoy the idea of bullets being able to kill the player themselves.
•	The various colors used in the game (red, green, blue for enemies, multicolor for the environment tiles) could have adjustments to their code so that they could be made more relevant to the gameplay or have a more important effect.
•	While it is understood that the game was still in development, the players complained how there was no definite end goal for the game.
•	The health system (hearts) could be brighter to indicate better the life of the player.
•	The AI dots could use improvements such as more frequent spawning and improved behavior (such as knowing when to rotate toward and turn to the player).
•	The damage system could be improved by tracking player collision and subsequent damage much better.
•	Shielding seemed unnecessary due to the lack of a challenge or NPCs.
•	Since players were usually conducting tests on an attached Xbox One controller, some suggested having a second form of input for shooting, such as allowing for the mouse or a second joystick to determine the direction the player is firing. Alternatively, an arrow or different player shape would be useful for helping players determine what direction would be firing.
Some of these suggestions have been taken into consideration now. Particularly, we have adjusted player projectiles to not kill the player themselves and added a pointer to indicate the direction the player and NPCs are firing. Additionally, we adjusted the NPCs to spawn and fire more frequently in the game. However, some have not been fully added in due to time constraints.


## Narrative Design

**Document how the narrative is present in the game via assets, gameplay systems, and gameplay.** 

## Press Kit and Trailer

**Include links to your presskit materials and trailer.**

**Describe how you showcased your work. How did you choose what to show in the trailer? Why did you choose your screenshots?**
 


## Game Feel

**Document what you added to and how you tweaked your game to improve its game feel.**
