(function ($) {
    "use strict"; // Start of use strict

    // Toggle the side navigation
    $("#menu-toggle").click(function (e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");

        //var arrClass = $(this).find('.fas');

        //if ($(arrClass).hasClass('fa-angle-double-left')) {
        //    $(arrClass).removeClass('fa-angle-double-left').addClass('fa-angle-double-right');
        //}
        //else if ($(arrClass).hasClass('fa-angle-double-right')) {
        //    $(arrClass).removeClass('fa-angle-double-right').addClass('fa-angle-double-left');
        //}
    });

    $('.nav-link-collapse').on('click', function () {
        $('.nav-link-collapse').not(this).removeClass('nav-link-show');
        $(this).toggleClass('nav-link-show');
    });



    $('.dropdown-menu a.dropdown-toggle').on('click', function (e) {
        if (!$(this).next().hasClass('show')) {
            $(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
        }
        var $subMenu = $(this).next(".dropdown-menu");
        $subMenu.toggleClass('show');

        $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e) {
            $('.dropdown-submenu .show').removeClass("show");
        });

        return false;
    });



    $(".btn-group, .dropdown").hover(
        function () {
            $('>.dropdown-menu', this).stop(true, true).fadeIn("fast");
            $(this).addClass('open');
        },
        function () {
            $('>.dropdown-menu', this).stop(true, true).fadeOut("fast");
            $(this).removeClass('open');
        });




})(jQuery); // End of use strict
