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
var colony_map: ColonyMap
var _overworld_cell: Vector2i

var _baseANT = preload("res://ants/baseANT.tscn")
var _descendANT = preload("res://ants/descendANT.tscn")
var _debugger = preload("res://ants/baseANTdebugger.tscn")

# Called when the node enters the scene tree for the first time.
func _ready():
	Messenger._colony = self
	pheromone_map = Messenger.get_pheromone_map()
	colony_map = Messenger.get_colony_map()
	Messenger.set_vertical_camera_position(colony_map.map_to_local(colony_map.spawn_location))
	
	_overworld_cell = pheromone_map.choose_random_cell()
	pheromone_map.entrance = _overworld_cell
	var colony_sprite = get_node("Sprite2D")
	remove_child(colony_sprite)
	pheromone_map.add_child(colony_sprite)
	colony_sprite.position = pheromone_map.map_to_local(_overworld_cell)
	Messenger.set_aerial_camera_position(colony_sprite.position)
	
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
	for i in range(30):
		var instance = _descendANT.instantiate()
		add_child(instance)
		instance._world = BaseANT.World.COLONY
		instance._colony_map = colony_map
		instance._pheromone_map = pheromone_map
		Messenger.move_ant_to_world(instance, instance.World.COLONY)
		instance.position = Vector2(264, 264)
		#var debugger = _debugger.instantiate()
		#instance.add_child(debugger)
		instance._current_cell = colony_map.local_to_map(instance.position)
		
	for i in range(100):
		var instance = _baseANT.instantiate()
		add_child(instance)
		instance._world = BaseANT.World.OVERWORLD
		instance._current_cell = pheromone_map.choose_random_neighbor(_overworld_cell)
		#var debugger = _debugger.instantiate()
		#instance.add_child(debugger)
		instance._colony_map = colony_map
		instance._pheromone_map = pheromone_map
		Messenger.move_ant_to_world(instance, instance.World.OVERWORLD)
		instance.position = pheromone_map.map_to_local(instance._current_cell)
		
	current_population += 1


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
	
	_create_ant(_baseANT)


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
	var instance = ant.instantiate()
	add_child(instance)
	instance._world = BaseANT.World.COLONY
	Messenger.move_ant_to_world(instance, instance.World.COLONY)
	instance._colony_map = colony_map
	instance._pheromone_map = pheromone_map
	instance.position = Vector2(264, 264)
	instance._current_cell = colony_map.local_to_map(instance.position)
