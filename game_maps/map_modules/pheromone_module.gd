class_name PheromoneModule
extends MapModule

var pheromones_by_cell: Dictionary # {Vector2i cell: PheromoneCloud pheromone}
const PHEROMONE_CLOUD = preload("res://game_maps/map_modules/pheromone_cloud.tscn")
#const PHEROMONE_CLOUD = preload("res://pheromones/pheromone_cloud_sprite.tscn")

func _setup() -> void:
	pass


func add_at_cell(pheromone: Pheromone, cell: Vector2i) -> void:
	var new_cloud = PHEROMONE_CLOUD.instantiate()
	new_cloud.location = cell
	new_cloud.pheromone = pheromone
	new_cloud.position = game_map.map_to_local(cell)
	add_child(new_cloud)
	new_cloud.start()
	pheromones_by_cell[cell] = new_cloud


func add_at_location(pheromone: Pheromone, location: Vector2) -> void:
	var cell = game_map.local_to_map(location)
	add_at_cell(pheromone, cell)


func remove(cell: Vector2i, cloud: PheromoneCloud) -> void:
	if pheromones_by_cell.has(cell) and pheromones_by_cell[cell] == cloud:
		pheromones_by_cell.erase(cell)


func get_pheromone_neighbors(cell: Vector2i) -> Dictionary: # {Vector2 neighbor_location: PheromoneCloud pheromone}
	var potential_neighbors = game_map.get_surrounding_cells(cell)
	var neighbors = {}
	for neighbor in potential_neighbors:
		if pheromones_by_cell.has(neighbor):
			neighbors[game_map.map_to_local(neighbor)] = pheromones_by_cell[neighbor]
	return neighbors


func get_empty_neighbors(cell: Vector2i) -> Array: # Vector2
	var potential_neighbors = game_map.get_surrounding_cells(cell)
	var neighbors = []
	for neighbor in potential_neighbors:
		if not pheromones_by_cell.has(neighbor):
			neighbors.append(game_map.map_to_local(neighbor))
	return neighbors
