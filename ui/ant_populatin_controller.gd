extends PanelContainer

var _increase: TextureButton
var _decrease: TextureButton
var _current: Label
var _target: Label

# Called when the node enters the scene tree for the first time.
func _ready():
	_increase = find_child("Increase")
	_increase.pressed.connect(Messenger.increase_ant_limit)
	
	_decrease = find_child("Decrease")
	_decrease.pressed.connect(Messenger.decrease_ant_limit)
	
	_current = find_child("CurrentPopulation")
	Messenger.current_population_updated.connect(_update_current)
	_target = find_child("TargetPopulation")
	Messenger.target_population_updated.connect(_update_target)
	
func _update_current():
	_current.text = str(Messenger.get_current_population())
	
func _update_target():
	_target.text = str(Messenger.get_target_population())
