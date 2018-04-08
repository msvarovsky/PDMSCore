/*
    http://www.designchemical.com/blog/index.php/jquery/jquery-simple-vertical-accordion-menu/
    */
/*  http://jsfiddle.net/yeyene/Bx5cu/29/  */
$(document).ready(function () {

    window.async.getFromController('/Async/AsyncList', 'list', null);
    

    $('li ul').slideUp();
    $('.no-js li a.MainMenuLevel1').on("click", function () {
        if ($(this).siblings('ul').is(":visible")) {
            $(this).siblings('ul').slideUp(100);
            $(this).removeClass("SelectedMenuItem");
        }
        else {
            $('ul ul').slideUp(100);
            $('.MainMenuLevel1').removeClass("SelectedMenuItem");
            $(this).addClass("SelectedMenuItem");
            $(this).siblings('ul').slideDown(200);
        }
        return false;
    });

    $('.no-js li a.MainMenuLevel2').on("click", function () {
        if ($(this).siblings('ul').is(":visible")) {
            $('ul ul ul').slideUp(400);
            $(this).removeClass("SelectedMenuItem");
        }
        else {
            $('ul ul ul').slideUp(100);
            $('.MainMenuLevel2').removeClass("SelectedMenuItem");
            $(this).addClass("SelectedMenuItem");
            $(this).siblings('ul').slideDown(200);
        }
        return false;
    });
});


//        https://www.w3schools.com/howto/tryit.asp?filename=tryhow_css_js_dropdown
function onPanelMenuClick() {
    document.getElementById("myDropdown").classList.toggle("show");
}
window.onclick = function () { alert('test'); }


$(function () {
    $.datepicker.setDefaults({
        //              showOn: "both",
        firstDay: 1,
        buttonImageOnly: true,
        numberOfMonths: 2,
        showWeek: true,
        showAnim: "slideDown",
        showButtonPanel: true
    });


    $("#datepicker").datepicker($.datepicker.regional["fr"]);
});

