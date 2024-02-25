class_name Body 
extends ANTiStateModule

@export var mover: Mover
@export var food_senser: FoodSenser

@export var eat_amount: float

var hunger: Timer
var starvation: Timer
var eat_timer: Timer
@export var time_until_hungry_seconds: float = 40
@export var time_until_starvation_seconds: float = 40
@export var time_to_eat: float = 3

var _food_module: FoodModule

var _last_food_cell: Vector2i


func clear_connections():
	_food_module = null


func connect_map_modules(map_modules: Array[Node]) -> void:
	for module in map_modules:
		if module is FoodModule:
			_food_module = module


func _setup():
	hunger = _setup_timer(hunger, time_until_hungry_seconds, true)
	starvation = _setup_timer(starvation, time_until_starvation_seconds, false)
	eat_timer = _setup_timer(eat_timer, time_to_eat, false)
	
	choice_making_signals = [hunger.timeout, eat_timer.timeout]
	death_signals = {starvation.timeout: "starvation"}
	
	food_senser.food_detected.connect(_on_food_detected)
	hunger.timeout.connect(hunger.stop)
	hunger.timeout.connect(starvation.start)
	eat_timer.timeout.connect(_stop_eat)


func update_priority() -> void:
	if not starvation.is_stopped() and priority < 3:
		priority = 3
		behavior = _seek_food
		exit_behavior = _nothing


func _seek_food() -> void:
	if entity.home_map == entity.current_map:
		mover.move_to(_food_module.get_food_source())
	else:
		mover.move_to(Vector2.ZERO)


func _eat():
	# calculate new hunger level, based on available food
	var amount_eaten = _food_module.take_food_from(_last_food_cell, eat_amount)
	var percentage = amount_eaten / eat_amount
	var actual_hunger_time = time_until_hungry_seconds * percentage
	
	mover.idle()
	
	# update timers
	eat_timer.start()
	hunger.stop()
	starvation.stop()
	hunger.wait_time = actual_hunger_time


func _stop_eat() -> void:
	eat_timer.stop()
	hunger.start()
	priority = 0
	behavior = _nothing
	exit_behavior = _nothing


func get_debug_text() -> String:
	var text = ""
	text += "Hunger: " + get_timer_time(hunger) + "\n"
	text += "Eat Timer: " + get_timer_time(eat_timer) + "\n"
	text += "Starvation: " + get_timer_time(starvation)
	return text


func get_timer_time(timer: Timer) -> String:
	return str(snappedf(timer.time_left, 0.01)) + " s"


func _on_food_detected(cell: Vector2i):
	_last_food_cell = cell
	if not starvation.is_stopped():
		priority = 10
		behavior = _eat
		exit_behavior = _stop_eat
