class_name DiggingState
extends EntityState


func enter(_data: Dictionary = {}) -> void:
	# look at target diggable tile
	entity.animator.play("dig") # play dig animation
	entity.mover.idle()
	entity.digger.dig()
	entered.emit()


#func exit() -> void:
	#var amount_to_excavate = entity.carrier.capacity
	#var amount_excavated = entity.digger.finish_digging(amount_to_excavate)
	#entity.carrier.carry("dirt", amount_excavated)

