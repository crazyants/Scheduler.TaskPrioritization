var EventBinder = (function () {
    'use strict';

    function _setApiControllerName(value) {
        $('#startSimulationBtn').attr('api-controller', value);
    }

    function _removeAllTaskResultElements() {
        var elements = document.getElementsByClassName('requestResult');

        while (elements[0]) {
            elements[0].parentNode.removeChild(elements[0]);
        };
    }

    function _toggleButtons() {
        $('#startSimulationBtn').parent().toggle();
        $('#stopSimulationsBtn').toggle();
    }

    function _removeAverageTimeElements() {
        $('#realtimeRequestsAverageResponseTime').html('');
        $('#realtimeRequestsAverageExecutionTime').html('');

        $('#batchRequestsAverageResponseTime').html('');
        $('#batchRequestsAverageExecutionTime').html('');
    }

    function _displayResult(response, parentElementSelector) {
        var template = $('#tableRowTemplate').clone();

        var element = $(template.html());
        element.children('.requestNumber').html(response.Id);
        element.children('.requestComplexity').html(response.Complexity);
        element.children('.requestIterations').html(response.Iterations);
        element.children('.requestResponseTime').html(response.ResponseTime);
        element.children('.requestExecutionTime').html(response.ExecutionTime);

        $(parentElementSelector).append(element);
    }

    function _onRealtimeRequestExecuted(response) {
        _displayResult(response, '#realtimeRequestsTable');
        _displayAverageTime('#realtimeRequestsTable', '#realtimeRequestsAverageExecutionTime', ' .requestExecutionTime', 'Average calculation time: ');
        _displayAverageTime('#realtimeRequestsTable', '#realtimeRequestsAverageResponseTime', ' .requestResponseTime', 'Average response time: ');
    }

    function _onBatchRequestExecuted(response) {
        _displayResult(response, '#batchRequestsTable');
        _displayAverageTime('#batchRequestsTable', '#batchRequestsAverageExecutionTime', ' .requestExecutionTime', 'Average calculation time: ');
        _displayAverageTime('#batchRequestsTable', '#batchRequestsAverageResponseTime', ' .requestResponseTime', 'Average response time: ');
    }

    function _displayAverageTime(parentElementSelector, destinationElementSelector, className, message) {
        var elements = $(parentElementSelector + className);

        var sum = 0;
        for (var i = 0; i < elements.length; i++) {
            sum += parseInt(elements[i].innerHTML, 10);
        }

        var averageResponseTime = parseInt(sum / elements.length, 10);
        $(destinationElementSelector).html(message + averageResponseTime + ' ms');
    }

    function _getApiControllerName() {
        return $('#startSimulationBtn').attr('api-controller');
    }

    function _getRealtimeRequestOptions() {
        var realtimeRequestOptions = {
            BaseUrl: CookieManager.GetCookieValue('requestUrl') + '/' + _getApiControllerName() + '/realtime',
            Count: CookieManager.GetCookieValue('numberOfRealtimeRequests'),
            Interval: CookieManager.GetCookieValue('intervalBetweenRealtimeRequests'),
            Complexity: CookieManager.GetCookieValue('realtimeRequestComplexity'),
            Iterations: CookieManager.GetCookieValue('realtimeRequestIterations'),
            SuccessCallback: _onRealtimeRequestExecuted
        };

        return realtimeRequestOptions;
    }

    function _getBatchRequestOptions() {
        var batchRequestOptions = {
            BaseUrl: CookieManager.GetCookieValue('requestUrl') + '/' + _getApiControllerName() + '/batch',
            Count: CookieManager.GetCookieValue('numberOfBatchRequests'),
            Interval: CookieManager.GetCookieValue('intervalBetweenBatchRequests'),
            Complexity: CookieManager.GetCookieValue('batchRequestComplexity'),
            Iterations: CookieManager.GetCookieValue('batchRequestIterations'),
            SuccessCallback: _onBatchRequestExecuted
        };

        return batchRequestOptions;
    }

    function _bindEvents() {
        $('#startSimulationBtn').click(function () {
            _removeAllTaskResultElements();
            _removeAverageTimeElements();

            var realtimeRequestOptions = _getRealtimeRequestOptions();
            var batchRequestOptions = _getBatchRequestOptions();

            TaskSimulator.StartSimulation(realtimeRequestOptions, _toggleButtons);
            TaskSimulator.StartSimulation(batchRequestOptions, _toggleButtons);

            _toggleButtons();
        });

        $('#startRealtimeRequestsSimulationBtn').click(function () {
            _removeAllTaskResultElements();
            _removeAverageTimeElements();
            
            var realtimeRequestOptions = _getRealtimeRequestOptions();
            TaskSimulator.StartSimulation(realtimeRequestOptions, _toggleButtons);

            _toggleButtons();
        });

        $('#startBatchRequestsSimulationBtn').click(function () {
            _removeAllTaskResultElements();
            _removeAverageTimeElements();

            var batchRequestOptions = _getBatchRequestOptions();
            TaskSimulator.StartSimulation(batchRequestOptions, _toggleButtons);

            _toggleButtons();
        });

        $('#stopSimulationsBtn').click(function () {
            TaskSimulator.StopSimulations();
            _toggleButtons();
        });
    }

    return {
        SetApiControllerName: _setApiControllerName,
        BindEvents: _bindEvents
    }
})();