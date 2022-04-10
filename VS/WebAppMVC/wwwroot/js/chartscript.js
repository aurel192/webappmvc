var seriesArray = [];

function initSeriesArray(arr) {
    //console.log('initSeriesArray', arr);
    seriesArray = [];
    if (Array.isArray(arr)) {
        //console.log('initSeriesArray if');
        arr.forEach(function (element) {
            seriesArray.push({
                points: [],//element.points,
                name: element.name,
                type: 'line',
                yAxisName: 'priceAxis',
                fill: element.color,
                enableAnimation: false,
                tooltip:
                {
                    visible: true,
                    format: " #series.name#  <br/> #point.x# : #point.y# "
                }
            });
        });
    }
    else {
        //console.log('initSeriesArray else');
        seriesArray =
            [
                { // [0] Price
                    points: [],
                    name: 'Price',
                    type: 'line',
                    yAxisName: 'priceAxis',
                    fill: "#000066",
                    enableAnimation: false,
                    tooltip:
                    {
                        visible: true,
                        format: " #series.name#  <br/> #point.x# : #point.y#   "
                    }
                },
                { /// [1] Capitalization
                    points: [],
                    name: 'Capitalization',
                    type: 'line',
                    yAxisName: 'capitalizationAxis',
                    fill: "#ff9933",
                    tooltip:
                    {
                        visible: true,
                        format: "#series.name#  <br/> #point.x# <br /> #point.y#"
                    }
                },
                { // [2] Volume
                    points: [],
                    name: 'Volume',
                    fill: "#007700",
                    type: 'column',
                    yAxisName: 'volumeAxis',
                    enableAnimation: false,
                    tooltip:
                    {
                        visible: true,
                        format: "#series.name# <br/> #point.x# <br /> #point.y#"
                    }
                }
            ];
    }
}


var seriesRSI = {// [3] RSI
    points: [],
    name: 'RSI',
    yAxisName: 'rsiAxis',
    type: 'line',
    fill: "#0099cc",
    tooltip:
    {
        visible: true,
        format: " #series.name#  <br/> #point.x# : #point.y#"
    }
};
var seriesMACDSignal = { // [4] MACD Signal
    points: [],
    name: 'MACD Signal',
    type: 'line',
    yAxisName: 'macdAxis',
    fill: "#0000BB",
    enableAnimation: false
};
var seriesMACD = { // [5]  MACD
    points: [],
    name: 'MACD',
    type: 'line',
    yAxisName: 'macdAxis',
    fill: "#BB0000",
    enableAnimation: false
};
var seriesMACDHist = { // [6] MACD Histogram
    points: [],
    name: 'MACD Histogram',
    fill: "#888888",
    type: 'column',
    yAxisName: 'macdAxisCol',
    enableAnimation: false
};

var seriesBBandsLower = { /// [7] BBands Lower
    points: [],
    name: 'BBands Lower',
    type: 'line',
    yAxisName: 'bblowerAxis',
    fill: "#e6ccff"
};

var seriesBBandsMiddle = { /// [8] BBands Middle
    points: [],
    name: 'BBands Middle',
    type: 'line',
    yAxisName: 'bbmiddleAxis',
    fill: "#ffb3ff"
};

var seriesBBandsUpper = { /// [9] BBands Upper
    points: [],
    name: 'BBands Upper',
    type: 'line',
    yAxisName: 'bbupperAxis',
    fill: "#e6ccff"
};

var seriesMA1 = { /// [10] MA 1
    points: [],
    name: 'MA 50',
    type: 'line',
    yAxisName: 'ma1Axis',
    fill: "#FF00FF"
}

var seriesMA2 = { /// [11] MA 2
    points: [],
    name: 'MA 200',
    type: 'line',
    yAxisName: 'ma2Axis',
    fill: "#007700"
};



// http://js.syncfusion.com/demos/web/#!/lime/chart/rangecolumn
// http://jsplayground.syncfusion.com/o3l2dgke

var titleVisible = true;
var legendVisible = false;

var VolumeAxis = {
    orientation: 'Vertical',
    hidePartialLabels: false,
    rowIndex: 2,
    plotOffset: 0,
    //range: { min: 0, max: 100 },// interval: 0.01
    majorGridLines: { visible: true },
    axisLine: { visible: false },
    name: 'volumeAxis',
    labelFormat: '{value}',
    labelPosition: "inside",
    tickLinesPosition: "Inside",
    title: { text: "Volume", visible: titleVisible, position: 'Inside' },
    visible: false,
    opposedPosition: false
};

