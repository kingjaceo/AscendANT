class_name descendANT
extends Entity

static var descendANTid = 0

@export var digger: Digger
@export var carrier: Carrier

#var _can_dig: bool
#var _target_dig_cell: Vector2i
#var _can_dump: bool
#var _target_dump_cell: Vector2i


#func _ready():
	#descendANTid += 1
	#ID = descendANTid
	#entity_name = "DescendANT"
	#
	##default_state_name = "SeekDiggableState"
	#current_map.cell_excavated.connect(_on_cell_excavated)
	#digger.dig_timer.timeout.connect(_on_finished_digging)
	#carrier.capacity_filled.connect(_on_capacity_filled)
	#carrier.dump_timer.timeout.connect(_on_finished_dumping)
	#
	#super._ready()
#
#
#func _on_arrived_at_next_cell():
	##super._on_arrived_at_next_cell()
	#if current_cell == _target_dig_cell and _check_diggable_around():
		#_make_a_choice()
	#if current_cell == _target_dump_cell:
		#_can_dump = true
		#_make_a_choice()
#
#
#func _on_finished_digging():
	#var amount_to_excavate = carrier.capacity
	#var amount_excavated = digger.finish_digging(amount_to_excavate)
	#carrier.carry("dirt", amount_excavated)
	#_can_dig = false
	#_make_a_choice()
#
#
#func _on_finished_dumping():
	#_can_dump = false
	#_make_a_choice()
#
#
#func _on_cell_excavated(cell: Vector2i):
	## check if the cell excavated is the one this entity is digging at
	#if cell == _target_dig_cell:
		#_make_a_choice()
#
#
#func _on_capacity_filled():
	#_make_a_choice()
#
#
#func _make_a_choice():
	#if _can_eat:
		#state_machine.change_state("EatingState", {"food_cell": _target_food_cell})
	#elif body.hungry: 
		#state_machine.change_state("SeekFoodState")
	#elif body.tired:
		#state_machine.change_state("RestingState")
	#elif _can_dump and state_machine.current_state_name != "DumpState":
		#state_machine.change_state("DumpState", carrier.data)
	#elif carrier.at_capacity:
		#var data = {"resource": carrier.most_resource, "amount": carrier.most_resource_amount}
		#state_machine.change_state("SeekDumpSiteState", data)
	#elif _can_dig and state_machine.current_state_name != "DiggingState":
		#state_machine.change_state("DiggingState")
	#else:
		#state_machine.change_state("SeekDiggableState")
#
#
#func _check_diggable_around() -> bool:
	#var colony_cell = current_map.get_colony_cell(current_cell)
	#var accesses = colony_cell.accesses
	#if len(accesses) > 0:
		#_can_dig = true
		#_target_dig_cell = accesses[randi() % len(accesses)]
		#digger.set_dig_cell(_target_dig_cell)
		#return true
	#return false
#
#
#func _change_map() -> bool:
	#return false
