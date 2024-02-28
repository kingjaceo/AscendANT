class_name PheromoneMarkerModule
extends ANTiStateModule

@export var mover: Mover
@export var pheromone_senser: PheromoneSenser
var _pheromone_module: PheromoneModule

var _active: bool = false

signal priority_updated
signal marked

func _setup():
	choice_making_signals = [priority_updated]
	mover.arrived_at_next_cell.connect(_update_priority)


func clear_connections():
	_pheromone_module = null


func connect_map_modules(map_modules: Array[Node]) -> void:
	for module in map_modules:
		if module is PheromoneModule:
			_pheromone_module = module


func _update_priority():
	behavior = mark
	exit_behavior = stop_mark
	priority = 10
	priority_updated.emit()


func mark():
	_pheromone_module.add_at_cell(mover.previous_cell)
	priority = 0
	behavior = _nothing
	exit_behavior = _nothing
	marked.emit()


func stop_mark():
	pass



