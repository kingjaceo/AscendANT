class_name EntityHungry
extends EntityState

func enter(_data: Dictionary = {}) -> void:
	entity.animator.play("walk")
	var target = entity.current_map.get_food_cell()
	entity.update_current_cell()
	entity.mover.path_to(target)

	entered.emit()
