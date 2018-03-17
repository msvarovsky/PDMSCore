
$(document).ready(function () {
  $(".Accordion").find(".Expanded").next().show();
});


var changeChevron = function() {
  $('.chevron.down').toggleClass('up'); }


// $(".accordion").click(function (e) {
$("*").click(function (e) {

  var target;
  var tagClasses = $(e.target).attr('class');
  var name;
  
  if (tagClasses.includes("MenuItemText") || tagClasses.includes("Chevron"))
  {
    target = $(e.target).parent();
    name = target["0"].nodeName.toUpperCase();
  }
  else if (tagClasses.includes("MenuItemL"))
  {
    target = e.target;
    name = target.nodeName.toUpperCase();
  }
  else
    return;
  

  var SlideUpSpeed = 50;
  var SlideDownSpeed = 200;

  if (!$(target).hasClass("Expanded")) 
  { //Sbalene
    $(target).siblings("[class^='MenuItemL']").removeClass("Expanded");
    $(target).siblings("[class^='MenuItemL']").next().slideUp(SlideUpSpeed);

    // Prehodi Chevron rozbaleneho menu zpet na sbaleny symbol.
    $(target).siblings("[class^='MenuItemL']").find(".Chevron").removeClass("ChevronExpanded");


    if (!$(target).hasClass("empty")) {
      $(target).addClass("Expanded");
      $(target).next().slideDown(SlideDownSpeed);
    }
  }
  else 
  { // ROZbalene
    $(target).removeClass("Expanded");
    $(target).next().slideUp(SlideUpSpeed);
  }

  //  Only 1 menu item can be selected.
  $(".MISelected").removeClass("MISelected");
  $(target).addClass("MISelected");


  // document.getElementById('debug').innerHTML = new Date().getTime();

  $(target).find(".Chevron").toggleClass('ChevronExpanded');



  ////////////////////////////////////////////////////////////////////

  ////////////////////////////////////////////////////////////////////

  // if (!$(target).hasClass("empty"))
  //   $(target).toggleClass("Expanded");

  // $(target).siblings().removeClass("MISelected");
  // $(target).siblings().children().removeClass("MISelected");

  // $(target).siblings().removeClass("Expanded");
  // $(target).siblings().children().removeClass("Expanded");

  // $(target).addClass("MISelected");

  //$(allAtDepth).slideUp("fast");
  //subItem.slideToggle("fast");

});
