class_name ColonyMap extends TileMap

var astar_grid = AStarGrid2D.new()
var cell_size = Vector2(16, 16)
var adjustment = cell_size / 2
var entrance = Vector2i(16, 0)
var spawn_location = Vector2i(24, 16)
const DIRT_LAYER = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	Messenger._colony_map = self
	astar_grid.region = Rect2i(0, 0, 33, 65)
	astar_grid.cell_size = cell_size
	var solid_tiles = get_used_cells(DIRT_LAYER)
	astar_grid.diagonal_mode = 3
	astar_grid.update()
	for solid_tile in solid_tiles:
		astar_grid.set_point_solid(solid_tile)

func get_bounds():
	var map_limits = get_used_rect()
	var map_cellsize = tile_set.tile_size
	var bounds = [0, 0, 0, 0]
	
	bounds[0] = map_limits.position.x * map_cellsize.x
	bounds[1] = map_limits.end.x * map_cellsize.x
	bounds[2] = map_limits.position.y * map_cellsize.y
	bounds[3] = map_limits.end.y * map_cellsize.y
	
	return bounds
	

func choose_random_cell():
	var allowed_cells = get_used_cells(1)
	var choice = allowed_cells[randi() % len(allowed_cells)]
	while astar_grid.is_point_solid(choice):
		choice = allowed_cells[randi() % len(allowed_cells)]
	return choice
	
	
func get_entrance():
	return entrance


func get_food_cell():
	return Vector2i(10, 17)

func get_point_path(start: Vector2i, end: Vector2i):
	return astar_grid.get_point_path(start, end)
