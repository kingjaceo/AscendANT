class_name descendANT
extends Entity

static var descendANTid = 0

@export var digger: Digger
@export var carrier: Carrier

var _can_dig: bool
var _target_dig_cell: Vector2i


func _ready():
	descendANTid += 1
	ID = descendANTid
	entity_name = "DescendANT"
	
	default_state_name = "SeekDiggableState"
	current_map.cell_excavated.connect(_on_cell_excavated)
	digger.dig_timer.timeout.connect(_on_finished_digging)
	
	super._ready()


func _on_arrived_at_next_cell():
	super._on_arrived_at_next_cell()
	if current_cell == _target_dig_cell and _check_diggable_around():
		_make_a_choice()


func _on_finished_digging():
	var time_left = digger.dig_timer.time_left
	_can_dig = false
	_make_a_choice()


func _on_cell_excavated(cell: Vector2i):
	# check if the cell excavated is the one this entity is digging at
	if cell == _target_dig_cell:
		_make_a_choice()


func _make_a_choice():
	if _can_eat:
		state_machine.change_state("EatingState", {"food_cell": _target_food_cell})
	elif body.hungry: 
		state_machine.change_state("SeekFoodState")
	elif body.tired:
		state_machine.change_state("RestingState")
	elif _can_dig and state_machine.current_state_name != "DiggingState":
		state_machine.change_state("DiggingState")
	else:
		state_machine.change_state("SeekDiggableState")


func _check_diggable_around() -> bool:
	var colony_cell = current_map.get_colony_cell(current_cell)
	var accesses = colony_cell.accesses
	if len(accesses) > 0:
		_can_dig = true
		_target_dig_cell = accesses[randi() % len(accesses)]
		digger.set_dig_cell(_target_dig_cell)
		return true
	return false
