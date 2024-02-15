class_name EntityDebugger
extends Node2D

var entity: Entity
@onready var state: Label = $State

func _ready():
	entity = get_parent()


func _process(_delta):
	rotation = -entity.rotation
	state.text = entity.state_machine.current_state_name
	queue_redraw()


func _draw():
	#draw_line(Vector2.ZERO, Vector2(2000, 2000), Color.BLACK, 5)
	if entity.mover._point_path_to_target:
		#draw_line(_parent.position, _parent._target_position, Color.BLUE, 1)
		#draw_line(_parent._next_cell_position, _parent._prev_position, Color.BLACK, 1)
		#draw_line(_parent.position, _parent._next_cell_position, Color.RED, 1)
		for point in entity.mover._point_path_to_target:
			var pos = entity.current_map.to_global(point)
			pos = to_local(pos)
			draw_circle(pos, 2, Color.WHITE)


func _write_message():
	var text = ""
	text = "Entity Debugger:\n"
	#text += "Debugger Position: " + str(_text_display.global_position) + "\n"
	#text += "Current Position: " + str(round(_parent.position)) + "\n"
	#text += "Current Rotation: " + str(_parent.rotation) + "\n"
	#text += "Current Tile Coord: " + str(_parent._current_cell) + "\n"
	##text += "Prev Tile Coord: " + str(_parent._prev_tile_coordinate) + "\n"
	#text += "Target Tile Coord: " + str(_parent._target_cell) + "\n"
	##text += "Target Tile Position: " + str(_parent._target_tile_position) + "\n\n"
	#text += "Current State: " + str(_parent.AntState.keys()[_parent._state]) + "\n"
	#text += "Point Path: " + str(_parent._point_path)
	#_text_display.text = text
