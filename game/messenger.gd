extends Node2D

#var pheromone_map: PheromoneMap
#var colony_map: ColonyMap
var colony: Colony
#
#@export var _vertical_camera: Camera2D
#@export var _aerial_camera: Camera2D

signal food_updated
signal eggs_updated
signal current_population_updated
signal target_population_updated


func increase_ant_limit():
	colony.increase_target_population()
	
func decrease_ant_limit():
	colony.decrease_target_population()

