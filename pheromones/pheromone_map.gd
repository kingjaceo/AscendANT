class_name PheromoneMap extends TileMap
'''
This script manages:
	- terrain
	- outlines
	- pheromones
'''
var _pheromones_by_cell = {}
var _tiles_with_pheromones = []
var _food_by_cell = {}

var PheromoneCell = preload("pheromone_cell.gd")

const TILE_SOURCE = 0
const TERRAIN_LAYER = 0
const FOOD_LAYER = 1
const OUTLINE_LAYER = 2
const PHEROMONE_LAYER = 3
const OUTLINE_COORDS = Vector2i(0,0)
const FOOD_PHEROMONE_TILE_COORD = Vector2i(4,1)

func get_tile_position(coordinate):
	return map_to_local(coordinate)
	

func get_tile_coordinate(position):
	return local_to_map(position)
	

func mark_cell(coordinate: Vector2i, pheromone: Pheromone, from: Vector2i):
	if pheromone:
		var pheromone_cell = _pheromones_by_cell[coordinate]
		pheromone_cell.add_pheromone(pheromone, from)
		var atlas_coord = pheromone.atlas_coord
		var alt_id = 0
		set_cell(PHEROMONE_LAYER, coordinate, TILE_SOURCE, atlas_coord, alt_id)
	

func get_pheromone_cell(coordinate):
	return _pheromones_by_cell[coordinate]
	

func get_surrounding_pheromone_cells(coordinate):
	var neighbors = get_surrounding_cells(coordinate)
	var valid_neighbors = []
	
	for neighbor_tile in neighbors:
		# check if the neighboring tile is a valid terrain tile
		var atlas_coords = get_cell_atlas_coords(TERRAIN_LAYER, neighbor_tile)
		if atlas_coords != Vector2i(-1, -1):
			valid_neighbors.append(_pheromones_by_cell[neighbor_tile])
		
	return valid_neighbors
	

func take_food_from(tile: Vector2i, amount: float):
	var amount_left = _food_by_cell[tile]
	var amount_taken = min(amount_left, amount)
	_food_by_cell[tile] -= amount_taken
	if _food_by_cell[tile] == 0:
		_remove_food(tile)
	return amount_taken
	

# Called when the node enters the scene tree for the first time.
func _ready():
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
	
	Messenger._pheromone_map = self

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	_update_pheromone_cells(delta)
	

func _update_pheromone_cells(delta):
	for tile in get_used_cells(PHEROMONE_LAYER):
		var pheromone_cell = _pheromones_by_cell[tile]
		pheromone_cell.decay_pheromones(delta)
		
		var alt_id = -1 # default value
		var atlas_coord = Vector2i(-1, -1) # default value
		var strength = 0 # default value
		
		if pheromone_cell.strongest_pheromone:
			strength = pheromone_cell.strongest_pheromone_percentage
			atlas_coord = pheromone_cell.strongest_pheromone.atlas_coord
		
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
		

# toggle functions
func toggle_pheromones():
	set_layer_enabled(PHEROMONE_LAYER, not is_layer_enabled(PHEROMONE_LAYER))
	
func toggle_outline():
	set_layer_enabled(OUTLINE_LAYER, not is_layer_enabled(OUTLINE_LAYER))
	
func toggle_terrain():
	set_layer_enabled(TERRAIN_LAYER, not is_layer_enabled(TERRAIN_LAYER))

func get_bounds():
	var map_limits = get_used_rect()
	var map_cellsize = tile_set.tile_size
	var bounds = [0, 0, 0, 0]
	
	bounds[0] = map_limits.position.x * map_cellsize.x
	bounds[1] = map_limits.end.x * map_cellsize.x
	bounds[2] = map_limits.position.y * map_cellsize.y
	bounds[3] = map_limits.end.y * map_cellsize.y
	
	return bounds

func _choose_random_neighbor(cell):
	var neighbors = get_surrounding_pheromone_cells(cell)
	return neighbors[randi() % len(neighbors)]
