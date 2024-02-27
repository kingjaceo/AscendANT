class_name ANTiStateModule
extends EntityModule

var priority: float = 0
var behavior: Callable = _nothing
var exit_behavior: Callable = _nothing


func get_debug_text() -> String:
	return "ANTiStateModule: NONE"


func update_priority() -> void:
	priority = 0


func _nothing() -> void:
	# represents no behavior, so that states can "go dormant"
	pass