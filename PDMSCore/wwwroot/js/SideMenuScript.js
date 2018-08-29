
$(document).ready(function () {
  $(".Accordion").find(".MIExpanded").next().show();
});

function OnNavItemClick(target, TypeOfNavItem, URL, data) {
    event.stopPropagation();
    var MenuItem;
    var SlideUpSpeed = 100;
    var SlideDownSpeed = 200;

    console.log("OnNavItemClick: URL = " + URL);

    if (TypeOfNavItem == "MIL") {
        console.log("OnNavItemClick: MenuItemL");
        MenuItem = $(target);
    }
    else if (TypeOfNavItem == "MIT" || TypeOfNavItem == "MICA") {
        console.log("OnNavItemClick: MenuItemText OR MIChevronArea");
        MenuItem = $(target).parent();
    }
    else if (TypeOfNavItem == "MIC") {
        console.log("OnNavItemClick: MIChevron");
        MenuItem = $(target).parent().parent();
    }

    $(MenuItem).parent().parent().find('.MISelected').removeClass('MISelected');    //  Removes the highlight/bold from all

    if ($(MenuItem).hasClass("MIExpanded")) {
        $(MenuItem).removeClass("MIExpanded");
        $(MenuItem).next().slideUp(SlideUpSpeed);
        
        var h = $(MenuItem).children()[1];
        var hh = $(h).children()[0];
        $(hh).removeClass('MIChevronExpanded'); //  Changes the chevron to contracted.
    }
    else {
        $(MenuItem).parent().children('.MenuContent').slideUp(SlideUpSpeed);    //  Closes all siblings (+ probably also the clicked one).
        $(MenuItem).parent().children('.MIExpanded').removeClass('MIExpanded'); //  Removes .MIExpanded from all where it is.
        $(MenuItem).parent().children("[class^='MenuItemL']").find('.MIChevronExpanded').removeClass('MIChevronExpanded');  //  Changes the chevron.

        $(MenuItem).addClass("MIExpanded"); //  Adds the MIExpanded to the clicked one.
        $(MenuItem).next().slideDown(SlideDownSpeed); //  Expands the MenuContent of the clicked one.

        var h = $(MenuItem).children()[1];
        var hh = $(h).children()[0];
        $(hh).addClass('MIChevronExpanded');    //  Changes the chevron to expanded.
    }

    $(MenuItem).addClass("MISelected"); // Adds the highlight/bold to the clicked item.


    if (URL !== undefined && URL != "")
        ReloadPageContent(URL, data);
    

    return;
}

//function OpenMenuUpdate(href) {
//  $.ajax({
//    url: "/Project/OpenMenuUpdate/",
//    type: "POST",
//    highlightPhrase: false,
//    dataType: "json",
//    data: { href: href },
//    success: function (data) { }
//  })
//}