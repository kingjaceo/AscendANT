extends Node2D

'''
All ANTs have the following behaviors:
	- wait / idle / rest
	
	- choose new target position
	-- use pathfinding inside the colony
	-- use pheromone tracing outside the colony
	-- turn to face the "correct" direction
	
	- move between colony and overworld
	
	- return home
	-- use pathfinding
	
	- die of old age or old age
	-- use timers
	
	- seek food
	
	- control animations

To facilitate this, all ANTs must know:
	- about the overworld's map (PheromoneMap)
	- about the colony's map (ColonyMap)

To facilitate this, all ANTs must track:
	- current map type location (colony or overworld)
	- current, previous, target locations
	- current movement state
	- lifetime / hunger timers 
'''
@onready var _animation_player = $AnimationPlayer
var _colony_map: ColonyMap
var _pheromone_map: PheromoneMap

var _world: World
var _movement_state: MovementState
var _walk_speed: float # tiles per second
var _turn_speed: float # seconds to rotate 360 degrees

var _previous_cell: Vector2i
var _current_cell: Vector2i
var _next_cell: Vector2i
var _target_cell: Vector2i
var _point_path
var _current_path_index: int
var _next_cell_position: Vector2

const EPSILON = 0.1

enum MovementState {IDLE, WALK}
enum World {COLONY, OVERWORLD}

func _ready():
	_movement_state = MovementState.IDLE
	_walk_speed = 20
	_turn_speed = 10
	

func _process(delta):
	pass
	

func _physics_process(delta):
	if _movement_state == MovementState.WALK:
		_turn_and_move(delta)
	

func _change_movement_state(new_state):
	if new_state == MovementState.IDLE:
		_animation_player.play("idle")
	if new_state == MovementState.WALK:
		_animation_player.play("walk")
	
	_movement_state = new_state
	

func _turn_and_move(delta):
	# check if arrived at next cell position
	var direction = _next_cell_position - position
	var distance_to_next_cell = direction.length_squared()
	if distance_to_next_cell <= EPSILON:
		_arrive_at_next_cell()
	else:
		_rotate_to_next_cell(delta, direction)
		_move_forward(delta, direction)
	

func _rotate_to_next_cell(delta, direction):
	if direction.length() > EPSILON:
		var angle_to = transform.x.angle_to(direction)
		rotate(sign(angle_to) * min(delta * _turn_speed, abs(angle_to)))
	

func _move_forward(delta, direction):
	position += transform.x * delta * _walk_speed
	

func _arrive_at_next_cell():
	_previous_cell = _current_cell
	_current_cell = _next_cell
	
	if _world == World.COLONY and _current_cell == _colony_map.get_entrance():
		# change worlds (ant leaves colony)
		_change_worlds()
	elif _world == World.OVERWORLD and _current_cell == _pheromone_map.get_entrance():
		# change worlds (ant enters colony)
		_change_worlds()
	
	if _current_cell == _target_cell:
		_choose_next_target()
	else:	
		_current_path_index += 1
		_next_cell = _get_next_cell()
		_next_cell_position = _get_next_cell_position()
	

func _get_next_cell():
	if _world == World.COLONY:
		var cell_position = _get_next_cell_position()
		return _colony_map.local_to_map(cell_position)
	if _world == World.OVERWORLD:
		return _target_cell
	

func _get_next_cell_position():
	if _world == World.COLONY:
		if _current_path_index < len(_point_path):
			return _point_path[_current_path_index] + _colony_map.adjustment
		else:
			pass
	if _world == World.OVERWORLD:
		return _pheromone_map.local_to_map(_target_cell)
	

func _choose_next_target(): # should be overridden by all ANT types
	if _world == World.COLONY:
		_target_cell = _colony_map.choose_random_cell()
		_point_path =  _colony_map.astar_grid.get_point_path(_current_cell, _target_cell)
		_current_path_index = min(len(_point_path) - 1, 1)
		_next_cell = _get_next_cell()
		_next_cell_position = _get_next_cell_position()
	if _world == World.OVERWORLD:
		_target_cell = _pheromone_map.choose_random_neighbor(_current_cell)
	

func _change_worlds():
	if _world == World.COLONY:
		_world = World.OVERWORLD
	if _world == World.OVERWORLD:
		_world = World.COLONY
		
	Messenger.change_worlds(self, _world)
	

func _eat_food():
	pass
	

func die():
	queue_free()


func _on_until_start_timeout():
	_choose_next_target()
	_change_movement_state(MovementState.WALK)
