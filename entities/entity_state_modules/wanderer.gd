class_name Wander
extends ANTiStateModule

@export var mover: Mover

var _target: Vector2

func _ready() -> void:
	priority = 1
	behavior = _wander
	mover.arrived_at_target.connect(_choose_new_target)


func update_priority() -> void:
	priority = 1


func _wander() -> void:
	mover.move_to(_target)


func _choose_new_target() -> void:
	var random_location = Vector2i(randi_range(-800, 800), randi_range(-800, 800))
	_target = random_location
