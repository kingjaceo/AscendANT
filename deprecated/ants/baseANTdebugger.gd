extends Node2D

var _parent
var _text_display: RichTextLabel

# Called when the node enters the scene tree for the first time.
func _ready():
	_text_display = get_node("TextDisplay")
	_parent = get_parent()
	#_text_display.global_position = Vector2(-450, -50)
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	var text = ""
	text = "baseANT Debugger:\n"
	text += "Debugger Position: " + str(_text_display.global_position) + "\n"
	text += "Current Position: " + str(round(_parent.position)) + "\n"
	text += "Current Rotation: " + str(snappedf(_parent.rotation, 0.01)) + "\n"
	text += "Current Tile Coord: " + str(_parent._current_cell) + "\n"
	#text += "Prev Tile Coord: " + str(_parent._prev_tile_coordinate) + "\n"
	text += "Target Tile Coord: " + str(_parent._target_cell) + "\n"
	#text += "Target Tile Position: " + str(_parent._target_tile_position) + "\n\n"
	#text += "Current State: " + str(_parent.AntState.keys()[_parent._state]) + "\n"
	#text += "Point Path: " + str(_parent._point_path)
	_text_display.text = text
	
	queue_redraw()
	

func _draw():
	if _parent._point_path:
		for point in _parent._point_path:
			point += _parent._current_map.adjustment
			draw_circle(point, 1, Color.WHITE)
			
	if _parent._current_map:
		var target_position = _parent._current_map.map_to_local(_parent._target_cell)
		var prev_position = _parent._current_map.map_to_local(_parent._previous_cell)
		draw_line(_parent.position, target_position, Color.BLUE, 1)
		draw_line(_parent._next_cell_position, prev_position, Color.BLACK, 1)
		draw_line(_parent.position, _parent._next_cell_position, Color.RED, 1)
		
