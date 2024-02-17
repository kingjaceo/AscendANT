class_name PheromoneMap 
extends GameMap

@export var colony_map: ColonyMap
@export var spawn_location: Vector2i = Vector2i(5, -6)
@export var entrance: Vector2i = Vector2i(4, -6)

var _pheromones_by_cell = {}
var _food_by_cell = {}
var adjustment: Vector2 = Vector2.ZERO
var pheromone_cell = preload("pheromone_cell.gd")

const TILE_SOURCE = 0
const TERRAIN_LAYER = 0
const FOOD_LAYER = 1
const OUTLINE_LAYER = 2
const PHEROMONE_LAYER = 3
const OUTLINE_COORDS = Vector2i(0,0)
const FOOD_PHEROMONE_TILE_COORD = Vector2i(4,1)
const IMPASSABLE_ATLAS_COORDS = Vector2i(-1, -1)


func _ready():
	spawn_locations = [spawn_location]
	_exits_at = {entrance: colony_map}
	_entrances_from = {colony_map: entrance}
	
	_place_sprite()
	#_setup_astar_grid()
	
	var tile_locations = get_used_cells(TERRAIN_LAYER)
	
	# create the pheromone layer
	for tile_location in tile_locations:
		var pheromone_cell = PheromoneCell.new(tile_location)
		_pheromones_by_cell[tile_location] = pheromone_cell
		
	# create the outline layer
	for tile_location in tile_locations:
		set_cell(OUTLINE_LAYER, tile_location, TILE_SOURCE, OUTLINE_COORDS)
	
	_set_food_pheromone_tiles()
	
	# enable/disable layers
	set_layer_enabled(TERRAIN_LAYER, true)
	set_layer_enabled(OUTLINE_LAYER, false)
	set_layer_enabled(PHEROMONE_LAYER, true)
	
	Messenger.pheromone_map = self


func _process(delta):
	_update_pheromone_cells(delta)


func get_random_cell():
	var cells = get_used_cells(TERRAIN_LAYER)
	var choice = cells[randi() % len(cells)]
	return choice


func mark_cell(coordinate: Vector2i, pheromone: Pheromone, from: Vector2i):
	if pheromone:
		var pher_cell = _pheromones_by_cell[coordinate]
		pher_cell.add_pheromone(pheromone, from)
		var atlas_coord = pheromone.atlas_coord
		var alt_id = 0
		set_cell(PHEROMONE_LAYER, coordinate, TILE_SOURCE, atlas_coord, alt_id)


func get_pheromone_cell(coordinate: Vector2i):
	return _pheromones_by_cell[coordinate]


func get_surrounding_pheromone_cells(coordinate: Vector2i):
	var neighbors = get_surrounding_cells(coordinate)
	var valid_neighbors = []
	
	for neighbor_tile in neighbors:
		# check if the neighboring tile is a valid terrain tile
		var atlas_coords = get_cell_atlas_coords(TERRAIN_LAYER, neighbor_tile)
		if atlas_coords != Vector2i(-1, -1) and neighbor_tile != entrance:
			valid_neighbors.append(_pheromones_by_cell[neighbor_tile])
			
	return valid_neighbors


func take_food_from(tile: Vector2i, amount: float):
	var amount_left = _food_by_cell[tile]
	var amount_taken = min(amount_left, amount)
	_food_by_cell[tile] -= amount_taken
	if _food_by_cell[tile] == 0:
		_remove_food(tile)
	return amount_taken	


func get_nearest_food_cell(_cell: Vector2i):
	return Vector2i.ZERO


func has_food(cell: Vector2i):
	return cell in _food_by_cell


func _update_pheromone_cells(delta):
	for tile in get_used_cells(PHEROMONE_LAYER):
		if tile == Vector2i(9, -5):
			pass
		var pher_cell = _pheromones_by_cell[tile]
		pher_cell.decay_pheromones(delta)
		
		var alt_id = -1 # default value
		var atlas_coord = Vector2i(-1, -1) # default value
		var strength = 0 # default value
		
		if pher_cell.strongest_pheromone:
			strength = pher_cell.strongest_pheromone_percentage
			atlas_coord = pher_cell.strongest_pheromone.atlas_coord
		
		# determine alt tile to draw based on strength
		if strength <= 0:
			alt_id = -1
		elif strength < 25:
			alt_id = 3
		elif strength < 50:
			alt_id = 2
		elif strength < 75:
			alt_id = 1
		elif strength > 75:
			alt_id = 0
			
		set_cell(PHEROMONE_LAYER, tile, TILE_SOURCE, atlas_coord, alt_id)


# toggle functions
func toggle_pheromones():
	set_layer_enabled(PHEROMONE_LAYER, not is_layer_enabled(PHEROMONE_LAYER))
	
func toggle_outline():
	set_layer_enabled(OUTLINE_LAYER, not is_layer_enabled(OUTLINE_LAYER))
	
func toggle_terrain():
	set_layer_enabled(TERRAIN_LAYER, not is_layer_enabled(TERRAIN_LAYER))


func choose_random_neighbor(cell: Vector2i):
	var neighbors = get_surrounding_pheromone_cells(cell)
	return neighbors[randi() % len(neighbors)].coordinates


func get_food_cell():
	return entrance


func get_point_path(start: Vector2i, end: Vector2i):
	#var point_path = astar_grid.get_point_path(start, end)
	#for i in range(len(point_path)):
		#point_path[i] += adjustment
	var point_path = [map_to_local(start), map_to_local(end)]
	return PackedVector2Array(point_path)


#func get_next_home_cell(cell: Vector2i) -> Vector2i:
	#if _hex_distance(entrance, cell) == 1:
		#return entrance
		#
	#var closest_cell
	#var distance = INF
	#for neighbor_cell in get_surrounding_pheromone_cells(cell):
		#var new_distance = _hex_distance(entrance, neighbor_cell.coordinates)
		#if new_distance < distance:
			#distance = new_distance
			#closest_cell = neighbor_cell.coordinates
	#return closest_cell


func _setup_astar_grid():
	astar_grid = AStarGrid2D.new()
	astar_grid.region = Rect2i(-40, -20, 80, 40)
	astar_grid.cell_size = tile_set.tile_size
	astar_grid.diagonal_mode = 3
	astar_grid.update()
	
	for cell in get_used_cells(TERRAIN_LAYER):
		if get_cell_atlas_coords(TERRAIN_LAYER, cell) == IMPASSABLE_ATLAS_COORDS:
			astar_grid.set_point_solid(cell)


func _remove_food(tile: Vector2i):
	var food_pheromone = Pheromones.FOOD
	_pheromones_by_cell[tile].delete_pheromone(food_pheromone)
	_food_by_cell.erase(tile)
	set_cell(FOOD_LAYER, tile, TILE_SOURCE, Vector2i(-1, -1))


func _set_food_pheromone_tiles():
	var tile_positions = get_used_cells(FOOD_LAYER)
	var from = Vector2i(0, 0)
	for tile in tile_positions:
		_pheromones_by_cell[tile].add_pheromone(Pheromones.FOOD, from)
		_food_by_cell[tile] = 100
		set_cell(PHEROMONE_LAYER, tile, TILE_SOURCE, FOOD_PHEROMONE_TILE_COORD, 0)


func _place_sprite():
	var colony_sprite = get_node("Sprite2D")
	add_child(colony_sprite)
	colony_sprite.position = map_to_local(entrance)
	Messenger.set_aerial_camera_position(colony_sprite.position)
