app.directive('goClick', function () {
    return function (scope, element, attrs) {
        var path;

        attrs.$observe('goClick', function (val) {
            path = val;
        });

        element.bind('click', function () {
            try {
                window.location.replace(path);
            } catch (e) {
                window.location = path;
            }
        });
    };
});