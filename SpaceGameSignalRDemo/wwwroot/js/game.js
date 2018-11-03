var Game = (function () {
	var background = new Image();
	var player;
	var ship;
	var spaceObjects = new SpaceObjects();
	var images = [];
	var paths = [
		'images/asteroids/asteroid1.png',
		'images/asteroids/asteroid2.png',
		'images/asteroids/asteroid3.png',
		'images/ships/ship1.png',
		'images/ships/ship2.png',
		'images/ships/ship3.png',
		'images/ships/ship4.png',
		'images/stations/portal.png',
	];

	var connection;

	return {
		init: function () {
			window.addEventListener('resize', this.resizeCanvas, false);

			connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
			connection.logging = true;

			connection.on("MoveMessage", function (moveData) {
				player.x = moveData.x;
				player.y = moveData.y;
				var ship = spaceObjects.getById(moveData.shipId);
				Game.moveShipTo(ship, moveData.targetX, moveData.targetY, moveData.speed);
				Game.redraw();
			});

			connection.on("TakeMessage", function (takeData) {
				spaceObjects.removeById(takeData.removedObjectId);
				var obj = spaceObjects.getById(takeData.takedBy.id);
				obj.type = takeData.takedBy.type;
				Game.redraw();
			});

			connection.on("ObjectAdded", function (newObjects) {
				spaceObjects.addMany(newObjects);	
			});

			connection.on("Jump", function () {
				Game.loadLayer(player.name);
			});

			connection.on("ObjectsUpdated", function (updatedObjects) {
				for (var i in updatedObjects) {
					var obj = spaceObjects.getById(updatedObjects[i].id);
					if (obj != undefined) {
						obj.x = updatedObjects[i].x;
						obj.y = updatedObjects[i].y;
						obj.speed = updatedObjects[i].speed;
					}						
				}
				Game.redraw();
			});

			connection.on("ObjectRemoved", function (spaceObject) {
				spaceObjects.removeById(spaceObject.id)
			});

			$('#mainCanvas').on('mousedown', function (e) {
				var rect = mainCanvas.getBoundingClientRect();
				var x = e.clientX - rect.left;
				var y = e.clientY - rect.top;

				var intersectedObject = spaceObjects.find(x, y);

				if (intersectedObject != undefined && (intersectedObject.type == 0 || intersectedObject.type == 1 || intersectedObject.type == 2)) {
					connection.invoke("TakeCommand", player.id, intersectedObject.id).catch(function (err) {
						return console.error(err.toString());
					});
				}
				else
				{
					connection.invoke("MoveCommand", player.id, x, y).catch(function (err) {
						return console.error(err.toString());
					});
				}
			});

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

			Game.loadLayer(name);
			connection.start()
				.then(function () {
					connection.invoke("NewPlayerCommand", player.id);
				})
				.catch(function (err) {
					return console.error(err.toString());
				});
		},

		loadLayer: function (name) {
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
					player.spaceLevel = data.player.spaceLevel;

					spaceObjects.erase();
					spaceObjects.addMany(data.spaceObjects);

					background.onload = function () {
						Game.resizeCanvas();
					}
					background.src = 'images/backgrounds/space' + player.spaceLevel + '.jpg';
				}
			});
		},

		moveShipTo: function (ship, x, y, speed) {
			if (ship != undefined) {
				ship.targetX = x;
				ship.targetY = y;
				ship.speed = speed;
			}
		},

		redraw: function () {
			var mainCanvas = document.getElementById("mainCanvas");
			var ctx = mainCanvas.getContext('2d');
			ctx.drawImage(background, 0, 0);
			var items = spaceObjects.getItems();
			for (var item in items) {
				ctx.drawImage(images[items[item].type],
					items[item].x - items[item].width / 2,
					items[item].y - items[item].height / 2);
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