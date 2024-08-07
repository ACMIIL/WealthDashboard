/*================================================
*
* Table of Contents :
* 1. Slick Slider
* 1. Navbar & Menu

*
================================================*/
"use strict";

/*===============================================
  1. Slick Slider
===============================================*/
$(document).on('ready', function() {
  $(".regular").slick({
    dots: true,
    arrows: false,
    infinite: true,
    slidesToShow: 1,
    slidesToScroll: 1,
    fade: true
  });
});


/*===============================================
  2. Navbar & Menu
===============================================*/
jQuery(document).ready(function($) {
  
  // Fixa navbar ao ultrapassa-lo
  var navbar = $('#navbar-main'),
                  distance = navbar.offset().top,
      $window = $(window);

  $window.scroll(function() {
      if ($window.scrollTop() >= distance) {
          navbar.removeClass('fixed-top').addClass('fixed-top');
                $("body").css("padding-top", "70px");
      } else {
          navbar.removeClass('fixed-top');
          $("body").css("padding-top", "0px");
      }
  });
});

// Mega Menu
window.addEventListener("resize", function() {
"use strict"; window.location.reload(); 
});
document.addEventListener("DOMContentLoaded", function()
{
/////// Prevent closing from click inside dropdown
document.querySelectorAll('.dropdown-menu').forEach(function(element)
{
   element.addEventListener('click', function(e)
   {
           e.stopPropagation();
   });
})
// make it as accordion for smaller screens
if (window.innerWidth < 992)
{
   // close all inner dropdowns when parent is closed
   document.querySelectorAll('.navbar .dropdown').forEach(function(everydropdown)
   {
           everydropdown.addEventListener('hidden.bs.dropdown', function()
           {
                   // after dropdown is hidden, then find all submenus
                   this.querySelectorAll('.submenu')
                           .forEach(function(everysubmenu)
                           {
                                   // hide every submenu as well
                                   everysubmenu
                                           .style
                                           .display =
                                           'none';
                           });
           })
   });
   document.querySelectorAll('.dropdown-menu a').forEach(function(element)
   {
           element.addEventListener('click', function(e)
           {
                   let nextEl = this.nextElementSibling;
                   if (nextEl && nextEl.classList
                           .contains('submenu'))
                   {
                           // prevent opening link if link needs to open dropdown
                           e.preventDefault();
                           console.log(nextEl);
                           if (nextEl.style.display ==
                                   'block')
                           {
                                   nextEl.style.display =
                                           'none';
                           }
                           else
                           {
                                   nextEl.style.display =
                                           'block';
                           }
                   }
           });
   })
}
// end if innerWidth
});
// DOMContentLoaded  end

