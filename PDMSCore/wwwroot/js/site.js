/*
    http://www.designchemical.com/blog/index.php/jquery/jquery-simple-vertical-accordion-menu/
    */
/*  http://jsfiddle.net/yeyene/Bx5cu/29/  */


$(document).ready(function () {

    console.log("site.js-ready");
    //OpenModal('myModal1', 'Hovno');
    return;

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
    var i;

    // Zavru vsechny pootevrene dd-menu
    if (!event.target.matches('.dd-menu-btn')) {
        var dropdowns = document.getElementsByClassName("dd-menu-content");
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }

    // Zavru otevreny modal dialog.
    var modals = document.getElementsByClassName("Modal");
    for (i = 0; i < modals.length; i++) {
        if (event.target == modals[i])
            modals[i].style.display = "none";
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

function onPanelMenuItemClick(panelOwnerID, panelID, panelMenuItemID) {
    console.log("onPanelMenuItemClick");

    if (panelMenuItemID == "save") {
        console.log("save");
        var form = $("#Form-" + panelID);
        if (form != null)
            form.submit();
    }
    else {
        $('#ajax_loader').show();
        $.ajax({
            //url: "/Project/PanelMenuItemClick/",
            url: "/Panel/PanelMenuItemClick/",
            type: "GET",
            data: { PanelOwnerID: panelOwnerID, PanelMenuID: panelID, PanelMenuItemID: panelMenuItemID },
            success: function (partialViewResult) {
                $('#ajax_loader').hide();
                if (partialViewResult.length > 0)
                    $("#Panel-" + panelID).replaceWith(partialViewResult);
            },
            error: function (result) {
                alert("error!" + result);
                $('#ajax_loader').hide();
            }
        });
    }
}

function ReloadPageContent(partialControlAndAction, data) {
    console.log("ReloadPageContent");
    $.ajax({
        url: "/" + partialControlAndAction,
        type: "GET",
        data: { ProjectID: data, Other: data },
        success: function (partialViewResult) {
            if (partialViewResult.length > 0)
                $("#PageContentID").html(partialViewResult);
        },
        error: function (result) {
            alert("error: ReloadPageContent: " + result);
        }
    });
}




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

