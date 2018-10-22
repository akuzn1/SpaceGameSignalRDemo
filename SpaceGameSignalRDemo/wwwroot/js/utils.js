var Utils = (function () {
	return {
		getShipById: function(array, id) {
			for (var i in array) {
				if (array[i].id == id)
					return array[i];
			}
		}
	}
})();