extends Sprite

var plains1 = preload("res://Images/UI/Plains_1.png")
var plains2 = preload("res://Images/UI/Plains_2.png")
var plains3 = preload("res://Images/UI/Plains_3.png")
var desert1 = preload("res://Images/UI/Desert_1.png")
var desert2 = preload("res://Images/UI/Desert_2.png")

var levels = { 0: plains1, 1: plains2, 2: plains3, 3: desert1, 4: desert2 }

func loadNewBackground(var key):
	var image = levels.get(key)
	texture = image
