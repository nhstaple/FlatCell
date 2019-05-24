# Nick's Idea #
## Brief ## 
Take Spore & Flatland, shove 'em together with procedural generation and this is what you get.

You play as a `geo`.

You have no `color`. But other things have `color`.

You play as a `dot`. You kill other `dots`.

You become a `line`. You kill other `lines`.

You become an `arrow`. You kill other `arrows`.

You're a triangle. Illuminati comfirmed 2k19.

## Inspiration ##

* [Spore](https://en.wikipedia.org/wiki/Spore_(2008_video_game))- the [first stage](https://www.youtube.com/watch?v=Q8NREo35qpE) where you're a lil' cell and have to battle it out to gain points / parts to evolve.
* This one Gamecube game where you were on an island and had a pet monster that you could evolve.
* [Flatland](https://en.wikipedia.org/wiki/Flatland)- good old 2D geometrical action.
* [Mini Metro](https://dinopoloclub.com/minimetro/)- procedural generation.

## Description ##
The canvas (game `world`) is composed of colored geometric Euclidean geometry, or `geos`.

Each 'geo' has the following components:

* 4D color value- red, blue, green, opacity. Add glow?
* Shape
* Diameter / Side Length
* Num of sides
* Health
* Armor - decreases amount of damage taken by %
* Damage -> how to implement? Guns? Ram 'em?
* What if each shape had a unique attack that's stylized by their color attributes? Then the player can cycle between 
previous generation attacks.

_Note: the weapons will scale and have an upper cap, encouraging the player to kill newer NPCs and acquiring their 
attacks. Weapons will have the following stats-_

* Damage
* Armor Piercing %
* Fire Rate
* Energy/Ammo? -> add this stat to the `geo`s
* Clip size? -> add this stat to the `geo`s

The `shapes` vary from `dot`, `line segment`, `arrow`, `triangle`, `square`, `pentagon`, ..., `circle`.

The player starts off as a colorless black `dot` on a colored canvas. The `world` is infinite and procedurally
generated, spawning enemies relative to the player's Evolution stage. The transition between evolitions can
be either incremental or instantanious. I feel like we should discuss which works for each transition.

| Stage of Evolution | Transition | Enemies |
| ------------------ | ---------- | ------- |
| `dot` | inc | `dot`, `line segment` |
| `line segment` | instant | `dot`, `line segment`, `arrow`, `triangle` |
| `arrow` | inc | `line segment`, `arrow`, `triangle` | 
| `triangle` | instant |  `arrow`, `triangle`, `square`, `pentagon` |
| `square` | instant?| `triangle`, `square`, `pentagon`, `hexagon`, ... |

The player deals damage to enemy `geos` that randomly drop a portion of their stats on death so the player. The player 
picks these up and has their attributes change morphing closer into a circle.

Here are examples of enemy drops.

| Drop | geo | Description |
| ---- | ----- | ------------|
| color | all | Drops a percantage of 1/n of it's values. |
| increase Diameter/Length | all | increases the size of the `geo` |
| decrease Diemater/Length | all | decreases the size of the `geo` | 
| increase speed | all | increase the movement speed of the `geo` |
| increase generation damage | all | increase the damage generation of `geo` weapon |
| increase armor/HP | all | increase the armor/HP of the `geo` |

## UI ##
### Elements ###
Player stats-

1. Shape/Color & Speed
2. HP
3. Armor
4. Active Weapon
5. Num of sides
6. Side length

Generational weapon stats-
1. Gen1 `dot`
2. Gen2 `arrow`
3. Gen3 `triangle`
...

Minimap?

## Movement/Physics ##
Things move along 2 axis at an ADSR curve while the input is applied. When the input is released the player will slow down
following a parabolic curve, ie coefficient of friction >= 0.5.

## Animation/Visuals ##
### Animation ###

TODO

### Visual Design ###

Flat, 2D, minimalist, and simplistic. Sample case-

![Mini Metro promo](https://steamcdn-a.akamaihd.net/steam/apps/287980/capsule_616x353_japanese.jpg?t=1542701556)

![Mini metro overview](https://steamcdn-a.akamaihd.net/steam/apps/287980/ss_c6337c67fbfa98abee6135819754321d4b4b5a84.1920x1080.jpg?t=1542701556)

![Mini metro closeup](https://dinopoloclub.com/minimetro/img/minimetro-twitter.jpg)

The world will be checkered grid with a varying background and white grid- add collision detection for the grid squares
so we can do things like change color/opacity, prevent player from progressing, etc., for that Game Feel yo.

## Input ##

| Value | Type | Range | Description |
| ----- | ---- | ----- | ----------- |
| moveY | axis | [-1, 1] | Player moves up/down. |
| moveX | axis | [-1, 1] | Player moves right/left. |
| fire1 | action | / | Player fires their primary attack. |
| fire2 | action | / | Player toggles their defenses that have a cooldown meter. |
| bump1 | action | / | Cycles the active generational weapon left. |
| bump2 | action | / | Cycles the active generational weapon right. |


## Game Logic ##

TODO

## Audio ##

TODO

## Gameplay Testing ##

TODO 

## Narrative Design ##
You're a speck on a speck, on a speck, on speck... (insert existential crisis here). By overcoming challenges you can 
better yourself and evolve.

## Pres Kit and Trailer ## 

TODO

## Game Feel ##

TODO

## Scope ##
### Target 1 ###

* implment `dot` and `line segment`
* 1st stage of evo, `dot` -> `line segment`

### Target 2 ###

* implement `arrow` and `triangle`
* 2nd stage of evo, `line segment` -> `arrow`

### Target 3 ### 

* implment `square`, and `pentagon`
* 3rd stage of evo, `arrow` -> `triangle`

### Target N ###
