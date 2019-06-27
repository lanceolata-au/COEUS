// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function() {

    var sidenav = document.querySelectorAll('.sidenav');
    var instances1 = M.Sidenav.init(sidenav);
    
    var datepicker = document.querySelectorAll('.datepicker');
    var instances2 = M.Datepicker.init(datepicker, {
        format: 'dd mmm yyyy',
        minDate: new Date(1930, 0 ,1),
        maxDate: new Date(2008, 0 ,1),
        yearRange: 60
    });

    var tabs = document.querySelectorAll('.tabs');
    var instance = M.Tabs.init(tabs, {
        duration: 300,
        swipeable: false
    });

    M.AutoInit();
    
});
