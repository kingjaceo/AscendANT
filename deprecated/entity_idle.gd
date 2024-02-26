class_name IdleState
extends EntityState

func enter(_data: Dictionary = {}) -> void:
	entity.animator.play("idle")
	entity.mover.idle()
	entity.body.rest()
	
	entered.emit()


func exit():
	entity.body.rest_timer.start()
	entity.body.recovery_timer.stop()
	
	exited.emit()
