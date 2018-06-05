<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="home.aspx.vb" Inherits="SGPC_FronEnd.home" %>


<header id="header" data-current-skin={{mactrl.currentSkin}} data-ng-include="'template/header.html'" data-ng-controller="headerCtrl as hctrl"></header>
<form id="form1" runat="server">

<section id="main">
    <aside id="sidebar" data-ng-include="'template/sidebar-left.html'" data-ng-class="{ 'toggled': mactrl.sidebarToggle.left === true }"></aside>

    <aside id="chat" data-ng-include="'template/chat.html'" data-ng-class="{ 'toggled': mactrl.sidebarToggle.right === true }"></aside>

    <section id="content">

        <div class="container">
            <div class="block-header">
                <h2>Dashboard   </h2>
                
                <ul class="actions">
                    <li>
                        <a href="">
                            <i class="zmdi zmdi-trending-up"></i>
                        </a>
                    </li>
                    <li>
                        <a href="">
                            <i class="zmdi zmdi-check-all"></i>
                        </a>
                    </li>
                    <li class="dropdown" uib-dropdown>
                        <a href="" uib-dropdown-toggle>
                            <i class="zmdi zmdi-more-vert"></i>
                        </a>

                        <ul class="dropdown-menu dropdown-menu-right">
                            <li>
                                <a href="">Refresh</a>
                            </li>
                            <li>
                                <a href="">Manage Widgets</a>
                            </li>
                            <li>
                                <a href="">Widgets Settings</a>
                            </li>
                        </ul>
                    </li>
                </ul>

            </div>

            <div class="card">
                <div class="card-header">
                    <h2>Sales Statistics <small>Vestibulum purus quam scelerisque, mollis nonummy metus</small></h2>

                    <ul class="actions">
                        <li>
                            <a href="">
                                <i class="zmdi zmdi-refresh-alt"></i>
                            </a>
                        </li>
                        <li>
                            <a href="">
                                <i class="zmdi zmdi-download"></i>
                            </a>
                        </li>
                        <li class="dropdown" uib-dropdown>
                            <a href="" uib-dropdown-toggle>
                                <i class="zmdi zmdi-more-vert"></i>
                            </a>

                            <ul class="dropdown-menu dropdown-menu-right">
                                <li>
                                    <a href="">Change Date Range</a>
                                </li>
                                <li>
                                    <a href="">Change Graph Type</a>
                                </li>
                                <li>
                                    <a href="">Other Settings</a>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </div>

                <div class="card-body">
                    <div class="chart-edge">

                        <div class="card-body card-padding">
                            
                            <tabset>
                                <tab heading="Gráfico" active="true" >
                                   <div id="graficoMensal" style="z-index:99999;"></div>
                                </tab>

                                <tab heading="Dados" >

                                    <div class="table-responsive">
                                    <asp:gridview id="gvResumoMensal" runat="server" enablesortingandpagingcallbacks="True"
                                     allowsorting="True" autogeneratecolumns="False" cellpadding="4" datasourceid="SqlDataSourceResumoGeral"
                                                                            enablemodelvalidation="True">
    
                                                <Columns>
                                                    <asp:BoundField DataField="DATA" HeaderText="DATA" SortExpression="DATA">
                                                        <ItemStyle  />
                                                    </asp:BoundField>
                                                    <%--   <asp:HyperLinkField DataNavigateUrlFields="DATA" 
                                                                DataNavigateUrlFormatString="rit.aspx?competencia={0}" HeaderText="DATA" />--%>
                                                    <asp:BoundField DataField="GASTO" HeaderText="GASTO" SortExpression="GASTO" DataFormatString="{0:c}">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="codigo_tipo" HeaderText="codigo_tipo" SortExpression="codigo_tipo">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                </Columns>
   
                                            </asp:gridview>
                            <asp:gridview id="gvResumoGeral" runat="server" allowpaging="True" enablesortingandpagingcallbacks="true" borderwidth="0"
                                autogeneratecolumns="true" class="table table-striped">
   
