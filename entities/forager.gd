extends ANTiStateModule

@export var _mover: Mover
@export var _pheromone_senser: PheromoneSenser


signal food_discovered
signal to_food_discovered


func _setup():
	priority = 2
	behavior = _scout


func _scout():
	# inspect neighboring cells for their pheromones
	
	# choose one of them to move toward
	var target = entity.current_map.get_random_cell_position(entity.position)
	_mover.move_to(target)

func _harvest():
	pass
