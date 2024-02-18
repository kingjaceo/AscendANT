class_name DiggingState
extends EntityState


func enter(_data: Dictionary = {}) -> void:
	# get position of the target diggable cell
	#entity.mover.look_toward_target()
	entity.animator.play("idle")
	
	entered.emit()


func exit() -> void:
	entity._can_dig = false
	entity.current_map.excavate_from(entity.current_cell, 100)
