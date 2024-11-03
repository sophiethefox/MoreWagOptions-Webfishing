extends Node

var PlayerAPI
var KeybindsAPI

func _ready():
	KeybindsAPI = get_node_or_null("/root/BlueberryWolfiAPIs/KeybindsAPI")
	PlayerAPI = get_node_or_null("/root/BlueberryWolfiAPIs/PlayerAPI")
	
	var toggle_wag = KeybindsAPI.register_keybind({
		"action_name": "toggle_wag",
		"title": "Toggle Wagging",
		"key": KEY_F,
	})
	
	KeybindsAPI.connect(toggle_wag, self, "_on_toggle_wag")


func _on_toggle_wag()->void :
	PlayerAPI.local_player._wag()


