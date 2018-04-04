
$(document).ready(function () {
  $(".Accordion").find(".MIExpanded").next().show();
});


var changeChevron = function() {
  $('.MIChevron.down').toggleClass('up'); }


// $(".accordion").click(function (e) {
$("*").click(function (e) {

  var target;
  var tagClasses = $(e.target).attr('class');
  var name;
  
  if (tagClasses.includes("MenuItemText") || tagClasses.includes("MIChevron"))
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

  if (!$(target).hasClass("MIExpanded")) 
  { //Sbalene
    $(target).siblings("[class^='MenuItemL']").removeClass("MIExpanded");
    $(target).siblings("[class^='MenuItemL']").next().slideUp(SlideUpSpeed);

    // Prehodi Chevron rozbaleneho menu zpet na sbaleny symbol.
    $(target).siblings("[class^='MenuItemL']").find(".MIChevron").removeClass("MIChevronExpanded");


    if (!$(target).hasClass("MIEmpty")) {
      $(target).addClass("MIExpanded");
      $(target).next().slideDown(SlideDownSpeed);
    }
  }
  else 
  { // ROZbalene
    $(target).removeClass("MIExpanded");
    $(target).next().slideUp(SlideUpSpeed);
  }

  //  Only 1 menu item can be selected.
  $(".MISelected").removeClass("MISelected");
  $(target).addClass("MISelected");


  // document.getElementById('debug').innerHTML = new Date().getTime();

  $(target).find(".MIChevron").toggleClass('MIChevronExpanded');



  ////////////////////////////////////////////////////////////////////

  ////////////////////////////////////////////////////////////////////

  // if (!$(target).hasClass("MIEmpty"))
  //   $(target).toggleClass("MIExpanded");

  // $(target).siblings().removeClass("MISelected");
  // $(target).siblings().children().removeClass("MISelected");

  // $(target).siblings().removeClass("MIExpanded");
  // $(target).siblings().children().removeClass("MIExpanded");

  // $(target).addClass("MISelected");

  //$(allAtDepth).slideUp("fast");
  //subItem.slideToggle("fast");

});
