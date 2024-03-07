extends Node

### Hex ###
# Map Coordinates "odd-q" (x: col, y: row)
# Axial Coordinates (q: NS Axis, r: NWSE Axis)
# Cube Coordinates (q, r, s), sum of coords = 0
var _cube_direction_vectors = [
	Vector3i(+1, 0, -1), Vector3i(+1, -1, 0), Vector3i(0, -1, +1), 
	Vector3i(-1, 0, +1), Vector3i(-1, +1, 0), Vector3i(0, +1, -1), 
]

func hex_distance(coord1, coord2) -> float:
	coord1 = _oddq_to_cube(coord1)
	coord2 = _oddq_to_cube(coord2)
	var q1 = coord1[0]
	var q2 = coord2[0]
	var r1 = coord1[1]
	var r2 = coord2[1]
	
	var distance = (abs(q1 - q2) + abs(q1 + r1 - q2 - r2) + abs(r1 - r2)) / 2
	
	return distance


func neighbors(oddq, radius):
	var neighbors = [oddq]
	for i in range(radius + 1):
		neighbors += _oddq_ring(oddq, i)
	return neighbors


func _cube_direction(direction):
	return _cube_direction_vectors[direction]


func _oddq_to_cube(oddq: Vector2i) -> Vector3i:
	var q = oddq[0]
	var r = oddq[1] - (oddq[0] - (oddq[0]&1)) / 2
	var s = -q - r
	return Vector3(q, r, s)


func _cube_to_oddq(cube: Vector3i) -> Vector2i:
	var col = cube[0]
	var row = cube[1] + (cube[0] - (cube[0]&1)) / 2
	return Vector2i(col, row)


func _cube_scale(hex, factor):
	return _oddq_to_cube(hex)

func _oddq_ring(oddq, radius):
	var results = []
	# this code doesn't work for radius == 0; can you see why?
	var cube = _oddq_to_cube(oddq)
	var ring_cube = cube + _cube_direction(4) * radius
	#var hex = _cube_add(cube, _cube_scale(_cube_direction(4), radius))
	for i in range(6):
		for j in range(radius):
			var hex = _cube_to_oddq(ring_cube)
			results.append(hex)
			ring_cube = _cube_neighbor(ring_cube, i)
	return results


func _cube_add(cube, vec):
	return cube + vec


func _cube_neighbor(cube, direction):
	return _cube_add(cube, _cube_direction(direction))
