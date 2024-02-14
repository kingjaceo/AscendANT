extends VBoxContainer

var _increase: Button
var _decrease: Button

# Called when the node enters the scene tree for the first time.
func _ready():
	_increase = get_node("Increase")
	_increase.pressed.connect(Messenger.ant_limit_increased)
	_decrease = get_node("Decrease")
	_decrease.presseed.connect(Messenger.ant_limit_decreased)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
