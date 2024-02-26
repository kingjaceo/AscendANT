class_name Queen extends Node2D

#@export var _messenger: Messenger
var _next_egg_time: float
var _time_elapsed: float

var _hunger_time: float = 10 # seconds before returning home to eat
var _hunger_timer: float = 0
var _starvation_time: float = 10
var _starvation_timer: float = 0
var _eat_amount: float = 1

# Called when the node enters the scene tree for the first time.
func _ready():
	_next_egg_time = 1
	_time_elapsed = 0
	position = Messenger.get_colony_map().map_to_local(Messenger.get_colony_map().spawn_location) + Vector2(0, -6)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	_time_elapsed += delta
	_hunger_timer += delta
	
	if _hunger_timer > _hunger_time:
		_starvation_timer += delta
		if _starvation_timer > _starvation_time:
			die()
		_eat_food()
	elif _time_elapsed > _next_egg_time and Messenger.colony_has_egg_capacity():
		Messenger.lay_egg()
		_time_elapsed = 0


func _eat_food():
	var actual_eat_amount = _eat_amount * (_hunger_timer / _hunger_time)
	var food_taken = Messenger.take_food_from_colony(actual_eat_amount)
	var fullness_percentage = food_taken / actual_eat_amount
	_hunger_timer = (1 - fullness_percentage) * _hunger_time
	
	if _hunger_timer < _hunger_time:
		_starvation_timer = 0

func die():
	queue_free()
	print("GAME OVER")
