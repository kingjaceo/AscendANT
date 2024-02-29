class_name PheromoneModule
extends MapModule

var pheromones_by_cell: Dictionary # {Vector2i cell: PheromoneCloud pheromone}
var pheromone_cloud = preload("res://game_maps/map_modules/pheromone_cloud.tscn")


func _setup() -> void:
	pass


func add_at_cell(cell: Vector2i) -> void:
	if not pheromones_by_cell.has(cell):
		var new_cloud = pheromone_cloud.instantiate()
		new_cloud.location = cell
		new_cloud.pheromone_lifetime = 20
		new_cloud.position = game_map.map_to_local(cell)
		pheromones_by_cell[cell] = new_cloud
		add_child(new_cloud)
		new_cloud.start()
	else:
		pheromones_by_cell[cell].reset(20)



func add_at_location(location: Vector2) -> void:
	var cell = game_map.local_to_map(location)
	add_at_cell(cell)


func remove(cell: Vector2i, cloud: PheromoneCloud) -> void:
	pheromones_by_cell.erase(cell)


func get_neighbors(cell: Vector2i) -> Dictionary: # {Vector2 neighbor_location: PheromoneCloud pheromone / false}
	var potential_neighbors = game_map.get_surrounding_cells(cell)
	var neighbors = {}
	for neighbor in potential_neighbors:
		if pheromones_by_cell.has(neighbor):
			neighbors[game_map.map_to_local(neighbor)] = pheromones_by_cell[neighbor]
		else:
			neighbors[game_map.map_to_local(neighbor)] = false
	
	return neighbors
