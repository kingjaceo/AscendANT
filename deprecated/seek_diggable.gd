class_name SeekDiggableState
extends EntityState

func enter(_data: Dictionary = {}) -> void:
	entity.animator.play("walk")
	var target = entity.current_map.get_diggable_from_cell(entity.current_cell)
	entity._target_dig_cell = target
	entity.mover.path_to(target)
	
	entered.emit()
