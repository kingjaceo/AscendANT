class_name PheromoneSenser
extends EntityModule

@export var mover: Mover
var _pheromone_module: PheromoneModule

signal pheromone_detected

#func _setup() -> void:
	#mover.arrived_at_next_cell.connect(_sense)


func connect_map_modules(map_modules: Array[Node]) -> void:
	for module in map_modules:
		if module is PheromoneModule:
			_pheromone_module = module


func clear_connections() -> void:
	_pheromone_module = null


#func sense() -> void:
	#return _pheromone_module.get_neighbors(mover.current_cell)
