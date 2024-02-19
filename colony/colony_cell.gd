class_name ColonyCell extends Resource

var coordinates: Vector2i
var accesses: Array
var accessed_by: Array
var dirt_left: float

func _init(cell: Vector2i, to: Array, from: Array) -> void:
	coordinates = cell
	accesses = to
	accessed_by = from
	dirt_left = 50
