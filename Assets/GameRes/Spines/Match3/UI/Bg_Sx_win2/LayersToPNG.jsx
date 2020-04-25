
// This script exports photoshop layers as individual images.
// It also write a JSON file that can be imported into Spine
// where the images will be displayed in the same positions.

// Settings.
var ignoreHiddenLayers = true;
var savePNGs = true;
var saveJSON = true;
var scaleFactor = 1;
showDialog();

function main () {
	// Output dir.
	var dir = app.activeDocument.path + "/images/";

	new Folder(dir).create();

	var name = decodeURI(app.activeDocument.name);
	name = name.substring(0, name.indexOf("."));

	app.activeDocument.duplicate();

	// Collect original layer visibility and hide all layers.
	var layers = [];
	getLayers(app.activeDocument, layers);

	var layerCount = layers.length;
	var layerVisibility = {};

	for (var i = layerCount - 1; i >= 0; i--) {
		var layer = layers[i];
		layerVisibility[layer] = layer.visible;
		layer.visible = false;
	}

	// Save JSON.
	if (saveJSON || savePNGs) {
		var json = "{\"bones\":[{\"name\":\"root\"}],\"slots\":[\n";
		for (var i = layerCount - 1; i >= 0; i--) {
			var layer = layers[i];
			
			if (ignoreHiddenLayers && !layerVisibility[layer]) continue;

			json += "{\"name\":\"" + trim(layer.name) + "\",\"bone\":\"root\",\"attachment\":\"" + trim(layer.name) + "\"},\n";
		}
		json += "],\"skins\":{\"default\":{\n";
		for (var i = layerCount - 1; i >= 0; i--) {
			var layer = layers[i];
			
			if (ignoreHiddenLayers && !layerVisibility[layer]) continue;
				
			var x = app.activeDocument.width.as("px") * scaleFactor;
			var y = app.activeDocument.height.as("px") * scaleFactor;
			
			layer.visible = true;
			if (!layer.isBackgroundLayer)
				app.activeDocument.trim(TrimType.TRANSPARENT, false, true, true, false);
			x -= app.activeDocument.width.as("px") * scaleFactor;
			y -= app.activeDocument.height.as("px") * scaleFactor;
			if (!layer.isBackgroundLayer)
				app.activeDocument.trim(TrimType.TRANSPARENT, true, false, false, true);
			var width = app.activeDocument.width.as("px") * scaleFactor;
			var height = app.activeDocument.height.as("px") * scaleFactor;

			// Save image.
			if (savePNGs) {
				if (scaleFactor != 1) scaleImage();

				var file = File(dir + "/" + trim(layer.name));
				if (file.exists) file.remove();
				activeDocument.saveAs(file, new PNGSaveOptions(), true, Extension.LOWERCASE);

				if (scaleFactor != 1) stepHistoryBack();
			}
			
			if (!layer.isBackgroundLayer) {
				stepHistoryBack();
				stepHistoryBack();
			}
			layer.visible = false;
			
			x += Math.round(width) / 2;
			y += Math.round(height) / 2;
			json += "\"" + trim(layer.name) + "\":{\"" + trim(layer.name) + "\":{\"x\":" + x + ",\"y\":" + y+ ",\"width\":" + Math.round(width) + ",\"height\":" + Math.round(height) + "}},\n";
		}
		json += "}}, \"animations\": { \"animation\": {} }}";

		if (saveJSON) {
			// Write file.
			var file = new File(dir + name + ".json");
			file.remove();
			file.open("a");
			file.lineFeed = "\n";
			file.write(json);
			file.close();
		}
	}

	activeDocument.close(SaveOptions.DONOTSAVECHANGES);
}

// Unfinished!
function hasLayerSets (layerset) {
	layerset = layerset.layerSets;
	for (var i = 0; i < layerset.length; i++)
		if (layerset[i].layerSets.length > 0) hasLayerSets(layerset[i]);
}

