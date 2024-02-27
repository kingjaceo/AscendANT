class_name PheromoneModule
extends MapModule

var pheromones_by_cell: Dictionary # {Vector2i cell: PheromoneCloud pheromone}
var pheromone_cloud = preload("res://game_maps/map_modules/pheromone_cloud.tscn")


func _setup():
	add(Vector2i(3, 3))
	add(Vector2i(3, 2))
	add(Vector2i(2, 2))
	add(Vector2i(2, 1))
	add(Vector2i(1, 1))


func add(cell: Vector2i):
	var new_cloud = pheromone_cloud.instantiate()
	new_cloud.location = cell
	new_cloud.pheromone_lifetime = 20
	new_cloud.position = game_map.map_to_local(cell)
	pheromones_by_cell[cell] = new_cloud
	add_child(new_cloud)
	new_cloud.start()


func remove(cell: Vector2i, cloud: PheromoneCloud):
	pheromones_by_cell.erase(cell)