</asp:gridview>

                        </div>
                                </tab>
                            </tabset>

                             

                           


                        </div>


                        <div class="flot-chart" data-curvedline-chart ng-show="false"></div>
                       

                        

                    </div>
                </div>
            </div>

            <div class="mini-charts">
                <div class="row">
                    <div class="col-sm-6 col-md-3">
                        <div class="mini-charts-item bgm-cyan">
                            <div class="clearfix">
                                <div class="chart stats-bar" data-sparkline-bar></div>
                                <div class="count">
                                    <small>Website Traffics</small>
                                    <h2>987,459</h2>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-6 col-md-3">
                        <div class="mini-charts-item bgm-lightgreen">
                            <div class="clearfix">
                                <div class="chart stats-bar-2" data-sparkline-bar></div>
                                <div class="count">
                                    <small>Website Impressions</small>
                                    <h2>356,785K</h2>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-6 col-md-3">
                        <div class="mini-charts-item bgm-orange">
                            <div class="clearfix">
                                <div class="chart stats-line" data-sparkline-line></div>
                                <div class="count">
                                    <small>Total Sales</small>
                                    <h2>$ 458,778</h2>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-6 col-md-3">
                        <div class="mini-charts-item bgm-bluegray">
                            <div class="clearfix">
                                <div class="chart stats-line-2" data-sparkline-line></div>
                                <div class="count">
                                    <small>Support Tickets</small>
                                    <h2>23,856</h2>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="dash-widgets">
                <div class="row">
                    <div class="col-md-3 col-sm-6">
                        <div id="site-visits" class="dash-widget-item bgm-teal">
                            <div class="dash-widget-header">
                                <div class="p-20">
                                    <div class="dash-widget-visits" data-sparkline-line></div>
                                </div>

                                <div class="dash-widget-title">For the past 30 days</div>

                                <ul class="actions actions-alt">
                                    <li class="dropdown" uib-dropdown>
                                        <a href="" uib-dropdown-toggle>
                                            <i class="zmdi zmdi-more-vert"></i>
                                        </a>

                                        <ul class="dropdown-menu dropdown-menu-right">
                                            <li>
                                                <a href="">Refresh</a>
                                            </li>
                                            <li>
                                                <a href="">Manage Widgets</a>
                                            </li>
                                            <li>
                                                <a href="">Widgets Settings</a>
                                            </li>
                                        </ul>
                                    </li>
                                </ul>
                            </div>

                            <div class="p-20">

                                <small>Page Views</small>
                                <h3 class="m-0 f-400">47,896,536</h3>

                                <br/>

                                <small>Site Visitors</small>
                                <h3 class="m-0 f-400">24,456,799</h3>

                                <br/>

                                <small>Total Clicks</small>
                                <h3 class="m-0 f-400">13,965</h3>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-6">
                        <div id="pie-charts" class="dash-widget-item">
                            <div class="bgm-pink">
                                <div class="dash-widget-header">
                                    <div class="dash-widget-title">Email Statistics</div>
                                </div>

                                <div class="clearfix"></div>

                                <div class="text-center p-20 m-t-25">
                                    <div class="easy-pie main-pie" data-percent="75" data-easypie-chart>
                                        <div class="percent">45</div>
                                        <div class="pie-title">Total Emails Sent</div>
                                    </div>
                                </div>
                            </div>

                            <div class="p-t-20 p-b-20 text-center">
                                <div class="easy-pie sub-pie-1" data-percent="56" data-easypie-chart>
                                    <div class="percent">56</div>
                                    <div class="pie-title">Bounce Rate</div>
                                </div>
                                <div class="easy-pie sub-pie-2" data-percent="84" data-easypie-chart>
                                    <div class="percent">84</div>
                                    <div class="pie-title">Total Opened</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-6">
                        <div class="dash-widget-item bgm-lime">
                            <div id="weather-widget" data-weather-widget></div>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-6">
                        <div id="best-selling" class="dash-widget-item" data-ng-controller="bestsellingCtrl as wctrl">
                            <div class="dash-widget-header">
                                <div class="dash-widget-title">Best Sellings</div>
                                <img src="img/widgets/alpha.jpg" alt="">
                                <div class="main-item">
                                    <small>Samsung Galaxy Alpha</small>
                                    <h2>$799.99</h2>
                                </div>
                            </div>

                            <div class="listview p-t-5">
                                <a class="lv-item" href="" data-ng-repeat="w in wctrl.bsResult.list">
                                    <div class="media">
                                        <div class="pull-left">
                                            <img class="lv-img-sm" data-ng-src="img/widgets/{{ w.img }}" alt="">
                                        </div>
                                        <div class="media-body">
                                            <div class="lv-title">{{ w.name }}</div>
                                            <small class="lv-small">{{ w.range }}</small>
                                        </div>
                                    </div>
                                </a>

                                <div class="clearfix"></div>

                                <a class="lv-footer" href="">
                                    View All
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-6">
                    <!-- Recent Items -->
                    <div class="card" data-ng-controller="recentitemCtrl as rictrl">
                        <div class="card-header">
                            <h2>Recent Items <small>Phasellus condimentum ipsum id auctor imperdie</small></h2>
                            <ul class="actions">
                                <li class="dropdown" uib-dropdown>
                                    <a href="" uib-dropdown-toggle>
                                        <i class="zmdi zmdi-more-vert"></i>
                                    </a>

                                    <ul class="dropdown-menu dropdown-menu-right">
                                        <li>
                                            <a href="">Refresh</a>
                                        </li>
                                        <li>
                                            <a href="">Settings</a>
                                        </li>
                                        <li>
                                            <a href="">Other Settings</a>
                                        </li>
                                    </ul>
                                </li>
                            </ul>
                        </div>

                        <div class="card-body m-t-0">
                            <table class="table table-inner table-vmiddle">
                                <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Name</th>
                                    <th style="width: 60px">Price</th>
                                </tr>
                                </thead>
                                <tbody>
                                <tr data-ng-repeat="w in rictrl.riResult.list">
                                    <td class="f-500 c-cyan">{{ w.id }}</td>
                                    <td>{{ w.name }}</td>
                                    <td class="f-500 c-cyan">{{ w.price }}</td>
                                </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="recent-items-chart" class="flot-chart" data-line-chart></div>
                    </div>

                    <!-- Todo Lists -->
                    <div id="todo-lists" data-ng-controller="todoCtrl as tctrl">
                        <div class="tl-header">
                            <h2>Todo Lists</h2>
                            <small>Add, edit and manage your Todo Lists</small>

                            <ul class="actions actions-alt">
                                <li class="dropdown" uib-dropdown>
                                    <a href="" uib-dropdown-toggle>
                                        <i class="zmdi zmdi-more-vert"></i>
                                    </a>

                                    <ul class="dropdown-menu dropdown-menu-right">
                                        <li>
                                            <a href="">Refresh</a>
                                        </li>
                                        <li>
                                            <a href="">Manage Widgets</a>
                                        </li>
                                        <li>
                                            <a href="">Widgets Settings</a>
                                        </li>
                                    </ul>
                                </li>
                            </ul>
                        </div>

                        <div class="clearfix"></div>

                        <div class="tl-body">
                            <div id="add-tl-item" data-ng-class="{ 'toggled': tctrl.addTodoStat }" data-ng-click="tctrl.addTodoStat = true">
                                <i class="add-new-item zmdi zmdi-plus" data-ng-click="tctrl.addTodo($event)"></i>

                                <div class="add-tl-body">
                                    <textarea placeholder="What you want to do..." data-ng-model="tctrl.todo"></textarea>

                                    <div class="add-tl-actions">
                                        <a class="zmdi zmdi-close" data-tl-action="dismiss" data-ng-click="tctrl.addTodoStat = false; $event.stopPropagation()"></a>
                                        <a class="zmdi zmdi-check" data-tl-action="save" data-ng-click="tctrl.addTodoStat = false; $event.stopPropagation()"></a>
                                    </div>
                                </div>
                            </div>


                            <div class="checkbox media" data-ng-repeat="w in tctrl.tdResult.list">
                                <div class="pull-right">
                                    <ul class="actions actions-alt">
                                        <li class="dropdown" uib-dropdown>
                                            <a href="" uib-dropdown-toggle>
                                                <i class="zmdi zmdi-more-vert"></i>
                                            </a>

                                            <ul class="dropdown-menu dropdown-menu-right">
                                                <li><a href="">Delete</a></li>
                                                <li><a href="">Archive</a></li>
                                            </ul>
                                        </li>
                                    </ul>
                                </div>
                                <div class="media-body">
                                    <label>
                                        <input type="checkbox">
                                        <i class="input-helper"></i>
                                        <span>{{ w.todo }}</span>
                                    </label>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-sm-6">
                    <!-- Calendar -->
                    <div id="calendar-widget" data-full-calendar></div>

                    <!-- Recent Posts -->
                    <div class="card" data-ng-controller="recentpostCtrl as rpctrl">
                        <div class="card-header ch-alt m-b-20">
                            <h2>Recent Posts <small>Phasellus condimentum ipsum id auctor imperdie</small></h2>
                            <ul class="actions">
                                <li>
                                    <a href="">
                                        <i class="zmdi zmdi-refresh-alt"></i>
                                    </a>
                                </li>
                                <li>
                                    <a href="">
                                        <i class="zmdi zmdi-download"></i>
                                    </a>
                                </li>
                                <li class="dropdown" uib-dropdown>
                                    <a href="" uib-dropdown-toggle>
                                        <i class="zmdi zmdi-more-vert"></i>
                                    </a>

                                    <ul class="dropdown-menu dropdown-menu-right">
                                        <li>
                                            <a href="">Change Date Range</a>
                                        </li>
                                        <li>
                                            <a href="">Change Graph Type</a>
                                        </li>
                                        <li>
                                            <a href="">Other Settings</a>
                                        </li>
                                    </ul>
                                </li>
                            </ul>

                            <button class="btn bgm-cyan btn-float"><i class="zmdi zmdi-plus"></i></button>
                        </div>

                        <div class="card-body">
                            <div class="listview">

                                <a class="lv-item" href="" data-ng-repeat="w in rpctrl.rpResult.list">
                                    <div class="media">
                                        <div class="pull-left">
                                            <img class="lv-img-sm" data-ng-src="img/profile-pics/{{ w.img }}" alt="">
                                        </div>
                                        <div class="media-body">
                                            <div class="lv-title">{{ w.user }}</div>
                                            <small class="lv-small">{{ w.text }}</small>
                                        </div>
                                    </div>
                                </a>

                                <div class="clearfix"></div>

                                <a class="lv-footer" href="">View All</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</section>

