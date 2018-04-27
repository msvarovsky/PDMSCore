/*
    http://www.designchemical.com/blog/index.php/jquery/jquery-simple-vertical-accordion-menu/
    */
/*  http://jsfiddle.net/yeyene/Bx5cu/29/  */
$(document).ready(function () {

    //window.async.getFromController('/Async/AsyncList', 'list', null);

    //$("#PartialViewWrapperHovno").addClass("fjgfdkhjgdf");

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

document.onclick = function (event) {

    // Zavru vsechny pootevrene dd-menu
    if (!event.target.matches('.dd-menu-btn')) {

        var dropdowns = document.getElementsByClassName("dd-menu-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}

//        https://www.w3schools.com/howto/tryit.asp?filename=tryhow_css_js_dropdown
function onPanelMenuClick(e) {
    // Nejdrive zavru vsechny pootevrene ...
    var dropdowns = document.getElementsByClassName("dd-menu-content");
    var i;
    for (i = 0; i < dropdowns.length; i++) {
        var openDropdown = dropdowns[i];
        if (openDropdown.classList.contains('show')) {
            openDropdown.classList.remove('show');
        }
    }
    // ... pak ukazu jenom ten na ktery bylo kliknuto.
    document.getElementById(e).classList.toggle("show");
}


function onPanelMenuItemClick(panelMenuID,panelMenuItemID) {

    //alert("LoadPartialView:" + panelMenuID);

    //var modal = $('<div>').dialog({ modal: true });
    //modal.dialog('widget').hide();
    $('#ajax_loader').show();


    $.ajax({
        url: "/Project/PanelMenuItemClick/",
        type: "GET",
        data: { PanelMenuID: panelMenuID, PanelMenuItemID: panelMenuItemID },
        success: function (partialViewResult) {
            $('#ajax_loader').hide();
            if (partialViewResult.length > 0)
                $("#Panel-" + panelMenuID).replaceWith(partialViewResult);
        },
        error: function (result) {
            alert("error!" + result);
            $('#ajax_loader').hide();
        }
    });


    //$.ajax({
    //    url: "/Project/PanelMenuItemClick/",
    //    type: "GET",
    //    data: { PanelMenuID: panelMenuID, PanelMenuItemID: panelMenuItemID }
    //})
    //    .done(function (partialViewResult) {
    //        if (partialViewResult.length > 0)
    //            $("#Panel-" + panelMenuID).replaceWith(partialViewResult);
    //    });


    //alert("ahojky refresh:" + PanelMenuID + ", ", PanelMenuItemID);
    //$.ajax({
    //    url: "/Project/PanelMenuItemClick/",
    //    type: "POST",
    //    highlightPhrase: false,
    //    dataType: "json",
    //    data: { PanelMenuID: panelMenuID, PanelMenuItemID: panelMenuItemID  },
    //    success: function (data) { }
    //})
}

function LoadPartialView(panelMenuID) {

    
    
}


//window.onclick = function () { alert('test'); }


$(function () {
    //$.datepicker.setDefaults({
    //    //              showOn: "both",
    //    firstDay: 1,
    //    buttonImageOnly: true,
    //    numberOfMonths: 2,
    //    showWeek: true,
    //    showAnim: "slideDown",
    //    showButtonPanel: true
    //});


    //$("#datepicker").datepicker($.datepicker.regional["fr"]);
});

