var TaskSimulator = (function () {
    'use strict';

    var _intervalListeners = [];

    function _postRequest(url, data, successCallback) {
        var startTime = new Date().getTime();

        $.ajax({
            type: 'POST',
            url: url,
            data: data,
            success: function (res, status, xhr) {
                var responseTime = new Date().getTime() - startTime;

                var response = {
                    Id: data.Id,
                    Complexity: data.EndValue,
                    Iterations: data.NumberOfIterations,
                    ExecutionTime: xhr.getResponseHeader('Execution-Time'),
                    ResponseTime: responseTime
                };

                successCallback(response);
            },
            async: true
        });
    }

    function _clearIntervalListener(intervalListener) {
        window.clearInterval(intervalListener);
        _intervalListeners = _intervalListeners.filter(function (el) { return el != intervalListener; });
    }

    function _startSimulation(requestOptions, onSimulationCompleted) {
        var requestNumber = 0;

        var intervalListener = setInterval(function () {
            if (++requestNumber > requestOptions.Count) {
                _clearIntervalListener(intervalListener);

                if (!_intervalListeners.length) {
                    onSimulationCompleted();
                }

                return;
            }

            var requestData = {
                Id: requestNumber,
                StartValue: 0,
                EndValue: requestOptions.Complexity,
                NumberOfIterations: requestOptions.Iterations
            };

            _postRequest(requestOptions.BaseUrl, requestData, requestOptions.SuccessCallback);
        }, requestOptions.Interval);

        _intervalListeners.push(intervalListener);
    }

    function _stopSimulations() {
        for (var i = 0; i < _intervalListeners.length; i++) {
            window.clearInterval(_intervalListeners[i]);
        }

        _intervalListeners = [];
    }

    return {
        StartSimulation: _startSimulation,
        StopSimulations: _stopSimulations
    }
})();