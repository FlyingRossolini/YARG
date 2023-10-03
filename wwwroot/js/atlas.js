const phTrace = {
    type: "indicator",
    mode: "number+gauge+delta",
    value: 5.7,
    number: { "font": { "size": 21 } },
    delta: { reference: 5.6 },
    domain: { x: [0.10, 1], y: [0.85, 0.95] },
    title: {
        text: "pH",
        font: {
            family: 'Open Sans',
            color: '#7f7f7f',
            size: 15
        },
    },
    gauge: {
        shape: "bullet",
        axis: {
            range: [null, 14],
            tickfont: {
                family: 'Open Sans',
                color: '#7f7f7f'
            }
        },
        steps: [
            { range: [0, 2.8], color: "#dc3912" },
            { range: [2.8, 5.6], color: "#d9fd07" },
            { range: [5.6, 8.4], color: "#109618" },
            { range: [8.4, 11.2], color: "#3366cc" },
            { range: [11.2, 14], color: "#990099" }
        ],
        bar: { color: "black" }
    }
};
const ecTrace = {
    type: "indicator",
    mode: "number+gauge+delta",
    value: 1015,
    number: { "font": { "size": 21 } },
    delta: { reference: 1066 },
    domain: { x: [0.10, 1], y: [0.60, 0.70] },
    title: {
        text: "EC",
        font: {
            family: 'Open Sans',
            color: '#7f7f7f',
            size: 15
        },
    },
    gauge: {
        shape: "bullet",
        axis: {
            range: [null, 1500],
            tickfont: {
                family: 'Open Sans',
                color: '#7f7f7f'
            }
        },
        steps: [
            /*{ range: [0, 950], color: "#dc3912" },*/
            { range: [950, 1200], color: "#109618" },
            /*{ range: [1250, 1500], color: "#dc3912" }*/
        ],
        bar: { color: "black" }
    }
};
const orpTrace = {
    type: "indicator",
    mode: "number+gauge+delta",
    value: 373,
    number: { "font": { "size": 21 } },
    delta: { reference: 387 },
    domain: { x: [0.10, 1], y: [0.1, 0.2] },
    title: {
        text: "ORP",
        font: {
            family: 'Open Sans',
            color: '#7f7f7f',
            size: 15
        },
    },
    gauge: {
        shape: "bullet",
        axis: {
            range: [-200, 600],
            tickfont: {
                family: 'Open Sans',
                color: '#7f7f7f'
            }
        },
        steps: [
            /*{ range: [0, 950], color: "#dc3912" },*/
            { range: [250, 400], color: "#109618" },
            /*{ range: [1250, 1500], color: "#dc3912" }*/
        ],
        bar: { color: "black" }
    }
};
const doTrace = {
    type: "indicator",
    mode: "number+gauge+delta",
    value: 7.3,
    number: { "font": { "size": 21 } },
    delta: { reference: 7.3 },
    domain: { x: [0.10, 1], y: [0.35, 0.45] },
    title: {
        text: "DO",
        font: {
            family: 'Open Sans',
            color: '#7f7f7f',
            size: 15
        },
    },
    gauge: {
        shape: "bullet",
        axis: {
            range: [null, 20],
            tickfont: {
                family: 'Open Sans',
                color: '#7f7f7f'
            }
        },
        steps: [
            /*{ range: [0, 950], color: "#dc3912" },*/
            { range: [6, 8], color: "#109618" },
            /*{ range: [1250, 1500], color: "#dc3912" }*/
        ],
        bar: { color: "black" }
    },
    hoverformat: '.2f XYZ',
};

const data = [phTrace, ecTrace, doTrace, orpTrace];
const layout = {
    height: 230,
    margin: { t: 10, r: 0, l: 0, b: 10 },
    grid: { rows: 4, columns: 1 },
};


//var layout = { width: 600, height: 450, margin: { t: 0, b: 0 } };
//       var layout = { margin: { t: 10, r: 25, l: 25, b: 10 } };
var defaultPlotlyConfiguration = {
    modeBarButtonsToRemove: ['toImage','toggleSpikelines', 'resetScale2d', 'sendDataToCloud', 'pan2d', 'zoomIn2d', 'editInChartStudio', 'zoomOut2d', 'autoScale2d', 'zoom2d', 'hoverClosestCartesian', 'hoverCompareCartesian', 'lasso2d', 'select2d'], displaylogo: false, showTips: true, responsive: true
};

Plotly.newPlot('phDiv', data, layout, defaultPlotlyConfiguration);
