var CookieManager = (function() {
    'use strict';

    function _setCookieValue(key) {
        $.cookie(key, $('#' + key).val());
    }

    function _getCookieValue(key) {
        return $.cookie(key);
    }

    function _bindCookieValue(key) {
        $('#' + key).val($.cookie(key));
    }

    return {
        SetCookieValue: _setCookieValue,
        GetCookieValue: _getCookieValue,
        BindCookieValue: _bindCookieValue
    }
})();