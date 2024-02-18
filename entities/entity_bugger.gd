class_name EntityDebugger
extends Node2D

var entity: Entity
@onready var state: Label = $State
@onready var timers: Label = $Timers


func _ready():
	entity = get_parent()


func _process(_delta):
	rotation = -entity.rotation
	state.text = entity.state_machine.current_state_name
	_update_timers()
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
			draw_circle(pos, 2, Color.BLUE)


func _update_timers():
	var text = ""
	text = "Lifetime: " + str(snappedf(entity.body.lifetime.time_left, 0.1)) + ", " + str(entity.body.lifetime.paused) + "\n"
	text += "Til Hunger: " + str(snappedf(entity.body.hunger.time_left, 0.1)) + ", " + str(entity.body.hunger.paused) + "\n"
	text += "Til Starvation: " + str(snappedf(entity.body.starvation.time_left, 0.1)) + "\n"
	text += "Til Rest: " + str(snappedf(entity.body.rest_timer.time_left, 0.1)) + "\n"
	text += "Til Exhaustion: " + str(snappedf(entity.body.exhaustion_timer.time_left, 0.1)) + "\n"
	text += "Til Recovered: " + str(snappedf(entity.body.recovery_timer.time_left, 0.1)) + "\n"
	text += "Til Done Eating: " + str(snappedf(entity.body.eat_timer.time_left, 0.1)) + "\n"
	timers.text = text
