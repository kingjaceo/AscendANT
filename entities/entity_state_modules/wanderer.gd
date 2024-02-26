class_name Wander
extends ANTiStateModule

@export var mover: Mover


func _ready():
	priority = 1
	behavior = _wander


func update_priority():
	priority = 1


func _wander():
	var random_location = Vector2i(randi_range(-800, 800), randi_range(-800, 800))
	mover.move_to(random_location)
