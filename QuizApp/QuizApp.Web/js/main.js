var app = angular.module("TiqIQApp", []);


app.factory("MainFact", [
    '$http','$q', function ($http,$q) {

        var data = {};        
        data.GetDataFromSvc = function () {
            var defer = $q.defer();

            $http.get("http://localhost:30979/DataService.svc/data").then(function(res) {
                return defer.resolve(res.data);
            }, function(err) {
                return defer.reject(err);
            });

            return defer.promise;
        }

        return data;

    }
]);

app.controller("MainCtrl", ['$scope', 'MainFact', function ($scope, MainFact) {

    MainFact.GetDataFromSvc().then(function(data) {
        $scope.data = data;
    });
    console.log($scope.data);
    $scope.title = "TiqIQ Data Result pulled from Service";
}]);
