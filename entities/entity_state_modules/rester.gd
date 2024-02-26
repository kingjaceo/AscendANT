class_name Rester
extends ANTiStateModule

@export var mover: Mover

@export var time_until_rest: float = 40
@export var rested_threshold: float = 0.1
@export var time_until_recovered: float = 2
@export var time_until_exhausted: float = 40

var rest_timer: Timer
var recovery_timer: Timer
var exhaustion_timer: Timer


func _setup():
	rest_timer = _setup_timer(rest_timer, time_until_rest, true)
	recovery_timer = _setup_timer(recovery_timer, time_until_recovered, false)
	exhaustion_timer = _setup_timer(exhaustion_timer, time_until_exhausted, false)
	
	choice_making_signals = [rest_timer.timeout, recovery_timer.timeout]
	
	death_signals = {exhaustion_timer.timeout: "exhaustion",}
	
	rest_timer.timeout.connect(exhaustion_timer.start)
	recovery_timer.timeout.connect(_stop_rest)


func update_priority() -> void:
	if not exhaustion_timer.is_stopped():
		priority = 2
		behavior = _rest
	else:
		priority = 0
		behavior = _nothing


func _rest() -> void:
	mover.idle()
	rest_timer.stop()
	recovery_timer.start()
	exhaustion_timer.stop()


func _stop_rest() -> void:
	var recovery_percentage = 1 - (time_until_recovered - recovery_timer.time_left) / time_until_recovered
	var rest_time = time_until_rest * recovery_percentage
	rest_timer.start(rest_time)
	recovery_timer.stop()
	if rest_time < rested_threshold * time_until_rest:
		#var exhaustion_time = time_until_exhausted * recovery_percentage
		exhaustion_timer.start()


func get_debug_text() -> String:
	var text = ""
	text += "Rest: " + _get_timer_time(rest_timer) + "\n"
	text += "Recovery: " + _get_timer_time(recovery_timer) + "\n"
	text += "Exhaustion: " + _get_timer_time(exhaustion_timer)
	return text
