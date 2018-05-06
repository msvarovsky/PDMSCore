/*
    http://www.designchemical.com/blog/index.php/jquery/jquery-simple-vertical-accordion-menu/
    */
/*  http://jsfiddle.net/yeyene/Bx5cu/29/  */


$(document).ready(function () {

    
    //window.async.getFromController('/Async/AsyncList', 'list', null);

    //$("#PartialViewWrapperHovno").addClass("fjgfdkhjgdf");
    OpenModal('myModal1', 'Hovno');

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


// When the user clicks on <span> (x), close the modal
$('.CancelBtn, .OkBtn').click(function () {
    //$(this).parent().css({"display":"none"}); // Nefunguje dobre
    //this.panentNode.style.display = "none";   // Nefunguje dobre

    var modals = document.getElementsByClassName("Modal");
    for (i = 0; i < modals.length; i++) {
        modals[i].style.display = "none";
    }
});



function OpenModal(dialogID, returnFieldID) {

    // alert(DialogID);
    // $(DialogID).style.display = "block";
    console.log("OpenModal");
    
    $.ajax({
        url: "/Project/ModalPartial/",
        type: "GET",
        data: { DialogID: dialogID, ReturnFieldID: returnFieldID },
        success: function (partialViewResult) {
            if (partialViewResult.length > 0) {
                $("#" + dialogID).html(partialViewResult);
            
            }
        },
        error: function (result) {
            $("#"+dialogID).html("<div class=\"ModalBody\">\r\n<span class=\"CloseModal\">&times;</span>\r\n<p>Ajax failed...</p>\r\n</div>");
        }

    });

    document.getElementById(dialogID).style.display = "block";

    
    // Get the modal
    // var modal = document.getElementById('myModal');



    //$("#ModalDialog1").OpenModal("#ModalDialog1");

    //$("ModalDialog1").dialog({
    //    autoOpen: true,
    //    width: 400,
    //    resizable: false,
    //    title: 'My Table',
    //    modal: true,
    //    open: function (event, ui) {
    //        $(this).load('@Url.Action("ModalPartialView", "Project")');
    //    },
    //    buttons: {
            
    //        "Close": function () {
    //            alert("close");
    //            $(this).dialog("close");
    //        }
    //    }
    //});
};




// $('#my-dialog').dialog({
//     autoOpen: false,
//     width: 400,
//     resizable: false,
//     modal: true
// });

//$('#show-modal').click(function () {
//    $('#my-dialog').load("/Project/ModalPartialView/", function () {
//        $(this).dialog('open');
//    });
//    return false;
//});



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

