var Utils = (function () {
	return {
		getShipById: function(array, id) {
			for (var i in array) {
				if (array[i].id == id)
					return array[i];
			}
		},

		removeItem: function (array, id) {
			for (var i in array) {
				if (array[i].id == id) {
					array.splice(i, 1);
					break;
				}
			}
		},		

		recalculatePositions: function (spaceObjects) {
			for (var i in spaceObjects) {
				if (spaceObjects[i].speed == 0)
					continue;

				var D = Math.sqrt((spaceObjects[i].targetX - spaceObjects[i].x) * (spaceObjects[i].targetX - spaceObjects[i].x) + (spaceObjects[i].targetY - spaceObjects[i].y) * (spaceObjects[i].targetY - spaceObjects[i].y));
				var koef = spaceObjects[i].speed / D;
				if (koef < 1) {
					spaceObjects[i].x += (spaceObjects[i].targetX - spaceObjects[i].x) * koef;
					spaceObjects[i].y += (spaceObjects[i].targetY - spaceObjects[i].y) * koef;
				}
				else {
					spaceObjects[i].x = spaceObjects[i].targetX;
					spaceObjects[i].y = spaceObjects[i].targetY;
					spaceObjects[i].speed = 0;
				}
			}
		},

		getSpaceObjectById: function (id, spaceObjects) {
			for (var i in spaceObjects) {
				if (spaceObjects[i].id == id)
					return spaceObjects[i];
			}
		},

		intersect: function (x, y, spaceObjects) {
			var result = [];
			for (var i in spaceObjects) {
				var D = Math.sqrt((x - spaceObjects[i].x) * (x - spaceObjects[i].x) + (y - spaceObjects[i].y) * (y - spaceObjects[i].y));
				if (D <= 20) {
					result[result.length] = spaceObjects[i];
				}
			} 
			return result;
		},
	}
})();