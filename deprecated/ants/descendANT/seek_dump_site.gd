class_name SeekDumpSiteState
extends EntityState


func enter(data: Dictionary = {}) -> void:	
	var  resource_to_dump = data["resource"]
	var target = entity.current_map.get_dump_site(resource_to_dump)
	entity._target_dump_cell = target
	entity.mover.path_to(target)
	
	entered.emit()

func exit():
	pass
