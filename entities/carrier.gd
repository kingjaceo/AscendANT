class_name Carrier
extends ANTiStateModule


func connect_map_modules(map_modules: Array[Node]) -> void:
	for module in map_modules:
		if module is FoodSenser:
			module.food_detected.connect(_on_food_detected)


func _on_food_detected() -> void:
	priority = 6
	behavior = _carry


func _carry():
	pass
