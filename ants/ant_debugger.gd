extends Node2D

@export var _parent: Node2D
@export var _text_display: RichTextLabel

# Called when the node enters the scene tree for the first time.
func _ready():
	_text_display.global_position = Vector2(-450, -50)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var text = ""
	text = "Ant Debugger:\n"
	text += "Debugger Position: " + str(_text_display.global_position) + "\n"
	text += "Current Position: " + str(round(_parent.position)) + "\n"
	text += "Current Tile Coord: " + str(_parent._current_tile_coordinate) + "\n"
	text += "Prev Tile Coord: " + str(_parent._prev_tile_coordinate) + "\n"
	text += "Target Tile Coord: " + str(_parent._target_tile_coordinate) + "\n"
	text += "Target Tile Position: " + str(_parent._target_tile_position) + "\n\n"
	text += "Current State: " + str(_parent.AntState.keys()[_parent._state]) + "\n"
	_text_display.text = text
	
	queue_redraw()
	

func _draw():
	draw_line(Vector2.ZERO, to_local(_parent._target_tile_position), Color(1, 1, 1), 5)
