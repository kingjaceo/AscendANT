class_name Entity
extends Node2D

@export var body: EntityBody
@export var mover: EntityMover
@export var animator: AnimationPlayer

var home_map: GameMap
var current_map: GameMap
var current_cell: Vector2i

@export var state_machine: StateMachine
var id_counter: int = 0
var ID: int

func _ready() -> void:
	home_map = current_map
	
	id_counter += 1
	ID = id_counter
	
	_connect_mover()
	_connect_body()
	
	state_machine.change_state("NormalState")


func _on_becoming_hungry() -> void:
	body.starvation.start()
	body.hungry = true
	state_machine.change_state("SeekFoodState")


func _on_tired() -> void:
	body.exhaustion_timer.start()
	body.tired = true


func _on_recovered() -> void:
	state_machine.change_state("NormalState")


func _on_eaten() -> void:
	body.hungry = false
	state_machine.change_state("NormalState")


func food_found(food_cell: Vector2i) -> void:
	if body.hungry:
		state_machine.change_state("EatingState", {"food_cell": food_cell})


func _on_arrived_at_next_cell(): # on arrival, just ask the state machine to update itself
	current_cell = current_map.local_to_map(position)
	
	# TODO: this sort of behavior should depend on additional modules, like "FoodSenser" and "WaterSenser" etc
	_check_food()
	_check_change_map()


func _on_arrived_at_target() -> void:
	# the entity should assess its choices:
	if body.hungry: 
		state_machine.change_state("SeekFoodState")
	elif body.tired:
		state_machine.change_state("RestingState")
	else:
		state_machine.change_state("NormalState")


func _check_food():
	for cell in current_map.get_surrounding_cells(current_cell):
		if cell == current_map.get_nearest_food_cell(cell):
			food_found(cell)
			return
	
	if current_map.has_food(current_cell):
		food_found(current_cell)


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


func _check_change_map():
	if current_cell in current_map.get_exits():
		var new_map = current_map.get_exit(current_cell)
		change_map(new_map)


func die(cause: String):
	print("Entity", ID, " died: ", cause, "!")
	queue_free()
