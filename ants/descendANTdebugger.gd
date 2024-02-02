extends Node2D

@export var _parent: Node2D
var _text_display: RichTextLabel

# Called when the node enters the scene tree for the first time.
func _ready():
	_text_display = get_node("TextDisplay")
	#_text_display.global_position = Vector2(-450, -50)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var text = ""
	text = "descendANT Debugger:\n"
	text += "Debugger Position: " + str(_text_display.global_position) + "\n"
	text += "Current Position: " + str(round(_parent.position)) + "\n"
	text += "Current Rotation: " + str(_parent.rotation) + "\n"
	text += "Current Tile Coord: " + str(_parent._current_cell) + "\n"
	#text += "Prev Tile Coord: " + str(_parent._prev_tile_coordinate) + "\n"
	text += "Target Tile Coord: " + str(_parent._target_cell) + "\n"
	#text += "Target Tile Position: " + str(_parent._target_tile_position) + "\n\n"
	text += "Current State: " + str(_parent.AntState.keys()[_parent._state]) + "\n"
	text += "Point Path: " + str(_parent._point_path)
	_text_display.text = text
	
	queue_redraw()
	

func _draw():
	if _parent._point_path:
		draw_line(_parent.position, _parent._target_position, Color.BLUE, 1)
		draw_line(_parent._next_cell_position, _parent._prev_position, Color.BLACK, 1)
		draw_line(_parent.position, _parent._next_cell_position, Color.RED, 1)
		for point in _parent._point_path:
			point += Vector2(8, 8)
			draw_circle(point, 1, Color.WHITE)
