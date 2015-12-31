
$(function () {
    debugger;

    setTimeout(loadCarouselagent, 1000);
    setTimeout(loadCarouselcar, 1000);
    //setTimeout(loadCarouselclassifiedCar, 1000);
    setTimeout(loadCarouselagenthorizon, 1000);
    setTimeout(loadCarouselcarhorizon, 1000);
    //setTimeout(loadCarouselclassifiedCarHorizon, 1000);

    function loadCarouselagent() {
        if ($('#ntooitive-slider-agent').length != 0) {
            $('#ntooitive-slider-agent').carouFredSel({
                auto: false,
                height: 'auto',
                prev: '#ntooitive-prev-agent',
                next: '#ntooitive-next-agent',
                mousewheel: true,
                responsive: true
            })
        }
    }
    function loadCarouselagenthorizon() {
        if ($('#ntooitive-slider-agent-horizon').length != 0) {
            $('#ntooitive-slider-agent-horizon').carouFredSel({
                auto: false,
                height: 'auto',
                prev: '#ntooitive-prev-agent-horizon',
                next: '#ntooitive-next-agent-horizon',
                mousewheel: true,
                responsive: true
            })
        }
    }

    function loadCarouselcar() {
        if ($('#ntooitive-slider-car').length != 0) {
            $('#ntooitive-slider-car').carouFredSel({
                auto: false,
                height: 'auto',
                prev: '#ntooitive-prev-car',
                next: '#ntooitive-next-car',
                mousewheel: true,
                responsive: true
            })
        }
    }

    function loadCarouselcarhorizon() {
        if ($('#ntooitive-slider-car-horizon').length != 0) {
            $('#ntooitive-slider-car-horizon').carouFredSel({
                auto: false,
                height: 'auto',
                prev: '#ntooitive-prev-car-horizon',
                next: '#ntooitive-next-car-horizon',
                mousewheel: true,
                responsive: true
            })
        }
    }
    /*
    function loadCarouselclassifiedCar() {
        if ($('#ntooitive-slider-classifiedCar').length != 0) {
            $('#ntooitive-slider-classifiedCar').carouFredSel({
                auto: false,
                height: 'auto',
                prev: '#ntooitive-prev-classifiedCar',
                next: '#ntooitive-next-classifiedCar',
                mousewheel: true,
                responsive: true
            })
        }
    }

    function loadCarouselclassifiedCarHorizon() {
        if ($('#ntooitive-slider-classifiedCar-horizon').length != 0) {
            $('#ntooitive-slider-classifiedCar-horizon').carouFredSel({
                auto: false,
                height: 'auto',
                prev: '#ntooitive-prev-classifiedCar-horizon',
                next: '#ntooitive-next-classifiedCar-horizon',
                mousewheel: true,
                responsive: true
            })
        }
    }*/

});