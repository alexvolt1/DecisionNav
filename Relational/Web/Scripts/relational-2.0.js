function RelReport() {

    var t = this;
    var TopicId;
    var memberselect;
    var filters;
    var hasfilters = false;
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
    var SelectedMembers = null;

    var $memberscollection = {};
    var buttonfilters = $("#btnfilters");
    var options = {
        editable: false,
        rowHeight: 20,
        enableAddRow: false,
        enableCellNavigation: false,
        forceFitColumns: false,
        frozenColumn: 1

    };
    var buffersize = 1000;
    var skip = buffersize;
    var take = buffersize;
    var searchstr = "";
    var s = 0;

    this.CallbackFromMembers = function (args) {

        if (!hasfilters) {
            t.loadreportmetadata(TopicId);
        }
        else {
            memberselect.Hide(); filters.Show(args.selectionchanged);
        }

    }
    this.CallbackFromFilters = function () {
        t.loadreportmetadata(TopicId);
    }
    // Report Data


    this.GetFilters = function (el) {
        var step = -1;
        var mems = memberselect.collectSelections();
        var prevFilters = null;
        var DTO = { 'Report': { 'TopicId': TopicId, 'SelectedMembers': mems, 'SelectedFilters': prevFilters, 'Level': step } };
        var data = JSON.stringify(DTO);
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: dataurl + "/GetFilters",
            data: data,
            error: function (msg) {
                t.onError(msg.responseText);
            },
            success: function (resp) {
                if (resp.d != null) {
                    var objdata = resp.d;
                    if (objdata.length == 0) {
                        hasfilters = false;
                        buttonfilters.hide();
                    }
                    else {
                        hasfilters = true;
                        buttonfilters.show();
                        buttonfilters.live("click", function () { memberselect.Hide(); filters.Show(); });
                        el.renderfilters(objdata);
                    }

                }
            }
        });


    }



    this.GetStep = function (el) {
        var step = el.index;
        var mems = memberselect.collectSelections();
        var filts = filters.collectSelections();
        var DTO = { 'Report': { 'TopicId': TopicId, 'SelectedMembers': mems, 'SelectedFilters': filts, 'Level': step } };
        var data = JSON.stringify(DTO);
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: dataurl + "/GetFilterValues",
            data: data,
            error: function (msg) {
                t.onError(msg.responseText);
            },
            success: function (resp) {
                if (resp.d != null) {
                    var objdata = resp.d;
                    el.renderfilters(objdata, step);
                }
            }
        });



    }


    this.GetHierarchies = function (el) {
        var data = "{'id': '" + TopicId + "'}";
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: dataurl + "/GetHierarchies",
            data: data,
            error: function (msg) {
                t.onError(msg.responseText);
            },
            success: function (resp) {
                if (resp.d != null) {
                    var objdata = resp.d;
                    el.renderhierarchies(objdata);
                }
            }
        });


    }



    this.GetMembers = function (el) {
        var data = "{'id': '" + TopicId + "'}";
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: dataurl + "/GetMembers",
            data: data,
            error: function (msg) {
                t.onError(msg.responseText);
            },
            success: function (resp) {
                if (resp.d != null) {
                    var objdata = resp.d;
                    el.renderalternatereport(objdata.AlternateID);
                    el.rendermembers(objdata.Members);
                    el.searchq = objdata.SearchQ;
                }
            }
        });


    }


    this.SetHierarchies = function (el, e) {

        var data = "{'id': '" + TopicId + "','hid':'" + e + "'}";
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: dataurl + "/SetHierarchies",
            data: data,
            error: function (msg) {
                t.onError(msg.responseText);
            },
            success: function (resp) {
                if (resp.d != null) {
                    var objdata = resp.d;
                    el.reset();
                    //el.emptymembers();
                    el.rendermembers(objdata.Members);
                }
            }
        });


    }




    this.GetLevel = function (el) {
        var level = el.index;
        if (level > 0) {
            //var mems = memberselect.collectSelections();
            var mems = memberselect.collectSelections(level);
            var filts = null;
            var DTO = { 'Report': { 'TopicId': TopicId, 'SelectedMembers': mems, 'SelectedFilters': filts, 'Level': level } };
            var data = JSON.stringify(DTO);
            $.ajax({
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: dataurl + "/GetLevel",
                data: data,
                error: function (msg) {
                    t.onError(msg.responseText);
                },
                success: function (resp) {
                    if (resp.d != null) {
                        var objdata = resp.d;
                        el.rendermembers(objdata.Members, level);
                    }
                }
            });

        }

    }

    this.keywordsearch = function (el, keyword, qid) {
        var mems = null;
        var filts = null;
        var DTO = { 'Report': { 'TopicId': TopicId, 'SelectedMembers': mems, 'SelectedFilters': filts, 'Level': 0, 'Keyword': keyword, 'Qid': qid } };
        var data = JSON.stringify(DTO);
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: dataurl + "/GetSearchResults",
            data: data,
            error: function (msg) {
                t.onError(msg.responseText);
            },
            success: function (resp) {
                if (resp.d != null) {
                    var objdata = resp.d;
                    el.rendersearchresults(objdata);
                }
            }
        });
    }


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

            start = l + 1;
            s = 1;
            this.logevent("Loading: dataview - " + l + "  start - " + start + " take - " + take);
            loadingIndicator.show();
            var req = $.ajax({
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: dataurl + "/GetData",
                data: "{'id':'" + TopicId + "','from':'" + start + "','to':'" + take + "','searchstring':'" + searchstr + "'}",
                error: function (msg) {
                    t.onError(msg.responseText);
                },
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

    this.sortdata = function (col, dir) {
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
                //data: "{'id':'" + TopicId + "','col':'" + col + "','dir':'" + dir + "'}",
                data: "{'id':'" + TopicId + "','col':'" + col + "','dir':'" + dir + "','buffer':'" + buffersize + "'}",
                error: function (msg) {
                    t.onError(msg.responseText);
                },
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

    this.loadreport = function () {
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
        var usealt = memberselect.isalternateselected();
        var mems = memberselect.collectSelections();
        var sert = memberselect.collectsearchstrings();
        var filts = filters.collectSelections();
        // if (mems) {
        var DTO = { 'Report': { 'TopicId': TopicId, 'SelectedMembers': mems, 'UseAlternateReport': usealt, 'SelectedFilters': filts, 'SelectedSearchStrings': sert } };
        var data = JSON.stringify(DTO);
        loadingIndicator.show();
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: dataurl + "/GetMetaData",
            data: data,
            error: function (msg) {
                t.onError(msg.responseText);
            },
            success: function (resp) {
                if (resp.d != null) {
                    var objdata = [];
                    objdata = $.parseJSON(resp.d);

                    columns = objdata;
                    t.loadreport();
                }
                else {
                    alert("Unable to get report metadata");
                }
                loadingIndicator.fadeOut();
            }
        });
        // }
    }

    this.ShowLoading = function () {
        loadingIndicator.show();
    }

    this.HideLoading = function () {
        loadingIndicator.fadeOut();
    }

    this.comparer = function (a, b) {
        var x = a[sortcol], y = b[sortcol];
        return (x == y ? 0 : (x > y ? 1 : -1));
    }

    this.onError = function (msg) {
        //t.logevent(msg);
        alert(msg);
        //hack to handle server interruptions
        openOverlay('#overlay-inAbox');

    }

    //
    function openOverlay(olEl) {
        $oLay = $(olEl);

        if ($('#overlay-shade').length == 0)
            $('body').prepend('<div id="overlay-shade"></div>');

        $('#overlay-shade').fadeTo(300, 0.6, function () {
            var props = {
                oLayWidth: $oLay.width(),
                scrTop: $(window).scrollTop(),
                viewPortWidth: $(window).width()
            };

            var leftPos = (props.viewPortWidth - props.oLayWidth) / 2;

            $oLay
                .css({
                    display: 'block',
                    opacity: 0,
                    top: '-=300',
                    left: leftPos + 'px'
                })
                .animate({
                    top: props.scrTop + $(window).height() / 2 - 140,
                    opacity: 1
                }, 600);
        });
    }

    function closeOverlay(targetURL) {
        $('.overlay').animate({
            top: '-=300',
            opacity: 0
        }, 400, function () {
            $('#overlay-shade').fadeOut(300);
            $(this).css('display', 'none');
        });

        //window.location.target = "_parent";
        window.location.href = targetURL;

    }

    $('#overlay-shade, .overlay a').live('click', function (e) {
        var targetURL = $('#overlay-inAbox a').attr('href');
        closeOverlay(targetURL);
        if ($(this).attr('href') == '#') e.preventDefault();
    });


    // Usage
    $('#overlaylaunch-inAbox').click(function (e) {
        openOverlay('#overlay-inAbox');
        e.preventDefault();
    });



    this.logevent = function (msg) {
        var console = $("#debug");
        var entries = console.find("p");
        if (entries.length > 20) {
            entries.first().remove();
        }
        console.append("<p>" + msg + "</p>");
    }

    this.init = function (topicid) {
        TopicId = topicid;

        if (!loadingIndicator) {
            loadingIndicator = $("<span class='loading-indicator'><label>Buffering...</label></span>").appendTo(document.body);
            var $g = $(containerdiv);
            loadingIndicator
                .css("position", "absolute")
                .css("top", $g.position().top + $g.height() / 2 - loadingIndicator.height() / 2)
                .css("left", $g.position().left + $g.width() / 2 - loadingIndicator.width() / 2);

            memberselect = $('#memsel').memberselect({
                onfinish: this.CallbackFromMembers
                , onneedhierarchies: this.GetHierarchies
                , onsetcurrenthierarchie: this.SetHierarchies
                , onneedmembers: this.GetMembers
                , onafterstepshow: this.GetLevel
                , onlogevent: this.logevent
                , onkeywordsearch: this.keywordsearch
            });
            filters = $('#filterselect').filterdialog({
                onfinish: this.CallbackFromFilters
                , onafterstepshow: this.GetStep
                , onneedfilters: this.GetFilters
                , onlogevent: this.logevent
            });

            $("#btnmselect").live("click", function () { filters.Hide(); memberselect.Show(); });
            loadingIndicator.fadeOut();



        }
    }
}