var PriceAxis = {
    orientation: 'Vertical',
    hidePartialLabels: false,
    rowIndex: 3,
    plotOffset: 0,
    //range: { min: 0, max: 2000, interval: 100 },
    majorGridLines: { visible: true },
    axisLine: { visible: false },
    name: 'priceAxis',
    labelFormat: '{value}',
    labelPosition: "inside",
    tickLinesPosition: "Inside",
    title: { text: "Price", visible: titleVisible, position: 'Inside' },
    opposedPosition: false
};

var CapitalizationAxis = {
    majorGridLines: { visible: false },
    orientation: 'Vertical',
    rowIndex: 3,
    opposedPosition: true,
    axisLine: { visible: false },
    //range: { min: 0, max: 5000, interval: 500 },
    name: 'capitalizationAxis',
    labelFormat: '{value} M',
    labelPosition: "inside",
    tickLinesPosition: "Inside",
    title: { text: "Capitalization (Millions)", visible: titleVisible, position: 'Inside' }
};

var BBlowerAxis = {
    majorGridLines: { visible: false },
    orientation: 'Vertical',
    rowIndex: 3,
    opposedPosition: true,
    range: { min: -100, max: 100 },
    axisLine: { visible: false },
    name: 'bblowerAxis',
    font: { size: "0px" },
    labelPosition: "inside",
    majorTickLines: { visible: false }
};
var BBmiddleAxis = {
    majorGridLines: { visible: false },
    orientation: 'Vertical',
    rowIndex: 3,
    opposedPosition: true,
    range: { min: -100, max: 100 },
    axisLine: { visible: false },
    name: 'bbmiddleAxis',
    font: { size: "0px" },
    labelPosition: "inside",
    majorTickLines: { visible: false }
};
var BBupperAxis = {
    majorGridLines: { visible: false },
    orientation: 'Vertical',
    rowIndex: 3,
    opposedPosition: true,
    range: { min: -100, max: 100 },
    axisLine: { visible: false },
    name: 'bbupperAxis',
    font: { size: "0px" },
    labelPosition: "inside",
    majorTickLines: { visible: false }
};

var MA1Axis = {
    majorGridLines: { visible: false },
    orientation: 'Vertical',
    rowIndex: 3,
    opposedPosition: true,
    range: { min: -100, max: 100 },
    axisLine: { visible: false },
    name: 'ma1Axis',
    font: { size: "0px" },
    labelPosition: "inside",
    majorTickLines: { visible: false }
};
var MA2Axis = {
    majorGridLines: { visible: false },
    orientation: 'Vertical',
    rowIndex: 3,
    opposedPosition: true,
    range: { min: -100, max: 100 },
    axisLine: { visible: false },
    name: 'ma2Axis',
    font: { size: "0px" },
    labelPosition: "inside",
    majorTickLines: { visible: false }
};

var MacdAxis = {
    orientation: 'Vertical',
    hidePartialLabels: false,
    rowIndex: 1,
    plotOffset: 0,
    range: { min: 0, max: 100 },// interval: 0.01
    majorGridLines: { visible: true },
    axisLine: { visible: false },
    name: 'macdAxis',
    title: { text: "MACD", visible: titleVisible, position: 'Inside' },
    majorTickLines: { visible: false },
    font: { size: "0px" },
    labelPosition: "inside",
    opposedPosition: false
};

var MacdAxisCol = {
    orientation: 'Vertical',
    hidePartialLabels: false,
    rowIndex: 1,
    plotOffset: 0,
    range: { min: -100, max: 100 }, // interval: 0.01
    majorGridLines: { visible: true },
    axisLine: { visible: false },
    name: 'macdAxisCol',
    title: { text: "MACD", visible: titleVisible },
    labelPosition: "inside",
    majorTickLines: { visible: false },
    font: { size: "0px" },
    visible: false,
    opposedPosition: true
};

