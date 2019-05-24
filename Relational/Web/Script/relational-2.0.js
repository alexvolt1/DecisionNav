function RelReport() {

    var t = this;
    var ReportId;
    var membertree;
    var filterstree;
    var dataurl = window.location.pathname;
    var next;
    var prev;
    var currentstep = 1;
    var containerdiv = "#gridcontainer";

    var servicerows = 1;

    var dataView;
    var grid;
    var pager;
    var totaldata = [{ "Id": "Total" }];
    var loadingIndicator = null;
    var memberselect = null;
    var columns = null;
    var SelectedMembers=null;
    var options = {
        editable: false,
        rowHeight: 20,
        enableAddRow: false,
        enableCellNavigation: false,
        forceFitColumns: false,
        frozenColumn: 1
        
    };
    var skip = 100;
    var take = 100;
    var searchstr = "";
    var s = 0;
  

    // Report Data

    this.getdata = function (init) {
        
            
            var start;
            var l;
            if (init) {
                start = 1;
                l = 0;
            }
            else {
                var vp = grid.getViewport();
                start = vp.bottom - servicerows;
                l = dataView.getLength();
                 }
            if (start >= l && s == 0) {
                s = 1;
                loadingIndicator.show();
                var req = $.ajax({
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    url: dataurl + "/GetData",
                    //data: "{'from':'" + start + "','to':'" + take + "','searchstring':'" + searchstr + "'}",
                    data: "{'id':'" + ReportId + "','from':'" + start + "','to':'" + take + "','searchstring':'" + searchstr + "'}",
                    error: t.onError,
                    success: function (resp) {
                        if (resp.d != null) {
                           
                            dataView.beginUpdate();
                            var objdata = [];
                            objdata = $.parseJSON(resp.d.data);
                            for (var i = 0; i < objdata.length; i++) {
                                dataView.addItem(objdata[i]);
                            }
                            if (init) { }
                            else { skip = vp.bottom; }
                            dataView.setTotalRecords(resp.d.rowscount);
                            dataView.endUpdate();
                            }
                        s = 0;
                       
                        loadingIndicator.fadeOut();
                    }
                });
            }
        
    }

    this.sortdata = function(col, dir) {
        // debugger;
        var start;
        var l;

        start = 0;
        l = 0;
        if (start >= l && s == 0) {
            s = 1;
            loadingIndicator.show();
            var req = $.ajax({
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: dataurl + "/Sort",
                data: "{'id':'" + ReportId + "','col':'" + col + "','dir':'" + dir + "'}",
                error: t.onError,
                success: function (resp) {
                    if (resp.d != null) {
                        dataView.beginUpdate();
                        var objdata = [];
                        objdata = $.parseJSON(resp.d.data);
                        for (var i = 0; i < objdata.length; i++) {
                            dataView.addItem(objdata[i]);
                        }
                        dataView.endUpdate();
                    }
                    s = 0;
                    loadingIndicator.fadeOut();
                }
            });
        }
    }

    this.loadreport = function() {
            dataView = new Slick.Data.DataView({ inlineFilters: true });
            var dataProvider = new TotalsDataView(dataView, columns);
            grid = new Slick.Grid(containerdiv, dataProvider, columns, options);
            var pager = new Slick.Controls.Pager(dataView, grid, $("#pager"));
            var s = $.getScrollbarWidth();
            var totalsPlugin = new TotalsPlugin(s, totaldata);
            grid.registerPlugin(totalsPlugin);
            grid.onViewportChanged.subscribe(function (e, args) {
                t.getdata();
            });
            grid.onSort.subscribe(function (e, args) {
                sortdir = args.sortAsc ? 1 : -1;
                sortcol = args.sortCol.field;

                //  alert(sortdir + " " + sortcol);
                dataView.beginUpdate();
                dataView.getItems().length = 0;
                t.sortdata(sortcol, sortdir);
                dataView.setFilterArgs(0);
                dataView.endUpdate();
                dataView.sort(t.comparer, args.sortAsc);
            });
            // wire up model events to drive the grid
            dataView.onRowCountChanged.subscribe(function (e, args) {
                grid.updateRowCount();
                grid.render();
            });


            // wire up model events to drive the grid
            dataView.onRowCountChanged.subscribe(function (e, args) {
                grid.updateRowCount();
                grid.render();
            });

            dataView.onRowsChanged.subscribe(function (e, args) {
                grid.invalidateRows(args.rows);
                grid.render();
                // totalsPlugin.render();
            });

            // initialize the model after all the events have been hooked up
            dataView.beginUpdate();
            t.getdata(true);
            dataView.setFilterArgs(0);
            dataView.endUpdate();
    }

    this.loadreportmetadata = function (reportid) {
        var mems = membertree.collectCheckedNodes(membertree);
        var prevFilters = filterstree.collectCheckedNodes(filterstree);
        if (mems) {
        var DTO = { 'Report': { 'ReportId': reportid, 'SelectedMembers': mems, 'SelectedFilters': prevFilters } };
        var data = JSON.stringify(DTO);
         loadingIndicator.show();
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: dataurl + "/GetMetaData",
            data: data,
            error: t.onError,
            success: function (resp) {
                if (resp.d != null) {
                    var objdata = [];
                    objdata = $.parseJSON(resp.d);
                    //debugger;
                    columns = objdata;
                    t.loadreport();
                }
                else {
                    alert("Unable to get report metadata");
                }
              loadingIndicator.fadeOut();
            }
        });
      }
    }

    /////




    // Member Select Old

    this.unloadmemberselect = function () {

        if (!memberselect) {
            memberselect = $(".item-details-form");
        }
        memberselect.fadeOut();
    }

    this.loadmemberselect = function () {

        if (!memberselect) {
            memberselect = $(".item-details-form");
        }
        var $g = $(containerdiv);

        memberselect
            .css("position", "absolute")
            .css("top", $g.position().top + $g.height() / 2 - memberselect.height() / 2)
            .css("left", $g.position().left + $g.width() / 2 - memberselect.width() / 2)
            .css("display", "inline-block");
        memberselect.show();

    }



    //External call from tree

    this.refreshfilters = function (param, li) {
        t.logevent("refresh filters");
        //!!!! TEST
       
        var ul = $(filterstree.element).find('li:first').children("ul");
        ul.html("").addClass('empty');
        filterstree.collapseAll();
       
    }

    this.checkedChanged = function (param, li) {
        
        if (param == "checked" && (li.find('div.checkbox:first').hasClass("hasnextstep") || li.parents('li:first').find('div.checkbox:first').hasClass("hasnextstep"))) {
                next.show();
            }
        else if (param == "unchecked" && li.parents('li:first').find('div.checkbox:first').hasClass("hasnextstep") && li.parents('li:first').find('div.checkbox.checked').length==2) {
                  
                  next.hide();
        }
        else if (param == "unchecked" && li.find('div.checkbox:first').hasClass("hasnextstep")) {
                 next.hide();
        }
    }

    this.onFiltersNodeBeforeExpand = function (tree, li, callback) {
        t.logevent("Relational:onFiltersNodeBeforeExpand");
        var ul = li.children("ul");
        if (ul.hasClass('empty')) {
            if (tree.options.dataurl == null) {
                return;
            }
            var memSel = tree.collectCheckedNodes(membertree);
            var prevFilters = tree.collectCheckedNodes(filterstree);
            var ps = [];
            ps.push('[' + li.find('label:first').text() + ']');
            li.parentsUntil(".ui-widget").filter('li').each(function () {
                ps.push('[' + $(this).find('label:first').text() + ']');
            });
            var ln = li.children('label').text();
            var level = li.attr('lvl');
            if (level == null) {
                level = -1;
            }
            var hid = li.attr('hid');
            if (hid == null) {
                hid = -1;
            }

            var data2 = { 'FilterDTO': { 'id': ReportId, 'ps': ps, 'pn': ln, 'hid': hid, 'level': level, 'memsel': memSel, 'prevFilters': prevFilters } };
            var data = JSON.stringify(data2);
          
            $.ajax({
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: tree.options.dataurl,
                data: data,
                error: function (msg) {

                    // debugger;
                    t.logevent(msg.responseText)
                   // alert(msg.responseText);
                   ;
                },
                success: function (resp) {
                    if (resp.d != null) {
                        var objdata = [];
                        objdata = $.parseJSON(resp.d);
                        ul.html("").removeClass('empty');
                        ul = tree._appendNodesChildren(li, ul, objdata);
                        callback(tree, li);
                    }
                }
            });
       }
        else {
            callback(tree, li);
       }
        
    }

    this.onMembersNodeBeforeExpand = function (tree, li, callback) {
        
        t.logevent("Relational:onMembersNodeBeforeExpand");
        var ul = li.children("ul");
        if (ul.hasClass('empty')) {
            if (tree.options.dataurl == null) {
                return;
            }
            var ps = [];
            ps.push('[' + li.find('label:first').text() + ']');
            li.parentsUntil(".ui-widget").filter('li').each(function () {
                ps.push('[' + $(this).find('label:first').text() + ']');
            });
            var ln = li.children('label').text();
            var level = li.attr('lvl');
            if (level == null) {
                level = -1;
            }
            var hid = li.attr('hid');
            if (hid == null) {
                hid = -1;
            }
            //var data = "{'ps':'" + ps + "','pn':'" + ln + "','hid':'" + hid + "','level':'" + level + "'}";
            var data = "{'id': '" + ReportId + "','ps':'" + ps + "','pn':'" + ln + "','hid':'" + hid + "','level':'" + level + "'}";
            $.ajax({
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: tree.options.dataurl,
                data: data,
                error: function (msg) {
                    // debugger;
                    t.logevent(msg.responseText)
                    // alert(msg.responseText);
                },
                success: function (resp) {
                    if (resp.d != null) {
                        var objdata = [];
                        objdata = $.parseJSON(resp.d);
                        ul.html("").removeClass('empty');
                        ul = tree._appendNodesChildren(li, ul, objdata);
                        callback(tree, li);
                    }
                }
            });
        }
        else {
            callback(tree, li);
        }
        
    }

    this.onexpand = function (li) {
       
    }

    this.comparer =  function(a, b) {
        var x = a[sortcol], y = b[sortcol];
        return (x == y ? 0 : (x > y ? 1 : -1));
    }

    this.onError = function(msg) {
        debugger;
        alert("error");
    }

    this.logevent = function(msg) {

        var console = $("#console");
        var entries = console.find("p");
        if (entries.length > 20) {
            entries.first().remove();
        }
        console.append("<p>" + msg + "</p>");


    }

    this.init = function (reportid) {
        ReportId = reportid;
        $("#btnmselect").live("click", function () { t.loadmemberselect() });
        if (!loadingIndicator) {
            loadingIndicator = $("<span class='loading-indicator'><label>Buffering...</label></span>").appendTo(document.body);
            var $g = $(containerdiv);
            loadingIndicator
                .css("position", "absolute")
                .css("top", $g.position().top + $g.height() / 2 - loadingIndicator.height() / 2)
                .css("left", $g.position().left + $g.width() / 2 - loadingIndicator.width() / 2);
            loadingIndicator.fadeOut();
        }

        $("#tree1").checkboxTree({
            relreport: t
                        , dataurl: dataurl + "/GetMembers"
                        , onCheckedChanged: t.refreshfilters
                        , onBeforeExpand: t.onMembersNodeBeforeExpand
        });
        membertree = $("#tree1").data("checkboxTree");


        /////////   FILTERS //////////////////////////////////////////////////////////////////////////////////////////////

        var f = $("#tree2");
        f.checkboxTree({
            relreport: t
                      , dataurl: dataurl + "/GetFilters"
                      , onCheckedChanged: t.checkedChanged
                      , onBeforeExpand: t.onFiltersNodeBeforeExpand
        });
        filterstree = f.data("checkboxTree");
        next = $("#nextfiltergroup");
        next.live("click", function () {
            t.logevent("Navigate to next filter group");

            var level = currentstep;
            var nextlevel = level + 1;
            if (filterstree.element.find("li:[lvl=" + nextlevel + "]").length > 0) {
                filterstree.hideLevel(level);
                filterstree.showLevel(nextlevel);
                return nextlevel;
            }
            else {
                t.logevent("step " + nextlevel + " is loading");

                var li = filterstree.element.find("li:first");
                var ul = li.children("ul");

                var memSel = filterstree.collectCheckedNodes(membertree);
                var ps = [];
                ps.push('[' + li.find('label:first').text() + ']');
                li.parentsUntil(".ui-widget").filter('li').each(function () {
                    ps.push('[' + $(this).find('label:first').text() + ']');
                });
                var ln = "";
                var hid = li.attr('hid');
                if (hid == null) {
                    hid = 0;
                }
                filterstree.hideLevel(level);
                var data2 = { 'FilterDTO': { 'id': ReportId, 'ps': ps, 'pn': ln, 'hid': hid, 'level': nextlevel, 'memsel': memSel } };
                var data = JSON.stringify(data2);
                $.ajax({
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    url: filterstree.options.dataurl,
                    data: data,
                    error: function (msg) {
                        //debugger;
                        //alert("error");
                        t.logevent(msg.responseText);
                    },
                    success: function (resp) {
                        if (resp.d != null) {
                            var objdata = [];
                            objdata = $.parseJSON(resp.d);
                            ul = filterstree._appendNodesChildren(li, ul, objdata);
                            currentstep = nextlevel;
                        }
                    }
                });

            };






            //currentstep = filterstree.nextLevel(currentstep);
            prev.show();
        }).hide();

        prev = $("#prevfiltergroup");
        prev.live("click", function () {
            t.logevent("Navigate to previous filter group");
            // debugger;
            currentstep = filterstree.prevLevel(currentstep);
            prev.hide();

        }).hide();
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////   

        $("#showreport").live("click", function () {
            //      var mems = membertree.collectCheckedNodes(membertree);
            //    if (mems) {
            t.loadreportmetadata(ReportId);
            t.unloadmemberselect();
            //  }

        });
        $("#cancel").live("click", function () {
            t.unloadmemberselect();
        });
        this.loadmemberselect();
    }

    this.oldinit = function () {
        //this.init = function (reportid) {
        //    ReportId = reportid;
        //    $("#btnmselect").live("click", function () { t.loadmemberselect() });
        //    if (!loadingIndicator) {
        //        loadingIndicator = $("<span class='loading-indicator'><label>Buffering...</label></span>").appendTo(document.body);
        //        var $g = $(containerdiv);
        //        loadingIndicator
        //            .css("position", "absolute")
        //            .css("top", $g.position().top + $g.height() / 2 - loadingIndicator.height() / 2)
        //            .css("left", $g.position().left + $g.width() / 2 - loadingIndicator.width() / 2);
        //        loadingIndicator.fadeOut();
        //    }

        //    $("#tree1").checkboxTree({
        //                      relreport: t
        //                    , dataurl: dataurl + "/GetMembers"
        //                    , onCheckedChanged: t.refreshfilters
        //                    , onBeforeExpand: t.onMembersNodeBeforeExpand
        //});
        //    membertree = $("#tree1").data("checkboxTree");


        //    /////////   FILTERS //////////////////////////////////////////////////////////////////////////////////////////////

        //    var f = $("#tree2");
        //    f.checkboxTree({relreport: t
        //                  , dataurl: dataurl + "/GetFilters"
        //                  , onCheckedChanged: t.checkedChanged
        //                  , onBeforeExpand: t.onFiltersNodeBeforeExpand
        //                 });
        //    filterstree = f.data("checkboxTree");
        //        next = $("#nextfiltergroup");
        //        next.live("click", function () {
        //            t.logevent("Navigate to next filter group");

        //            var level = currentstep;
        //            var nextlevel = level + 1;
        //            if (filterstree.element.find("li:[lvl=" + nextlevel + "]").length > 0) {
        //                filterstree.hideLevel(level);
        //                filterstree.showLevel(nextlevel);
        //                return nextlevel;
        //            }
        //            else {
        //                t.logevent("step " + nextlevel + " is loading");

        //                var li = filterstree.element.find("li:first");
        //                var ul = li.children("ul");

        //                var memSel = filterstree.collectCheckedNodes(membertree);
        //                var ps = [];
        //                ps.push('[' + li.find('label:first').text() + ']');
        //                li.parentsUntil(".ui-widget").filter('li').each(function () {
        //                    ps.push('[' + $(this).find('label:first').text() + ']');
        //                });
        //                var ln = "";
        //                var hid = li.attr('hid');
        //                if (hid == null) {
        //                    hid = 0;
        //                }
        //                filterstree.hideLevel(level);
        //                var data2 = { 'FilterDTO': { 'id': ReportId, 'ps': ps, 'pn': ln, 'hid': hid, 'level': nextlevel, 'memsel': memSel } };
        //                var data = JSON.stringify(data2);
        //                $.ajax({
        //                    type: 'POST',
        //                    contentType: "application/json; charset=utf-8",
        //                    dataType: "json",
        //                    url: filterstree.options.dataurl,
        //                    data: data,
        //                    error: function (msg) {
        //                        //debugger;
        //                        //alert("error");
        //                        t.logevent(msg.responseText);
        //                    },
        //                    success: function (resp) {
        //                        if (resp.d != null) {
        //                            var objdata = [];
        //                            objdata = $.parseJSON(resp.d);
        //                            ul = filterstree._appendNodesChildren(li, ul, objdata);
        //                            currentstep = nextlevel;
        //                        }
        //                    }
        //                });

        //            };






        //            //currentstep = filterstree.nextLevel(currentstep);
        //            prev.show();
        //        }).hide();

        //        prev = $("#prevfiltergroup");
        //        prev.live("click", function () {
        //            t.logevent("Navigate to previous filter group");
        //           // debugger;
        //            currentstep = filterstree.prevLevel(currentstep);
        //            prev.hide();

        //        }).hide();
        //     ///////////////////////////////////////////////////////////////////////////////////////////////////////////////   

        //    $("#showreport").live("click", function () {
        //  //      var mems = membertree.collectCheckedNodes(membertree);
        // //    if (mems) {
        //        t.loadreportmetadata(ReportId);
        //        t.unloadmemberselect();
        //   //  }

        //    });
        //    $("#cancel").live("click", function () {
        //        t.unloadmemberselect();
        //    });
        //    this.loadmemberselect();
        //}
    }


    this.ShowLoading = function () {

        loadingIndicator.show();

    }

    this.HideLoading = function () {

        loadingIndicator.fadeOut();
    }

}