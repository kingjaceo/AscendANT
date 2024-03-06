class_name Forager
extends ANTiStateModule

@export var _mover: Mover
@export var _pheromone_marker: PheromoneMarker
var _pheromone_module: PheromoneModule
var _wander_direction: Vector2

const EXPLORED = preload("res://pheromones/explored.tres")
const TO_FOOD = preload("res://pheromones/to_food.tres")

signal food_discovered
signal to_food_discovered
signal stuck

func _setup():
	priority = 2
	behavior = _scout
	choice_making_signals = [stuck]
	_choose_new_target()


func connect_map_modules(map_modules: Array[Node]) -> void:
	for module in map_modules:
		if module is PheromoneModule:
			_pheromone_module = module


func _scout():
	_pheromone_marker.current_pheromone = EXPLORED
	var neighbor_pheromone_clouds = _pheromone_module.get_pheromone_neighbors(_mover.current_cell)
	var empty_neighbors = _pheromone_module.get_empty_neighbors(_mover.current_cell)
	var to_food_neighbors = []

	for neighbor_position in neighbor_pheromone_clouds:
		var pheromone_clouds = neighbor_pheromone_clouds[neighbor_position]
		for cloud in pheromone_clouds:
			var pheromone = cloud.pheromone
			if pheromone.type == Pheromones.Type.FOOD:
				_mover.move_to(neighbor_position)
				return
			if pheromone.type == Pheromones.Type.TO_FOOD:
				to_food_neighbors.append(neighbor_position)
	
	if len(to_food_neighbors) > 0:
		var choice = to_food_neighbors[randi() % len(to_food_neighbors)]
		_mover.move_to(choice)
		return
	if len(empty_neighbors) > 0:
		var choice = empty_neighbors[randi() % len(empty_neighbors)]
		_mover.move_to(choice)
		return
		
	priority = 3
	behavior = _wander
	stuck.emit()


func _wander() -> void:
	var target = entity.position + _wander_direction * 10
	priority = 2
	behavior = _scout
	_mover.move_to(target)


func _choose_new_target() -> void:
	_wander_direction = Vector2(randf_range(-1, 1), randf_range(-1, 1))
