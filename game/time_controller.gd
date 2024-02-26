extends Node2D

var _game_time_scale: float

func _ready():
	_game_time_scale = 2
	
func _process(_delta):
	if Input.is_action_pressed("pause"):
		if Engine.time_scale > 0:
			Engine.time_scale = 0
		else:
			Engine.time_scale = _game_time_scale
	
	if Input.is_action_pressed("0.5x speed"):
		_game_time_scale = 0.5
		Engine.time_scale = _game_time_scale
				
	if Input.is_action_pressed("1x speed"):
		_game_time_scale = 1
		Engine.time_scale = _game_time_scale
	
	if Input.is_action_pressed("1.5x speed"):
		_game_time_scale = 1.5
		Engine.time_scale = _game_time_scale
		
	if Input.is_action_pressed("2x speed"):
		_game_time_scale = 2
		Engine.time_scale = _game_time_scale
	
	if Input.is_action_pressed("3x speed"):
		_game_time_scale = 3
		Engine.time_scale = _game_time_scale
