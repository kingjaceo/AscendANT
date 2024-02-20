class_name Carrier
extends Node

@export var capacity: float = 100
var resources: Dictionary = {}

func carry(resource_name: String, amount: float):
	if resources.has(resource_name):
		resources[resource_name] += min(capacity, amount)
	else:
		resources[resource_name] = min(capacity, amount)
	capacity = max(capacity - amount, 0)
