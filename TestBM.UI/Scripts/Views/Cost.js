$(document).ready(function(){
    
    $(function () {
        //$('#costForm').validator();
        $('#costForm').on('submit', function (e) {
            var costAmount = $("#exampleInputAmount").val();
            var costInfo = $("#exampleInputInfo").val();
            setData(costAmount, costInfo);
        });

    });
});

function setData(costAmount, costInfo) {
    $.ajax(
        {
            url: 'http://localhost:12177/api/insertcost?username=gio2&costimport=' + costAmount + '&costinfo=' + costInfo,
            method: 'POST',
            async: false,
            dataType: 'json'
        }
                 ).done(function (result) {
                     console.log(result);                     
                 }).fail(function (req, textStatus, errorThrown) {
                     console.log(req, textStatus, errorThrown);
                 }).always(function () {
                     console.log('Always');
                 });
}

function getCosts(username) {
    $.ajax(
        {
            url: 'http://localhost:12177/api/getcost?username=gio2',
            method: 'GET',
            dataType: 'json'
        }
                 ).done(function (result) {
                     console.log(result);
                     renderChartLine(result);
                 }).fail(function (req, textStatus, errorThrown) {
                     console.log(req, textStatus, errorThrown);
                 }).always(function () {
                     console.log('Always');
                 });
}

function renderChartLine(data) {
    for (var i = 0, len = data.length; i < len; i++) {
        $('#tablebody').append('<tr><th scope="row">' + (i + 1) + '</th><td>' + data[i].CostInfo + '</td><td>' + data[i].CostDetail + '</td></tr>');
    }
    $("#costList").trigger("create");
}