class_name EntityEating
extends EntityState

#var _eat_timer: Timer
#@export var eat_time: float

#func _ready():
	#_eat_timer = Timer.new()


func enter(data: Dictionary = {}) -> void:
	entity.mover.idle()
	entity.body.starvation.stop()
	#entity.body.eat_timer.start()
	entity.body.eat(data["food_cell"])
	
	entered.emit()


func exit():
	entity.body.hunger.start()
	
	exited.emit()
