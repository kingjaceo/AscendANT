class_name PheromoneEmitter
extends EntityModule

@export var pheromone: Pheromone
@export var radius: int = 1

var _pheromone_module: PheromoneModule
var current_pheromone: Pheromone

signal priority_updated
signal marked


func _setup():
	await get_tree().create_timer(1).timeout
	var cell = entity.current_map.local_to_map(entity.position)
	var neighbor_cells = HexMath.neighbors(cell, radius)
	for neighbor in neighbor_cells:
		_pheromone_module.add_pheromone_to_cell(pheromone, neighbor)


func clear_connections():
	_pheromone_module = null


func connect_map_modules(map_modules: Array[Node]) -> void:
	for module in map_modules:
		if module is PheromoneModule:
			_pheromone_module = module
