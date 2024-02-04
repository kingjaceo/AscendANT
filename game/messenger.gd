extends Node2D

var _overworld: Node2D
var _pheromone_map: PheromoneMap
var _colony_world: Node2D
var _colony_map: ColonyMap
var _colony: Colony


signal food_updated
signal eggs_updated
signal current_population_updated
signal target_population_updated

func mark_cell(coordinate: Vector2i, pheromone: Pheromone, from: Vector2i):
	_pheromone_map.mark_cell(coordinate, pheromone, from)
	

func get_pheromone_cell(coordinate: Vector2i):
	return _pheromone_map.get_pheromone_cell(coordinate)
	

func get_surrounding_pheromone_cells(coordinate: Vector2i):
	var surrounding_tiles = _pheromone_map.get_surrounding_pheromone_cells(coordinate)
	return surrounding_tiles

func get_tile_position(coordinate):
	return _pheromone_map.get_tile_position(coordinate)
	

func get_tile_coordinate(position):
	return _pheromone_map.get_tile_coordinate(position)
	

func get_home_tile():
	return _pheromone_map.local_to_map(_colony.position)


func toggle_terrain():
	_pheromone_map.toggle_terrain()

func toggle_pheromones():
	_pheromone_map.toggle_pheromones()

func toggle_outline():
	_pheromone_map.toggle_outline()


func add_food_to_colony(amount: float):
	_colony.add_food(amount)
	
	
func take_food_from_tile(tile: Vector2i, amount: float):
	amount = _pheromone_map.take_food_from(tile, amount)
	return amount 


func take_food_from_colony(amount: float):
	amount = _colony.take_food(amount)
	return amount
	

func colony_has_egg_capacity():
	return _colony.eggs < _colony.egg_capacity


func lay_egg():
	_colony.add_egg()

func update_food():
	food_updated.emit()

func get_food_amount():
	return _colony.food
	
func update_eggs():
	eggs_updated.emit()
	
func get_egg_amount():
	return _colony.eggs

func increase_ant_limit():
	_colony.increase_target_population()
	
func decrease_ant_limit():
	_colony.decrease_target_population()
	
func get_current_population():
	return _colony.current_population
	
func get_target_population():
	return _colony.target_population
	
func update_current_population():
	current_population_updated.emit()

func update_target_population():
	target_population_updated.emit()

func ant_died():
	_colony.ant_died()
	
func get_colony_map_bounds():
	return _colony_map.get_bounds()

func get_pheromone_map():
	return _pheromone_map
	
func get_colony_map():
	return _colony_map
	
func move_ant_to_world(ant: BaseANT, world: BaseANT.World):
	if world == BaseANT.World.COLONY:
		ant.get_parent().remove_child(ant)
		_colony_world.add_child(ant)
		ant._current_map = ant._colony_map
		ant._current_cell = _colony_map.get_entrance()
		ant.position = _colony_map.map_to_local(_colony_map.get_entrance())
	if world == BaseANT.World.OVERWORLD:
		ant.get_parent().remove_child(ant)
		_overworld.add_child(ant)
		ant._current_map = ant._pheromone_map
		ant._current_cell = _pheromone_map.get_entrance()
		ant.position = _pheromone_map.map_to_local(_pheromone_map.get_entrance())

func _ready():
	print(_overworld)