function scaleImage() {
	var imageSize = app.activeDocument.width.as("px");
	app.activeDocument.resizeImage(UnitValue(imageSize * scaleFactor, "px"), undefined, 72, ResampleMethod.BICUBICSHARPER);
}

function stepHistoryBack () {
	var desc = new ActionDescriptor();
	var ref = new ActionReference();
	ref.putEnumerated( charIDToTypeID( "HstS" ), charIDToTypeID( "Ordn" ), charIDToTypeID( "Prvs" ));
	desc.putReference(charIDToTypeID( "null" ), ref);
	executeAction( charIDToTypeID( "slct" ), desc, DialogModes.NO );
}

function getLayers (layer, collect) {
	if (!layer.layers || layer.layers.length == 0) return layer;
	for (var i = 0, n = layer.layers.length; i < n; i++) {
		// For checking if its an adjustment layer, but it also excludes
		// LayerSets so we need to find the different types needed.
		//if (layer.layers[i].kind == LayerKind.NORMAL) {
			var child = getLayers(layer.layers[i], collect)
			if (child) collect.push(child);
		//}
	}
}

function trim (value) {
	return value.replace(/^\s+|\s+$/g, "");
}

function hasFilePath() {
	var ref = new ActionReference();
	ref.putEnumerated( charIDToTypeID("Dcmn"), charIDToTypeID("Ordn"), charIDToTypeID("Trgt") ); 
	return executeActionGet(ref).hasKey(stringIDToTypeID('fileReference'));
}

function showDialog () {
	if (!hasFilePath()) {
		alert("File path not found.\nYou need to save the document before continuing.");
		return;
	}

	var dialog = new Window("dialog", "Export Layers");

	dialog.savePNGs = dialog.add("checkbox", undefined, "Save PNGs"); 
	dialog.savePNGs.value = savePNGs;
	dialog.savePNGs.alignment = "left";

	dialog.saveJSON = dialog.add("checkbox", undefined, "Save JSON");
	dialog.saveJSON.alignment = "left";
	dialog.saveJSON.value = saveJSON;

	dialog.ignoreHiddenLayers = dialog.add("checkbox", undefined, "Ignore hidden layers");
	dialog.ignoreHiddenLayers.alignment = "left";
	dialog.ignoreHiddenLayers.value = ignoreHiddenLayers;

	var scaleGroup = dialog.add("panel", [0, 0, 180, 50], "Image Scale");
	var scaleText = scaleGroup.add("edittext", [10,10,40,30], scaleFactor * 100); 
	scaleGroup.add("statictext", [45, 12, 100, 20], "%");
	var scaleSlider = scaleGroup.add("slider", [60, 10,165,20], scaleFactor * 100, 1, 100);
	scaleText.onChanging = function() {
		scaleSlider.value = scaleText.text;
		if (scaleText.text < 1 || scaleText.text > 100) {
			alert("Valid numbers are 1-100.");
			scaleText.text = scaleFactor * 100;
			scaleSlider.value = scaleFactor * 100;
		}
	};
	scaleSlider.onChanging = function() { scaleText.text = Math.round(scaleSlider.value); };

	var confirmGroup = dialog.add("group", [0, 0, 180, 50]);
	var runButton = confirmGroup.add("button", [10, 10, 80, 35], "Ok");
	var cancelButton = confirmGroup.add("button", [90, 10, 170, 35], "Cancel");
	cancelButton.onClick = function() { this.parent.close(0); return; };
	runButton.onClick = function() {
		savePNGs = dialog.savePNGs.value;
		saveJSON = dialog.saveJSON.value;
		ignoreHiddenLayers = dialog.ignoreHiddenLayers.value;
		scaleFactor = scaleSlider.value / 100;
		main();
		this.parent.close(0);
	};

	dialog.orientation = "column";
	dialog.center();
	dialog.show();
}
