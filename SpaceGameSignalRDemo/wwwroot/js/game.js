var Game = (function () {
	var mainCanvas = document.getElementById("mainCanvas");
	var background = new Image();
	var player;
	var ship;
	var spaceObjects = [];
	var images = [];
	var paths = [
		'images/asteroids/asteroid1.png',
		'images/asteroids/asteroid2.png',
		'images/asteroids/asteroid3.png',
		'images/ships/ship1.png',
		'images/ships/ship2.png',
	];

	return {
		init: function () {
			window.addEventListener('resize', this.resizeCanvas, false);

			$('#mainCanvas').on('mousedown', function (e) {
				var rect = mainCanvas.getBoundingClientRect();
				var x = e.clientX - rect.left;
				var y = e.clientY - rect.top;
				Game.moveShipTo(x, y);
			});

			setInterval(function () {
				for (var i in spaceObjects) {
					if ((spaceObjects[i].targetX == 0 && spaceObjects[i].targetY == 0)
						|| (spaceObjects[i].targetX == spaceObjects[i].x && spaceObjects[i].targetY == spaceObjects[i].y))
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
					}
				}
				Game.redraw();
			}, 50);

			for (var i = 0; i < paths.length; i++) {
				images[i] = new Image();
				images[i].src = paths[i];
			}

			background.src = 'images/backgrounds/space1.jpg';
			background.onload = function () {
				Game.resizeCanvas();
			}			

			var name = prompt("Enter your name...");
			player = new Player(name);

			$.ajax({
				type: 'POST',
				accepts: 'application/json',
				url: 'api/game',
				data: JSON.stringify(name),
				contentType: 'application/json',
				success: function (data) {
					player.id = data.player.id;
					player.expirience = data.player.experience;
					player.shipId = data.player.shipId;
					player.level = data.player.level;

					for (var item in data.spaceObjects) {
						spaceObjects[spaceObjects.length] = data.spaceObjects[item];
					}

					ship = Utils.getShipById(spaceObjects, player.shipId);

					background.onload = function () {
						Game.resizeCanvas();
					}			
					background.src = 'images/backgrounds/space' + player.level + '.jpg';
				}
			});

		},

		moveShipTo: function (x, y) {
			if (ship != undefined) {
				ship.targetX = x;
				ship.targetY = y;
			}
		},

		redraw: function () {
			var ctx = mainCanvas.getContext('2d');
			ctx.drawImage(background,
				0, 0, background.naturalHeight, background.naturalHeight,
				0, 0, mainCanvas.width, mainCanvas.width * background.naturalHeight / background.naturalHeight);
			for (var item in spaceObjects) {
				ctx.drawImage(images[spaceObjects[item].type],
					spaceObjects[item].x - spaceObjects[item].width / 2,
					spaceObjects[item].y - spaceObjects[item].height / 2);
			}
		},

		resizeCanvas: function () {
			mainCanvas.width = window.innerWidth;
			mainCanvas.height = window.innerHeight;
			Game.redraw();
		},
	}
})();
Game.init();