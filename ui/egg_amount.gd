extends Label

func _ready():
	Messenger.eggs_updated.connect(_update_amount)
	
func _update_amount():
	text = str(Messenger.get_egg_amount())
