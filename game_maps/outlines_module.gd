class_name OutlinesModule
extends MapModule

@export var outline_atlas_coord: Vector2i
@export var outline_sheet_id: int
@export_range(0, 1, 0.01) var alpha_level: float

const OUTLINE_LAYER: int = 2

func _setup():
	# add this tile to the whole GameMap
	var outlined_cells = game_map.get_used_cells(game_map.WALKABLE_LAYER)
	for cell in outlined_cells:
		game_map.set_cell(OUTLINE_LAYER, cell, outline_sheet_id, outline_atlas_coord)
	
	game_map.set_layer_modulate(OUTLINE_LAYER, Color(1, 1, 1, alpha_level))
