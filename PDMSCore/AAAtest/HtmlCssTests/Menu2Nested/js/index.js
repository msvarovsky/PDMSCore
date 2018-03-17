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

  function getChildNodes(node) {
    var children = new Array();
    for(var child in node.childNodes) {
        if(node.childNodes[child].nodeType == 1) {
            children.push(child);
        }
    }
    return children;
  }

  // $(".menuitem").click(function() {
  $(".menuitem").click(function() {

    event.stopPropagation();


    //alert("dfas");
    
    $(this).siblings().find("ul").removeClass("otevrene");
    //$(this).siblings("ul").removeClass("otevrene");


    //$(this).children("ul").addClass("otevrene");
    $(this).children("ul").toggleClass("otevrene");

    
    var deda = $(this).parent();
    deda.css({"border": "2px solid yellow"});


    $(this).children("ul").css({"border": "2px solid green"});


    if (deda == $(this).children("ul"))
    {
      alert("deda");
    }
    

    //$(this).children("ul").css({"border": "2px solid green"});
    

    //alert($(this).prop("tagName"));
    //alert($(this).children().first().prop("tagName"));
    


    event.preventDefault();

    //$(this).children("ul").css({"border": "2px solid green"});

  });


  $(".menu-list").find(".accordion-toggle").click(function() {
    // $(this).next().toggleClass("open").slideToggle("fast");
   // $(this).siblings().slideToggle();

    //$(this).find("ul").slideToggle();
    
    // $(this).css({"border": "2px solid blue"});
    // $(this).children("ul").css({"border": "2px solid blue"});

    
    $(this).siblings().find("ul").removeClass("Aktivni");
    $(this).siblings().css({"border": "2px solid green"});

    //$(this).toggleClass("Aktivni");
    $(this).children("ul").toggleClass("Aktivni");
    //$(this).children("ul").css({"border": "2px solid blue"});

    $(this).children("ul").slideToggle();

    
    
    // $(this).children().hasClass("toggle").css({"border": "2px solid blue"});
    // $(this).children().find("ul").slideToggle();

    //$(this).find("ul").css({"border": "2px solid blue"});
    

    
    // $(this).siblings().find("ul").slideToggle();

    // $(this).siblings().css({"border": "2px solid blue"});
    
    // $(this).toggleClass("active-tab").find(".menu-link").toggleClass("active");
    // $(this).find("ul").slideToggle("fast");
    // $(this).toggleClass("Akualni");
    
    //$(this).siblings().removeClass("kliknuto");
    


    // $(".menu-list .accordion-content")
    //   .not($(this).next())
    //   .slideUp("fast");
      // .removeClass("open");
    
      // $(".menu-list .accordion-toggle")
      // .not(jQuery(this))
      // .removeClass("active-tab")
      // .find(".menu-link")
      // .removeClass("active");
  });
}); // jQuery load