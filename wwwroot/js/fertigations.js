
// Replaced with 'Google Charts Color' array found at:
// https://gist.github.com/mikebmou/1323655

const D3Colors = [
    "#3366cc",
    "#dc3912",
    "#ff9900",
    "#109618",
    "#990099",
    "#0099c6",
    "#dd4477",
    "#66aa00",
    "#b82e2e",
    "#316395",
    "#3366cc",
    "#994499",
    "#22aa99",
    "#aaaa11",
    "#6633cc",
    "#e67300",
    "#8b0707",
    "#651067",
    "#329262",
    "#5574a6",
    "#3b3eac",
    "#b77322",
    "#16d620",
    "#b91383",
    "#f4359e",
    "#9c5935",
    "#a9c413",
    "#2a778d",
    "#668d1c",
    "#bea413",
    "#0c5922",
    "#743411"
];

var dataContainer = document.getElementById('fertigation-container');
var ws = JSON.parse(dataContainer.getAttribute('data-ws'));
var p = JSON.parse(dataContainer.getAttribute('data-pots'));
var potCount = JSON.parse(dataContainer.getAttribute('data-potCount'));
var sunr = JSON.parse(dataContainer.getAttribute('data-sunrise'));
var suns = JSON.parse(dataContainer.getAttribute('data-sunset'));
var flgMorning = JSON.parse(dataContainer.getAttribute('data-flgMorning'));
var flgEvening = JSON.parse(dataContainer.getAttribute('data-flgEvening'));

var localizerReservoir = dataContainer.getAttribute('data-localizer_Reservoir');
var localizerSunset = dataContainer.getAttribute('data-localizer_Sunset');
var localizerSunrise = dataContainer.getAttribute('data-localizer_Sunrise');
var localizerNow = dataContainer.getAttribute('data-localizer_Now');
var localizertickformat = dataContainer.getAttribute('data-localizer_tickformat');

var traces = [];
var myshapes = [];
var myticktexts = [];
var mytickvals = [];
var myrange = [];

myrange.push(-1);
myrange.push(potCount + 1);

myticktexts.push(localizerReservoir);
mytickvals.push(0);


for (var i = 0; i < ws.length; i++) {
    var xtrace = [];
    var ytrace = [];

    // add res plot to morning if not splashing awake
    if (i === 0 && flgMorning == false) {
        xtrace.push(sunr);
        xtrace.push(ws[i].efStartTime);
        ytrace.push(0);
        ytrace.push(0);

        var sunrisetrace = {
            name: '',
            x: xtrace,
            y: ytrace,
            hoverinfo: '',
            marker: { color: 'white' }
        }

        traces.push(sunrisetrace);

        var sunriseshape = {
            x0: sunr,
            x1: ws[i].efStartTime,
            y0: -0.35,
            y1: 0.35,
            line: { width: 0 },
            type: 'rect',
            xref: 'x',
            yref: 'y',

            opacity: 1,
            fillcolor: D3Colors[0]
        }

        myshapes.push(sunriseshape);
    }
    // add res plots (the end of the previous -> start of the current)
    if (i != ws.length - 1) {
        xtrace.push(ws[i].efEndTime);
        xtrace.push(ws[i + 1].efStartTime);
        ytrace.push(0);
        ytrace.push(0);

        var restrace = {
            name: '',
            x: xtrace,
            y: ytrace,
            marker: { color: 'white' }
        }

        traces.push(restrace);

        var resshape = {
            x0: ws[i].efEndTime,
            x1: ws[i + 1].efStartTime,
            y0: -0.35,
            y1: 0.35,
            line: { width: 0 },
            type: 'rect',
            xref: 'x',
            yref: 'y',
            opacity: 1,
            fillcolor: D3Colors[0]
        }

        myshapes.push(resshape);

    }
    //add res plot to evening if not splashing asleep
    if (i === ws.length - 1 && flgEvening == false) {
        xtrace.push(ws[i].efEndTime);
        xtrace.push(suns);
        ytrace.push(0);
        ytrace.push(0);

        var restrace = {
            name: '',
            x: xtrace,
            y: ytrace,
            marker: { color: 'white' }
        }

        traces.push(restrace);

        var resshape = {
            x0: ws[i].efEndTime,
            x1: suns,
            y0: -0.35,
            y1: 0.35,
            line: { width: 0 },
            type: 'rect',
            xref: 'x',
            yref: 'y',
            opacity: 1,
            fillcolor: D3Colors[0]
        }

        myshapes.push(resshape);

    }

    // add normal water schedule plots
    xtrace.push(ws[i].efStartTime);
    xtrace.push(ws[i].efEndTime);
    ytrace.push(ws[i].potQueuePosition);
    ytrace.push(ws[i].potQueuePosition);

    var basetrace = {
        name: '',
        x: xtrace,
        y: ytrace,
        hoverinfo: "x",
        marker: { color: 'white' }
    };

    traces.push(basetrace);

    var shape = {
        x0: ws[i].efStartTime,
        x1: ws[i].efEndTime,
        y0: ws[i].potQueuePosition - 0.35,
        y1: ws[i].potQueuePosition + 0.35,
        line: { width: 0 },
        type: 'rect',
        xref: 'x',
        yref: 'y',
        opacity: 1,
        fillcolor: D3Colors[ws[i].potQueuePosition]
    };

    myshapes.push(shape);

}

