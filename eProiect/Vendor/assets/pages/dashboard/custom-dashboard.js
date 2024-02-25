'use strict';
$(document).ready(function() {
    $(document).ready(function() {
        areaChart();
    });
    /*Area chart*/
    function areaChart() {
        var chart = AmCharts.makeChart("statestics-chart", {
            "type": "serial",
            "marginTop": 0,
            "hideCredits": true,
            "marginRight": 0,
            "dataProvider": [{
                "year": "Jan",
                "value": 0.98
            }, {
                "year": "Feb",
                "value": 1.87
            }, {
                "year": "Mar",
                "value": 0.97
            }, {
                "year": "Apr",
                "value": 1.64
            }, {
                "year": "May",
                "value": 0.40
            }, {
                "year": "Jun",
                "value": 2.90
            }, {
                "year": "Jul",
                "value": 5.2
            }, {
                "year": "Aug",
                "value": 0.77
            }, {
                "year": "Sap",
                "value": 3.1
            }],
            "valueAxes": [{
                "axisAlpha": 0,
                "dashLength": 6,
                "gridAlpha": 0.1,
                "position": "left"
            }],
            "graphs": [{
                "id": "g1",
                "bullet": "round",
                "bulletSize": 9,
                "lineColor": "#4680ff",
                "lineThickness": 2,
                "negativeLineColor": "#4680ff",
                "type": "smoothedLine",
                "valueField": "value"
            }],
            "chartCursor": {
                "cursorAlpha": 0,
                "valueLineEnabled": false,
                "valueLineBalloonEnabled": true,
                "valueLineAlpha": false,
                "color": '#fff',
                "cursorColor": '#FC6180',
                "fullWidth": true
            },
            "categoryField": "year",
            "categoryAxis": {
                "gridAlpha": 0,
                "axisAlpha": 0,
                // "fillAlpha": 1,
                // "fillColor": "#FAFAFA",
                // "minorGridAlpha": 0,
                // "minorGridEnabled": true
            },
            "export": {
                "enabled": true
            }
        });
        /*donut chart*/


        // website Traffic
        // Email sent start
        var chart = AmCharts.makeChart("email-sent", {
            "type": "serial",
            "theme": "light",
            "hideCredits": true,
            "dataDateFormat": "YYYY-MM-DD",
            "precision": 2,
            "valueAxes": [{
                "id": "v1",
                "title": "Sales",
                "position": "left",
                "autoGridCount": false,
                "labelFunction": function(value) {
                    return Math.round(value);
                }
            }, {
                "id": "v2",
                "title": "",
                "gridAlpha": 0,
                "fontSize": 0,
                "axesAlpha": 0,
                "position": "left",
                "autoGridCount": false
            }],
            "graphs": [{
                "id": "g3",
                "valueAxis": "v1",
                "lineColor": "#4680ff",
                "fillColors": "#4680ff",
                "fillAlphas": 1,
                "type": "column",
                "title": "Actual Sales",
                "valueField": "sales2",
                "clustered": true,
                "columnWidth": 0.4,
                "legendValueText": "$[[value]]M",
                "balloonText": "[[title]]<br /><b style='font-size: 130%'>$[[value]]M</b>"
            }, {
                "id": "g4",
                "valueAxis": "v1",
                "lineColor": "#FC6180",
                "fillColors": "#FC6180",
                "fillAlphas": 1,
                "type": "column",
                "title": "Target Sales",
                "valueField": "sales1",
                "clustered": true,
                "columnWidth": 0.4,
                "legendValueText": "$[[value]]M",
                "balloonText": "[[title]]<br /><b style='font-size: 130%'>$[[value]]M</b>"
            }, {
                "id": "g1",
                "valueAxis": "v2",
                "bullet": "round",
                "bulletBorderAlpha": 0,
                "bulletColor": "transparent",
                "bulletSize": 0,
                "hideBulletsCount": 50,
                "lineThickness": 3,
                "dashLength": 10,
                "lineColor": "#93BE52",
                "type": "smoothedLine",
                "title": "Market Days",
                "useLineColorForBulletBorder": true,
                "valueField": "market1",
                "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
            }, {
                "id": "v3",
                "valueAxis": "v1",
                "lineColor": "#FFB64D",
                "fillColors": "#FFB64D",
                "fillAlphas": 1,
                "type": "column",
                "title": "Actual Sales",
                "valueField": "sales2",
                "clustered": true,
                "columnWidth": 0.4,
                "legendValueText": "$[[value]]M",
                "balloonText": "[[title]]<br /><b style='font-size: 130%'>$[[value]]M</b>"
            }],
            "chartCursor": {
                "pan": true,
                "valueLineEnabled": true,
                "valueLineBalloonEnabled": true,
                "cursorAlpha": 0,
                "valueLineAlpha": 0.2
            },
            "categoryField": "date",
            "categoryAxis": {
                "parseDates": true,
                "dashLength": 0,
                "axisAlpha": 0,
                "GridAlpha": 0,
                "minorGridEnabled": true
            },
            "legend": {
                "useGraphSettings": true,
                "position": "top"
            },
            "balloon": {
                "borderThickness": 1,
                "shadowAlpha": 0
            },
            "export": {
                "enabled": true
            },
            "dataProvider": [{
                "date": "2013-01-16",
                "market1": 91,
                "market2": 75,
                "sales1": 5,
                "sales2": 8
            }, {
                "date": "2013-01-17",
                "market1": 74,
                "market2": 78,
                "sales1": 4,
                "sales2": 6
            }, {
                "date": "2013-01-18",
                "market1": 78,
                "market2": 88,
                "sales1": 5,
                "sales2": 2
            }, {
                "date": "2013-01-19",
                "market1": 85,
                "market2": 89,
                "sales1": 8,
                "sales2": 9
            }, {
                "date": "2013-01-20",
                "market1": 82,
                "market2": 89,
                "sales1": 9,
                "sales2": 6
            }, {
                "date": "2013-01-21",
                "market1": 83,
                "market2": 85,
                "sales1": 3,
                "sales2": 5
            }, {
                "date": "2013-01-22",
                "market1": 78,
                "market2": 92,
                "sales1": 5,
                "sales2": 7
            }]
        });
        // Email sent Ends
        var ctx = document.getElementById('total-value-graph-1').getContext("2d");
        var myChart = new Chart(ctx, {
            type: 'line',
            data: buildchartjs('#fff', [45, 25, 35, 20, 45, 20, 40, 10, 30, 45], '#3a73f1'),
            options: buildchartoption(),
        });
        var ctx = document.getElementById('total-value-graph-2').getContext("2d");
        var myChart = new Chart(ctx, {
            type: 'line',
            data: buildchartjs('#fff', [10, 25, 35, 20, 10, 20, 15, 45, 15, 10], '#e55571'),
            options: buildchartoption(),
        });

        function buildchartjs(a, b, f) {
            if (f == null) {
                f = "rgba(0,0,0,0)";
            }
            return {
                labels: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October"],
                datasets: [{
                    label: "",
                    borderColor: a,
                    borderWidth: 2,
                    hitRadius: 30,
                    pointHoverRadius: 4,
                    pointBorderWidth: 50,
                    pointHoverBorderWidth: 12,
                    pointBackgroundColor: Chart.helpers.color("#000000").alpha(0).rgbString(),
                    pointBorderColor: Chart.helpers.color("#000000").alpha(0).rgbString(),
                    pointHoverBackgroundColor: a,
                    pointHoverBorderColor: Chart.helpers.color("#000000").alpha(.1).rgbString(),
                    fill: true,
                    backgroundColor: f,
                    data: b,
                }]
            };
        }

        function buildchartoption() {
            return {
                title: {
                    display: !1
                },
                tooltips: {
                    enabled: true,
                    intersect: !1,
                    mode: "nearest",
                    xPadding: 10,
                    yPadding: 10,
                    caretPadding: 10
                },
                legend: {
                    display: !1,
                    labels: {
                        usePointStyle: !1
                    }
                },
                responsive: !0,
                maintainAspectRatio: !0,
                hover: {
                    mode: "index"
                },
                scales: {
                    xAxes: [{
                        display: !1,
                        gridLines: !1,
                        scaleLabel: {
                            display: !0,
                            labelString: "Month"
                        }
                    }],
                    yAxes: [{
                        display: !1,
                        gridLines: !1,
                        scaleLabel: {
                            display: !0,
                            labelString: "Value"
                        },
                        ticks: {
                            beginAtZero: !0
                        }
                    }]
                },
                elements: {
                    point: {
                        radius: 4,
                        borderWidth: 12
                    }
                },
                layout: {
                    padding: {
                        left: 0,
                        right: 0,
                        top: 5,
                        bottom: 0
                    }
                }
            };
        }
    };
});
