class_name ColonyMap 
extends GameMap

@export var pheromone_map: PheromoneMap
@export var spawn_location: Vector2i = Vector2i(15, 11)
@export var exit: Vector2i = Vector2i(10, -1)
@export var entrance: Vector2i = Vector2i(10, 0)
@export var cell_size: Vector2 = Vector2(64, 64)
@export var adjustment: Vector2 = cell_size / 2
@export var temp_food_cell = Vector2i(7, 11)

var astar_grid = AStarGrid2D.new()

var expand_choices: Array[Vector2i]
var expand_choice: Vector2i
var colony: Colony

var _valid_cells: Array[Vector2i]
var _colony_cells = {}
var _food_by_cell = {}

const DIRT_LAYER = 0
const NAV_TERRAIN_SET = 0
const IMPASSABLE_ATLAS_COORDS = Vector2i(10, 1)

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	spawn_locations = [spawn_location]
	_exits_at = {exit: pheromone_map}
	_entrances_from = {pheromone_map: entrance}
	
	
	Messenger.colony_map = self
	astar_grid.region = Rect2i(0, -6, 33, 65)
	astar_grid.cell_size = cell_size
	astar_grid.diagonal_mode = 3
	astar_grid.update()
	
	for cell in get_used_cells(0):
		if get_cell_atlas_coords(DIRT_LAYER, cell) == IMPASSABLE_ATLAS_COORDS:
			astar_grid.set_point_solid(cell)
		else:
			_valid_cells.append(cell)
		
	colony = Messenger.colony
	_food_by_cell[temp_food_cell] = 100
	_create_colony_cells()


func get_bounds() -> Array[float]:
	var map_limits = get_used_rect()
	var map_cellsize = tile_set.tile_size
	var bounds: Array[float] = [0.0, 0.0, 0.0, 0.0]
	
	bounds[0] = map_limits.position.x * map_cellsize.x
	bounds[1] = map_limits.end.x * map_cellsize.x
	bounds[2] = map_limits.position.y * map_cellsize.y
	bounds[3] = map_limits.end.y * map_cellsize.y
	
	return bounds


func get_random_cell() -> Vector2i:
	var choice = _valid_cells[randi() % len(_valid_cells)]
	return choice


func get_nearest_food_cell(cell: Vector2i) -> Vector2i:
	return temp_food_cell


func get_food_cell() -> Vector2i:
	return temp_food_cell


func get_point_path(start: Vector2i, end: Vector2i) -> PackedVector2Array:
	var point_path = astar_grid.get_point_path(start, end)
	for i in range(len(point_path)):
		point_path[i] += adjustment
	return point_path


func has_food(cell: Vector2i) -> bool:
	return cell == temp_food_cell


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
