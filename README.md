# BattleCity
A BattleCity like game with tank upgrades and AI for opponents with different behaviors
# Overview
A game similar to battlecity, the main features of which are:
- Two opposing teams
- Modular tank system
- AI with different behaviors
- Procedural map generation

# Game features
## Two opposing teams
There are 2 opposing teams in the game, red and blue. the default player is blue. Each team can have tanks controlled by AI
## Modular tank system
Each consists of 3 parts: tower, hull, cannon.
The starting modules of the tank are set randomly. 

After the destruction of the tank, a random module will remain at the place of his death, this module can be equipped by other tanks, regardless of their color


Tank modules have their own characteristics and are visually different. Each module affects certain characteristics of the tank. T

he config file for modules is located along the path: ``` Assets/GameData.json ```

## AI
The game features 3 AI tanks with different behaviors:
- Assistant
  - Helps the nearest ally tank, attacks targets together with the selected tank and protects the ally in case of an attack on him
- Destroyer
  - Ignores enemy tanks, its main task is to destroy the enemy base
- Stormtrooper
  - Ignores enemy base, its main task is to destroy the enemy tanks
  
 Each AI picks modules that are better than its current models.
 
 The search for the path of tanks works based on the [A* algorithm](https://en.wikipedia.org/wiki/A*_search_algorithm)
 
 ## Procedural map generation
 Each game start and during the start of a new round, the map is randomly generated. 
 If there is no route from base to base, the generation process is repeated
 
 <img src = "https://user-images.githubusercontent.com/51063161/230929665-92176685-90a0-4c38-9aeb-e39e251b2ffc.gif" width="250" height="250"/>

