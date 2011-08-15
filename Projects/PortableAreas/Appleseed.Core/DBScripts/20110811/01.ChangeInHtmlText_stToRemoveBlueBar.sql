DELETE FROM rb_HtmlText_st WHERE ModuleID = 5
GO
INSERT rb_HtmlText_st
(ModuleID, DesktopHtml, MobileSummary, MobileDetails)
VALUES (5, '<style type="text/css">
        /****************/
        /****SLIDER*****/
        /***************/
/*contenedor */	
.slideshow{
    width:940px;
    height:300px;
    margin-top:10px;
    overflow:hidden;
    position:relative;}
    
/* JPG */	
.slider_image{
    width:430px;
    height:300px;
    display:inline-block;}	
    
/* TEXT */
.wrap_text{
    height:300px;
    width:390px;
    right:-440px;
    top:30px;
    color:#fff;
    position:absolute;}	

/* LINKS */
.continue{
    float:right;
    color:#fff!important;
    font-family:Helvetica, Arial;
    font-size:14px;
}
.continue:hover{
    float:right;
    color:#ccc!important;
    font-family:Helvetica, Arial;
    font-size:14px;
}
#fsn {
    display:block;
    height:25px;
    margin:10px auto -5px;
    position:relative;
    text-align:center;
    width:940px;
    z-index:0;
}
#fsn ul {
    display:block;
    height:20px;
    list-style:none outside none;
    margin:0 auto;
    overflow:hidden;
    padding:5px 0 0;
    position:relative;
    width:125px;
}
#fs_pagination a{
    display:block;
    float:left;
    height:17px;
    margin:0 10px 0 0;
    padding:0;
    width:17px;
    background-color:#ededed;
    color:#22C0FD;
    font-size:10px;
    text-indent:-9999px;
    font-weight:bold;
}
#fs_pagination a:hover,
#fs_pagination a:active,
 a.activeSlide{ 
    background-color:#333!important;
    text-indent:-9999px;
}

#Content_moduleType{
    color:#666;}
</style>
<!-- include Cycle plugin -->
<script type="text/javascript" src="~/aspnet_client/js/jquery.cycle.min.js"></script>
<!--  initialize the slideshow when the DOM is ready -->
<script type="text/javascript">
$(document).ready(function () {
    $(''.slideshow'').after(''<div id="fsn"><ul id="fs_pagination">'').cycle({
        timeout: 5000, // milliseconds between slide transitions (0 to disable auto advance)
        fx: ''fade'', // choose a transition type, ex: fade, scrollUp, shuffle, etc...            
        pager: ''#fs_pagination'', // selector for element to use as pager container
        pause: true, // true to enable "pause on hover"
        pauseOnPagerHover: 0 // true to pause when hovering over pager link
   });
});
</script>
    <!-- Jquery Container-->
    <ul class="slideshow">
            <!-- IMAGE & TEXT Number (1) -->
            <li class="content">
                <img class="slider_image" src="~/Design/DesktopLayouts/Education/images/slider1.jpg" width="430" height="300"/>
                    <div class="wrap_text">
                        <h1>Grow your business.</h1>
                            <h2>You downloaded Appleseed Portal to help you run your company online. As we work to make this easier for you in the upcoming versions. You still have to do some basic things. You should consider taking these steps as soon as you can if you haven''t.</h2>
                        <br/>
                    <a class="continue" href="">Continue Reading » </a>			
                </div>
            </li>
            <!-- IMAGE & TEXT Number (2) -->
            <li class="content">
                <img  class="slider_image" src="~/Design/DesktopLayouts/Education/images/slider2.jpg" width="430" height="300" />
                    <div class="wrap_text">
                        <h1>Connect with others.</h1>
                            <h2>There are several ways for you to connect to other entrepreneurs, prospective customers and business partners. Here are a few suggestions that can get you started in your quest to build a great company.</h2>
                        <br/>
                    <a class="continue" href="">Continue Reading » </a>
                    </div>		
            </li>
            <!-- IMAGE & TEXT Number (3) -->
            <li class="content">
                <img class="slider_image" src="~/Design/DesktopLayouts/Education/images/slider3.jpg" width="430" height="300" />
                    <div class="wrap_text">				
                        <h1>Trade with the world.</h1>
                            <h2>Once you have your business setup, and have a network of businesses to help you deliver your products and services, you are ready to trade. Start small, and go&nbsp;big. Sky is the limit, until you surpass it.</h2>
                        <br/>
                    <a class="continue" href="">Continue Reading » </a>
                </div>			
            </li>
    </ul>
        <!-- End Jquery Container-->', '', '')