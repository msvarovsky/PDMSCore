
$(document).ready(function () {
  $(".Accordion").find(".MIExpanded").next().show();
});


var changeChevron = function () {
    $('.MIChevron.down').toggleClass('up');
}


function OnNavItemClick(target, data) {

    event.stopPropagation();

    var MenuItem;

    if (data == "MenuItemL") {
        console.log("OnNavItemClick: MenuItemL");
        MenuItem = $(target);
    }
    else if (data == "MenuItemText" || data == "MIChevronArea") {
        console.log("OnNavItemClick: MenuItemText OR MIChevronArea");
        MenuItem = $(target).parent();
    }
    else if (data == "MIChevron") {
        console.log("OnNavItemClick: MIChevron");
        MenuItem = $(target).parent().parent();
    }


    var SlideUpSpeed = 50;
    var SlideDownSpeed = 200;

    if ($(MenuItem).hasClass("MIExpanded")) {
        $(MenuItem).removeClass("MIExpanded");
        $(MenuItem).next().slideUp(SlideUpSpeed);

        var h = $(MenuItem).children()[1];
        var hh = $(h).children()[0];
        $(hh).removeClass('MIChevronExpanded');
    }
    else {
        $(MenuItem).addClass("MIExpanded");
        $(MenuItem).next().slideDown(SlideUpSpeed);

        var h = $(MenuItem).children()[1];
        var hh = $(h).children()[0];
        $(hh).addClass('MIChevronExpanded');
    }

    return;
    /////////////////////




    if (!$(target).hasClass("MIExpanded")) { //Sbalene
        $(target).siblings("[class^='MenuItemL']").removeClass("MIExpanded");
        $(target).siblings("[class^='MenuItemL']").next().slideUp(SlideUpSpeed);


        if (!$(target).hasClass("MIEmpty")) {
            $(target).addClass("MIExpanded");
            $(target).next().slideDown(SlideDownSpeed);
        }
    }
    else { // ROZbalene
        $(target).removeClass("MIExpanded");

        
    }

    //  Only 1 menu item can be selected.
    //$(".MISelected").removeClass("MISelected");
    //$(target).addClass("MISelected");
    //$(target).find(".MIChevron").toggleClass('MIChevronExpanded');




}

// $(".accordion").click(function (e) {
$("*").click(function (e) {
    
    var target;
    var tagClasses = $(e.target).attr('class');

    console.log("click - navigation");

    return;

    if (typeof tagClasses == 'undefined')
        return;


    if ((tagClasses.indexOf("MenuItemText") != -1) || (tagClasses.indexOf("MIChevron") != -1)) {
        target = $(e.target).parent();
        ////event.stopPropagation();//  Nemuze byt na zacatku teto funkce.
        if (tagClasses.indexOf("MenuItemText") != -1) {
            //console.log("MenuItemText");
            var href = e.target.getAttribute("href");
            OpenMenuUpdate(href);
        }
        if (tagClasses.indexOf("MIChevron") != -1) {
            //console.log("MIChevron: Redirecting to MenuItemText...");
            var children = $(target).children(".MenuItemText");
            ///children[0].click();
            return;
        }
    }
    else if (tagClasses.indexOf("MenuItemL") != -1) {
        //console.log('MenuItemL: Redirecting to MenuItemText...');
        ////event.stopPropagation();    //  Nemuze byt na zacatku teto funkce.
        target = e.target;
        var children = $(target).children(".MenuItemText");
        children[0].click();
        return;
    }
    else
        return;


    var SlideUpSpeed = 50;
    var SlideDownSpeed = 200;

    if (!$(target).hasClass("MIExpanded")) { //Sbalene
        $(target).siblings("[class^='MenuItemL']").removeClass("MIExpanded");
        $(target).siblings("[class^='MenuItemL']").next().slideUp(SlideUpSpeed);

        // Prehodi Chevron rozbaleneho menu zpet na sbaleny symbol.
        $(target).siblings("[class^='MenuItemL']").find(".MIChevron").removeClass("MIChevronExpanded");


        if (!$(target).hasClass("MIEmpty")) {
            $(target).addClass("MIExpanded");
            $(target).next().slideDown(SlideDownSpeed);
        }
    }
    else { // ROZbalene
        $(target).removeClass("MIExpanded");
        $(target).next().slideUp(SlideUpSpeed);
    }

    //  Only 1 menu item can be selected.
    $(".MISelected").removeClass("MISelected");
    $(target).addClass("MISelected");
    $(target).find(".MIChevron").toggleClass('MIChevronExpanded');

    ////////////////////////////////////////////////////////////////////

    /*if (!$(target).hasClass("MIEmpty"))
      $(target).toggleClass("MIExpanded");
    $(target).siblings().removeClass("MISelected");
    $(target).siblings().children().removeClass("MISelected");
    $(target).siblings().removeClass("MIExpanded");
    $(target).siblings().children().removeClass("MIExpanded");
    $(target).addClass("MISelected");
    $(allAtDepth).slideUp("fast");
    subItem.slideToggle("fast");*/

});

function OpenMenuUpdate(href) {
  //console.log('OpenMenuUpdate.href:', href);

  $.ajax({
    url: "/Project/OpenMenuUpdate/",
    type: "POST",
    highlightPhrase: false,
    dataType: "json",
    data: { href: href },
    success: function (data) { }
  })
}