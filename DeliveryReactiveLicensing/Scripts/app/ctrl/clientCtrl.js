app.controller('clientController', function ($scope) {
    $scope.m = {};

    $scope.initCountry = function () {
        if ($scope.m.CountryId <= 0) {
            $scope.m.country = $scope.lstCountries[0];
            $scope.m.CountryId = $scope.m.country.Key;
        }
        else {
            $scope.m.country = undefined;
            for (var i = 0; i < $scope.lstCountries.length; i++) {
                var country = $scope.lstCountries[i];
                if (country.Key === $scope.m.CountryId) {
                    $scope.m.country = country;
                    $scope.m.CountryId = $scope.m.country.Key;
                    break;
                }
            }
            
            if ($scope.m.country === undefined) {
                $scope.m.country = $scope.lstCountries[0];
                $scope.m.CountryId = $scope.m.country.Key;
            }

        }
    };

});