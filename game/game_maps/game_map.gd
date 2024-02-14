class_name GameMap
extends TileMap



func get_bounds() -> Array[float]:
	return []


func get_point_path(start: Vector2i, end: Vector2i) -> PackedVector2Array:
	return PackedVector2Array()


func get_nearest_food_cell(cell: Vector2i) -> Vector2i:
	return Vector2i()


func get_entrance_cell() -> Vector2i:
	return Vector2i()


func get_random_cell() -> Vector2i:
	return Vector2i()


func food_at(cell: Vector2i) -> bool:
	return false


func take_food_from(cell: Vector2i, amount: float) -> float:
	return 0


func _create_map_cells() -> void:
	return
