﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="main.aspx.vb" Inherits="SGPC_FronEnd.main" %>

<!DOCTYPE html>

<html data-ng-app="SGPCAPP" xmlns="http://www.w3.org/1999/xhtml" data-ng-controller="materialadminCtrl as mactrl">
<head runat="server">
      <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE"/>
    <meta http-equiv="CACHE-CONTROL" content="NO-STORE"/> 

     <!-- Vendor CSS -->
    <link href="vendors/bower_components/animate.css/animate.min.css" rel="stylesheet"/>
    <link href="vendors/bower_components/material-design-iconic-font/dist/css/material-design-iconic-font.min.css" rel="stylesheet"/>
    <link href="vendors/bower_components/bootstrap-sweetalert/lib/sweet-alert.css" rel="stylesheet"/>
    <link href="vendors/bower_components/angular-loading-bar/src/loading-bar.css" rel="stylesheet"/>
    <link href="vendors/bower_components/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.min.css" rel="stylesheet"/>

   


    <title></title>
</head>
<body  data-ng-class="{ 'sw-toggled': mactrl.layoutType === '1'}">   
   
        <!-- CSS -->
    <link href="css/app.min.1.css" rel="stylesheet" id="app-level">
    <link href="css/app.min.2.css" rel="stylesheet">
    <link href="css/demo.css" rel="stylesheet">
         <data ui-view></data>

 
   
         <!-- Older IE warning message -->
    <!--[if lt IE 9]>
        <div class="ie-warning">
            <h1 class="c-white">Warning!!</h1>
            <p>You are using an outdated version of Internet Explorer, please upgrade <br/>to any of the following web browsers to access this website.</p>
            <div class="iew-container">
                <ul class="iew-download">
                    <li>
                        <a href="http://www.google.com/chrome/">
                            <img src="img/browsers/chrome.png" alt="">
                            <div>Chrome</div>
                        </a>
                    </li>
                    <li>
                        <a href="https://www.mozilla.org/en-US/firefox/new/">
                            <img src="img/browsers/firefox.png" alt="">
                            <div>Firefox</div>
                        </a>
                    </li>
                    <li>
                        <a href="http://www.opera.com">
                            <img src="img/browsers/opera.png" alt="">
                            <div>Opera</div>
                        </a>
                    </li>
                    <li>
                        <a href="https://www.apple.com/safari/">
                            <img src="img/browsers/safari.png" alt="">
                            <div>Safari</div>
                        </a>
                    </li>
                    <li>
                        <a href="http://windows.microsoft.com/en-us/internet-explorer/download-ie">
                            <img src="img/browsers/ie.png" alt="">
                            <div>IE (New)</div>
                        </a>
                    </li>
                </ul>
            </div>
            <p>Sorry for the inconvenience!</p>
        </div>
    <![endif]-->



    <!-- Core -->
    <script src="vendors/bower_components/jquery/dist/jquery.min.js"></script>

    <!-- Angular -->
    <script src="vendors/bower_components/angular/angular.min.js"></script>
    <script src="vendors/bower_components/angular-animate/angular-animate.min.js"></script>
    <script src="vendors/bower_components/angular-resource/angular-resource.min.js"></script>

    <!-- Highcharts -->
    <script src="https://code.highcharts.com/highcharts.js"></script>

    <!-- Angular Modules -->
    <script src="vendors/bower_components/angular-ui-router/release/angular-ui-router.min.js"></script>
    <script src="vendors/bower_components/angular-loading-bar/src/loading-bar.js"></script>
    <script src="vendors/bower_components/oclazyload/dist/ocLazyLoad.min.js"></script>
    <script src="vendors/bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js"></script>

   

    <!-- Common Vendors -->
    <script src="vendors/bower_components/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.concat.min.js"></script>
    <script src="vendors/bower_components/bootstrap-sweetalert/lib/sweet-alert.min.js"></script>
    <script src="vendors/bower_components/Waves/dist/waves.min.js"></script>
    <script src="vendors/bootstrap-growl/bootstrap-growl.min.js"></script>
    <script src="vendors/bower_components/ng-table/dist/ng-table.min.js"></script>


    <!-- Placeholder for IE9 -->
    <!--[if IE 9 ]>
        <script src="vendors/bower_components/jquery-placeholder/jquery.placeholder.min.js"></script>
    <![endif]-->
    <!-- Using below vendors in order to avoid misloading on resolve -->
    <script src="vendors/bower_components/flot/jquery.flot.js"></script>
    <script src="vendors/bower_components/flot.curvedlines/curvedLines.js"></script>
    <script src="vendors/bower_components/flot/jquery.flot.resize.js"></script>
    <script src="vendors/bower_components/moment/min/moment.min.js"></script>
    <script src="vendors/bower_components/fullcalendar/dist/fullcalendar.min.js"></script>
    <script src="vendors/bower_components/flot-orderBars/js/jquery.flot.orderBars.js"></script>
    <script src="vendors/bower_components/flot/jquery.flot.pie.js"></script>
    <script src="vendors/bower_components/flot.tooltip/js/jquery.flot.tooltip.min.js"></script>
    <script src="vendors/bower_components/angular-nouislider/src/nouislider.min.js"></script>


    <!-- App level -->
    <script src="js/app.js"></script>
    <script src="js/config.js"></script>    
    <script src="js/controllers/main.js"></script>
    <script src="js/controllers/home.js"></script>
    <script src="js/services.js"></script>
    <script src="js/templates.js"></script>
    <script src="js/controllers/ui-bootstrap.js"></script>
    <script src="js/controllers/table.js"></script>



    <!-- Template Modules -->
    <script src="js/modules/template.js"></script>
    <script src="js/modules/ui.js"></script>
    <script src="js/modules/charts/flot.js"></script>
    <script src="js/modules/charts/other-charts.js"></script>
    <script src="js/modules/form.js"></script>
    <script src="js/modules/media.js"></script>
    <script src="js/modules/components.js"></script>
    <script src="js/modules/calendar.js"></script>
    <script src="js/modules/demo.js"></script>
    
    
    
</body>
</html>