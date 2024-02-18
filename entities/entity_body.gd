class_name EntityBody 
extends Node

@export var speed: float
@export var capacity: float
@export var eat_amount: float

@export var lifetime_seconds: float
@export var time_until_hungry_seconds: float
@export var time_until_starvation_seconds: float
@export var time_to_eat: float
@export var time_until_rest: float
@export var time_until_recovered: float
@export var time_until_exhausted: float

var entity: Entity
var hungry: bool
var tired: bool

var lifetime: Timer
var hunger: Timer
var starvation: Timer
var eat_timer: Timer
var rest_timer: Timer
var recovery_timer: Timer
var exhaustion_timer: Timer


func _ready():
	entity = get_parent()
	setup_timers()


func setup_timers():
	lifetime = Timer.new()
	add_child(lifetime)
	lifetime.wait_time = lifetime_seconds 
	lifetime.start()
	
	hunger = Timer.new()

	add_child(hunger)
	hunger.wait_time = time_until_hungry_seconds
	#hunger.timeout.connect(hungry.emit)
	hunger.start()
	
	starvation = Timer.new()
	add_child(starvation)
	starvation.wait_time = time_until_starvation_seconds
	
	eat_timer = Timer.new()
	eat_timer.one_shot = true
	add_child(eat_timer)
	eat_timer.wait_time = time_to_eat
	
	rest_timer = Timer.new()
	rest_timer.one_shot = true
	add_child(rest_timer)
	rest_timer.wait_time = time_until_rest
	rest_timer.start()
	
	recovery_timer = Timer.new()
	recovery_timer.one_shot = true
	add_child(recovery_timer)
	recovery_timer.wait_time = time_until_recovered
	
	exhaustion_timer = Timer.new()
	add_child(exhaustion_timer)
	exhaustion_timer.wait_time = time_until_exhausted


func eat(cell: Vector2i):
	# calculate new hunger level, based on available food
	var amount_eaten = entity.current_map.take_food_from(cell, eat_amount)
	var percentage = amount_eaten / eat_amount
	var actual_hunger_time = time_until_hungry_seconds * percentage
	
	# update timers
	eat_timer.start()
	hunger.stop()
	starvation.stop()
	hunger.wait_time = actual_hunger_time


func rest():
	rest_timer.stop()
	recovery_timer.start()
	exhaustion_timer.stop()
