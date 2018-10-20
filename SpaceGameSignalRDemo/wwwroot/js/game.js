var Game = (function () {
	var mainCanvas = document.getElementById("mainCanvas");
	var background = new Image();
	var player;

	return {
		init: function () {
			window.addEventListener('resize', this.resizeCanvas, false);
			background.src = 'images/backgrounds/space1.jpg';			
			background.onload = function () {
				Game.resizeCanvas();
			}
			var name = prompt("Enter your name...");
			player = new Player(name);
		},

		redraw: function () {
			var ctx = mainCanvas.getContext('2d');
			ctx.drawImage(background,
				0, 0, background.naturalHeight, background.naturalHeight,
				0, 0, mainCanvas.width,	mainCanvas.width * background.naturalHeight / background.naturalHeight);
		},

		resizeCanvas: function () {
			mainCanvas.width = window.innerWidth;
			mainCanvas.height = window.innerHeight;
			Game.redraw();
		},
	}
})();
Game.init();