$(function () {
    //var $;
    var url_Start = 'http://localhost:1371';
    debugger;
    /*
    if (window.jQuery === undefined || window.jQuery.fn.jquery !== '1.10.2') {
        var script_tag = document.createElement('script');
        script_tag.setAttribute("type", "text/javascript");
        script_tag.setAttribute("src", url_Start + "/Content/Widget/jquery-1.10.2.js");

        if (script_tag.readyState) {
            script_tag.onreadystatechange = function () { // For old versions of IE
                if (this.readyState == 'complete' || this.readyState == 'loaded') {
                    scriptLoadHandler();
                }
            };
        } else {
            script_tag.onload = scriptLoadHandler;
        }
        (document.getElementsByTagName("head")[0] || document.documentElement).appendChild(script_tag);
    } else {
        debugger;
        $ = window.jQuery;
        main();
    }

    function scriptLoadHandler() {
        $ = window.jQuery.noConflict(true);
        if (!jQuery().carouFredSel) {
            var script_cycle = document.createElement('script');
            script_cycle.setAttribute("type", "text/javascript");
            script_cycle.setAttribute("src", url_Start + "/Content/Widget/jquery.carouFredSel-6.2.1-packed.js");
            (document.getElementsByTagName("head")[0] || document.documentElement).appendChild(script_cycle);
        }
        main();
    }
*/
    main();
    function main() {
        debugger;
        document.createElement('x-sandiegouniontribune-search');
        document.createElement('x-sandiegouniontribune-featured-agent');
        document.createElement('x-sandiegouniontribune-featured-cars');
        document.createElement('x-sandiegouniontribune-classified-cars');

        var url = '';
        var data = '';


        if ($('x-sandiegouniontribune-search').length != 0) {
            url = url_Start + '/WidgetApi/SearchHome';
            data = {};

            $.ajax({
                type: 'GET',
                url: url,
                contentType: 'text/plain',
                xhrFields: {
                    // The 'xhrFields' property sets additional fields on the XMLHttpRequest.
                    // This can be used to set the 'withCredentials' property.
                    // Set the value to 'true' if you'd like to pass cookies to the server.
                    // If this is enabled, your server must respond with the header
                    // 'Access-Control-Allow-Credentials: true'.
                    withCredentials: false
                },

                headers: {
                },

                success: function (data) {
                    $('x-sandiegouniontribune-search').html(data);
                },

                error: function () {
                }
            });
        }

        if ($('x-sandiegouniontribune-featured-agent').length != 0) {
            var count = $('x-sandiegouniontribune-featured-agent').attr('data-count');
            var type = $('x-sandiegouniontribune-featured-agent').attr('data-type');
            url = url_Start + '/WidgetApi/GetAllFeaturedAgent?count=' + count + '&type=' + type;
            data = {};
            //jQuery('x-sandiegouniontribune-featured-agent').load(url, data, function (response, status, xhr) { });

            $.ajax({
                type: 'GET',
                url: url,
                contentType: 'text/plain',
                xhrFields: {
                    // The 'xhrFields' property sets additional fields on the XMLHttpRequest.
                    // This can be used to set the 'withCredentials' property.
                    // Set the value to 'true' if you'd like to pass cookies to the server.
                    // If this is enabled, your server must respond with the header
                    // 'Access-Control-Allow-Credentials: true'.
                    withCredentials: false
                },

                headers: {
                },

                success: function (data) {
                    $('x-sandiegouniontribune-featured-agent').html(data);

                },

                error: function () {
                }
            });
        }


        if ($('x-sandiegouniontribune-featured-cars').length != 0) {
            var count = $('x-sandiegouniontribune-featured-cars').attr('data-count');
            var type = $('x-sandiegouniontribune-featured-cars').attr('data-type');
            url = url_Start + '/WidgetApi/GetAllFeaturedCars?count=' + count + '&type=' + type;
            data = {};
            //jQuery('x-sandiegouniontribune-featured-agent').load(url, data, function (response, status, xhr) { });

            $.ajax({
                type: 'GET',
                url: url,
                contentType: 'text/plain',
                xhrFields: {
                    // The 'xhrFields' property sets additional fields on the XMLHttpRequest.
                    // This can be used to set the 'withCredentials' property.
                    // Set the value to 'true' if you'd like to pass cookies to the server.
                    // If this is enabled, your server must respond with the header
                    // 'Access-Control-Allow-Credentials: true'.
                    withCredentials: false
                },

                headers: {
                },

                success: function (data) {
                    //console.log(data);
                    $('x-sandiegouniontribune-featured-cars').html(data);

                },

                error: function () {
                }
            });
        }

        if ($('x-sandiegouniontribune-classified-cars').length != 0) {
            var count = $('x-sandiegouniontribune-classified-cars').attr('data-count');
            var type = $('x-sandiegouniontribune-classified-cars').attr('data-type');
            url = url_Start + '/WidgetApi/GetAllClassifiedCars?count=' + count + '&type=' + type;
            data = {};
            //jQuery('x-sandiegouniontribune-featured-agent').load(url, data, function (response, status, xhr) { });

            $.ajax({
                type: 'GET',
                url: url,
                contentType: 'text/plain',
                xhrFields: {
                    // The 'xhrFields' property sets additional fields on the XMLHttpRequest.
                    // This can be used to set the 'withCredentials' property.
                    // Set the value to 'true' if you'd like to pass cookies to the server.
                    // If this is enabled, your server must respond with the header
                    // 'Access-Control-Allow-Credentials: true'.
                    withCredentials: false
                },

                headers: {
                },

                success: function (data) {
                    console.log(data);
                    $('x-sandiegouniontribune-classified-cars').html(data);

                },

                error: function () {
                }
            });
        }

    } //end main
});