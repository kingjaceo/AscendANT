extends Node2D

var pheromone_map: PheromoneMap
var colony_map: ColonyMap
var colony: Colony

@export var _vertical_camera: Camera2D
@export var _aerial_camera: Camera2D

signal food_updated
signal eggs_updated
signal current_population_updated
signal target_population_updated


func _ready():
	pass

func set_camera(world: BaseANT.World, camera: Camera2D):
	if world == BaseANT.World.COLONY:
		_vertical_camera = camera
	if world == BaseANT.World.OVERWORLD:
		_aerial_camera = camera
	

func get_pheromone_cell(coordinate: Vector2i):
	return pheromone_map.get_pheromone_cell(coordinate)
	

func get_surrounding_pheromone_cells(coordinate: Vector2i):
	var surrounding_tiles = pheromone_map.get_surrounding_pheromone_cells(coordinate)
	return surrounding_tiles

func get_tile_position(coordinate):
	return pheromone_map.get_tile_position(coordinate)
	

func get_tile_coordinate(position):
	return pheromone_map.get_tile_coordinate(position)
	

func get_home_tile():
	return pheromone_map.local_to_map(colony.position)


func toggle_terrain():
	pheromone_map.toggle_terrain()

func toggle_pheromones():
	pheromone_map.toggle_pheromones()

func toggle_outline():
	pheromone_map.toggle_outline()


func add_food_to_colony(amount: float):
	colony.add_food(amount)
	
	
func take_food_from_cell(cell: Vector2i, amount: float):
	amount = pheromone_map.take_food_from(cell, amount)
	return amount 


func take_food_from_colony(amount: float):
	amount = colony.take_food(amount)
	return amount
	

func colony_has_egg_capacity():
	return colony.eggs < colony.egg_capacity


func lay_egg():
	colony.add_egg()

func update_food():
	food_updated.emit()

func get_food_amount():
	return colony.food
	
func update_eggs():
	eggs_updated.emit()
	
func get_egg_amount():
	return colony.eggs

func increase_ant_limit():
	colony.increase_target_population()
	
func decrease_ant_limit():
	colony.decrease_target_population()
	
func get_current_population():
	return colony.current_population
	
func get_target_population():
	return colony.target_population
	
func update_current_population():
	current_population_updated.emit()

func update_target_population():
	target_population_updated.emit()

func ant_died():
	colony.ant_died()
	
func get_colony_map_bounds():
	return colony_map.get_bounds()

func get_pheromone_map():
	return pheromone_map
	
func get_colony_map():
	return colony_map
	
#func move_ant_to_world(ant: BaseANT, world: BaseANT.World):
	#if world == BaseANT.World.COLONY:
		#ant.get_parent().remove_child(ant)
		#_colony_world.add_child(ant)
		#ant.set_map(ant.colony_map)
		#ant.set_current_cell(colony_map.entrance)
		##ant.position = colony_map.map_to_local(colony_map.entrance)
	#if world == BaseANT.World.OVERWORLD:
		#ant.get_parent().remove_child(ant)
		#_overworld.add_child(ant)
		#ant.set_map(ant.pheromone_map)
		#ant.set_current_cell(pheromone_map.choose_random_neighbor(pheromone_map.entrance))
		##ant.position = pheromone_map.map_to_local(ant._current_cell)

#func set_vertical_camera_position(pos: Vector2):
	#_vertical_camera.position = pos

#func set_aerial_camera_position(pos: Vector2):
	#_aerial_camera.position = pos

func get_ui_adjustment(world: BaseANT.World):
	if world == BaseANT.World.OVERWORLD:
		return Vector2(-200, 0)
	else:
		return Vector2(0, 0)

#func camera_follow(game_map: GameMap, node: Node2D):
	#game_map.camera.follow(node)
	#if world == BaseANT.World.COLONY:
		#_vertical_camera.follow(node)
	#if world == BaseANT.World.OVERWORLD:
		#_aerial_camera.follow(node)


func get_camera_zoom():
	return _vertical_camera.zoom.x
