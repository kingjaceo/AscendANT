class_name Wander
extends ANTiStateModule

var _mover: EntityMover


func _ready():
	behavior = _wander


func connect_modules(modules: Array[Node]) -> void:
	for module in modules:
		if module.name == "Mover":
			_mover = module


func _wander():
	var random_location = Vector2i(randi() % 1200, randi() % 1200)
	_mover.move_to(random_location)