var RsiAxis = {
    range: { min: 0, max: 100 },
    maximumLabels: '0.1',
    labelPosition: "inside",
    font: { size: "0px" },
    majorTickLines: { visible: false },
    axisLine: { visible: false },
    title: { text: "RSI", visible: titleVisible, position: 'Inside' },
    name: 'rsiAxis',
    rowIndex: 0,
    stripLine:
        [
            {
                start: 0,
                end: 29.5,
                color: '#FFFFFF',
                zIndex: 'behind',
                borderWidth: 0,
                visible: true
            },
            {
                start: 29.5,
                end: 30.5,
                color: '#bcbcbc',
                zIndex: 'behind',
                borderWidth: 0,
                visible: true
            },
            {
                start: 30.5,
                end: 69.5,
                color: '#FFFFFF',
                zIndex: 'behind',
                borderWidth: 0,
                visible: true
            },
            {
                start: 69.5,
                end: 70.5,
                color: '#bcbcbc',
                zIndex: 'behind',
                borderWidth: 0,
                visible: true
            },
            {
                start: 70.5,
                end: 100,
                color: '#FFFFFF',
                zIndex: 'behind',
                borderWidth: 0,
                visible: true
            }
        ]
};

var annotations = [];
var axes = [];
var rowDefinitions = [];

function initAxes(arg) {
    axes = [];
    rowDefinitions = [];
    switch (arg) {
        case 'price':
            PriceAxis.title.text = "Price";
            PriceAxis.labelFormat = '{value}';
            rowDefinitions.push({// Volume
                rowHeight: 13,
                unit: 'percentage'
            });
            rowDefinitions.push({// Price + Capitalization    
                rowHeight: 87,
                unit: 'percentage'
            });
            axes.push(VolumeAxis);
            axes.push(CapitalizationAxis);
            break;
        case 'pricemacdrsi': 
            // 13rsi,55macd,20,12
            PriceAxis.title.text = "Price";
            PriceAxis.labelFormat = '{value}';
            rowDefinitions.push({// RSI
                rowHeight: 15,
                unit: 'percentage'
            });
            rowDefinitions.push({// MACD
                rowHeight: 15,
                unit: 'percentage'
            });
            rowDefinitions.push({// VOLUME
                rowHeight: 5,
                lineColor: "gray",
                lineWidth: 1,
                unit: 'percentage'
            });
            rowDefinitions.push({// CHART
                rowHeight: 65,
                unit: 'percentage'
            });
            axes.push(MacdAxis);
            axes.push(MacdAxisCol);
            axes.push(RsiAxis);
            axes.push(VolumeAxis);
            axes.push(CapitalizationAxis);
            axes.push(BBlowerAxis);
            axes.push(BBmiddleAxis);
            axes.push(MA1Axis);
            axes.push(MA2Axis);
            //console.log('axes', axes);
            break;
        case 'compare':
            PriceAxis.title.text = "Percent";
            PriceAxis.labelFormat = '{value} %';
            break;
    }
}

function MultipleAxesChart(div, title, subtitle, macdrsi, height, width) {
    if (landscape) {
        width = '100%';
    }
    else {
        width = parseInt(realWidth * 1.23);
    }
    console.log('MultipleAxesChart height=' + height + ' realWidth=' + realWidth + ' width=' + width);
    $(div).ejChart(
        {
            rowDefinitions: rowDefinitions,
            //Initializing Primary X Axis
            primaryXAxis:
            {
                //title: { text: "Month" },
                labelRotation: 270,
                labelPlacement: "BetweenTicks",
                desiredIntervals: 15
            },
            //Initializing Primary Y Axis
            primaryYAxis: PriceAxis,
            axes: axes,
            annotations: annotations,
            //Initializing Series
            series: seriesArray,
            load: "loadTheme",
            canResize: true,
            //commonSeriesOptions: { trendlines: [{ visibility: 'visible', fill: '#9b02a3', name:'exponential'}] },
            //commonSeriesOptions :{trendlines:[{ fill:'#0000FF' }]} 
            //commonSeriesOptions: { enableAnimation: false },
            //trendlines: [{ visibility: 'visible' }]
            // type:'exponential'
            // https://help.syncfusion.com/api/js/ejchart#members
            title: {
                text: title,
                subTitle: {
                    text: subtitle,
                    font: { fontWeight: "lighter" }
                }
            },
            size: {
                height: height.toString(),
                width: width.toString() // '100%'//width.toString()
            },
            legend: { visible: legendVisible }
        });

    var chart = $('#chart' + cntr).ejChart();
    //console.log('chart', chart);
}