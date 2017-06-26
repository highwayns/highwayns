/*
	jquery.flatheights.js
	Version: 2010-09-15
*/

/*
======================================================================
	$.changeLetterSize.addHandler(func)
	文字の大きさが変化した時に実行する処理を追加
======================================================================
*/

jQuery.changeLetterSize = {
	handlers : [],
	interval : 1000,
	currentSize: 0
};

(function($) {

	var self = $.changeLetterSize;

	/* 文字の大きさを確認するためのins要素 */
	var ins = $('<ins>M</ins>').css({
		display: 'block',
		visibility: 'hidden',
		position: 'absolute',
		padding: '0',
		top: '0'
	});

	/* 文字の大きさが変わったか */
	var isChanged = function() {
		ins.appendTo('body');
		var size = ins[0].offsetHeight;
		ins.remove();
		if (self.currentSize == size) return false;
		self.currentSize = size;
		return true;
	};

	/* 文書を読み込んだ時点で
	   文字の大きさを確認しておく */
	$(isChanged);

	/* 文字の大きさが変わっていたら、
	   handlers中の関数を順に実行 */
	var observer = function() {
		if (!isChanged()) return;
		$.each(self.handlers, function(i, handler) {
			handler();
		});
	};

	/* ハンドラを登録し、
	   最初の登録であれば、定期処理を開始 */
	self.addHandler = function(func) {
		self.handlers.push(func);
		if (self.handlers.length == 1) {
			setInterval(observer, self.interval);
		}
	};

})(jQuery);

/*
======================================================================
	$(expr).flatHeights()
	$(expr)で選択した複数の要素について、それぞれ高さを
	一番高いものに揃える
======================================================================
*/

(function($) {

	/* 対象となる要素群の集合 */
	var sets = [];

	/* 高さ揃えの処理本体 */
	var flatHeights = function(set) {
		var maxHeight = 0;
		set.each(function(){
			var height = this.offsetHeight;
			if (height > maxHeight) maxHeight = height;
		});
		set.css('height', maxHeight + 'px');
	};

	/* 要素群の高さを揃え、setsに追加 */
	jQuery.fn.flatHeights = function() {
		if (this.length > 1) {
			flatHeights(this);
			sets.push(this);
		}
		return this;
	};

	/* 高さ揃えを再実行する処理 */
	var reflatting = function() {
		$.each(sets, function() {
			this.height('auto');
			flatHeights(this);
		});
	};

	/* 文字の大きさが変わった時に高さ揃えを再実行 */
	$.changeLetterSize.addHandler(reflatting);

	/* ウィンドウの大きさが変わった時に高さ揃えを再実行 */
	$(window).resize(reflatting);

})(jQuery);


/*
Copyright (c) 2007, KITAMURA Akatsuki

Permission is hereby granted, free of charge, to any person obtaining a
copy of this software and associated documentation files (the "Software"),
to deal in the Software without restriction, including without limitation
the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
*/
