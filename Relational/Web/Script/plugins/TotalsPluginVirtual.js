function TotalsPlugin(scrollbarWidth, totalsData) {
    this._scrollbarWidth = scrollbarWidth;
    this._totalsData = totalsData;
}

TotalsPlugin.prototype._totalsData = [];

TotalsPlugin.prototype._scrollOffset = 0;

TotalsPlugin.prototype._scrollbarWidth = 32;

TotalsPlugin.prototype._rowHeight = 10;

TotalsPlugin.prototype._$totalsViewport = null;

TotalsPlugin.prototype._$totalsViewportRight = null;

TotalsPlugin.prototype._$totalsRow = null;

TotalsPlugin.prototype._$totalsRowRight = null;

TotalsPlugin.prototype.init = function (grid) {

  this._grid = grid;
  this._rowHeight = grid.getOptions().rowHeight;

    var canvases = grid.getCanvases();
    var viewport = canvases[0].parentElement;
    var width = viewport.offsetWidth;

    var viewportRight = canvases[1].parentElement;
    var widthRight = viewportRight.offsetWidth;
 
 

  if (viewport.scrollHeight > viewport.offsetHeight) {
    width -= this._scrollbarWidth;
  }

  if (viewportRight.scrollHeight > viewportRight.offsetHeight) {
      widthRight -= this._scrollbarWidth;
  }

  this._$totalsViewport = $('<div class="slick-viewport totals-viewport">').css({ top: (this._rowHeight + this._scrollbarWidth) * -1, width: width });
  this._$totalsViewportRight = $('<div class="slick-viewport totals-viewport-right">').css({ top: (this._rowHeight + this._scrollbarWidth) * -1, width: widthRight - this._scrollbarWidth });

  this._$totalsViewport.insertAfter(viewport);
  this._$totalsViewportRight.insertAfter(viewportRight);
  this._appendTotalsRow(grid);

  var self = this;
  grid.onColumnsResized.subscribe(function() { self._handleColumnsResized.apply(self, arguments) });
  grid.onColumnsReordered.subscribe(function() { self._handleColumnsReordered.apply(self, arguments) });
  grid.onScroll.subscribe(function() { self._handleScroll.apply(self, arguments) });
};

TotalsPlugin.prototype.destroy = function () {
    this._$totalsViewportRight.remove();
    this._$totalsViewport.remove();
};


TotalsPlugin.prototype._appendTotalsRow = function (grid) {
    var canvases = grid.getCanvases();
    var viewport = canvases[0].parentElement;
    var width = viewport.offsetWidth;

    var viewportRight = canvases[1].parentElement;
    var widthRight = viewportRight.offsetWidth;
   

  var $totalsRow = $('<div class="ui-widget-content slick-row totals"></div>').css({ position: 'relative', width: width });
  var $totalsRowRight = $('<div class="ui-widget-content slick-row totals"></div>').css({ position: 'relative', width: widthRight });
  var totals = this._totalsData;
  var columns = grid.getColumns();
  var $cell;
  for (var i = 0, l = 2; i < l; i++) {
      $cell = $('<div class="slick-cell totals"></div>').addClass('l' + i + ' r' + i);
      if (columns[i].formatter == "Slick.Formatters.Dollar") {
          var v = eval(columns[i].formatter);
          var d = eval(columns[i].Total);
          if (v) {
              value = v(null, null, d || 0);
              $cell.text(value);
          }
      }
      else {
          $cell.text(totals[0][columns[i].id]);
      }
          $totalsRow.append($cell);
  }

  for (var i = 2, l = columns.length; i < l; i++) {
      $cell = $('<div class="slick-cell totals"></div>').addClass('l' + i + ' r' + i);
      if (columns[i].formatter == "Slick.Formatters.Dollar") {
          var v = eval(columns[i].formatter);
          var d = eval(columns[i].Total);
          if (v) {
              value = v(null, null, d || 0);
              $cell.text(value);
          }
      }
      else {
          $cell.text(totals[0][columns[i].id]);
      }
      $totalsRowRight.append($cell);
  }
  


  this._$totalsViewport.empty().append($totalsRow);
  this._$totalsRow = $totalsRow;
  this._$totalsViewportRight.empty().append($totalsRowRight);
  this._$totalsRowRight = $totalsRowRight;

};

TotalsPlugin.prototype._handleColumnsResized = function (event, update) {


    var canvases = update.grid.getCanvases();

    var viewport = canvases[0].parentElement;
    var width = viewport.offsetWidth;

    var viewportRight = canvases[1].parentElement;
    var widthRight = viewportRight.offsetWidth;

    var top = (viewport.scrollWidth > viewport.offsetWidth) ? this._rowHeight + this._scrollbarWidth : this._rowHeight;
    this._$totalsRow.width(canvases[0].scrollWidth);

    var top = (viewportRight.scrollWidth > viewportRight.offsetWidth) ? this._rowHeight + this._scrollbarWidth : this._rowHeight;
    this._$totalsRowRight.width(canvases[1].scrollWidth);
};

TotalsPlugin.prototype._handleColumnsReordered = function(event, update) {
    this._appendTotalsRow(update.grid);
};

TotalsPlugin.prototype._handleScroll = function(event, update) {
  if (this._scrollOffset != update.scrollLeft) {
    this._scrollOffset = update.scrollLeft;
    this._$totalsRowRight.css('left', this._scrollOffset * -1);
  }
};
