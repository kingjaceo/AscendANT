class_name DumpState
extends EntityState


func enter(data: Dictionary = {}) -> void:
	var resource_to_dump = data["resource"]
	var amount_to_dump = data["amount"]
	entity.carrier.dump(resource_to_dump, amount_to_dump, entity.current_map)


#func exit() -> void:
	#entity.carrier.finish_dumping()
