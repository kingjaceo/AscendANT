extends Node

enum Names {NONE, EXPLORED, TO_FOOD, TO_WATER, FOOD, WATER}
var Pheromone = preload("pheromone.gd")
var pheromones = {}

func _ready():
	pheromones[Names.EXPLORED] = Pheromone.new(Names.EXPLORED, 20, Vector2i(4,0))
	pheromones[Names.TO_FOOD] = Pheromone.new(Names.TO_FOOD, 60, Vector2i(4,1))
	pheromones[Names.FOOD] = Pheromone.new(Names.FOOD, INF, Vector2i(4,1))
