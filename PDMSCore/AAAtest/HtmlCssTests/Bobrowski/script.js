
  $(".me").click(function (e) {
    alert("me");
  });


  // $(".accordion").click(function (e) {
  $("*").click(function (e) {
    var headers = ["H1", "H2", "H3", "H4", "H5", "H6", "div"];

    var target = e.target,
      name = target.nodeName.toUpperCase();


    if ($.inArray(name, headers) > -1) {
      var subItem = $(target).next();

      //slideUp all elements (except target) at current depth or greater
      var depth = $(subItem).parents().length;
      var allAtDepth = $(".accordion p, .accordion div").filter
      (
        function () 
        {
          if ($(this).parents().length >= depth && this !== subItem.get(0)) 
          {
            return true;
          }
        }
      );
      $(allAtDepth).slideUp("fast");

      //slideToggle target content and adjust bottom border if necessary
      // subItem.slideToggle("fast", function () {
      //   $(".accordion :visible:last").css("border-radius", "0 0 10px 10px");
      // });
      subItem.slideToggle("fast");


      $(target).css({ "border-bottom-right-radius": "0", "border-bottom-left-radius": "0" });
    }
  });