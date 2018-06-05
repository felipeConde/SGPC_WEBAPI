$()
{
    var app = angular.module('SGPCapp', []);

    function padding_left(s, c, n) {
        if (!s || !c || s.length >= n) {
            return s;
        }
        var max = (n - s.length) / c.length;
        for (var i = 0; i < max; i++) {
            s = c + s;
        }
        return s;
    }

    // right padding s with c to a total of n chars
    function padding_right(s, c, n) {
        if (!s || !c || s.length >= n) {
            return s;
        }
        var max = (n - s.length) / c.length;
        for (var i = 0; i < max; i++) {
            s += c;
        }
        return s;
    }

    function GetDate()
    {
        var monthNames = [
          "Janeiro", "Fevereiro", "Março",
          "Abril", "Maio", "Junho", "Julho",
          "Agosto", "Setembro", "Outubro",
          "Novembro", "Dezembro"
        ];
        var date = new Date();
        var day = date.getDate();
        var monthIndex = date.getMonth();
        var year = date.getFullYear();

        return padding_left(day.toString(),"0","2") + '/' + monthNames[monthIndex] + '/' + year;
    }

    app.controller('ConfigController', function ($scope) {
        $scope.txtIntro = "Bem vindo ao SGPC"
        var d = new Date();
        $scope.txtData = GetDate();
 
    });

}