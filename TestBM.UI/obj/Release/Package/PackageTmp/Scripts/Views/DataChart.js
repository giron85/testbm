
function getDailyData(username) {
    $.ajax(
        {
            url: 'http://localhost:12177/api/dailybudgetremain?username=' + username,
            method: 'GET',
            //data: queryStringData,
            dataType: 'json'
        }
                 ).done(function (result) {                    
                     console.log(result);
                     renderDailyChartDonut(result);
                 }).fail(function (req, textStatus, errorThrown) {
                     console.log(req, textStatus, errorThrown);
                 }).always(function(){
                     console.log('Always');
                 });
}

function getMontlyData(username) {
    $.ajax(
        {
            url: 'http://localhost:12177/api/montlybudgetremain?username=' + username,
            method: 'GET',
            //data: queryStringData,
            dataType: 'json'
        }
                 ).done(function (result) {
                     console.log(result);
                     renderMontlyChartDonut(result);
                 }).fail(function (req, textStatus, errorThrown) {
                     console.log(req, textStatus, errorThrown);
                 }).always(function () {
                     console.log('Always');
                 });
}

function renderDailyChartDonut(data) {
    Morris.Donut({
        element: 'daily-budget-donut',
        data: [{
            label: "Consumed daily budget",
            value: Math.round((data.DailyBudget - data.RemainingDailyBudget) * 100) / 100
        }, {
            label: "Remaining daily budget",
            value: Math.round(data.RemainingDailyBudget * 100) / 100
        }],
        colors: ["#d8402f", "#95e866"],
        resize: true
    });
}

function renderMontlyChartDonut(data) {
    Morris.Donut({
        element: 'montly-budget-donut',
        data: [{
            label: "Consumed montly budget",
            value: Math.round((data.MontlyBudget - data.MontlyBudgetRemain) * 100) / 100
        }, {
            label: "Remaining montly budget",
            value: Math.round(data.MontlyBudgetRemain * 100) / 100
        }],
        colors: ["#d8402f", "#95e866"],
        resize: true
    });
}



