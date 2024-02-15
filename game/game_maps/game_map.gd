class_name GameMap
extends TileMap

var spawn_locations: Array[Vector2i]
var _entrances_from: Dictionary # {GameMap: Vector2i}
var _exits_at: Dictionary # {Vector2i: GameMap}


func set_entrance(map: GameMap, cell: Vector2i) -> void:
	_entrances_from[map] = cell


func get_entrance(map: GameMap) -> Vector2i:
	return _entrances_from[map]


func get_entrances() -> Dictionary:
	return _entrances_from


func set_exit(cell: Vector2i, map: GameMap) -> void:
	_exits_at[cell] = map


func get_exit(cell: Vector2i) -> GameMap:
	return _exits_at[cell]


func get_exits() -> Dictionary:
	return _exits_at


func get_bounds() -> Array[float]:
	return []


func get_point_path(_start: Vector2i, _end: Vector2i) -> PackedVector2Array:
	return PackedVector2Array()


func get_nearest_food_cell(_cell: Vector2i) -> Vector2i:
	return Vector2i()


func get_entrance_cell() -> Vector2i:
	return Vector2i()


func get_random_cell() -> Vector2i:
	return Vector2i()


func food_at(_cell: Vector2i) -> bool:
	return false


func take_food_from(_cell: Vector2i, _amount: float) -> float:
	return 0


func _create_map_cells() -> void:
	return
