class_name EntityState
extends State

var entity: Entity

func _ready() -> void:
	#await owner.ready
	
	entity = owner as Entity
	assert(entity != null)


func enter(_data: Dictionary = {}):
	pass


func exit():
	pass
