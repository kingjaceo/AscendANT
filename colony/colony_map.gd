class_name ColonyMap extends TileMap

var astar_grid = AStarGrid2D.new()

const DIRT_LAYER = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	Messenger._colony_map = self
	astar_grid.region = Rect2i(0, 0, 33, 65)
	astar_grid.cell_size = Vector2(16, 16)
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
	
