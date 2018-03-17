$(function() {
  function slideMenu() {
    var activeState = $("#menu-container .menu-list").hasClass("active");
    $("#menu-container .menu-list").animate(
      {
        left: activeState ? "0%" : "-100%"
      },
      400
    );
  }
  
  $("#menu-wrapper").click(function(event) {
    event.stopPropagation();
    $("#hamburger-menu").toggleClass("open");
    $("#menu-container .menu-list").toggleClass("active");
    slideMenu();

    $("body").toggleClass("overflow-hidden");
  });



  $(".menu-list").find(".accordion-toggle").click(function() {
    // $(this).next().toggleClass("open").slideToggle("fast");
    $(this).next().slideToggle();
    $(this).toggleClass("active-tab").find(".menu-link").toggleClass("active");


    // console.log($(this));
    // console.log($(this).parent());
    // $(this).parent().toggleClass("ParentSelected");
    // $(this).parent().find(".accordion-toggle").toggleClass("ParentPlusAccordion");
    // $(this).find("active-tab").toggleClass("THIS");
    // $(this).parent().find("active-tab").not(JQuery(this)).toggleClass("Not This");
    // console.log($(this).find("active-tab"));

    $(this).toggleClass("Akualni");
    
    //$(this).siblings().removeClass("kliknuto");
    
    
    // $(this).siblings("ul").css({"border": "2px solid blue"});
    $(this).siblings("ul").not(JQuery($(this).next("ul"))).css({"border": "2px solid blue"});
    
    
    
    //$(this).siblings().find("li").not(jQuery(this)).css({"border": "2px solid blue"});
       //.css({"color": "red", "border": "2px solid red"});
    // $(this).parent().find(".accordion-toggle").
    //$(this).parent().find(".accordion-toggle").toggleClass("Predesle");





    // $(".menu-list .accordion-content")
    //   .not($(this).next())
    //   .slideUp("fast");
      // .removeClass("open");
    $(".menu-list .accordion-toggle")
      .not(jQuery(this))
      .removeClass("active-tab")
      .find(".menu-link")
      .removeClass("active");
  });
}); // jQuery load