class_name Entity
extends Node2D

@export var body: EntityBody
@export var mover: EntityMover
@export var animator: AnimationPlayer
var default_state_name: String = "NormalState"

var home_map: GameMap
var current_map: GameMap
var current_cell: Vector2i

var _can_eat: bool = false
var _target_food_cell: Vector2i

@export var state_machine: ANTiStateMachine
var ID: int
var entity_name: String


func _ready() -> void:
	home_map = current_map
	
	_connect_mover()
	_connect_body()
	
	state_machine.change_state(default_state_name)


func update_current_cell():
	current_cell = current_map.local_to_map(position)


func _on_becoming_hungry() -> void:
	body.starvation.start()
	body.hungry = true
	_make_a_choice()


func _on_tired() -> void:
	body.exhaustion_timer.start()
	body.tired = true
	_make_a_choice()


func _on_recovered() -> void:
	body.tired = false
	_make_a_choice()
	


func _on_eaten() -> void:
	body.hungry = false
	_can_eat = false
	_make_a_choice()


func _on_arrived_at_next_cell(): # on arrival, just ask the state machine to update itself
	current_cell = current_map.local_to_map(position)
	# TODO: this sort of behavior should depend on additional modules, like "FoodSenser" and "WaterSenser" etc
	if (body.hungry and _check_food()) or _check_change_map():
		_make_a_choice()


func _on_arrived_at_target() -> void:
	#_make_a_choice()
	pass


func _check_food() -> bool:
	if current_map.has_food(current_cell):
		_target_food_cell = current_cell
		_can_eat = body.hungry
		return true
	return false


func _connect_mover():
	mover.arrived_at_next_cell.connect(_on_arrived_at_next_cell) # ???
	mover.arrived_at_target.connect(_on_arrived_at_target)


func _connect_body():
	body.hunger.timeout.connect(_on_becoming_hungry)
	body.eat_timer.timeout.connect(_on_eaten)
	body.rest_timer.timeout.connect(_on_tired)
	body.recovery_timer.timeout.connect(_on_recovered)
	
	body.lifetime.timeout.connect(die.bind("old age"))
	body.starvation.timeout.connect(die.bind("starvation"))
	body.exhaustion_timer.timeout.connect(die.bind("exhaustion"))


func change_map(new_map: GameMap):
	current_cell = new_map.get_entrance(current_map)
	current_map = new_map
	get_parent().remove_child(self)
	current_map.add_child(self)

	position = current_map.map_to_local(current_cell)


func _check_change_map() -> bool:
	if current_cell in current_map.get_exits():
		var new_map = current_map.get_exit(current_cell)
		change_map(new_map)
		return true
	return false


func _make_a_choice() -> void:
	# the entity should assess its choices:
	if _can_eat:
		state_machine.change_state("EatingState", {"food_cell": _target_food_cell})
	elif body.hungry: 
		state_machine.change_state("SeekFoodState")
	elif body.tired:
		state_machine.change_state("RestingState")
	else:
		state_machine.change_state(default_state_name)


func die(cause: String) -> void:
	print(entity_name, ID, " died: ", cause, "!")
	queue_free()
