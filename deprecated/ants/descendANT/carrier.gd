class_name Carrier
extends Node

@export var capacity: float = 100
@export var dump_time: float = 3
var resources: Dictionary = {}
var at_capacity: bool = false
var most_resource: String
var most_resource_amount: float
var dump_timer: Timer
var data: Dictionary = {"resource": "dirt", "amount": 0}

signal capacity_filled


func _ready() -> void:
	dump_timer = Timer.new()
	add_child(dump_timer)
	dump_timer.one_shot = true


func carry(resource_name: String, amount: float) -> void:
	if resources.has(resource_name):
		resources[resource_name] += min(capacity, amount)
	else:
		resources[resource_name] = min(capacity, amount)
	capacity = max(capacity - amount, 0)
	_update_most_resource(resource_name)
	if capacity == 0 and not at_capacity:
		at_capacity = true
		capacity_filled.emit()


func dump(resource: String, amount: float, map: GameMap) -> void:
	dump_timer.start()
	resources[resource] -= amount
	capacity += amount
	at_capacity = false
	map.dump(resource, amount)


func _update_most_resource(resource_name: String) -> void:
	if resources[resource_name] > most_resource_amount:
		most_resource = resource_name
		most_resource_amount = resources[resource_name]
		data["resource"] = most_resource
		data["amount"] = most_resource_amount
