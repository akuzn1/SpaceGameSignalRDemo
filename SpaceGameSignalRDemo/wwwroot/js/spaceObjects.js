var SpaceObjects = function () {
	var objects = []
	return {
		getById: function (id) {
			for (var i in objects) {
				if (objects[i].id == id)
					return objects[i];
			}
		},

		getItems: function () {
			return objects;
		},

		removeById: function (id) {
			for (var i in objects) {
				if (objects[i].id == id) {
					objects.splice(i, 1);
					break;
				}
			}
		},

		erase: function () {
			objects = [];
		},

		add: function (object) {
			objects[objectbjects.length] = object;
		},

		addMany: function (objectList) {
			for (var i in objectList) {
				objects[objects.length] = objectList[i];
			}
		},

		recalculatePositions: function () {
			for (var i in objects) {
				if (objects[i].speed == 0)
					continue;

				var D = Math.sqrt((objects[i].targetX - objects[i].x) * (objects[i].targetX - objects[i].x) + (objects[i].targetY - objects[i].y) * (objects[i].targetY - objects[i].y));
				var koef = objects[i].speed / D;
				if (koef < 1) {
					objects[i].x += (objects[i].targetX - objects[i].x) * koef;
					objects[i].y += (objects[i].targetY - objects[i].y) * koef;
				}
				else {
					objects[i].x = objects[i].targetX;
					objects[i].y = objects[i].targetY;
					objects[i].speed = 0;
				}
			}
		},

		findAll: function (x, y) {
			var result = [];
			for (var i in objects) {
				var D = Math.sqrt((x - objects[i].x) * (x - objects[i].x) + (y - objects[i].y) * (y - objects[i].y));
				if (D <= 20) {
					result[result.length] = objects[i];
				}
			}
			return result;
		},

		find: function (x, y) {
			for (var i in objects) {
				var D = Math.sqrt((x - objects[i].x) * (x - objects[i].x) + (y - objects[i].y) * (y - objects[i].y));
				if (D <= objects[i].width / 2) {
					return objects[i];
				}
			}
		},
	}
};