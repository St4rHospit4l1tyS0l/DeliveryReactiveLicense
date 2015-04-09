app.controller('upsertController', function ($scope, $rootScope, $timeout) {
    $scope.WaitFor = false;
    $scope.MsgError = "";
    $scope.Model = {};

    $scope.setError = function (msg) {
        $scope.MsgError = msg;
        $timeout(function () {
            $scope.MsgError = "";
        }, 8000);
    };

    $scope.setSuccess = function (msg) {
        $scope.MsgSuccess = msg;
        $timeout(function () {
            $scope.MsgSuccess = "";
        }, 8000);
    };

    $scope.submit = function (formId, urlToPost, typeReturn) {
        $scope.MsgError = "";
        $scope.MsgSuccess = "";

        if ($(formId).valid() == false) {
            $scope.Invalid = true;
            return false;
        }
        $scope.WaitFor = true;

        switch (typeReturn) {
            case 1:
                $.post(urlToPost, $(formId).serialize())
                    .success($scope.handleSuccessWithId)
                    .error($scope.handleError);
                break;
            case 2:
                $.post(urlToPost, $(formId).serialize())
                    .success($scope.handleSuccessWithOutClose)
                    .error($scope.handleError);
                break;
            default:
                $.post(urlToPost, $(formId).serialize())
                    .success($scope.handleSuccess)
                .error($scope.handleError);
                break;
        }

        return true;
    };


    $scope.handleSuccessWithOutClose = function (res) {
        $scope.WaitFor = false;

        try {
            if (res.HasError === false) {
                $scope.setSuccess(res.Message);
                $rootScope.$broadcast("onSuccessRes", res);
                return;
            }

            $scope.setError(res.Message);
            $scope.$apply();

        } catch (e) {
            $scope.setError("Error inesperado de datos. Por favor intente más tarde.");
        }
    };

    $scope.handleSuccessWithId = function (resp) {
        $scope.WaitFor = false;

        try {
            if (resp.HasError === false) {
                $rootScope.$broadcast("onLastId", resp.Id);
                $scope.Model.dlg.modal('hide');
                $scope.Model.def.resolve({ isCancel: false });
                return;
            }

            $scope.setError(resp.Message);
            $scope.$apply();

        } catch (e) {
            $scope.setError("Error inesperado de datos. Por favor intente más tarde.");
        }
    };


    $scope.handleSuccess = function (resp) {
        $scope.WaitFor = false;

        try {
            if (resp.HasError === false) {
                $scope.Model.dlg.modal('hide');
                $scope.Model.def.resolve({ isCancel: false });
                return;
            }

            $scope.setError(resp.Message);
            $scope.$apply();

        } catch (e) {
            $scope.setError("Error inesperado de datos. Por favor intente más tarde.");
        }
    };

    $scope.handleError = function () {
        $scope.WaitFor = false;
        $scope.setError("Error de red. Por favor intente más tarde.");
        $scope.$apply();
    };

    $scope.cancel = function () {
        $scope.Model.dlg.modal('hide');
        $scope.Model.def.reject({ isCancel: true });
    };

    $scope.setDlg = function (dlg, urlToSubmit) {
        $scope.Model.dlg = dlg;
        $scope.Model.url = urlToSubmit;

        dlg.on('hidden.bs.modal', function () {
            dlg.data('modal', null);
            dlg.replaceWith("");
        });
    };
});
