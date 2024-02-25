class_name FoodModule
extends MapModule

@export var food_source: Vector2i

func get_food_source() -> Vector2i:
	return food_source


func food_at(cell: Vector2i) -> bool:
	return true
