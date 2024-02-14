class_name Entity
extends Node2D

## perhaps should be a high-level state machine, defining behaviors for several states, eg
@onready var brain: EntityBrain = %EntityBrain
@onready var body: EntityBody = %EntityBody
@onready var mover: EntityMover = %EntityMover

#@export var _entity_data = load()
var current_map: GameMap
var colony_map: GameMap
var current_cell: Vector2i

@export var state_machine: StateMachine
var id_counter: int = 0
var ID: int

func _ready():
	current_map = get_parent().get_node("ColonyMap64") 
	
	id_counter += 1
	ID = id_counter
	
	mover.arrived_at_next_cell.connect(_on_arrived_at_next_cell) # ???
	mover.arrived_at_target.connect(_on_arrived_at_target)
	
	body.hunger.timeout.connect(_on_becoming_hungry)
	body.eat_timer.timeout.connect(_on_eaten)
	body.lifetime.timeout.connect(die.bind("old age"))
	body.starvation.timeout.connect(die.bind("starvation"))
	#_brain.found_food.connect(_on_finding_food)
	
	state_machine.change_state("NormalState")


func _on_becoming_hungry():
	body.hungry = true
	state_machine.change_state("HungryState")


func _on_eaten():
	body.hungry = false
	state_machine.change_state("NormalState")


func food_found(food_cell: Vector2i):
	if body.hungry:
		state_machine.change_state("EatingState", {"food_cell": food_cell})


func _on_arrived_at_next_cell(): # on arrival, just ask the state machine to update itself
	current_cell = current_map.local_to_map(position)
	# TODO: this sort of behavior should depend on additional modules, like "FoodSenser" and "WaterSenser" etc
	_check_food()


func _on_arrived_at_target():
	state_machine.change_state(state_machine.current_state_name)


func _check_food():
	for cell in current_map.get_surrounding_cells(current_cell):
		if cell == current_map.get_nearest_food_cell(cell):
			food_found(cell)
			return
	
	if current_cell == current_map.get_nearest_food_cell(current_cell):
		food_found(current_cell)


func die(cause: String):
	print("Entity", ID, " died: ", cause, "!")
	queue_free()
