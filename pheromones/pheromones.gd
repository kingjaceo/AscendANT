extends Node

enum Type {EXPLORED, TO_FOOD, FOOD}

#enum Names {NONE, EXPLORED, TO_FOOD, TO_WATER, FOOD, WATER}
#var pheromone = preload("pheromone.gd")
#var pheromones = {}
#
#var EXPLORED: Pheromone
#var TO_FOOD: Pheromone
#var FOOD: Pheromone
#
#func _ready():
	#EXPLORED = pheromone.new(Names.EXPLORED, 20, Vector2i(4,0))
	#TO_FOOD = pheromone.new(Names.TO_FOOD, 20, Vector2i(4,1))
	#FOOD = pheromone.new(Names.FOOD, INF, Vector2i(4,1))
