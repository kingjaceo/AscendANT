class_name EntityNormal
extends EntityState


func enter(_data: Dictionary = {}) -> void:
	entity.animator.play("walk")
	var target = entity.current_map.get_random_cell()
	entity.mover.path_to(target)
	
	entered.emit()
	
