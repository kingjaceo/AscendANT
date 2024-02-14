class_name EntityBody 
extends Node
'''
The body is concerned with tracking:
	1. lifetime
	2. hunger
	3. starvation
	4. idling/resting
	5. recovering
The body is concerned with controlling:
	1. special physical activity (eg, harvesting/dumping food, digging, building)
The body signals:
	1. for a new target on hungry
	2. to cease movement on resting
	3. to resume movement on recovering
	4. the success of a special activity
'''

@export var speed: float
@export var capacity: float
@export var _eat_amount: float

@export var lifetime_seconds: float
@export var time_until_hungry_seconds: float
@export var time_until_starvation_seconds: float
@export var time_to_eat: float

var entity: Entity
var hungry: bool

var lifetime: Timer
var hunger: Timer
var starvation: Timer
var eat_timer: Timer


func _ready():
	entity = get_parent()
	setup_timers()


func setup_timers():
	lifetime = Timer.new()
	add_child(lifetime)
	lifetime.wait_time = lifetime_seconds 
	lifetime.start()
	
	hunger = Timer.new()
	hunger.one_shot = true
	add_child(hunger)
	hunger.wait_time = time_until_hungry_seconds
	#hunger.timeout.connect(hungry.emit)
	hunger.start()
	
	starvation = Timer.new()
	add_child(starvation)
	starvation.wait_time = time_until_starvation_seconds
	
	eat_timer = Timer.new()
	add_child(eat_timer)
	eat_timer.wait_time = time_to_eat


func eat(cell: Vector2i):
	# calculate new hunger level, based on available food
	var amount_eaten = entity.current_map.take_food_from(cell, _eat_amount)
	var percentage = amount_eaten / _eat_amount
	var actual_hunger_time = time_until_hungry_seconds * percentage
	
	# update timers
	eat_timer.start()
	starvation.stop()
	hunger.wait_time = actual_hunger_time
	
