extends Node

enum Names {NONE, EXPLORED, TO_FOOD, TO_WATER, FOOD, WATER}
var Pheromone = preload("pheromone.gd")
var pheromones = {}

var EXPLORED: Pheromone
var TO_FOOD: Pheromone
var FOOD: Pheromone

func _ready():
	EXPLORED = Pheromone.new(Names.EXPLORED, 20, Vector2i(4,0))
	TO_FOOD = Pheromone.new(Names.TO_FOOD, 20, Vector2i(4,1))
	FOOD = Pheromone.new(Names.FOOD, INF, Vector2i(4,1))
