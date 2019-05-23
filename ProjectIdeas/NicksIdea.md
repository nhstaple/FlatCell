# Nick's Idea #
## Brief ## 
Take Spore & Flatland, shove 'em together with procedural generation and this is what you get.

You play as a `geo`.

You have no `color`. But other things have `color`.

You play as a `dot`. You kill other `dots`.

You become a `line`. You kill other `lines`.

You become and `arrow`. You kill other `arrows`.

You're a triangle. Illuminati comfirmed 2k19.

## Inspiration ##

* Spore- the first stage where you're a lil' cell and have to battle it out to gain points / parts to evolve.
* This one Gamecube game where you were on an island and had a pet monster that you could evolve.
* Flatland- good old 2D geometrical action.
* Metro- procedural generation.

## Description ##
The canvas (game `world`) is composed of colored geometric Euclidean geometry, or `geos`.

Each 'geo' has the following components:

* 4D color value- red, blue, green, opacity. Add glow?
* Shape
* Diameter / Side Length
* Health
* Armor
* Damage -> how to implement? Guns? Ram 'em?

The `shapes` vary from `dot`, `line segment`, `arrow`, `triangle`, `square`, `pentagon`, ..., `circle`.

The player starts off as a colorless black dot on a colored canvas. The `world` is infinite and procedurally
generated, spawning enemies relative to the player's Evolution stage. The transition between evolitions can
be either incremental or instantanious. I feel like we should discuss which works for each transition.

| Stage of Evolution | Transition | Enemies |
| ------------------ | ---------- | ------- |
| `dot` | inc | `dot`, `line segment` |
| `line segment` | instant | `dot`, `line segment`, `arrow`, `triangle` |
| `arrow` | inc | `line segment`, `arrow`, `triangle` | 
| `triangle` | instant |  `arrow`, `triangle`, `square`, `pentagon` |
| `square` | instant?| `triangle`, `square`, `pentagon`, `hexagon`, ... |

The player deals damage to enemy `geos` that randomly drop a portion of their stats so the player can pick up
and morph into that shape. Here are examples of enemy drops.

| Drop | geo | Description |
| ---- | ----- | ------------|
| color | all | Drops a percantage of 1/n of it's values. |
| increase Diameter/Length | all | increases the size of the `geo` |
| decrease Diemater/Length | all | decreases the size of the `geo` | 
| increase speed | all | increase the movement speed of the `geo` |
| increase damage | all | increase the damage of the `geo` |
| increase armor/HP | all | increase the armor/HP of the `geo` |

## Scope ##
### Target 1 ###

* implment `dot` and `line segment`
* 1st stage of evo, `dot` -> `line segment`

### Target 2 ###
