class_name ANTiStateModule
extends EntityModule

var priority: float = 0
var behavior: Callable = _nothing
var exit_behavior: Callable = _nothing


func update_priority() -> void:
	pass


func _nothing() -> void:
	# represents no behavior, so that states can "go dormant"
	pass
