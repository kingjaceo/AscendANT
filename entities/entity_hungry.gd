class_name EntityHungry
extends EntityState

func enter(data: Dictionary = {}) -> void:
	entity.body.starvation.start()
	
	var target = entity.current_map.get_food_cell()
	entity.mover.path_to(target)

	entered.emit()
