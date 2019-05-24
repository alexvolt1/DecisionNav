(function ($) {

    var $el;
    // Default settings.
    var defaults = {
        RelReport: null,                 // The Report
        el: '',                 // The container element or selector.
        effect: 'none',             // The switch effect, 'none' and 'slide' are supported.
        duration: 350,                // The effect duration.
        initiallyHidden: false,                // Initial State.
        startAt: 0,                  // The index (zero-based) of the initial step.
        showCancel: true,               // Whether to show the "Cancel" button.
        showPrev: true,               // Whether to show the "Previous" button, which is hidden on the first step.
        showNext: true,               // Whether to show the "Next" button, which is hidden on the last step.
        showFinish: false,              // Whether to show the "Finish" button on every step.
        buttonEffect: 'disable',          // The switch buttonEffect, 'hide' and 'disable' are supported.
        activeCls: 'active',           // The current active tab.
        activatedCls: 'activated',        // The steps in front of the current step.
        onprev: $.noop,               // After switching to the previous step, this callback will be called.
        onafterstepshow: $.noop,               // On Step
        onnext: $.noop,               // After switching to the next step, this callback will be called.
        oncancel: function () { this.Hide() },               // When trigger the "Cancel" button, this callback will be called.
        onfinish: $.noop,               // When trigger the "Finsh" button, this callback will be called.
        onbeforeprev: $.noop,               // This callback will be called before switching to the previous step.
        onbeforenext: $.noop,                // This callback will be called before switching to the next step.
        onneedmembers: $.noop,
        onneedhierarchies: $.noop,
        onsetcurrenthierarchie: $.noop,
        onlogevent: $.noop,
        onkeywordsearch: $.noop
    };

    function emptyObject(obj) {
        Object.keys(obj).forEach(function (k) { delete obj[k] })
    }

    // The `Step` constructor.
    function MemberSelect(opts) {
        // Merge the settings.
        opts = this.opts = $.extend({}, defaults, opts);
        if (!opts.el) {
            throw 'Need the selector';
        }
        this.$el = $(opts.el);
        this.$el.addClass('memsel-wrapper');
        if (opts.initiallyHidden) {
            this.Hide();
        }

        this.$header = $([
            "<ul class='memsel-header cf'>",
            " <li class='memsel-header-item active' data-body='#tab1'>Browse Hierarchy</li>",
            " <li class='memsel-header-item' data-body='#tab2'>Keyword Search</li>",
            "</ul>",
        ].join("\n")).appendTo(this.$el);

        this.$hierarchies = $([
            "<div class='memsel-hierarchy cf'>",
            "</div>",
        ].join("\n")).appendTo(this.$el);


        this.$members = $([
            "<ul class='memsel-members'>",
            "<li class='memsel-member-item active' data-body='#member0'>Loading Members...</li>",
            "</ul>",
        ].join("\n"));
        this.$members.appendTo(this.$el);


        this.$body = $([
            "<div class='memsel-body'>",
            "<p>Loading...</p>",
            " </div>",
        ].join("\n")).appendTo(this.$el);


        this.$footer = $([
            "<div class='memsel-footer'>",
            "<div class='alternate-report'>Subtotal By Relationship Number</div>",
            "<a href='javascript:void(0)' class='memsel-btn memsel-reset'>Reset</a>",
            "<a href='javascript:void(0)' class='memsel-btn memsel-prev'>Back</a>",
            "<a href='javascript:void(0)' class='memsel-btn memsel-next'>Next</a>",
            "<a href='javascript:void(0)' class='memsel-btn memsel-finish'>Show Report</a>",
            "<a href='javascript:void(0)' class='memsel-btn memsel-cancel'>Cancel</a>",
            "</div>"
        ].join("\n")).appendTo(this.$el);


        this.btns = {
            reset: this.$el.find('.memsel-footer .memsel-reset'),
            cancel: this.$el.find('.memsel-footer .memsel-cancel'),
            prev: this.$el.find('.memsel-footer .memsel-prev'),
            next: this.$el.find('.memsel-footer .memsel-next'),
            finish: this.$el.find('.memsel-footer .memsel-finish')
        };

        this.alternatereport = this.$el.find('.memsel-footer .alternate-report');
        this.members = {};
        //this.searchq = [{name:"ObligoR NamE", id:0}, {name:"RA",id:1}];
        this.searchq = [];
        this.selectionchanged = false;

        // Indicate and control if the previous and next buttons are enabled.
        this.prevEnabled = this.nextEnabled = true;
        this.index = opts.startAt;
        this.initHierarchies();
        //this.initMembers();
        return this;
    }

    // All the prototype properties and methods.
    $.extend(MemberSelect.prototype, {
        init: function () {
            this.initButtons();
            this.bindEvents();
        },

        initHierarchies: function () {
            this.$hierarchies.hide();
            this.opts.onneedhierarchies.call(0, this);
        },

        initMembers: function () {
            this.opts.onneedmembers.call(0, this);
        },
        renderhierarchies: function (h) {
            var t = this;
            try {
                if (h.length > 1) {
                    function getHierarchies(item, index) {
                        $availlHierarchiesOpt = " <option value='" + h[index].ID + "'>" + h[index].Name + "</option>";
                        if (h[index].isCurrent) {
                            $(".memsel-hierarchy-select select").prepend($availlHierarchiesOpt);
                        }
                        else {
                            $(".memsel-hierarchy-select select").append($availlHierarchiesOpt);
                        }
                    }
                    $hierarchiescontant = $([
                        " <span class='label-hierarchy-item'>Select Hierarchy</span>",
                        " <div class='memsel-hierarchy-select'>",
                        " <select>",
                        //  " <option value='0'>Use All Hierarchies</option>",
                        " </select>",
                        "</div>",
                    ].join("\n")).appendTo(this.$hierarchies).ready(function () {
                        h.forEach(getHierarchies),
                            t.buildCustDropdown(),
                            t.setWrapperHeight(),
                            t.$hierarchies.show()
                        t.initMembers();
                    });
                }
                else {
                    t.initMembers();
                }
            }
            catch (e) {
                this.logevent(e);
            }
        },
        setWrapperHeight: function () {
            var $wrapperContent = $('.memsel-wrapper');
            var $hierarchyContent = $('.memsel-hierarchy.cf');
            var height = $wrapperContent.height() + $hierarchyContent.height();
            $wrapperContent.height(height);
        },

        setKeywordSearchHeight: function () {
            var $keywordsearch = $('.memsel-wrapper #keywordsearch');
            var $hierarchyContent = $('.memsel-hierarchy.cf');
            var height = $keywordsearch.height() + $hierarchyContent.height() - 4;
            $keywordsearch.height(height);
        },

        renderalternatereport: function (data) {
            if (data != null && data == true) {
                this.alternatereport.show()
                    .on("click", function () {
                        $(this).toggleClass("checked");
                    });
            }
            else {
                this.alternatereport.hide();
            }
        },
        rendermembers: function (data, level) {
            var t = this;
            t.selectionchanged = true;
            if (level == null) {

                this.$members.empty();
                emptyObject(t.members);
                this.$body.empty();
                $(data).each(function (index) {
                    $("<li class='memsel-member-item' data-body='#member" + index + "'>" + this.Name + "</li>")
                        .appendTo(t.$members);
                    var v = $("<div id='member" + index + "' class='memsel-body-item '></div>")
                        .appendTo(t.$body);
                    var m = "<p> Loading " + index + "...</p>";
                    if (index == 0) {
                        m = $("<ul class='member-data-items'></ul>");
                        $("<li class='member-data-item checked default'>All</li>")
                            .appendTo(m);
                        $(this.values).each(function () {
                            $("<li class='member-data-item'>" + this.Text + "</li>")
                                .appendTo(m);
                        });
                        t.members[0] = { initializated: true, id: "member" + this.index };
                    }//index 0
                    else {
                        t.members[index] = { initializated: false, id: "member" + this.index };
                    }
                    v.append(m);

                });

                // this.totalCount = t.$el.find('.memsel-member-item').index;
                this.init();
            }//level ==null or 0

            else {
                var v = this.$body.find("#member" + level);
                $(data).each(function (index) {
                    m = $("<ul class='member-data-items'></ul>");
                    $("<li class='member-data-item checked default'>All</li>")
                        .appendTo(m);
                    $(this.values).each(function () {
                        $("<li class='member-data-item'>" + this.Text + "</li>")
                            .appendTo(m);
                    });
                    v.empty();
                    v.append(m);
                });

                t.members[level] = { initializated: true, id: "member" + this.index };
                //   this.invalidateBelowSelections(this.index);
                this.logevent("Member Refreshed");
            }
        },
        initButtons: function () {
            if (!this.opts.showCancel) {
                this.btns.cancel.remove();
            }
            if (!this.opts.showPrev) {
                this.btns.prev.remove();
            }
            this.gotoStep(this.index);
        },
        Show: function () {
            this.$el.removeClass('hide');
        },

        Hide: function () {
            this.$el.addClass('hide');
        },





        buildCustDropdown: function () {
            var t = this;
            var x, i, j, selElmnt, a, b, c;
            /*look for any elements with the class "memsel-hierarchy-select":*/
            x = document.getElementsByClassName("memsel-hierarchy-select");
            for (i = 0; i < x.length; i++) {

                selElmnt = x[i].getElementsByTagName("select")[0];
                /*for each element, create a new DIV that will act as the selected item:*/
                a = document.createElement("DIV");
                a.setAttribute("class", "select-selected");
                a.innerHTML = selElmnt.options[selElmnt.selectedIndex].innerHTML;
                x[i].appendChild(a);
                /*for each element, create a new DIV that will contain the option list:*/
                b = document.createElement("DIV");
                b.setAttribute("class", "select-items select-hide");
                for (j = 0; j < selElmnt.length; j++) {
                    /*for each option in the original select element,
                    create a new DIV that will act as an option item:*/
                    c = document.createElement("DIV");
                    c.innerHTML = selElmnt.options[j].innerHTML;
                    c.addEventListener("click", function (e) {
                        /*when an item is clicked, update the original select box,
                        and the selected item:*/
                        var y, i, k, s, h;
                        s = this.parentNode.parentNode.getElementsByTagName("select")[0];
                        h = this.parentNode.previousSibling;
                        for (i = 0; i < s.length; i++) {
                            if (s.options[i].innerHTML == this.innerHTML) {
                                s.selectedIndex = i;
                                h.innerHTML = this.innerHTML;
                                t.setCurrentHierarchy(s.value);
                                // t.logevent("-->" + h.innerHTML + "  [" + s.value + "]");
                                y = this.parentNode.getElementsByClassName("same-as-selected");
                                for (k = 0; k < y.length; k++) {
                                    y[k].removeAttribute("class");
                                }
                                this.setAttribute("class", "same-as-selected");
                                break;
                            }
                        }
                        h.click();
                    });
                    b.appendChild(c);
                }
                x[i].appendChild(b);
                a.addEventListener("click", function (e) {
                    /*when the select box is clicked, close any other select boxes,
                    and open/close the current select box:*/
                    e.stopPropagation();
                    closeAllSelect(this);
                    this.nextSibling.classList.toggle("select-hide");
                    this.classList.toggle("select-arrow-active");
                });
            }

            function closeAllSelect(elmnt) {
                /*a function that will close all select boxes in the document,
                except the current select box:*/
                var x, y, i, arrNo = [];
                x = document.getElementsByClassName("select-items");
                y = document.getElementsByClassName("select-selected");
                for (i = 0; i < y.length; i++) {
                    if (elmnt == y[i]) {
                        arrNo.push(i)
                    } else {
                        y[i].classList.remove("select-arrow-active");
                    }
                }
                for (i = 0; i < x.length; i++) {
                    if (arrNo.indexOf(i)) {
                        x[i].classList.add("select-hide");
                    }
                }
            }
            /*if the user clicks anywhere outside the select box,
            then close all select boxes:*/
            document.addEventListener("click", closeAllSelect);

        },





        setCurrentHierarchy: function (id) {
            this.opts.onsetcurrenthierarchie.call(0, this, id);
            //        this.logevent("--> [" + id + "]");

        },


        // Bind events to the buttons.
        bindEvents: function () {
            var self = this;
            self.$el.unbind()
                .delegate('.memsel-header-item', 'click', function () {
                    self.resetkeysearch();
                    self.navigatetotab(this);
                })
                .delegate('.memsel-footer .memsel-reset', 'click', function () {
                    self.reset();
                })
                .delegate('.memsel-footer .memsel-cancel', 'click', function () {
                    self.cancel();
                })
                .delegate('.memsel-footer .memsel-finish', 'click', function () {
                    self.finish();
                })
                .delegate('.memsel-footer .memsel-prev', 'click', function () {
                    self.opts.onbeforeprev.call(self, self.index);
                    self.prevEnabled && self.prev();
                })
                .delegate('.memsel-footer .memsel-next', 'click', function () {
                    self.opts.onbeforenext.call(self, self.index);
                    self.nextEnabled && self.next();
                })
                .delegate('.memsel-body .member-data-item', 'click', function () {
                    self.togglememberitem(this);
                })
                .delegate('.search-data-item', 'click', function () {
                    self.togglesearchitem(this);
                })
                .delegate('.memsel-search', 'click', function () {
                    self.searchbykeywords($("#searchstring").attr("value"), $("#searchq").attr("value"));
                })
                .delegate('.memsel-resetsearch', 'click', function () {
                    self.resetkeysearch();
                })
                .delegate('.memsel-wrapper  .memsel-member-item', 'click', function () {
                    var activeindex = $(this).parent().find(".active").index();
                    var index = $(this).index();

                    self.logevent("-- Member " + index + " clicked --");

                    if (!$(this).hasClass("active")) {
                        self.logevent("-- Member index : " + index);
                        self.sequence = activeindex < index ? "next" : "prev";
                        self.gotoStep(index, self.opts.onafterstepshow);
                    }

                });

        },
        navigatetotab: function (el) {
            var t = $(el);
            var self = this;
            t.siblings().removeClass("active");
            t.addClass("active");
            var btns = self.btns;
            var $search = self.$el.find("#keywordsearch");
            if (t.data("body") == "#tab2") {
                if ($search.length == 0) {
                    self.$search = $([
                        "<div id='keywordsearch'>",
                        "<table>",
                        "<tr><td><span>Search By:</span></td><td colspan='2'></td></tr>",
                        "<tr><td><span id='searchqc'></span></td>",
                        "<td colspan='2'><input type='text' id='searchstring'></td></tr>",
                        "<tr><td></td><td><a href='javascript:void(0)' class='memsel-search'>Search</a></td><td><a href='javascript:void(0)' class='memsel-resetsearch'>Reset</a></td></tr>",
                        "</table>",
                        "<ul class='search-data-items'>",
                        "</ul>",
                        "</div>"
                    ].join("\n")).appendTo(self.$el).ready(function () {
                        self.setKeywordSearchHeight();
                    });

                    var $searchqc = self.$el.find("#searchqc");
                    if (self.searchq != null) {
                        var sq = $("<select id='searchq'></select>");
                        $.each(self.searchq, function () {
                            sq.append($("<option></option>").attr("value", this.ID).html(this.ColumnName));
                        });
                        $searchqc.append(sq);
                    }
                    else {
                        $searchqc.text("Not Set");
                        $('.memsel-search').hide();
                    }
                }
                $search.show();
                btns.prev.hide();
                btns.next.hide();
                btns.reset.hide();
            }
            else {
                if ($search != null) {
                    $search.hide();
                    btns.prev.show();
                    btns.next.show();
                    btns.reset.show();
                }
            }
        },

        resetkeysearch: function () {
            $(".search-data-items").empty();
        },

        searchbykeywords: function (value, qid) {
            if (value != null && value != "") {
                $(".search-data-items").empty();
                this.opts.onkeywordsearch.call(this, this, value, qid);
            }
        },

        rendersearchresults: function (data) {
            var $results = $(".search-data-items");
            if (data.length > 0 && data[0] == "") {
                $("<li class='search-empty-item'>No results found</li>")
                    .appendTo($results);
            }
            else {
                $("<li class='search-data-item checked default'>All</li>")
                    .appendTo($results);
                for (i = 0; i < data.length; i++) {
                    $("<li class='search-data-item'>" + data[i] + "</li>")
                        .appendTo($results);
                }
            }
        },

        togglesearchitem: function (el) {
            var t = $(el);
            if (!t.hasClass("default")) {
                t.toggleClass("checked");
                var v = t.parent().find(".checked").not(".default").length;

                this.logevent("Checked Search Item:" + v);

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
        },


        togglememberitem: function (el) {
            var t = $(el);
            if (!t.hasClass("default")) {
                t.toggleClass("checked");
                var v = t.parent().find(".checked").not(".default").length;
                this.logevent("Checked Member Item:" + v + " items");
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

            var index = $(el).closest('div').index(".memsel-body-item");
            this.logevent("Selection changed, member " + index + " validated");
            this.members[index].initializated = true;
            this.logevent("Member actual validation status is: " + this.members[index].initializated);
            this.invalidateBelowSelections(index);
        },
        invalidateBelowSelections: function (index) {
            this.selectionchanged = true;
            this.logevent("Invalidating Selections Below");
            this.logevent("member index is: " + index);
            for (var key in this.members) {
                if (this.members.hasOwnProperty(key) && this.members[key].initializated == true && key > index) {
                    this.logevent("invalidate member : " + key + " -> " + this.members[key].id);
                    this.members[key].initializated = false;
                    $(".memsel-body").find("#" + this.members[key].id).html("<p> Loading " + key + "...</p>");
                }
            }
        },
        emptymembers: function () {
            var v = this.members;
            Object.keys(v).forEach(function (k) { delete v[k] });
        },
        reset: function () {
            this.$body.find(".checked").not(".default").removeClass('checked');
            this.gotoStep(0, this.opts.onafterstepshow);
            this.$body.find(".default").addClass("checked");
            for (var key in this.members) {
                if (this.members.hasOwnProperty(key) && this.members[key].initializated == true) {
                    this.members[key].initializated = false;
                    $(".memsel-body").find("#" + this.members[key].id).html("<p> Loading " + key + "...</p>");
                }
            }
        },
        isalternateselected: function () {
            return this.alternatereport.hasClass("checked");
        },
        collectsearchstrings: function () {
            this.logevent("Selections Collected");
            var SearchstringsSelected = [];
            var t = this.$el;
            var keysearch = $(".search-data-items");
            if (keysearch.length > 0) {
                var Searchstring = {};
                Searchstring['name'] = $("#searchq").attr("value");
                var values = [];
                if (keysearch.find(".default").hasClass("checked")) {
                    Searchstring['level'] = "ALL";
                    values.push($("#searchstring").attr("value"));
                    Searchstring['values'] = values;
                    SearchstringsSelected.push(Searchstring);
                }
                else {
                    Searchstring['level'] = "Selected";
                    keysearch.find(".checked")
                        .each(function () {
                            values.push($(this).text());
                        });
                    Searchstring['values'] = values;
                    SearchstringsSelected.push(Searchstring);
                }
            }
            return SearchstringsSelected;
        },

        collectSelections: function (level) {

            var self = this;

            this.logevent("Selections being Collected");
            this.logevent("Level : " + level);
            var MembersSelected = [];
            var t = this.$el;
            t.find(".memsel-body-item")
                .each(function (index) {
                    var skip = false;
                    if (level != null && level < index) {
                        skip = true;
                        self.logevent("Don't collect this: " + index);
                    }


                    self.logevent("collecting member: " + index);

                    if ((self.members.hasOwnProperty(index)
                        && self.members[index].initializated == true) && skip == false) {
                        self.logevent("member " + index + " validated, collecting selections ");
                        var m = $(this);
                        var v = m.find(".checked").not(".default");
                        if (v.length > 0) {
                            var Member = {};
                            Member['name'] = m.attr("id");
                            var values = [];
                            v.each(function () {
                                values.push($(this).text());
                            });
                            Member['values'] = values;
                            MembersSelected.push(Member);
                        }



                    }
                    else {
                        if (skip == true) {
                            self.logevent("member " + index + " validated, but we need above only, skipping selections ");
                        }
                        else {
                            self.logevent("member " + index + " invalidated, skipping selections ");
                        }

                    }
                });
            return MembersSelected;
        },


        // Skip to the index (zero-based) step.
        gotoStep: function (index, callback) {
            this.logevent("---- gotostep " + index + " ---- ");
            this.index = index;
            //var $activeHeader = this.$el.find('.memsel-header-item').eq(index),
            var $activeHeader = this.$el.find('.memsel-member-item').eq(index),
                $activeBody = $($activeHeader.data('body'));
            var effect = this.sequence ? this.opts.effect : 'none';


            if (this.sequence && !this.isSetDimension) {
                var $stepBody = this.$el.find('.memsel-body');
                this.stepBodyWidth = $stepBody.width();
            }
            this.effectExecutors[effect].call(this, $activeHeader, $activeBody, callback);
            var btns = this.btns;
            if (!this.opts.buttonEffect == 'disable') {
                if (index <= 0) {
                    btns.prev.hide();
                } else {
                    btns.prev.show();
                }
                if (index >= this.totalCount - 1) {
                    btns.next.hide();
                    btns.finish.show();
                } else {
                    btns.next.show();
                    btns.finish.hide();
                }
                if (this.opts.showFinish) {
                    // btns.finish.show();
                }
            }
        },

        // Skip to the previous step.
        prev: function () {
            if (this.index > 0) {
                this.index--;
                this.sequence = 'prev';
                this.gotoStep(this.index, this.opts.onafterstepshow);

            }
        },

        // Skip to the next step.
        next: function () {
            if (this.index < Object.keys(this.members).length - 1) {
                this.index++;
                this.sequence = 'next';
                this.gotoStep(this.index, this.opts.onafterstepshow);
            }
        },

        // Cancel the step.
        cancel: function () {
            this.opts.oncancel.call(this, this.index);
        },

        // Finish the step.
        finish: function () {
            this.opts.onfinish.call(this, this);
            this.selectionchanged = false;
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

            $activeHeader.siblings('.memsel-member-item').removeClass(activeCls + ' ' + activatedCls)
                .end().addClass(activeCls).removeClass(activatedCls)
                .prevAll('.memsel-member-item').addClass(activatedCls);

            $activeBody.siblings('.memsel-body-item').removeClass(activeCls + ' ' + activatedCls)
                .end().addClass(activeCls).removeClass(activatedCls)
                .prevAll('.memsel-body-item').addClass(activatedCls);

        },
        // The switch effect executor.
        effectExecutors: {

            // None effect.
            none: function ($activeHeader, $activeBody, callback) {
                this.activeClass($activeHeader, $activeBody);

                this.members[this.index].id = this.index;
                this.logevent("MemberID: " + this.members[this.index].id);
                this.logevent("Sequence: " + this.sequence);
                this.logevent("member " + this.index + " initialized: " + this.members[this.index].initializated);


                if (this.index == 0) {
                    this.logevent("member 0: Refresh canceled");
                }
                //else if (this.sequence == "prev" && this.members[this.index].initializated==true) {
                else if (this.members[this.index].initializated == true) {
                    this.logevent("member " + this.index + " already initialized : Refresh canceled");
                }
                else {
                    this.logevent("Refreshing member: " + this.index);

                    if (!this.members[this.index].initializated) {
                        //this.invalidateBelowSelections(this.index);
                        $(".memsel-body").find("#member" + this.index).html("<p> Loading " + this.index + "...</p>");
                        //alert("MEMBER WILL BE REFRESHED: " + this.index);
                        callback && callback.call(this, this);
                    }
                    //alert(this.members[this.index].initializated);
                }
            },

            // Slide horizontally.
            slide: function ($activeHeader, $activeBody, callback) {
                var self = this,
                    sequenceMap = {
                        prev: {
                            left: -self.stepBodyWidth,
                            $el: $activeBody.next('.memsel-body-item')
                        },
                        next: {
                            left: self.stepBodyWidth,
                            $el: $activeBody.prev('.memsel-body-item')
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


    var noConflict = $.fn.memberselect;

    // Support the calling on the jQuery object.
    $.fn.memberselect = function (opts) {
        opts = $.extend({ el: this }, opts);
        return new MemberSelect(opts);
    };

    $.fn.memberselect.noConflict = noConflict;

})(jQuery);