<footer id="footer" data-ng-include="'template/footer.html'"></footer>

    </form>

<script>
 $(function () {
        //alert('teste');
        $('#graficoMensal').highcharts({

            chart: {
                type: 'line',
                options3d: {
                    enabled: true,
                    alpha: 0,
                    beta: 0,
                    viewDistance: 25,
                    depth: 40
                },
                marginTop: 80,
                marginRight: 40
            },
              colors: ['#5F9EA0', '#FFA500', '#FFB6C1', '#00BFFF', '#4682B4', '#BDB76B', '#DCDCDC', '#FF6347', '#008B45', '#FFB90F', '#9F79EE'],
            title: {
                 text: 'Evolução Por Tipo'
            },

            xAxis: {
                categories: [<%=graficoLabel %>]
            },

            yAxis: {
                allowDecimals: false,
                min: 0,
                //type: 'logarithmic',
                title: {
                    text: 'Gasto'
                },
                stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                },
                  formatter: function() {
						return 'R$ ' + Highcharts.numberFormat(this.total, 2, ',', '.');
					}
            }
            },

            tooltip: {
                headerFormat: '<b>{point.key}</b><br>',
                pointFormat: '<span style="color:{series.color}">\u25CF</span> {series.name}: R$ {point.y:.2f}',
                formatter: function() {
						return '<b>'+ this.series.name +'</b><br/>'+
							this.x +': '+ 'R$ ' + Highcharts.numberFormat(this.y, 2, ',', '.');
					}
            },

            plotOptions: {
                column: {
                    stacking: 'normal',
                    depth: 40,
                     dataLabels: {
                    enabled: false
                }
                },

                series: {
                    cursor: 'normal',
                    point: {
                        events: {
                            click: function (e) {
                            GraficoCusto(this.category);
                            }
                        }
                    },
                    marker: {
                        lineWidth: 1
                    }
                }
            },

            series: [
//            {
//                name: 'John',
//                data: [5, 3, 4, 7, 2]
//            }, {
//                name: 'Joe',
//                data: [3, 4, 4, 2, 5]
//            }, {
//                name: 'Jane',
//                data: [2, 5, 6, 2, 1]
//            }, {
//                name: 'Janet',
//                data: [3, 0, 4, 4, 3]
//            }

	
                 <% if ExibeMovel then %>
                {
					name: 'Telefonia Móvel',
					data: [<%=graficoData %>]
                    ,lineWidth:4
                    
				} 
                 <% end if %>
                  <% if ExibeFixo then %>
                ,{
					name: 'Telefonia Fixa',
					data: [<%=graficoData2 %>]
                    ,lineWidth:4
				}  
                   <% end if %>
                   <% if Exibe0800 then %>
                ,{
					name: '0800',
					data: [<%=graficoData3 %>]
                    ,lineWidth:4
				}  
                 <% end if %>
                <% if Exibe3003 then %>
                ,{
					name: 'Num. Único',
					data: [<%=graficoData4 %>]
                    ,lineWidth:4
				}
                <% end if %>
                <% if ExibeServico then %>
                ,{
					name: 'Serviços',
					data: [<%=graficoData5 %>]
                    ,lineWidth:4
				}
                 <% end if %>
                   <% if ExibeDados then %>
                ,{
					name: 'Link de Dados',
					data: [<%=graficoData6 %>]
                    ,lineWidth:4
				}
                 <% end if %>
                 ]


        });

     //$('#graficoMensal').highcharts.reflow();
    });

   


</script>

<asp:SqlDataSource ID="SqlDataSourceResumoGeral" runat="server" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"
    SelectCommand=""></asp:SqlDataSource>