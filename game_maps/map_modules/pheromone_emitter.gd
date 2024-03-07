class_name PheromoneEmitter
extends Module

@export var pheromone: Pheromone
@export var radius: int = 1

var _pheromone_module: PheromoneModule
var current_pheromone: Pheromone
var _pheromone_clouds: Dictionary
#const PHEROMONE_CLOUD = preload("res://game_maps/map_modules/pheromone_cloud.tscn")

signal priority_updated
signal marked


func _setup():
	map_object.removed.connect(_decay)
	await get_tree().create_timer(0.1).timeout
	var cell = map_object.current_map.local_to_map(map_object.position)
	var neighbor_cells = HexMath.neighbors(cell, radius)
	for neighbor in neighbor_cells:
		_pheromone_clouds[neighbor] = _pheromone_module.add_pheromone_to_cell(pheromone, neighbor)


func clear_connections():
	_pheromone_module = null


func connect_map_modules(map_modules: Array[Node]) -> void:
	for module in map_modules:
		if module is PheromoneModule:
			_pheromone_module = module


func _decay():
	for cloud_location in _pheromone_clouds:
		if _pheromone_clouds[cloud_location]:
			_pheromone_clouds[cloud_location].start()
