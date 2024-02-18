class_name descendANT
extends Entity

static var descendANTid = 0

var _can_dig: bool
var _target_dig_cell: Vector2i


func _ready():
	descendANTid += 1
	ID = descendANTid
	entity_name = "DescendANT"
	
	default_state_name = "SeekDiggableState"
	current_map.cell_excavated.connect(_on_cell_excavated)
	super._ready()


func _on_arrived_at_next_cell():
	super._on_arrived_at_next_cell()
	_check_diggable_around()


func _on_finished_digging():
	state_machine.change_state("SeekDiggableState")


func _on_cell_excavated(cell: Vector2i):
	# check if the cell excavated is the one this entity is digging at
	if cell == _target_dig_cell:
		state_machine.change_state("SeekDiggableState")


func _make_a_choice():
	if body.hungry: 
		state_machine.change_state("SeekFoodState")
	elif body.tired:
		state_machine.change_state("RestingState")
	elif _can_dig:
		state_machine.change_state("DiggingState")
	else:
		state_machine.change_state("SeekDiggableState")


func _check_diggable_around():
	var colony_cell = current_map.get_colony_cell(current_cell)
	var accesses = colony_cell.accesses
	if len(accesses) > 0:
		_can_dig = true
		_target_dig_cell = accesses[randi() % len(accesses)]
