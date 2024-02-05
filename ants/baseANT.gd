class_name BaseANT extends Node2D

'''
All ANTs have the following behaviors:
	- wait / idle / rest
	- choose a new target
	- move between colony and overworld
	- ** return to colony
	- die of old age, starvation, **exposure, or **damage
	- seek food when hungry
	- control animations

To facilitate this, all ANTs knows about:
	- the overworld's map (PheromoneMap)
	- the colony's map (ColonyMap)

To facilitate this, all ANTs track:
	- current world type (colony or overworld)
	- current, previous, target cells
	- current movement state
	- lifetime / hunger / starvation / rest timers 
'''
var ID: int

@onready var _animation_player = $AnimationPlayer
var _current_map: TileMap
var _colony_map: ColonyMap
var _pheromone_map: PheromoneMap
var _world_just_changed: bool

var _world: World
var _movement_state: MovementState
var _walk_speed: float # tiles per second
var _turn_speed: float # seconds to rotate 360 degrees

var _previous_cell: Vector2i
var _current_cell: Vector2i
var _next_cell: Vector2i
var _target_cell: Vector2i
var _target_world: World
var _point_path
var _current_path_index: int
var _next_cell_position: Vector2

var _hungry: bool = false

const EPSILON = 0.1
const TOLERANCE = 1

enum MovementState {IDLE, WALK}
enum World {COLONY, OVERWORLD}

func _ready():
	ID = Messenger.get_next_ant_ID()
	_connect_timers()
	_connect_area2D()
	_movement_state = MovementState.IDLE
	_walk_speed = 40
	_turn_speed = 10
	

func _physics_process(delta):
	if _movement_state == MovementState.WALK:
		_turn_and_move(delta)
	

func _change_movement_state(new_state):
	if new_state == MovementState.IDLE:
		_change_animation("idle")
		_start_timer("UntilActive")
	if new_state == MovementState.WALK:
		_change_animation("walk")
		_start_timer("UntilRest")
	
	_movement_state = new_state
	

func _turn_and_move(delta):
	# check if arrived at next cell position
	var direction = _next_cell_position - position
	var distance_to_next_cell = direction.length()
	if distance_to_next_cell <= TOLERANCE:
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
	
	_check_world_change()
	_check_target()
	_update_next_cell()
		
	_current_cell = _next_cell


func _check_world_change():
	if _world_just_changed:
		_world_just_changed = false
	elif _world == World.COLONY and _current_cell == _colony_map.entrance:
		_change_worlds()
		_world_just_changed = true
	elif _world == World.OVERWORLD and _current_cell == _pheromone_map.entrance:
		_change_worlds()
		_world_just_changed = true
		

func _check_target():
	if _current_cell == _target_cell:
		_choose_next_target()
		

func _update_next_cell():
	if len(_point_path) > 0:
		_next_cell_position = _point_path[_current_path_index] + _current_map.adjustment
		_next_cell = _current_map.local_to_map(_next_cell_position)
		_current_path_index += 1
	

func _choose_next_target():
	if _hungry:
		_choose_food_target()
	elif _world == World.COLONY:
		_choose_colony_target()
	elif _world == World.OVERWORLD:
		_choose_overworld_target()


func _change_worlds():
	if _world == World.COLONY:
		_world = World.OVERWORLD
	elif _world == World.OVERWORLD:
		_world = World.COLONY
	Messenger.move_ant_to_world(self, _world)
	_choose_next_target()
	

func _eat_food():
	_stop_timer("UntilStarvation")
	_start_timer("UntilHunger")
	_hungry = false
	

func die(cause: String = "unknown"):
	print("Ant", ID, " died (", cause, ")!")
	queue_free()


func _on_until_start_timeout():
	_choose_next_target()
	_update_next_cell()
	_change_movement_state(MovementState.WALK)

		
func _choose_colony_target(): # should be overwridden
	_target_cell = _colony_map.choose_random_cell()
	_point_path =  _colony_map.get_point_path(_current_cell, _target_cell)
	_current_path_index = 0


func _choose_overworld_target(): # should be overwridden
	_target_cell = _pheromone_map.choose_random_neighbor(_current_cell)
	_point_path = _pheromone_map.get_point_path(_current_cell, _target_cell)
	_current_path_index = 0

func _choose_food_target():
	_target_cell = _current_map.get_food_cell()
	_point_path =  _current_map.get_point_path(_current_cell, _target_cell)
	_current_path_index = 0
	
func _on_until_rest_timeout():
	_change_movement_state(MovementState.IDLE)

func _on_until_active_timeout():
	_change_movement_state(MovementState.WALK)

#func _on_until_hunger_timeout():
	##_target_cels
	##if _world == World.OVERWORLD:
		##_point_path = path_to_home()
	##if _world == World.COLONY:
		##_point_path = path_to_target_cell()
	#$UntilStarvation.start()


func _connect_timers():
	if has_node("AntTimer"):
		var ant_timer = get_node("AntTimer/Lifetime")
		ant_timer.timeout.connect(die)
		
		ant_timer = get_node("AntTimer/UntilHunger")
		var ant_timer_2 = get_node("AntTimer/UntilStarvation")
		ant_timer.timeout.connect(_become_hungry)
		ant_timer_2.timeout.connect(die)
		
		ant_timer = get_node("AntTimer/UntilRest")
		ant_timer.timeout.connect(_on_until_rest_timeout)
		
		ant_timer = get_node("AntTimer/UntilActive")
		ant_timer.timeout.connect(_on_until_active_timeout)
		
		ant_timer = get_node("AntTimer/UntilStart")
		ant_timer.timeout.connect(_on_until_start_timeout)
	

func _become_hungry():
	_hungry = true
	_start_timer("UntilStarvation")
	
func camera_follow():
	Messenger.camera_follow(_world, self)
	
func _change_animation(name: String):
	if has_node("AnimationPlayer"):
		get_node("AnimationPlayer").play(name)

func _start_timer(name: String):
	if has_node("AntTimer"):
		get_node("AntTimer/" + name).start()
		
func _stop_timer(name: String):
	if has_node("AntTimer"):
		get_node("AntTimer/" + name).stop()

func _connect_area2D():
	if has_node("Area2D"):
		get_node("Area2D").mouse_entered.connect(camera_follow)
#func _input(event):
	#if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		##print("it happened!")
		#var mouse_local =  to_local(event.position)
		#var glob_pos = global_position
		#var distance = (mouse_local - position).length()
		#if distance < 6:
			#print("it happened!")
			#Messenger.camera_follow(_world, self)
