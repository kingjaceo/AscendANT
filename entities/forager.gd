class_name Forager
extends ANTiStateModule

@export var _mover: Mover
#@export var _pheromone_senser: PheromoneSenser
var _pheromone_module: PheromoneModule
var _wander_direction: Vector2


signal food_discovered
signal to_food_discovered
signal stuck

func _setup():
	choice_making_signals = [stuck]
	_choose_new_target()


func update_priority():
	priority = 2
	behavior = _scout


func connect_map_modules(map_modules: Array[Node]) -> void:
	for module in map_modules:
		if module is PheromoneModule:
			_pheromone_module = module


func _scout():
	# inspect neighboring cells for their pheromones
	var neighbor_pheromones = _pheromone_module.get_neighbors(_mover.current_cell)
	var valid_neighbors = []
	# choose one of them to move toward
	for neighbor_position in neighbor_pheromones:
		if not neighbor_pheromones[neighbor_position]:
			valid_neighbors.append(neighbor_position)
	
	if len(valid_neighbors) == 0:
		priority = 3
		behavior = _wander
		#stuck.emit()
		return
	var choice = valid_neighbors[randi() % len(valid_neighbors)]
	_mover.move_to(choice)


func _wander() -> void:
	var target = entity.position + _wander_direction * 100
	_mover.move_to(target)


func _choose_new_target() -> void:
	_wander_direction = Vector2i(randi_range(-1, 1), randi_range(-1, 1))
	#_ta/rget = random_location
