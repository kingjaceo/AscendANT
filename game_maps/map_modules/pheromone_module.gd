class_name PheromoneModule
extends MapModule

var pheromones_by_cell: Dictionary # {Vector2i cell: PheromoneCloud pheromone}
#const PHEROMONE_CLOUD = preload("res://game_maps/map_modules/pheromone_cloud.tscn")
const PHEROMONE_CLOUD = preload("res://pheromones/alt_pheromone_cloud.tscn")
var PHEROMONE_EMITTER
@export var _food_module: FoodModule


func _setup() -> void:
	for cell in game_map.get_used_cells(game_map.WALKABLE_LAYER):
		pheromones_by_cell[cell] = []
	_food_module.food_spawned.connect(_place_emitter)
	_food_module.food_removed.connect(_remove_food_pheromones)


func mark_cell(pheromone: Pheromone, cell: Vector2i) -> void:
	if pheromones_by_cell.has(cell):
		var new_cloud = _create_cloud(pheromone, cell)
		add_child(new_cloud)
		new_cloud.dispersed.connect(remove)
		new_cloud.start()
		pheromones_by_cell[cell].append(new_cloud)


func mark_location(pheromone: Pheromone, location: Vector2) -> void:
	var cell = game_map.local_to_map(location)
	mark_cell(pheromone, cell)


func remove(cell: Vector2i, position: int) -> void:
	for cloud in pheromones_by_cell[cell].slice(position):
		cloud.index -= 1
	pheromones_by_cell[cell].pop_at(position)


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
		if pheromones_by_cell.has(neighbor) and len(pheromones_by_cell[neighbor]) == 0:
			neighbors.append(game_map.map_to_local(neighbor))
	return neighbors


func _place_emitter(pheromone: Pheromone, cell: Vector2i) -> void:
	pass


func _remove_food_pheromones(cell: Vector2i):
	pass


func _create_cloud(pheromone: Pheromone, cell: Vector2i) -> PheromoneCloud:
	var new_cloud = PHEROMONE_CLOUD.instantiate()
	new_cloud.location = cell
	new_cloud.pheromone = pheromone
	new_cloud.position = game_map.map_to_local(cell)
	new_cloud.index = len(pheromones_by_cell[cell])
	return new_cloud
