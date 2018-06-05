materialAdmin


 .controller('homeCtrl', function ($timeout, $scope,$window,$http, messageService, ServiceResumoFaturas) {


     ////////////SGPC////////////////////////
     //PEGA O VALOR FATURADO
     //this.faturado = ServiceResumoFaturas.getFaturado().data;
     //alert(ServiceResumoFaturas.getFaturado()[0]);


     var codigo = $window.sessionStorage.getItem("codigo_usuario");
     var categorias;
     var dados;
   
     //$http.post('api/Home', { codigo: codigo })
     //       .then(function successCallback(response) {

     //           alert(response.data.Categorias);
     //           categorias = response.data.Categorias
     //           dados = response.data.Dados;
     //           //CarregaGrafico();

     //       }, function errorCallback(response) {
     //           alert("erro");
     //       });        
    
 })


//====================================
    // GRAFICO HOME
    //====================================

    .controller('TABGraficoCtrl', function ($scope, $window) {
        $scope.tabs = [
            {
                title: 'Gráfico',
                content: 'In hac habitasse platea dictumst. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos hymenaeos. Nam eget dui. In ac felis quis tortor malesuada pretium. Phasellus consectetuer vestibulum elit. Duis lobortis massa imperdiet quam. Pellentesque commodo eros a enim. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In ac dui quis mi consectetuer lacinia. Phasellus a est. Pellentesque commodo eros a enim. Cras ultricies mi eu turpis hendrerit fringilla. Donec mollis hendrerit risus. Vestibulum turpis sem, aliquet eget, lobortis pellentesque, rutrum eu, nisl. Praesent egestas neque eu enim. In hac habitasse platea dictumst.'
            },
            {
                title: 'Tabela',
                content: 'Duis arcu tortor, suscipit eget, imperdiet nec, imperdiet iaculis, ipsum. Vestibulum purus quam, scelerisque ut, mollis sed, nonummy id, metus. Nulla sit amet est. Praesent ac massa at ligula laoreet iaculis. Vivamus aliquet elit ac nisl. Nulla porta dolor. Cras dapibus. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus.',
            }
        ];

    })

   

