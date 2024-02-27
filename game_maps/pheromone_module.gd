class_name PheromoneModule
extends MapModule

var pheromones_by_cell: Dictionary # {Vector2i cell: PheromoneCloud pheromone}
var pheromone_cloud = preload("res://game_maps/map_modules/pheromone_cloud.tscn")


func _setup():
	add_at_cell(Vector2i(3, 3))
	add_at_cell(Vector2i(3, 2))
	add_at_cell(Vector2i(2, 2))
	add_at_cell(Vector2i(2, 1))
	add_at_cell(Vector2i(1, 1))


func add_at_cell(cell: Vector2i):
	if not pheromones_by_cell.has(cell):
		var new_cloud = pheromone_cloud.instantiate()
		new_cloud.location = cell
		new_cloud.pheromone_lifetime = 20
		new_cloud.position = game_map.map_to_local(cell)
		pheromones_by_cell[cell] = new_cloud
		add_child(new_cloud)
		new_cloud.start()


func add_at_location(location: Vector2):
	var cell = game_map.local_to_map(location)
	add_at_cell(cell)

func remove(cell: Vector2i, cloud: PheromoneCloud):
	pheromones_by_cell.erase(cell)

#func mark_position(pos: Vector2, color: Color) -> void:
	#var cell = game_map.local_to_map(pos)
	#if _pheromones_by_cell.has(cell):
		#print("already clicked: ", cell)
	#else:
		#_pheromones_by_cell[cell] = color
		#var particles = GPUParticles2D.new()
		#particles
