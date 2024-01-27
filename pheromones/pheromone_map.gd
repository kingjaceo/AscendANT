class_name PheromoneMap extends TileMap
'''
This script manages:
	- terrain
	- outlines
	- pheromones
'''
var _pheromones_by_cell = {}
var _tiles_with_pheromones = []

var PheromoneCell = preload("pheromone_cell.gd")

var home_tile = Vector2i(-1, 1)

const TERRAIN_LAYER = 0
const FOOD_LAYER = 1
const OUTLINE_LAYER = 2
const PHEROMONE_LAYER = 3
const OUTLINE_COORDS = Vector2i(0,0)
const FOOD_PHEROMONE_TILE_COORD = Vector2i(4,1)

# Called when the node enters the scene tree for the first time.
func _ready():
	var tile_locations = get_used_cells(TERRAIN_LAYER)
	
	# create the pheromone layer
	for tile_location in tile_locations:
		var pheromone_cell = PheromoneCell.new(tile_location)
		_pheromones_by_cell[tile_location] = pheromone_cell
		
	# create the outline layer
	for tile_location in tile_locations:
		set_cell(OUTLINE_LAYER, tile_location, 1, OUTLINE_COORDS)
	
	set_food_pheromone_tiles()
	
	# enable/disable layers
	set_layer_enabled(TERRAIN_LAYER, true)
	set_layer_enabled(OUTLINE_LAYER, true)
	set_layer_enabled(PHEROMONE_LAYER, true)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	_update_pheromone_cells(delta)
	

func get_tile_position(coordinate):
	return map_to_local(coordinate)
	

func get_tile_coordinate(position):
	return local_to_map(position)
	

func mark_cell(coordinate: Vector2i, pheromone: Pheromone):
	var pheromone_cell = _pheromones_by_cell[coordinate]
	pheromone_cell.add_pheromone(pheromone)
	var atlas_coord = pheromone.atlas_coord
	var alt_id = 0
	set_cell(PHEROMONE_LAYER, coordinate, 1, atlas_coord, alt_id)
#	_tiles_with_pheromones.append(coordinate)
	

func _update_pheromone_cells(delta):
	for tile in get_used_cells(PHEROMONE_LAYER):
		var pheromone_cell = _pheromones_by_cell[tile]
		pheromone_cell.decay_pheromones(delta)
		pheromone_cell.update_strongest_pheromone()
		
		var alt_id = -1
		var atlas_coord = Vector2i(-1, -1)
		var strength = 0
		if pheromone_cell.strongest_pheromone:
			strength = pheromone_cell.strongest_pheromone_percentage
			atlas_coord = pheromone_cell.strongest_pheromone.atlas_coord

		
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
			
		set_cell(PHEROMONE_LAYER, tile, 1, atlas_coord, alt_id)
		

func get_surrounding_pheromone_cells(coordinate):
	var neighbors = get_surrounding_cells(coordinate)
	var pheromone_neighbors = []
	
	for neighbor in neighbors:
		var atlas_coords = get_cell_atlas_coords(0, neighbor)
		if atlas_coords != Vector2i(-1, -1):
			pheromone_neighbors.append(_pheromones_by_cell[neighbor])
		
	return pheromone_neighbors
	

func set_food_pheromone_tiles():
	var tile_positions = get_used_cells(FOOD_LAYER)
	for tile in tile_positions:
		_pheromones_by_cell[tile].add_pheromone(Pheromones.pheromones[Pheromones.Names.FOOD])
		set_cell(PHEROMONE_LAYER, tile, 1, FOOD_PHEROMONE_TILE_COORD, 0)

func toggle_pheromones():
	set_layer_enabled(PHEROMONE_LAYER, not is_layer_enabled(PHEROMONE_LAYER))
	
func toggle_outline():
	set_layer_enabled(OUTLINE_LAYER, not is_layer_enabled(OUTLINE_LAYER))
	
func toggle_terrain():
	set_layer_enabled(TERRAIN_LAYER, not is_layer_enabled(TERRAIN_LAYER))
