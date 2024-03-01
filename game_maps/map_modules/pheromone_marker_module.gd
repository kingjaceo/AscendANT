class_name PheromoneMarker
extends ANTiStateModule

@export var mover: Mover
#@export var pheromone_senser: PheromoneSenser
var _pheromone_module: PheromoneModule
var current_pheromone: Pheromone
#const EXPLORED = preload("res://pheromones/explored.tres")
#const TO_FOOD = preload("res://pheromones/to_food.tres")

var _active: bool = false

signal priority_updated
signal marked

func _setup():
	choice_making_signals = [priority_updated, marked]
	mover.arrived_at_next_cell.connect(_update_priority)


func clear_connections():
	_pheromone_module = null


func connect_map_modules(map_modules: Array[Node]) -> void:
	for module in map_modules:
		if module is PheromoneModule:
			_pheromone_module = module


func _update_priority():
	behavior = mark
	priority = 10
	priority_updated.emit()


func mark():
	_pheromone_module.add_at_cell(current_pheromone, mover.previous_cell)
	priority = 0
	behavior = _nothing
	marked.emit()
