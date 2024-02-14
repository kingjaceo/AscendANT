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
@export var _behavior: Behavior

var _current_map: TileMap
var colony_map: ColonyMap:
	get:
		return colony_map
	set(map):
		colony_map = map
var pheromone_map: PheromoneMap:
	get:
		return pheromone_map
	set(map):
		pheromone_map = map
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
var _point_path: Array
var _current_path_index: int
var _next_cell_position: Vector2

var _hungry: bool = false

const EPSILON = 0.0001
const TOLERANCE = 1

enum MovementState {IDLE, WALK}
enum World {COLONY, OVERWORLD}

func _ready():
	pheromone_map = Messenger.pheromone_map
	colony_map = Messenger.colony_map
	ID = Messenger.get_next_ant_ID()
	_connect_timers()
	_connect_area2D()
	_movement_state = MovementState.IDLE
	_walk_speed = 100
	_turn_speed = 100
	
func _physics_process(delta):
	if _movement_state == MovementState.WALK:
		_turn_and_move(delta)
	

func set_map(map: TileMap) -> void:
	_current_map = map
	
func set_current_cell(cell: Vector2i) -> void:
	position = _current_map.map_to_local(cell)
	_current_cell = cell
	_choose_next_target()
	_update_next_cell()
	
	

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
	
	_behavior.move_to_new_cell(_world, _current_cell)
	_check_world_change()
	_check_target()
	_update_next_cell()
	
	_current_cell = _next_cell


func _check_world_change():
	if _world_just_changed:
		_world_just_changed = false
	elif _world == World.COLONY and _current_cell == colony_map.entrance:
		_change_worlds()
		_world_just_changed = true
	elif _world == World.OVERWORLD and _current_cell == pheromone_map.entrance:
		_change_worlds()
		_world_just_changed = true
		

func _check_target():
	#if _current_cell == colony_map.get_food_cell():
		#dump_food()
	if _current_cell == _target_cell:
		_choose_next_target()
		

func _update_next_cell():
	if len(_point_path) > 0:
		_next_cell_position = _point_path[_current_path_index] + _current_map.adjustment
		_next_cell = _current_map.local_to_map(_next_cell_position)
		_current_path_index += 1
	

func _choose_next_target():
	if _hungry:
		_target_cell = _behavior.get_food_target_cell(_current_map)
		_point_path = _behavior.get_world_point_path(_world, _current_cell, _target_cell)
	else:
		_target_cell = _behavior.get_world_target_cell(_world, _current_cell)
		_point_path = _behavior.get_world_point_path(_world, _current_cell, _target_cell)
	
	_current_path_index = min(len(_point_path) - 1, 0)


func _change_worlds():
	if _world == World.COLONY:
		_world = World.OVERWORLD
	elif _world == World.OVERWORLD:
		_world = World.COLONY
	Messenger.move_ant_to_world(self, _world)
	

func eat_food():
	_stop_timer("UntilStarvation")
	_start_timer("UntilHunger")
	_hungry = false
	

func die(cause: String = "unknown"):
	Messenger.ant_died()
	print("Ant", ID, " died (", cause, ")!")
	queue_free()


func _on_until_start_timeout():
	_choose_next_target()
	_update_next_cell()
	_change_movement_state(MovementState.WALK)
		

func _on_until_rest_timeout():
	_change_movement_state(MovementState.IDLE)

func _on_until_active_timeout():
	_change_movement_state(MovementState.WALK)
	

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

func _input(event):
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		# Get the mouse position in global coordinates
		var mouse_global = get_global_mouse_position()
		var distance = (mouse_global - position).length()
		if distance < 6:
			Messenger.camera_follow(_world, self)
