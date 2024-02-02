extends Node2D

var _pheromone_map: PheromoneMap

# Called when the node enters the scene tree for the first time.
func _ready():
	z_index = 100
	_pheromone_map = get_parent()
	var theme = Theme.new()
	theme.default_font_size = 12
	for tile in _pheromone_map.get_used_cells(_pheromone_map.TERRAIN_LAYER):
		var label = Label.new()
		label.text = str(tile)
		label.theme = theme
		var size = label.get_minimum_size()
		var offset = size / 2
		label.position = _pheromone_map.map_to_local(tile) - offset
		label.set("theme_override_colors/font_color", Color.BLACK)
		add_child(label)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
