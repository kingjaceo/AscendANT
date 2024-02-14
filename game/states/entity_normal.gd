class_name EntityNormal
extends EntityState


func enter(data: Dictionary = {}) -> void:
	var target = entity.current_map.get_random_cell()
	entity.mover.path_to(target)
	
	entered.emit()
	
