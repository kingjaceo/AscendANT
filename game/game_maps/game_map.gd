class_name GameMap
extends TileMap

@export var camera: Camera2D

var spawn_locations: Array[Vector2i]
var astar_grid: AStarGrid2D

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
	var map_limits = get_used_rect()
	var map_cellsize = tile_set.tile_size
	var bounds: Array[float] = [0.0, 0.0, 0.0, 0.0]
	
	bounds[0] = map_limits.position.x * map_cellsize.x
	bounds[1] = map_limits.end.x * map_cellsize.x
	bounds[2] = map_limits.position.y * map_cellsize.y
	bounds[3] = map_limits.end.y * map_cellsize.y
	
	return bounds


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


func _hex_distance(coord1, coord2):
	var q1 = coord1[0]
	var q2 = coord2[0]
	var r1 = coord1[1] - (coord1[0] - (coord1[0]&1)) / 2
	var r2 = coord2[1] - (coord2[0] - (coord2[0]&1)) / 2
	
	var distance = (abs(q1 - q2) + abs(q1 + r1 - q2 - r2) + abs(r1 - r2)) / 2
	
	return distance
