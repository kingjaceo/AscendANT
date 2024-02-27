class_name PheromoneModule
extends MapModule

var _pheromones_by_cell: Dictionary # {Vector2i cell: Color color}
func mark_position(pos: Vector2, color: Color) -> void:
	var cell = game_map.local_to_map(pos)
	if _pheromones_by_cell.has(cell):
		print("already clicked: ", cell)
	else:
		_pheromones_by_cell[cell] = color
		var particles = GPUParticles2D.new()
		particles
