app.controller('periodController', function ($scope, $timeout, sharedSvc) {
    $scope.m = {};
    $scope.lstPeriods = undefined;
    $scope.periodCount = -1;

    $scope.payPeriod = function(id, startDate, endDate, dlgIdModal, url) {
        window.showConfirmService(id, dlgIdModal, url, "Periodo de la Licencia",
            "¿Desea marcar como pagado el periodo de " + startDate + " al " + endDate + "?",
            function() {
                for (var i = 0, len = $scope.lstPeriods.length; i < len; i++) {
                    var period = $scope.lstPeriods[i];
                    if (period.LicensePeriodId === id) {
                        period.IsPayed = true;
                        break;
                    }
                }
            }, undefined, true);
    };

    $scope.initCatalog = function(lstCat, m, keyItem, keyId) {
        return sharedSvc.initCatalog(lstCat, m, keyItem, keyId);
    };

    $scope.initPeriods = function() {
        $scope.lstPeriods = JSON.parse($scope.m.lstPeriodsTx);
    };

    $scope.deletePeriod = function (id, divScope, urlToGo) {

        if (id > 0) {
            window.showObsolete(id, divScope, urlToGo, function() {
                $scope.doDeletePeriod(id);
            }, undefined, true);
        }
        else {
            $scope.doDeletePeriod(id); 
        }

    };

    $scope.doDeletePeriod = function(id) {
        for (var i = $scope.lstPeriods.length-1; i >= 0; i--) {
            var period = $scope.lstPeriods[i];
            if (period.LicensePeriodId === id)
                $scope.lstPeriods.splice(i, 1);
        }
    };

    $scope.addPeriods = function() {
        var lstPeriods = [];
        try {
            $scope.MsgErrorIn = "";
            var dtStr = $scope.m.StartDate.split(/\//);

            var dtStart = new Date(dtStr[2], dtStr[0]-1, dtStr[1], 0, 0, 0, 0);

            var dtEnd;
            for (var i = 0; i < $scope.m.PeriodNumber; i++) {

                if ($scope.m.PeriodTypesId !== 2) {
                    dtEnd = new Date(dtStart.getFullYear(), dtStart.getMonth() + 1, dtStart.getDate(), 0, 0, 0, 0);
                } else {
                    dtEnd = new Date(dtStart.getFullYear() + 1, dtStart.getMonth(), dtStart.getDate(), 0, 0, 0, 0);
                }

                lstPeriods.push({
                    LicensePeriodId: $scope.periodCount--,
                    IsPayed: false,
                    DateStartTx: window.formatDtA(dtStart),
                    DateEndTx: window.formatDtA(dtEnd),
                    ServerNumber: $scope.m.ServerNumber,
                    ClientNumber: $scope.m.ClientNumber,
                    Activation: 'NA',
                    InsDateTime: 'NA',
                    InsUserName: 'NA',
                    CatPeriodId: $scope.m.PeriodTypesId
                });

                dtStart = dtEnd;
            }

            if ($scope.lstPeriods === undefined) {
                $scope.lstPeriods = lstPeriods;
            } else {
                $scope.lstPeriods.pushArray(lstPeriods);
            }

        } catch(e) {
            $scope.setError("Hubo un error al agregar los periodos. Por favor revise que los datos sean correctos.");
        } 
    };
    
    $scope.setError = function(msg) {
        $scope.MsgErrorIn = msg;
        $timeout(function () {
            $scope.MsgErrorIn = "";
        }, 8000);
    };

    $scope.trySubmit = function (formId, urlToPost, divScope, typeReturn) {
        $scope.MsgErrorIn = "";
        var scope = angular.element($(divScope)).scope();
        var lstPeriods = [];

        if ($scope.lstPeriods !== undefined && $scope.lstPeriods.length > 0 ) {
            for (var i = 0, len = $scope.lstPeriods.length; i < len; i++) {
                var period = $scope.lstPeriods[i];
                if (period.LicensePeriodId <= 0) {
                    lstPeriods.push(period);
                }
            }
        }

        if (lstPeriods.length <= 0) {
            $scope.setError("No hay periodos que guardar");
            return;
        }

        $scope.m.LstPeriodsTx = JSON.stringify(lstPeriods);

        $timeout(function() {
            scope.submit(formId, urlToPost, typeReturn);
        }, 1);

    };

    $scope.$on("onSuccessRes", function(ev, res) {
        try {
            var lstPeriodsNew = JSON.parse(res.Data);
            var period, periodNew;
            var lstPeriods = $scope.lstPeriods;
            var lenP = lstPeriods.length;

            for (var i = 0, len = lstPeriodsNew.length; i<len; i++) {
                periodNew = lstPeriodsNew[i];

                for (var j = 0; j < lenP; j++) {
                    period = lstPeriods[j];
                    if (periodNew.LicensePeriodBeforeId === period.LicensePeriodId) {
                        period.LicensePeriodId = periodNew.LicensePeriodId;
                        period.InsDateTime = periodNew.InsDateTime;
                        period.InsUserName = periodNew.InsUserName;
                        break;
                    }
                }
            }
        } catch(e) {
                 
        }
    });

});

