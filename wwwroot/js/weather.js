document.addEventListener("DOMContentLoaded", function () {
    var weatherMappings = {
        "Sunny": {
            iconClass: "fa-sun",
            colorClass: "sunny-color"
        },
        "Chance of showers": {
            iconClass: "fa-cloud-showers-heavy",
            colorClass: "showers-color"
        },
        "Flooding": {
            iconClass: "fa-water",
            colorClass: "flooding-color"
        },
        "Cloudy": {
            iconClass: "fa-cloud",
            colorClass: "cloudy-color"
        },
        "Nighttime": {
            iconClass: "fa-bed",
            colorClass: "nighttime-color"
        },
        // Add more mappings for other weather conditions
    };

    var icons = document.querySelectorAll(".weather-icon");

    icons.forEach(function (icon) {
        var weatherText = icon.getAttribute("data-weather");
        var mapping = weatherMappings[weatherText] || { iconClass: "fa-2x", colorClass: "" };
        icon.classList.add(mapping.iconClass, mapping.colorClass);
    });
});