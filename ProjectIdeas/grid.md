# FlatCell- Grid #

The `game world` of **FlatCell** is a 2D canvas that geometric objects are drawn on. To outline this- we have a 2D grid.

The idea is to have the following layers of reality-

* 2D - flat grid
* 3D - grid is projected onto rolling surfaces, and the player is now a sphere.
* n+D - grid is projected onto a sheet that morphs depending on the objects mass, etc. The player is an orbiting body or somethin.

## 2D ##

The `grid` acts kinda like graph paper. The length of the squares, I'll call it `unit length` should scale to the `evolution
stage` / `player level`. As the player gets bigger the grid should get bigger as well. Smaller enemies should still display
properly as being smaller than the player as reference.

Each cell in the grid will have it's own color component, and color component's from neibhboring cells can be added together to influence
the color of the current cell.

For example,

The cell is blue but adjacent cells are green. Then green will "bleed into" the blue, looking kinda like a water painting. If this is too
compled for the time frame a single color is fine.

## Collision ##

Each edge of the grid should support collision detection. What happens to each side is up to the Game Feel person.

##### Scope 1 ######

* The edges change opacity, fade in/out, change color, disappear, etc. This will change based on Game Feel but setting up the framework
is the objective. 
* The cells change color instantaniously based on the player's color.

##### Scope 2 ######

* The cell's color changes over time based off it's old color and the player's color.

##### Scope 3 ######

* The cell's color is influences by neighboring cells.
