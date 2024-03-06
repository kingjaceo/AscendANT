class_name MapObject
extends Entity

@export var object_name: String
@onready var modules = %Modules


func _ready() -> void:
	ID_counter += 1
	ID = ID_counter
	
	modules = modules.get_children()
	
	home_map = current_map
	_connect_map_modules(current_map.map_modules)
	_connect_module_signals()
	_setup()


func _setup():
	pass


func _connect_map_modules(map_modules: Array[Node]) -> void:
	for module in modules:
		module.clear_connections()
		module.connect_map_modules(map_modules)


func _connect_module_signals():
	for module in modules:
		for death_signal in module.death_signals:
			var reason = module.death_signals[death_signal]
			death_signal.connect(_die.bind(reason))


func _die(cause: String) -> void:
	print(self, " died: ", cause, "!")
	died.emit()
	queue_free()


func _to_string() -> String:
	return object_name + str(ID)

