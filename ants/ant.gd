extends Node2D

'''
The Ant needs the following functionality:
	1. know what tile it is currently located on
	2. know what tile it just came from
	3. ability to choose next tile from surrounding tiles
'''

@export var _messenger: Node2D

var _target_tile_coordinate: Vector2i
var _target_tile_position: Vector2

var _current_tile_coordinate: Vector2i
var _current_tile_position: Vector2

var _prev_tile_coordinate: Vector2i
var _prev_tile_position: Vector2

var _state: AntState
var _scout_state: ScoutState

enum AntState {WAITING, CHOOSING, MOVING, TURNING}
enum ScoutState {SCOUTING, FOUND, SURROUNDED}

var _time_elapsed = 0
var _wait_time = 0.1 # seconds to wait before choosing next action
var _walk_speed = 3 # tiles per second
var _turn_speed = 10 # seconds to rotate 360 degrees

const EPSILON = 0.1

# Called when the node enters the scene tree for the first time.
func _ready():
	# set first state
	_state = AntState.WAITING
	
	# set current tile info
	_current_tile_coordinate = _messenger.get_tile_coordinate(position)
	var tile_position = _messenger.get_tile_position(_current_tile_coordinate)
	position = tile_position
	_current_tile_position = tile_position
	
	# set prev tile info
	_prev_tile_coordinate = _current_tile_coordinate
	_prev_tile_position = _current_tile_position


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	_time_elapsed += delta
			
	if _state == AntState.WAITING:
		# the ant waits for its next movement
		if _time_elapsed > _wait_time:
			_state = AntState.CHOOSING
	
	if _state == AntState.CHOOSING:
		# the ant scans the neighboring tiles and chooses its next tile
		_choose_tile()
		_state = AntState.TURNING


func _physics_process(delta):
	if _state == AntState.TURNING:
		# the ant turns to face the target tile
		_rotate_to_target(delta)
		
	elif _state == AntState.MOVING:
		# the ant moves toward the center of the target tile
		_move_to_target(delta)


func _rotate_to_target(delta):
	var direction = (_target_tile_position - global_position)
	var angle_to = transform.x.angle_to(direction)
	
	if abs(angle_to) < EPSILON:
		_state = AntState.MOVING
		
	rotate(sign(angle_to) * min(delta * _turn_speed, abs(angle_to)))
	

func _move_to_target(delta):
	var direction = (_target_tile_position - _current_tile_position)
	var distance_left = (_target_tile_position - global_position).length_squared()
	
	if distance_left < EPSILON:
		global_position = _target_tile_position
		
		_state = AntState.WAITING
		_time_elapsed = 0
		
		_prev_tile_coordinate = _current_tile_coordinate
		_prev_tile_position = _current_tile_position
		
		_current_tile_coordinate = _target_tile_coordinate
		_current_tile_position = _target_tile_position
		
		if _scout_state == ScoutState.SCOUTING:
			_messenger.mark_cell(_current_tile_coordinate, Pheromones.pheromones[Pheromones.Names.EXPLORED])
		elif _scout_state == ScoutState.FOUND:
			_messenger.mark_cell(_current_tile_coordinate, Pheromones.pheromones[Pheromones.Names.TO_FOOD])
	else:
		global_position += direction * delta * _walk_speed
		

func _choose_tile():
	var choice
	
	if _scout_state == ScoutState.SCOUTING:
		choice = _choose_scout_tile()
	else:
		choice = _choose_homeward_tile()
		
	_target_tile_coordinate = choice
	_target_tile_position = _messenger.get_tile_position(_target_tile_coordinate)
	

func _choose_homeward_tile():
	var choice
	var neighbors = _messenger.get_surrounding_pheromone_cells(_current_tile_coordinate)
	var home = _messenger.get_home_tile()
	var current_distance = _hex_distance(home, _current_tile_coordinate)
	if current_distance == 0:
		_scout_state = ScoutState.SCOUTING
		return _choose_scout_tile()
	
	for neighbor in neighbors:
		var new_distance = _hex_distance(home, neighbor.coordinates)
		if new_distance < current_distance:
			choice = neighbor.coordinates
	
	return choice

func _choose_scout_tile():
	var neighbors = _messenger.get_surrounding_pheromone_cells(_current_tile_coordinate)
	var choices = []
	
	for neighbor in neighbors:
		if neighbor.strongest_pheromone and neighbor.strongest_pheromone.name == Pheromones.Names.FOOD:
			_scout_state = ScoutState.FOUND
			_messenger.mark_cell(_current_tile_coordinate, Pheromones.pheromones[Pheromones.Names.TO_FOOD])
			return _choose_homeward_tile()
		if not neighbor.strongest_pheromone:
			choices.append(neighbor.coordinates)
			
	var choice
	var choice_index
	if len(choices) == 0:
		choice_index = randi() % len(neighbors)
		choice = neighbors[choice_index].coordinates
	else:
		choice_index = randi() % len(choices)
		choice =  choices[choice_index]
		
	return choice


func _hex_distance(coord1, coord2):
	var q1 = coord1[0]
	var q2 = coord2[0]
	var r1 = coord1[1] - (coord1[0] - (coord1[0]&1)) / 2
	var r2 = coord2[1] - (coord2[0] - (coord2[0]&1)) / 2
	
	var distance = (abs(q1 - q2) + abs(q1 + r1 - q2 - r2) + abs(r1 - r2)) / 2
	
	return distance