//add 'right now' indicator
var datetime = moment().format('YYYY-MM-DD HH:mm:00');
var datetime2 = moment(datetime).add(-1, 'minutes').format('YYYY-MM-DD HH:mm:00');

if (moment().isAfter(sunr) && moment().isBefore(suns)) {
    var todayshape = {
        type: 'rect',
        x0: datetime2,
        x1: datetime,
        y0: 0.05,
        y1: 1,
        line: { width: 0 },
        xref: 'x',
        yref: 'paper',
        opacity: 0.9,
        fillcolor: D3Colors[D3Colors.length - 1]
    };

    myshapes.push(todayshape);

}


// add sunrise indicator
var sunriseind = moment(sunr).add(-1, 'minutes').format('YYYY-MM-DD HH:mm:00');
var sunriseindshape = {
    type: 'rect',
    x0: sunriseind,
    x1: sunr,
    y0: 0.1,
    y1: 1,
    line: { width: 0 },
    xref: 'x',
    yref: 'paper',
    opacity: 0.9,
    fillcolor: D3Colors[D3Colors.length - 1]
};

myshapes.push(sunriseindshape);

// add sunset indicator
var sunsetind = moment(suns).add(-1, 'minutes').format('YYYY-MM-DD HH:mm:00');
var sunsetindshape = {
    type: 'rect',
    x0: sunsetind,
    x1: suns,
    y0: 0.1,
    y1: 1,
    line: { width: 0 },
    xref: 'x',
    yref: 'paper',
    opacity: 0.9,
    fillcolor: D3Colors[D3Colors.length - 1]
};

myshapes.push(sunsetindshape);
if (moment().isAfter(sunr) && moment().isBefore(suns)) {
    var srsstext = {
        x: [sunr, datetime,
            suns],
        y: [-0.9, -0.9, -0.9],
        mode: 'text',
        text: [localizerSunrise, localizerNow, localizerSunset],
        hoverinfo: ["x", "x", "x"],
        showlegend: false
    };
} else {
    var srsstext = {
        x: [sunr, suns],
        y: [-0.9, -0.9],
        mode: 'text',
        text: [localizerSunrise, localizerSunset],
        hoverinfo: ["x", "x"],
        showlegend: false
    };
}

traces.push(srsstext);

for (var j = 0; j < p.length; j++) {
    myticktexts.push(p[j].name);
    mytickvals.push(p[j].queuePosition);
};

data = traces,
    layout = {
        xaxis: {
            type: 'date',
            showgrid: false,
            zeroline: true,
            // tickformat: '%-I:%M %p'
            tickformat: localizertickformat //'%-d-%b %-I:%M %p'
        },
        font: {
            family: 'Open Sans',
            color: '#7f7f7f'
        },
        yaxis: {
            range: myrange,
            showgrid: true,
            ticktext: myticktexts,
            tickvals: mytickvals,
            zeroline: false,
            autorange: false
        },
        height: 320,
        shapes: myshapes,
        hovermode: 'closest',
        showlegend: false,
        margin: {
            l: 60,
            r: 0,
            b: 30,
            t: 8,
            pad: 1
        },
        hoverlabel: {
            bgcolor: 'black',
            font: { color: 'white', family: 'Open Sans' }

        }

    };

var defaultPlotlyConfiguration = {
    modeBarButtonsToRemove: ['toImage','toggleSpikelines', 'resetScale2d', 'sendDataToCloud', 'pan2d', 'zoomIn2d', 'editInChartStudio', 'zoomOut2d', 'autoScale2d', 'zoomIn2d', 'hoverClosestCartesian', 'hoverCompareCartesian', 'lasso2d', 'select2d'], displaylogo: false, showTips: true, responsive: true
};


Plotly.newPlot('fertigation-container', data, layout, defaultPlotlyConfiguration);
