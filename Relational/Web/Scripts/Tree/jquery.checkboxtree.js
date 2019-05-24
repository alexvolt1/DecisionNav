$.widget(".checkboxTree", {

    _allDescendantChecked: function(li) {
        return (li.find('li div.checkbox:not(.checked)').length == 0);
    },
    _allDescendantUnChecked: function (li) {
        return (li.find('li div.checkbox.checked').length == 0);
    },

    _appendNodesChildren: function (li, ul, objdata) {


        var t = this;
        var name;
        var hid;
        var l;
        var lf;
        ischecked = li.find('div.checkbox.checked').length > 0 ? " checked" : "";
        radio = li.find('div.checkbox:first').hasClass('exclusive') ? " radio" : "";
        for (var i = 0; i < objdata.items.length; i++) {
            name = objdata.items[i].Name;
            fieldtype = objdata.items[i].fieldtype;
            hid = objdata.items[i].HierId;
            l = objdata.items[i].Level;
            lf = objdata.items[i].Leaf;
            exl = objdata.items[i].Exclusive == true ? " exclusive checked" : "";
            hasnext = objdata.items[i].HasNextStep == true ? " hasnextstep " : "";
            rmin = objdata.items[i].RMin != null ? " rmin = '" + objdata.items[i].RMin + "'" : "";
            rmax = objdata.items[i].RMax != null ? " rmax = '" + objdata.items[i].RMax + "'" : "";


            ul.append("<li class='" + (lf ? "leaf" : "collapsed") + "' hid='" + hid +
             "' " + rmin + rmax + " ' lvl='" + l + "' fieldtype='" + fieldtype +
             "'><span class='" + (lf ? "" : "ui-icon ui-icon-triangle-1-e") + "'></span><div class='checkbox" + ischecked + exl + radio + hasnext + "'></div><label>"
              + name + "</label>" + (lf ? "" : "<ul class='empty' style='display: none;'><li><label>Loading...</label></li></ul>") + "</li>");
        }
        if (radio) {
            t.check(ul.find('li:first'));
        }
        for (var i = 0; i < objdata.logentries.length; i++) {
            t._logevent(objdata.logentries[i]);
        }

        return ul;

    },





    //////////////////////////////////////////////////////////////////////////////////////
    prevLevel: function (level) {
        var t = this;
        
        var prevlevel = level - 1;
        if (this.element.find("li:[lvl=" + prevlevel + "]").length > 0) {
            t.hideLevel(level);
            t.showLevel(prevlevel);
            return prevlevel;
        }
        else {
            return level;
        };
    },

    nextLevel: function (level) {
    },

    showLevel: function (level) {

        
            this.element.find("li").each(function () {
                if ($(this).attr("lvl") == level) {
                    $(this).fadeIn();
                }
            });
      


    },

    hideLevel: function (level) {
           this.element.find("li").each(function () {
           if ($(this).attr("lvl") == level) {
                $(this).fadeOut();
           }
        });
    },

    //////////////////////////////////////////////////////////////////////////////////////////

  
    _create: function() {

        var t = this;

        // setup collapse engine tree
        if (this.options.collapsable) {

            // build collapse engine's anchors
            this.options.collapseAnchor = (this.options.collapseImage.length > 0) ? '<img src="' + this.options.collapseImage + '" />' : '';
            this.options.expandAnchor = (this.options.expandImage.length > 0) ? '<img src="' + this.options.expandImage + '" />' : '';
            this.options.leafAnchor = (this.options.leafImage.length > 0) ? '<img src="' + this.options.leafImage + '" />' : '';

            // initialize leafs
            this.element.find("li:not(:has(ul))").each(function() {
                $(this).prepend($('<span />'));
                t._markAsLeaf($(this));
            });

            // initialize checked nodes
            this.element.find("li:has(ul):has(div.checkbox.checked)").each(function () {
                $(this).prepend($('<span />'));
                t.options.initializeChecked == 'collapsed' ? t.collapse($(this)) : t.expand($(this));
            });

            // initialize unchecked nodes
            this.element.find("li:has(ul):not(:has(div.checkbox.checked))").each(function () {
                $(this).prepend($('<span />'));
                t.options.initializeUnchecked == 'collapsed' ? t.collapse($(this)) : t.expand($(this));
            });

            // bind collapse/expand event
            this.element.find('li span').live("click", function() {
                var li = $(this).parents("li:first");

                if (li.hasClass('collapsed')) {
                    t.expand(li);
                } else

                if (li.hasClass('expanded')) {
                    t.collapse(li);
                }
            });

            // bind collapse all element event
            $(this.options.collapseAllElement).bind("click", function() {
                t.collapseAll();
            });

            // bind expand all element event
            $(this.options.expandAllElement).bind("click", function() {
                t.expandAll();
            });

            // bind collapse on uncheck event
            if (this.options.onUncheck.node == 'collapse') {
                this.element.find('div.checkbox:not(.checked)').live("click", function () {
                    t.collapse($(this).parents("li:first"));
                });
            } else

            // bind expand on uncheck event
            if (this.options.onUncheck.node == 'expand') {
                this.element.find('.checkbox').live("click", function () {
                    t.expand($(this).parents("li:first"));
                });
            }

            // bind collapse on check event
            if (this.options.onCheck.node == 'collapse') {
                this.element.find('div.checkbox').live("click", function () {
                    t.collapse($(this).parents("li:first"));
                });
            } else

            // bind expand on check event
            if (this.options.onCheck.node == 'expand') {
                this.element.find('div.checkbox.checked').live("click", function () {
                    t.expand($(this).parents("li:first"));
                });
            }
        }

        // bind node uncheck event
        this.element.find('div.checkbox.checked:not(.radio):not(.exclusive)').live('click', function () {
            var li = $(this).parents('li:first'); 
            if (t.options.onCheckedChanged != null) {
                t._callback("unchecked",li, t.options.onCheckedChanged);
            }
               t.uncheck(li);
        });

        // bind node check event
        this.element.find('div.checkbox:not(.checked)').live('click', function() {
            var li = $(this).parents('li:first'); 
            if (t.options.onCheckedChanged != null) {
                t._callback("checked", li, t.options.onCheckedChanged);
            }
            t.check(li);
        });



        // add essential css class
        this.element.addClass('ui-widget-checkboxTree');

        // add jQueryUI css widget class
        this.element.addClass('ui-widget ui-widget-content');

        return this;

    },

    _callback: function (param, el, callbackFunc) {
      
            callbackFunc(param,el);
    },

    _checkAncestors: function(li) {
        li.parentsUntil(".ui-widget").filter('li').find('div.checkbox:first:not(.checked)').addClass('checked').change();
    },



    collectCheckedNodes: function (tree) {
       
        tree._clearconsole();
        var MembersSelected = [];
        $(tree.element).find('div.checkbox.checked').each(function () {
            var li = $(this).parent('li');
            var parent = tree._parentNode(li).find('label:first').text();
            var memText = li.find('label:first').text();
            var hid = li.attr('hid');
            var level = li.attr('lvl');
            var fieldtype = li.attr('fieldtype');
            var rmin = li.attr('rmin');
            var rmax = li.attr('rmax');
            var lf = li.hasClass('leaf');
           // var msg = parent + '  ' + hid + '  ' + level + '  ' + lf +  '   ' + memText;
          //  tree._logevent(msg);
            var Member = {};
            Member['parent'] = parent;
            Member['hid'] = hid;
            Member['level'] = level;
            Member['fieldtype'] = fieldtype;
            Member['leaf'] = lf;
            Member['value'] = memText;
            Member['rmin'] = rmin;
            Member['rmax'] = rmax;
            MembersSelected.push(Member);

        });

        return MembersSelected;

    },


    _checkDescendants: function (li) {
        if(li.find('div.checkbox:first').hasClass('exclusive')){
            li.find('li div.checkbox:first').addClass('checked').change();
        }
        else{
           li.find('li div.checkbox:not(.checked):not(.radio)').addClass('checked').change();
        }
       
    },

    _checkOthers: function(li) {
        li.addClass('exclude');
        li.parents('li').addClass('exclude');
        li.find('li').addClass('exclude');
        $(this.element).find('li').each(function() {
            if (!$(this).hasClass('exclude')) {
                $(this).find('div.checkbox:first:not(.checked)').addClass('checked').change();
            }
        });
        $(this.element).find('li').removeClass('exclude');
    },

    _destroy: function() {
        this.element.removeClass(this.options.cssClass);

        $.Widget.prototype.destroy.call(this);
    },

    _getRoot: function () {
         return $('.ui-widget-checkboxTree');
    },

    _isRoot: function(li) {
        var parents = li.parentsUntil('.ui-widget-checkboxTree');
        return 0 == parents.length;
    },

    _markAsCollapsed: function(li) {
        if (this.options.expandAnchor.length > 0) {
            li.children("span").html(this.options.expandAnchor);
        } else
        if (this.options.collapseUiIcon.length > 0) {
            li.children("span").removeClass(this.options.expandUiIcon).addClass('ui-icon ' + this.options.collapseUiIcon);
        }
        li.removeClass("expanded").addClass("collapsed");
    },

    _markAsExpanded: function(li) {
        if (this.options.collapseAnchor.length > 0) {
            li.children("span").html(this.options.collapseAnchor);
        } else
        if (this.options.expandUiIcon.length > 0) {
            li.children("span").removeClass(this.options.collapseUiIcon).addClass('ui-icon ' + this.options.expandUiIcon);
        }
        li.removeClass("collapsed").addClass("expanded");
    },

    _markAsLeaf: function(li) {
        if (this.options.leafAnchor.length > 0) {
            li.children("span").html(this.options.leafAnchor);
        } else
        if (this.options.leafUiIcon.length > 0) {
            li.children("span").addClass('ui-icon ' + this.options.leafUiIcon);
        }
        li.addClass("leaf");
    },

    _parentNode: function(li) {
        return li.parents('li:first');
    },

    _uncheckAncestors: function(li) {
        li.parentsUntil(".ui-widget").filter('li').find('div.checkbox:first.checked').removeClass('checked').change();
    },

    _uncheckDescendants: function(li) {
        li.find('li div.checkbox.checked').removeClass('checked').change();
    },

    _clearconsole: function () {

        var console = $("#console");
        console.html('');


    },

    _logevent:function(msg) {

        var console = $("#console");
        var entries = console.find("p");
        if (entries.length > 30) {
            entries.first().remove();
        }
        console.append("<p>" + msg + "</p>");


    },

    _uncheckOthers: function(li) {
        li.addClass('exclude');
        li.parents('li').addClass('exclude');
        li.find('li').addClass('exclude');
        $(this.element).find('li').each(function() {
            if (!$(this).hasClass('exclude')) {
                $(this).find('div.checkbox.checked').removeClass('checked').change();
            }
        });
        $(this.element).find('li').removeClass('exclude');
    },

    _uncheckSiblings:function(li){
        li.addClass('exclude');
        li.parents('li').find('li').each(function () {
            if (!$(this).hasClass('exclude')) {
                $(this).find('div.checkbox.checked:first').removeClass('checked').change();
            }
        });
        li.removeClass('exclude');
    },

    uncheck: function (li) {

      
       
        li.find('div.checkbox.checked:not(.radio):not(.exclusive)').removeClass('checked').change();

        // handle others
        if (this.options.onUncheck.others == 'check') {
            this._checkOthers(li);
        } else

            if (this.options.onUncheck.others == 'uncheck') {
            this._uncheckOthers(li);
        }

        // handle descendants
        if (this.options.onUncheck.descendants == 'check') {
            this._checkDescendants(li);
        } else

            if (this.options.onUncheck.descendants == 'uncheck') {
            this._uncheckDescendants(li);
        }

        // handle ancestors
        if (this.options.onUncheck.ancestors == 'check') {
            this._checkAncestors(li);
        } else

            if (this.options.onUncheck.ancestors == 'uncheck') {
            this._uncheckAncestors(li);
            }


           

                if (this.options.onUncheck.empty == 'uncheckParent') {
                    if (!this._isRoot(li) && this._allDescendantUnChecked(this._parentNode(li))) {
                this.uncheck(this._parentNode(li));
            }
        }
    },

    uncheckAll: function () {
        $(this.element).find('input:checkbox:checked').attr('checked', false).change();
    },

    check: function (li) {
        // handle siblings
        if (li.find('div.checkbox:first').hasClass('radio')) {
            this._uncheckSiblings(li);
          }

        li.find('div.checkbox:first').addClass('checked').change();
        

        // handle others
        if (this.options.onCheck.others == 'check') {
            this._checkOthers(li);
        } else

            if (this.options.onCheck.others == 'uncheck') {
                this._uncheckOthers(li);
            }

        // handle descendants
        if (this.options.onCheck.descendants == 'check') {
            this._checkDescendants(li);
        } else

            if (this.options.onCheck.descendants == 'uncheck') {
                this._uncheckDescendants(li);
            }

        // handle ancestors
        if (this.options.onCheck.ancestors == 'check') {
            this._checkAncestors(li);
        } else

            if (this.options.onCheck.ancestors == 'uncheck') {
                this._uncheckAncestors(li);
            }

        if (this.options.onCheck.full == 'checkParent') {
            if (!this._isRoot(li) && this._allDescendantChecked(this._parentNode(li))) {
                this.check(this._parentNode(li));
            }
        }



    },

    checkAll: function() {
        $(this.element).find('div.checkbox:first:not(.checked)').addClass('checked').change();
    },

    collapse: function(li) {
        if (li.hasClass('collapsed') || (li.hasClass('leaf'))) {
            return;
        }

        var t = this;

        li.children("ul").hide(this.options.collapseEffect, {}, this.options.collapseDuration);

        setTimeout(function() {
            t._markAsCollapsed(li, t.options);
        }, t.options.collapseDuration);

        t._trigger('collapse', li);
    },

    collapseAll: function() {
        var t = this;
        $(this.element).find('li.expanded').each(function() {
            t.collapse($(this));
        });
    },

    _onBeforeExpand: function (tree, li, func) {
        func(tree, li);
    },

    expand: function (li) {
       if (li.hasClass('expanded') || (li.hasClass('leaf'))) {
            return;
        }
        if (this.options.onBeforeExpand != null) {
            var func = this.options.onBeforeExpand;
            func(this, li, this._expand);
            }
        else {
            this._expand(this,li);
        }
    },


    _expand: function (t,li) {
        li.children("ul").show(t.options.expandEffect, {}, t.options.expandDuration);
        setTimeout(function () {
            t._markAsExpanded(li, t.options);
        }, t.options.expandDuration);
        t._trigger('expand', li);
    },

    expandAll: function() {
        var t = this;
        $(this.element).find('li.collapsed').each(function() {
            t.expand($(this));
        });
    },


   
    options: {
        /**
         * Defines relreport class.????
         */
        relreport: null,

        /**
        * Defines data url.
        */
        dataurl: null,

        /**
         * Defines if tree has collapse capability.
         */
        collapsable: true,
        /**
         * Defines an element of DOM that, if clicked, trigger collapseAll() method.
         * Value can be either a jQuery object or a selector string.
         * @deprecated will be removed in jquery 0.6.
         */
        collapseAllElement: '',
        /**
         * Defines duration of collapse effect in ms.
         * Works only if collapseEffect is not null.
         */
        collapseDuration: 300,
        /**
         * Defines the effect used for collapse node.
         */
        collapseEffect: 'blind',
        /**
         * Defines URL of image used for collapse anchor.
         * @deprecated will be removed in jquery 0.6.
         */
        collapseImage: '',
        /**
         * Defines jQueryUI icon class used for collapse anchor.
         */
        collapseUiIcon: 'ui-icon-triangle-1-e',

        /**
         * Defines an element of DOM that, if clicked, trigger expandAll() method.
         * Value can be either a jQuery object or a selector string.
         * @deprecated will be removed in jquery 0.6.
         */
        expandAllElement: '',
        /**
         * Defines duration of expand effect in ms.
         * Works only if expandEffect is not null.
         */
        expandDuration: 300,
        /**
         * Defines the effect used for expand node.
         */
        expandEffect: 'blind',

        expandImage: '',
        /**
         * Defines jQueryUI icon class used for expand anchor.
         */
        expandUiIcon: 'ui-icon-triangle-1-se',
        /**
         * Defines if checked node are collapsed or not at tree initializing.
         */
        initializeChecked: 'expanded', // or 'collapsed'
        /**
         * Defines if unchecked node are collapsed or not at tree initializing.
         */
        initializeUnchecked: 'collapsed', // or 'collapsed'
        /**
         * Defines URL of image used for leaf anchor.
         * @deprecated will be removed in jquery 0.6.
         */
        leafImage: '',
        /**
         * Defines jQueryUI icon class used for leaf anchor.
         */
        leafUiIcon: '',

        /**
         * Defines additional action to perform on node check/uncheck.
         * Available values: callback function.
        */
        onCheckedChanged: null,

        onBeforeExpand: null,


        /**
         * Defines which actions trigger when a node is checked.
         * Actions are triggered in the following order:
         * 1) node
         * 2) others
         * 3) descendants
         * 4) ancestors
         */


        onCheck: {
            /**
           * Defines action to perform on ancestors of the checked node.
           * Available values: null, 'checkParent', 'uncheckParent'.
            */
            full: 'checkParent',
            /**
             * Defines action to perform on ancestors of the checked node.
             * Available values: null, 'check', 'uncheck'.
             */
            ancestors: 'check',
            /**
             * Defines action to perform on descendants of the checked node.
             * Available values: null, 'check', 'uncheck'.
             */
            descendants: 'check',
            /**
             * Defines action to perform on checked node.
             * Available values: null, 'collapse', 'expand'.
             */
            node: '',
            /**
             * Defines action to perform on each other node (checked one excluded).
             * Available values: null, 'check', 'uncheck'.
             */
            others: ''
        },
        /**
         * Defines which actions trigger when a node is unchecked.
         * Actions are triggered in the following order:
         * 1) node
         * 2) others
         * 3) descendants
         * 4) ancestors
         */
        onUncheck: {
            /**
            * Defines action to perform on ancestors of the checked node.
            * Available values: null, 'checkParent', 'uncheckParent'.
            */
            empty: 'uncheckParent',
            /**
             * Defines action to perform on ancestors of the unchecked node.
             * Available values: null, 'check', 'uncheck'.
             */
            ancestors: '',
            /**
             * Defines action to perform on descendants of the unchecked node.
             * Available values: null, 'check', 'uncheck'.
             */
            descendants: '',
            /**
             * Defines action to perform on unchecked node.
             * Available values: null, 'collapse', 'expand'.
             */
            node: '',

            /**
             * Defines action to perform on each other node (unchecked one excluded).
             * Available values: null, 'check', 'uncheck'.
             */
            others: ''
        }
    }

});
