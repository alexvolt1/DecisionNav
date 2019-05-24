/*!
 * simple-step
 * https://github.com/Alex1990/simple-step
*/
; (function ($) {
    var $el;
    // Default settings.
    var defaults = {
        el: '',                 // The container element or selector.
        effect: 'slide',             // The switch effect, 'none' and 'slide' are supported.
        duration: 350,                // The effect duration.
        initiallyHidden: true,                // Initial State.
        startAt: 0,                  // The index (zero-based) of the initial step.
        showCancel: true,               // Whether to show the "Cancel" button.
        showPrev: true,               // Whether to show the "Previous" button, which is hidden on the first step.
        showNext: true,               // Whether to show the "Next" button, which is hidden on the last step.
        showFinish: false,              // Whether to show the "Finish" button on every step.
        buttonEffect: 'disable',          // The switch buttonEffect, 'hide' and 'disable' are supported.
        activeCls: 'active',           // The current step.
        activatedCls: 'activated',        // The steps in front of the current step.
        onprev: $.noop,               // After switching to the previous step, this callback will be called.
        onafterstepshow: $.noop,               // On Step
        onnext: $.noop,               // After switching to the next step, this callback will be called.
        oncancel: function () { this.Hide() },               // When trigger the "Cancel" button, this callback will be called.
        onfinish: $.noop,               // When trigger the "Finsh" button, this callback will be called.
        onbeforeprev: $.noop,               // This callback will be called before switching to the previous step.
        onbeforenext: $.noop,                // This callback will be called before switching to the next step.
        onneedfilters: $.noop,
        onlogevent: $.noop,
        onbeforeshow: $.noop
    };

    // The `FilterDialog` constructor.
    function FilterDialog(opts) {
        // Merge the settings.
        opts = this.opts = $.extend({}, defaults, opts);
        if (!opts.el) {
            throw 'Need the selector';
        }
        this.$el = $(opts.el);
        this.$el.addClass('filterselect-wrapper');
        if (opts.initiallyHidden) {
            this.Hide();
        }

        this.$header = $([
            "<ul class='filterselect-header cf'>",
            "<li class='filterselect-header-item'>Loading...</li>",
            "</ul>"
        ].join("\n")).appendTo(this.$el);

        this.$body = $([
            "<div class='filterselect-body'>",
            "</div>"
        ].join("\n")).appendTo(this.$el);

        this.$footer = $([
            "<div class='filterselect-footer'>",
            "<a href='javascript:void(0)' class='filterselect-btn filterselect-prev'>Previous</a>",
            "<a href='javascript:void(0)' class='filterselect-btn filterselect-next'>Next</a>",
            "<a href='javascript:void(0)' class='filterselect-btn filterselect-finish'>Show Report</a>",
            "<a href='javascript:void(0)' class='filterselect-btn filterselect-cancel'>Cancel</a>",
            "</div>"
        ].join("\n")).appendTo(this.$el);

        this.btns = {
            cancel: this.$el.find('.filterselect-footer .filterselect-cancel'),
            prev: this.$el.find('.filterselect-footer .filterselect-prev'),
            next: this.$el.find('.filterselect-footer .filterselect-next'),
            finish: this.$el.find('.filterselect-footer .filterselect-finish')
        };
        this.filters = {};

        // Indicate and control if the previous and next buttons are enabled.
        this.prevEnabled = this.nextEnabled = true;
        this.index = opts.startAt;
        //this.totalCount = this.$el.find('.filterselect-header-item').length;

        this.initFilters();
        return this;
    }

    // All the prototype properties and methods.
    $.extend(FilterDialog.prototype, {

        init: function () {
            this.initButtons();
            this.bindEvents();
        },
        initFilters: function () {
            this.resetValues();
            this.opts.onneedfilters.call(-1, this);
            //this.opts.onneedfilters.call(0, this);
        },
        resetValues: function () {
            var $filtervalues = $(".filter-data-items").empty();
            $filtervalues.each(function () {
                $("<span> Loading...</span>").appendTo(this);
            });

        },

        renderfilters: function (data, step) {
            var t = this;
            if (step == null) {
                var steps = data.length;

                if (steps < 2) {
                    t.opts.showNext = false;
                    t.opts.showPrev = false;
                }
                else {
                    this.btns.prev.text("Report Filter 1");
                    this.btns.next.text("Report Filter 2");
                }


                this.$header.empty();
                for (i = 0; i < steps; i++) {
                    this.$header.append("<li class='filterselect-header-item' data-body='#step" + i + "'>Report Filters " + (i + 1) + "</li>");
                }
                this.totalCount = this.$el.find('.filterselect-header-item').length;
                for (i = 0; i < steps; i++) {//   steps
                    var activestep = i == 0 ? "active" : "";
                    var $step = $("<div id='step" + i + "' class='filterselect-body-item " + activestep + "'></div>");
                    var $filters = $("<ul class='filterselect-filters'></ul>");
                    var filters = data[i];
                    for (f = 0; f < filters.length; f++) { //filters
                        var filter = data[i][f];
                        var activefilter = f == 0 ? "active" : "";
                        $filters.append($("<li class='filterselect-filter-item " + activefilter + "' data-body='#filter" + i + f + "'>" + filter.Name + "</li>"));
                        var single = filter.AllowAll == true ? "" : " single ";
                        var hide = f > 0 ? " hide" : "";
                        var $filtervalues = $("<ul uid='" + filter.FilterUID + "' id='filter" + i + f + "' class='filter-data-items" + single + hide + "'></ul>");
                        $("<span> Loading...</span>").appendTo($filtervalues);
                        $step.append($filtervalues);
                    }
                    $step.prepend($filters);
                    $step.appendTo(this.$body);
                }
                this.init();
                this.logevent("Finished Initializing Filters");
            }
            else {
                for (f = 0; f < data.length; f++) { //filters
                    var step = data[f];
                    for (i = 0; i < step.length; i++) {//   steps
                        var filter = step[i];
                        var values = filter.ValueItems;
                        var $filtervalues = t.$el.find("#filter" + f + i);
                        if (values != null) {
                            var single = filter.AllowAll == true ? "" : " single ";
                            $filtervalues.empty();
                            if (filter.AllowAll) {
                                $("<li class='filter-data-item checked default'>All " + filter.Name + "</li>").appendTo($filtervalues);
                            }
                            for (v = 0; v < values.length; v++) { //values
                                var checked = (single == " single " && v == 0) || values[v].Selected ? " checked" : "";
                                $("<li class='filter-data-item" + checked + "'>" + values[v].Name + "</li>").appendTo($filtervalues);
                            }
                        }
                    }
                }
                this.logevent("Finished Refreshing values");
            }
        },
        renderfilters_old: function (data, step) {
            //    var t = this;
            //  if (step == null) {
            //      var steps = data.length;

            //      if (steps < 2) {
            //          t.opts.showNext = false;
            //          t.opts.showPrev = false;
            //      }
            //      else {
            //          this.btns.prev.text("Report Filter 1");
            //          this.btns.next.text("Report Filter 2");
            //      }


            //        this.$header.empty();
            //        for (i = 0; i < steps; i++) {
            //            this.$header.append("<li class='filterselect-header-item' data-body='#step" + i + "'>Report Filters " + (i + 1) + "</li>");
            //        }
            //        this.totalCount = this.$el.find('.filterselect-header-item').length;
            //        for (i = 0; i < steps; i++) {//   steps
            //            var activestep = i == 0 ? "active" : "";
            //            var $step = $("<div id='step" + i + "' class='filterselect-body-item " + activestep + "'></div>");
            //            var $filters = $("<ul class='filterselect-filters'></ul>");
            //            var filters = data[i];
            //            for (f = 0; f < filters.length; f++) { //filters
            //                var filter = data[i][f];
            //                var activefilter = f == 0 ? "active" : "";
            //                $filters.append($("<li class='filterselect-filter-item " + activefilter + "' data-body='#filter" + i + f + "'>" + filter.Name + "</li>"));
            //                var single = filter.AllowAll == true ? "" : " single ";
            //                var hide = f > 0 ? " hide" : "";
            //                var $filtervalues = $("<ul id='filter" + i + f + "' class='filter-data-items" + single + hide + "'></ul>");
            //                                    $("<span> Loading...</span>").appendTo($filtervalues);
            //                $step.append($filtervalues);
            //            }
            //            $step.prepend($filters);
            //            $step.appendTo(this.$body);
            //        }
            //        this.init();
            //        this.logevent("Finished Initializing Filters");
            //    }
            //    else {
            //            for (f = 0; f < data.length; f++) { //filters
            //            var step = data[f];
            //            for (i = 0; i < step.length; i++) {//   steps
            //                var filter = step[i];
            //                var values = filter.ValueItems;
            //                var $filtervalues = t.$el.find("#filter" + f + i);
            //                if (values != null) {
            //                    var single = filter.AllowAll == true ? "" : " single ";
            //                    $filtervalues.empty();
            //                    if (filter.AllowAll) {
            //                        $("<li class='filter-data-item checked default'>All " + filter.Name + "</li>").appendTo($filtervalues);
            //                    }
            //                    for (v = 0; v < values.length; v++) { //values
            //                        var checked = (single == " single " && v == 0) || values[v].Selected ? " checked" : "";
            //                        $("<li class='filter-data-item" + checked + "'>" + values[v].Name + "</li>").appendTo($filtervalues);
            //                    }
            //                }
            //            }
            //        }
            //        this.logevent("Finished Refreshing values");
            //    }
        },

        // Initialize the buttons.
        initButtons: function () {
            if (!this.opts.showCancel) {
                this.btns.cancel.remove();
            }
            if (!this.opts.showPrev) {
                this.btns.prev.remove();
            }
            if (!this.opts.showNext) {
                this.btns.next.remove();
            }
            this.gotoStep(this.index);
        },
        Show: function (s) {
            this.logevent("Show argument " + s);
            this.logevent("Show Filters " + this.index);
            this.logevent("Start Refreshing Step " + this.index + " values");
            if (s != null && s == true) {
                this.resetValues();
                this.opts.onafterstepshow.call(this, this);
            }


            this.$el.removeClass('hide');
        },
        Hide: function () {
            this.$el.addClass('hide');
        },

        // Bind events to the buttons.
        bindEvents: function () {
            var self = this;

            self.$el
                .delegate('.filterselect-footer .filterselect-cancel', 'click', function () {
                    self.cancel();
                })
                .delegate('.filterselect-footer .filterselect-finish', 'click', function () {
                    if (self.index >= self.totalCount - 1) {
                        self.finish();
                    }
                })
                .delegate('.filterselect-footer .filterselect-prev', 'click', function () {
                    self.opts.onbeforeprev.call(self, self.index);
                    self.prevEnabled && self.prev();
                })
                .delegate('.filterselect-footer .filterselect-next', 'click', function () {
                    self.opts.onbeforenext.call(self, self.index);
                    self.nextEnabled && self.next();
                })
                .delegate('.filterselect-body .filterselect-filter-item', 'click', function () {
                    self.togglefilterbody(this);
                })
                .delegate('.filterselect-body .filter-data-item', 'click', function () {
                    self.togglefilteritem(this);
                });
        },
        // Show Filter Body
        togglefilterbody: function (el) {

            var filter = $(el);
            var fid = filter.data('body');
            var step = filter.closest('.filterselect-body-item');
            // Hide all filter items for the step
            step.find('.filter-data-items').addClass('hide');
            // Show Selected One
            step.find('.filter-data-items' + fid).removeClass('hide');
            // Toggle Caller
            if (!filter.hasClass('active')) {
                step.find('.' + filter[0].className).removeClass('active');
                filter.addClass('active');
            }

        },
        // Toggle Filter Item
        togglefilteritem: function (el) {
            t = $(el);

            if (t.parent().hasClass("single")) {
                t.siblings().removeClass("checked");
                t.toggleClass("checked");
            }
            else {

                if (!t.hasClass("default")) {
                    t.toggleClass("checked");
                    var v = t.parent().find(".checked").not(".default").length;
                    if (v > 0) {
                        t.parent().find(".default").removeClass("checked");
                    }
                    else {
                        t.parent().find(".default").addClass("checked");
                    }
                }
                else {
                    if (!t.hasClass("checked")) {
                        t.parent().find(".checked").not(".default").removeClass("checked");
                        t.addClass("checked");
                    }
                }
            }





        },
        collectSelections: function () {
            this.logevent(" Filter Selections Collected");
            var FiltersSelected = [];
            var t = this.$el;
            t.find(".filter-data-items").not(".default")
                .each(function () {
                    var f = $(this);
                    var v = f.find(".checked");
                    if (v.length > 0) {
                        var Filter = {};
                        Filter['name'] = f.attr("id");
                        Filter['uid'] = f.attr("uid");
                        var values = [];
                        v.each(function () {
                            values.push($(this).text());
                        });
                        Filter['values'] = values;
                        FiltersSelected.push(Filter);
                    }
                });
            //debugger;
            return FiltersSelected;
        },

        // Skip to the index (zero-based) step.
        gotoStep: function (index, callback) {
            this.index = index;
            var $activeHeader = this.$el.find('.filterselect-header-item').eq(index),
                $activeBody = $($activeHeader.data('body'));
            var effect = this.sequence ? this.opts.effect : 'none';
            // When click the "previous" or "next" button, set the style of all the step bodies.
            // The reason why set the style in here is the width of the step bodies isn't your 
            // expected if the step is hidden at first.
            if (this.sequence && !this.isSetDimension) {
                var $stepBody = this.$el.find('.filterselect-body');

                this.stepBodyWidth = $stepBody.width();

                $stepBody.css({
                    position: 'relative',
                    width: this.stepBodyWidth,
                    height: $stepBody.height(),
                    overflow: 'hidden'
                });
                $stepBody.find('.filterselect-body-item').css({
                    position: 'absolute',
                    top: 0,
                    left: 0,
                    width: '100%'
                });
            }
            this.logevent("Step activated :" + this.index);
            // if (this.index>0) {
            //    this.opts.onafterstepshow.call(this, this);
            //}
            // 

            this.effectExecutors[effect].call(this, $activeHeader, $activeBody, callback);
            var btns = this.btns;
            // Determine if the buttons is displayed or not
            //  if (!this.opts.buttonEffect == 'disable') {
            if (index <= 0) {
                // btns.prev.hide();
            } else {
                // btns.prev.show();
            }

            if (index >= this.totalCount - 1) {
                //btns.next.hide();
                //btns.finish.show();
                btns.finish.css("color", "#555");
            } else {
                //btns.next.show();
                //btns.finish.hide();
                btns.finish.css("color", "#eee");
            }

            if (this.opts.showFinish) {
                btns.finish.show();
            }
            //  }
        },

        // Skip to the previous step.
        prev: function () {
            if (this.index > 0) {
                this.index--;
                this.sequence = 'prev';
                this.gotoStep(this.index, this.opts.onprev);
            }
        },

        // Skip to the next step.
        next: function () {
            if (this.index < this.totalCount - 1) {
                this.index++;
                this.sequence = 'next';
                this.gotoStep(this.index, this.opts.onnext);
            }
        },

        // Cancel the step.
        cancel: function () {
            this.opts.oncancel.call(this, this.index);
        },

        // Finish the step.
        finish: function () {
            this.opts.onfinish.call(this, this.index);
            this.Hide();
        },
        logevent: function (msg) {
            this.opts.onlogevent.call(this, msg);
        },

        // Switch the `class` of the steps.
        // The value of the variable `activeCls` will be added to the current step.
        // And, the value of the variable `activatedCls` will be added to all the previous steps.
        activeClass: function ($activeHeader, $activeBody) {
            var activeCls = this.opts.activeCls,
                activatedCls = this.opts.activatedCls;

            $activeHeader.siblings('.filterselect-header-item').removeClass(activeCls + ' ' + activatedCls)
                .end().addClass(activeCls).removeClass(activatedCls)
                .prevAll('.filterselect-header-item').addClass(activatedCls);

            $activeBody.siblings('.filterselect-body-item').removeClass(activeCls + ' ' + activatedCls)
                .end().addClass(activeCls).removeClass(activatedCls)
                .prevAll('.filterselect-body-item').addClass(activatedCls);
        },

        // The switch effect executor.
        effectExecutors: {

            // None effect.
            none: function ($activeHeader, $activeBody, callback) {
                this.activeClass($activeHeader, $activeBody);
                callback && callback.call(this, this.index);
            },

            // Slide horizontally.
            slide: function ($activeHeader, $activeBody, callback) {
                var self = this,
                    sequenceMap = {
                        prev: {
                            left: -self.stepBodyWidth,
                            $el: $activeBody.next('.filterselect-body-item')
                        },
                        next: {
                            left: self.stepBodyWidth,
                            $el: $activeBody.prev('.filterselect-body-item')
                        }
                    };

                $activeBody.css({
                    left: sequenceMap[self.sequence].left
                })
                    .show()
                    .animate({
                        left: 0
                    }, self.opts.duration, function () {
                        sequenceMap[self.sequence].$el.hide();
                        self.activeClass($activeHeader, $activeBody);
                        callback && callback.call(self, self.index);
                    });

                sequenceMap[self.sequence].$el
                    .animate({
                        left: -sequenceMap[self.sequence].left
                    }, self.opts.duration);
            }
        }
    });

    var noConflict = $.fn.filterdialog;

    // Support the calling on the jQuery object.
    $.fn.filterdialog = function (opts) {
        opts = $.extend({ el: this }, opts);
        return new FilterDialog(opts);
    };

    $.fn.filterdialog.noConflict = noConflict;

})(jQuery);
