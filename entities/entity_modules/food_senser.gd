class_name FoodSenser
extends EntityModule

@export var mover: Mover
var _food_module: FoodModule

signal food_detected


func connect_map_modules(map_modules: Array[Node]) -> void:
	for module in map_modules:
		if module is FoodModule:
			_food_module = module


func _setup():
	mover.arrived_at_target.connect(_sense_food)


func _sense_food():
	if _food_module.food_at(entity.position):
		food_detected.emit(entity.position)
