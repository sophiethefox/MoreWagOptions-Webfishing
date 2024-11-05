extends Node

onready var PlayerAPI = get_tree().root.get_node("BlueberryWolfiAPIs/PlayerAPI")
onready var KeybindsAPI = get_tree().root.get_node("BlueberryWolfiAPIs/KeybindsAPI")

func _ready():
	
	var toggle_wag = KeybindsAPI.register_keybind({
		"action_name": "toggle_wag",
		"title": "Toggle Wagging",
		"key": KEY_F,
	})
	var wag_slower = KeybindsAPI.register_keybind({
		"action_name": "wag_slower",
		"title": "Wag Slower",
		"key": KEY_BRACKETLEFT,
	})
	var wag_faster = KeybindsAPI.register_keybind({
		"action_name": "wag_faster",
		"title": "Wag Faster",
		"key": KEY_BRACKETRIGHT,
	})
	
	KeybindsAPI.connect(toggle_wag, self, "_on_toggle_wag")
	KeybindsAPI.connect(wag_slower, self, "_on_wag_slower")
	KeybindsAPI.connect(wag_faster, self, "_on_wag_faster")


func _on_toggle_wag()->void :
	PlayerAPI.local_player._wag()
	
func _on_wag_slower()->void :
	pass
	
func _on_wag_faster()->void :
	pass


