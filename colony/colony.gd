class_name Colony extends Node2D

#@export var _messenger: Messenger
var food: float
var eggs: float
var egg_capacity: float
var current_population: int
var target_population: int
var _food_decay_rate: float # food lost per second
var _egg_hatch_time: float
var _egg_timer: float

var pheromone_map: PheromoneMap
@export var colony_map: GameMap

var _baseANT = preload("res://entities/ants/baseANT.tscn")
var _ANT = preload("res://entities/ants/ANT.tscn")
var _descendANT = preload("res://entities/ants/descendANT/descendANT.tscn")
var _debugger = preload("res://entities/ants/baseANTdebugger.tscn")
var _entity = preload("res://entities/entity.tscn")

# Called when the node enters the scene tree for the first time.
func _ready():
	Messenger.colony = self
	pheromone_map = Messenger.pheromone_map
	
	_set_attributes()
	_create_ants()
		
	Messenger.update_current_population()
	Messenger.update_target_population()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	food -= delta * _food_decay_rate
	Messenger.update_food()
	
	if eggs > 0:
		_egg_timer += delta
	
	if eggs > 0 and _egg_timer > _egg_hatch_time and current_population < target_population:
		hatch_egg()
		_egg_timer = 0


func _create_ants():
	# produce the first ANTs
	for i in range(0):
		# create instance
		_create_ant(_entity)
		
	# TEST: produce the first descendANTs
	for i in range(0):
		_create_ant(_descendANT)


func _set_attributes():
	food = 10
	eggs = 0
	egg_capacity = 20
	_food_decay_rate = 0.2
	_egg_hatch_time = 5
	current_population = 0
	target_population = 0
	

func add_egg():
	eggs += 1
	Messenger.update_eggs()
	
func hatch_egg():
	eggs -= 1
	Messenger.update_eggs()
	Messenger.update_current_population()
	
	_create_ant(_ANT)


func take_food(amount: float):
	amount = min(amount, food)
	food -= amount
	Messenger.update_food()
	return amount


func add_food(amount: float):
	food += amount
	Messenger.update_food()


func increase_target_population():
	target_population += 1
	Messenger.update_target_population()
	
func decrease_target_population():
	target_population -= 1
	Messenger.update_target_population()


func ant_died():
	current_population -= 1
	Messenger.update_current_population()


func _create_ant(ant):
	var spawn_cell = colony_map.spawn_locations[0]
	var instance = ant.instantiate()
	instance.current_map = colony_map
	instance.current_cell = spawn_cell
	colony_map.add_child(instance)
	instance.position = colony_map.map_to_local(spawn_cell)
	current_population += 1
