extends Label

func _ready():
	Messenger.food_updated.connect(_update_amount)
	
func _update_amount():
	text = str(snappedf(Messenger.get_food_amount(), 0.1))
