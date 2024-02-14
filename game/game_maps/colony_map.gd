class_name ColonyMap 
extends GameMap

var astar_grid = AStarGrid2D.new()
var cell_size = Vector2(64, 64)
var adjustment = cell_size / 2
var entrance = Vector2i(10, 8)
var spawn_location = Vector2i(15, 11)
var expand_choices: Array[Vector2i]
var expand_choice: Vector2i
var colony: Colony

var _valid_cells: Array[Vector2i]
var _colony_cells = {}
var _food_by_cell = {}
var _temp_food_cell = Vector2i(7, 11)

const DIRT_LAYER = 0
const NAV_TERRAIN_SET = 0
const IMPASSABLE_ATLAS_COORDS = Vector2i(10, 1)

# Called when the node enters the scene tree for the first time.
func _ready():
	Messenger.colony_map = self
	astar_grid.region = Rect2i(0, -6, 33, 65)
	astar_grid.cell_size = cell_size
	#var solid_tiles = get_used_cells(DIRT_LAYER)
	astar_grid.diagonal_mode = 3
	astar_grid.update()
	#for solid_tile in solid_tiles:
		#astar_grid.set_point_solid(solid_tile)
		
	for cell in get_used_cells(0):
		if get_cell_atlas_coords(DIRT_LAYER, cell) == IMPASSABLE_ATLAS_COORDS:
			astar_grid.set_point_solid(cell)
		else:
			_valid_cells.append(cell)
		
	colony = Messenger.colony
	_food_by_cell[_temp_food_cell] = 100
	_create_colony_cells()

func get_bounds():
	var map_limits = get_used_rect()
	var map_cellsize = tile_set.tile_size
	var bounds = [0, 0, 0, 0]
	
	bounds[0] = map_limits.position.x * map_cellsize.x
	bounds[1] = map_limits.end.x * map_cellsize.x
	bounds[2] = map_limits.position.y * map_cellsize.y
	bounds[3] = map_limits.end.y * map_cellsize.y
	
	return bounds
	

func get_random_cell():
	#var allowed_cells = get_used_cells(1)
	var choice = _valid_cells[randi() % len(_valid_cells)]
	#while astar_grid.is_point_solid(choice):
		#choice = _valid_cells[randi() % len(_valid_cells)]
	return choice


func get_nearest_food_cell(cell: Vector2i) -> Vector2i:
	return _temp_food_cell
	
	
func get_entrance():
	return entrance


func get_food_cell():
	return _temp_food_cell

func get_point_path(start: Vector2i, end: Vector2i):
	var point_path = astar_grid.get_point_path(start, end)
	for i in range(len(point_path)):
		point_path[i] += adjustment
	return point_path


func _create_colony_cells():
	#var seeds = [Vector2i(10, 11), Vector2i(6, 11)]
	var top = Vector2i(10, 11)
	var depth = 34
	var bottom = top + Vector2i(0, depth)
	
	var next = top + Vector2i(0, 1)
	var current = top
	var prev = null
	
	#expand_choices = [next]
	expand_choice = top
	
	for i in range(1, depth - 1):
		next = top + Vector2i(0, 1)
		var colony_cell = ColonyCell.new(current, [next], [prev])
		_colony_cells[current] = colony_cell
		prev = current
		current = next
		

func food_around(cell: Vector2i):
	return cell in _food_by_cell


func take_food_from(cell: Vector2i, amount: float) -> float:
	var amount_taken = min(_food_by_cell[cell], amount)
	_food_by_cell[cell] -= amount_taken
	return amount_taken

func excavate_cell(cell: Vector2i, dirt_moved: float):
	var actual_dirt_moved = min(dirt_moved, _colony_cells[cell].dirt_left)
	_colony_cells[cell].dirt_left -= actual_dirt_moved
	if _colony_cells[cell].dirt_left <= 0:
		expand_choice = cell
		set_cells_terrain_connect(0, [cell], 0, 0)
	return actual_dirt_moved
