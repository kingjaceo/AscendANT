extends Node2D
@onready var animation_player = $AnimationPlayer
'''
The Ant needs the following functionality:
	1. know what tile it is currently located on
	2. know what tile it just came from
	3. ability to choose next tile from surrounding tiles
'''
var _target_tile_coordinate: Vector2i
var _target_tile_position: Vector2
var _current_tile_coordinate: Vector2i
var _current_tile_position: Vector2
var _prev_tile_coordinate: Vector2i
var _prev_tile_position: Vector2

var _target_pheromone: Pheromone
var _mark_pheromone: Pheromone

var _state: AntState
var _scout_state: ScoutState

var _hunger_return_time: float = 60 # seconds before returning home to eat
var _hunger_return_timer: float = 0
var _starvation_time: float = 20
var _starvation_timer: float = 0
var _food_held: float = 0
var _food_capacity: float = 25
var _eat_amount: float = 1

enum AntState {WAITING, CHOOSING, MOVING, TURNING}
enum ScoutState {SCOUTING, TO_HOME, TO_FOOD}

var _time_elapsed = 0
var _wait_time = 0.0 # seconds to wait before choosing next action
var _walk_speed = 1 # tiles per second
var _turn_speed = 10 # seconds to rotate 360 degrees

const EPSILON = 0.1

# Called when the node enters the scene tree for the first time.
func _ready():
	animation_player.play("walking")
	$Lifetime.timeout.connect(die)
	
	# set first state
	_state = AntState.WAITING
	
	# set current tile info
	_current_tile_coordinate = Messenger.get_tile_coordinate(position)
	var tile_position = to_local(Messenger.get_tile_position(_current_tile_coordinate))
	position = tile_position
	_current_tile_position = tile_position
	
	# set prev tile info
	_prev_tile_coordinate = _current_tile_coordinate
	_prev_tile_position = _current_tile_position
	
	# set pheromone info
	_target_pheromone = Pheromones.FOOD
	_mark_pheromone = Pheromones.EXPLORED


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	_time_elapsed += delta
	_hunger_return_timer += delta
	if _hunger_return_timer > _hunger_return_time:
		_scout_state = ScoutState.TO_HOME
		_starvation_timer += delta
		if _starvation_timer > _starvation_time:
			die()
			
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
		_arrive_at_new_tile()
	else:
		position += direction * delta * _walk_speed
		

func _arrive_at_new_tile():
	position = _target_tile_position
		
	_state = AntState.WAITING
	_time_elapsed = 0
	
	_prev_tile_coordinate = _current_tile_coordinate
	_prev_tile_position = _current_tile_position
	
	_current_tile_coordinate = _target_tile_coordinate
	_current_tile_position = _target_tile_position
	
	if _scout_state != ScoutState.TO_FOOD:
		Messenger.mark_cell(_current_tile_coordinate, _mark_pheromone, _prev_tile_coordinate)
	

func _choose_tile():
	var choice
	
	if _scout_state == ScoutState.SCOUTING:
		_choose_scout_tile()
	elif _scout_state == ScoutState.TO_HOME:
		_choose_to_home_tile()
	elif _scout_state == ScoutState.TO_FOOD:
		_choose_to_food_tile()
		
	_target_tile_position = Messenger.get_tile_position(_target_tile_coordinate)
	

func _choose_to_home_tile():
	var choice_tile
	var neighbors = Messenger.get_surrounding_pheromone_cells(_current_tile_coordinate)
	var home = Messenger.get_home_tile()
	var current_distance = _hex_distance(home, _current_tile_coordinate)
	
	if current_distance == 0:
		_scout_state = ScoutState.SCOUTING
		_mark_pheromone = Pheromones.EXPLORED
		_deliver_food()
		_eat_food()
		_choose_scout_tile()
		return
	
	for pheromone_cell in neighbors:
		var new_distance = _hex_distance(home, pheromone_cell.coordinates)
		if new_distance < current_distance:
			choice_tile = pheromone_cell.coordinates
	
	_target_tile_coordinate = choice_tile
	

func _choose_scout_tile():
	var neighbors = Messenger.get_surrounding_pheromone_cells(_current_tile_coordinate)
	var choices = []
	
	for neighbor in neighbors:
		if _check_for_food(neighbor):
			return
			
		if neighbor.strongest_pheromone and neighbor.strongest_pheromone.name == Pheromones.Names.TO_FOOD:
			_scout_state = ScoutState.TO_FOOD
			_mark_pheromone = null
			_target_tile_coordinate = neighbor.coordinates
			return
			
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
		
	_target_tile_coordinate = choice
	

func _check_for_food(neighbor):
	if neighbor.strongest_pheromone and neighbor.strongest_pheromone.name == Pheromones.Names.FOOD:
		_scout_state = ScoutState.TO_HOME
		_mark_pheromone = Pheromones.TO_FOOD
		Messenger.mark_cell(_current_tile_coordinate, _mark_pheromone, neighbor.coordinates)
		_grab_food(neighbor.coordinates)
		_choose_to_home_tile()
		return true
	

func _choose_to_food_tile():
	var neighbors = Messenger.get_surrounding_pheromone_cells(_current_tile_coordinate)
	var choices = []
	
	for neighbor in neighbors:
		if _check_for_food(neighbor):
			return
		
		# collect list of TO_FOOD neighbors
		var target_pheromone = Pheromones.TO_FOOD
		if neighbor.pheromone_strengths.has(target_pheromone):
			choices.append(neighbor)
		
	# if this tile is a TO_FOOD tile, we will move to its FROM tile
	var current_pheromone_tile = Messenger.get_pheromone_cell(_current_tile_coordinate)
	var to_food_pheromone = Pheromones.TO_FOOD
	if current_pheromone_tile.pheromone_directions.has(to_food_pheromone):
		var from_tile = current_pheromone_tile.pheromone_directions[to_food_pheromone]
		_target_tile_coordinate = from_tile
		
	# otherwise, return home
	else:
		_scout_state = ScoutState.TO_HOME
		_mark_pheromone = null
		_choose_to_home_tile()
		return
	

func _hex_distance(coord1, coord2):
	var q1 = coord1[0]
	var q2 = coord2[0]
	var r1 = coord1[1] - (coord1[0] - (coord1[0]&1)) / 2
	var r2 = coord2[1] - (coord2[0] - (coord2[0]&1)) / 2
	
	var distance = (abs(q1 - q2) + abs(q1 + r1 - q2 - r2) + abs(r1 - r2)) / 2
	
	return distance


func _deliver_food():
	Messenger.add_food_to_colony(_food_held)
	_food_held = 0
	

func _grab_food(tile):
	_food_held += Messenger.take_food_from_tile(tile, _food_capacity)


func _eat_food():
	var actual_eat_amount = _eat_amount * (_hunger_return_timer / _hunger_return_time)
	var food_taken = Messenger.take_food_from_colony(actual_eat_amount)
	var fullness_percentage = food_taken / actual_eat_amount
	_hunger_return_timer = (1 - fullness_percentage) * _hunger_return_time
	
	if _hunger_return_timer < _hunger_return_time:
		_starvation_timer = 0

func die():
	Messenger.ant_died()
	queue_free()
