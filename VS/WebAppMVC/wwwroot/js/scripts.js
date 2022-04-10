var mobile = undefined;

$(function() {
    mobile = MobileUI();
    SetScreenSizeVars();
});
var height = $(window).height();
var width = $(window).width();
var realWidth = 0;
var realHeight = 0;
var days = 0;
var intradayInterval = null;

var landscape = (window.orientation === 0 || window.orientation === 180) ? false : true;
window.addEventListener("orientationchange", function () {
    SetScreenSizeVars();
});

function SetScreenSizeVars(){   
    switch (window.orientation) {  
        case 0:
        case 180:
            landscape = false;
            break; 
        case -90:
        case 90: 
            landscape = true;
            break;
        }
    height = $(window).height();
    width = $(window).width();
    if (landscape) {
        realHeight = Math.min(height, width);
        realWidth = Math.max(height, width);
    }
    else {
        realHeight = Math.max(height, width);
        realWidth = Math.min(height, width);
    }
    var listViewHeight = (landscape ? (realHeight - 170) : (realHeight - 220));

    if (!(typeof nestedListObj === 'undefined')) {
        nestedListObj.height = listViewHeight;
    }
    if (!(typeof selectedListObj === 'undefined')) {
        selectedListObj.height = listViewHeight;
    }
    if (!(typeof ListObj === 'undefined')) {
        ListObj.height = listViewHeight - 10;
    }

    console.log('SetScreenSizeVars mobile=', mobile, 'landscape=', landscape, 'realHeight=', realHeight, 'realWidth=', realWidth);
}


var btns = ['1d', '5d', '1m', '3m', '6m', '1y', '5y', '10y', 'max'];
var intradaybtns = ['1min', '5min', '15min', '30min', '60min'];

function AjaxCall(type, url, data, callback) {
    var originalTitle = document.title;
    document.title = "LOADING...";
    $.ajax({
        type: type,
        url: url,
        data: data,
        error: function (e) {
            var htmlErrorText = '';
            htmlErrorText = FindSubstringBetweenTwoString(e.responseText, '<div class="titleerror">Exception: ', '</div>');
            document.title = originalTitle;
            /*
            switch (e.status) {
                case 0:
                    htmlErrorText += '<b>Unable to establish connection</b>';
                    break;
                case 404:
                    htmlErrorText += '<b>Not found (404)</b>';
                    break;
                default:
                    htmlErrorText += '<b>ExceptionMessage:</b> ' + e.responseJSON.ExceptionMessage;
                    htmlErrorText += '<br><b>ExceptionType:</b>  ' + e.responseJSON.ExceptionType;
                    htmlErrorText += '<br><b>Message:</b>  ' + e.responseJSON.Message;
                    htmlErrorText += '<br><b>StackTrace:</b>  ' + e.responseJSON.StackTrace;
                    break;
            }
            */
            swal({
                //title: 'ERROR (' + e.status + ') - ' + e.statusText,
                title: 'ERROR',
                html: htmlErrorText,
                type: 'error',
                showCancelButton: false,
                width: '45%',
                confirmButtonColor: '#3085d6'
            }).then((result) => {
            });
            return callback(false, e);
        },
        success: function (response) {
            document.title = originalTitle;
            if (response.hasOwnProperty('error') && response.error !== null) {
                swal({
                    title: 'ERROR',
                    html: response.error,
                    type: 'error',
                    showCancelButton: false,
                    width: '45%',
                    confirmButtonColor: '#3085d6'
                }).then((result) => {
                });
                response = false;
            }
            return callback(response);
        }
    });
}

function FindSubstringBetweenTwoString(findin, startwith, endswith) {
    var startIdx = findin.indexOf(startwith) + startwith.length;
    var endIdx = findin.indexOf(endswith);
    var result = findin.substring(startIdx, endIdx);
    return result;
}

function pad(num, size) {
    var s = num+"";
    while (s.length < size) s = "0" + s;
    return s;
}

function EnableButton(id, enable) {
    document.getElementById(id).disabled = !enable;
}

function EnableButtons(enable) {
    btns.forEach(function (btn) {
        EnableButton(btn + 'Btn', enable);
    });
    Loader(!enable);
}

function ResetButtonsStyle() {
    btns.forEach(function (btn) {
        document.getElementById(btn + 'Btn').classList = "btn btn-default btn-xs";
    });
}

function HideIntradayButtonDiv() {
    document.getElementById('intradaybuttonsdiv').style = "text-align:center;margin-top:15px;display:none;";
}

function ShowIntradayButtonDiv() {
    document.getElementById('intradaybuttonsdiv').style = "text-align:center;margin-top:15px;display:block;";
}

function setDateRange(arg, arg2, arg3, callApi) {
    var d = new Date();
    days = 0;
    var intradaybuttonsdiv = document.getElementById('intradaybuttonsdiv');
    if (intradaybuttonsdiv !== null) {
        HideIntradayButtonDiv();
    }
    
    switch(arg){
        case '1d':
            days = 1;
            if (intradaybuttonsdiv !== null) {
                ShowIntradayButtonDiv();
                if (arg3 === null) {
                    ResetButtonsStyleIntraday();
                    return;
                }
                else {
                    ResetButtonsStyleIntraday();
                    document.getElementById(arg3 + 'Btn').classList = "btn btn-primary btn-xs";
                    intradayInterval = arg3;
                }
            }
            break;
        case '5d':
            d.setDate(d.getDate() - 5);
            break;
        case '1m':
            d.setMonth(d.getMonth() - 1);
            break;
        case '3m':
            d.setMonth(d.getMonth() - 3);
            break;
        case '6m':
            d.setMonth(d.getMonth() - 6);
            break;
        case '1y':
            d.setMonth(d.getMonth() - 1*12);
            break;
        case '5y':
            d.setMonth(d.getMonth() - 5*12);
            break;
        case '10y':
            d.setMonth(d.getMonth() - 10*12);
            break;
        case 'max':
            d.setYear(1970);
            break;
    }
    datepickerFrom.value = d;
    datepickerTo.value = new Date();
    var success = false;
    if (callApi !== false) {
        if (arg2 === 'price')
            success = GetpriceData();
        else if (arg2 === 'compare')
            success = GetCompareData();
    }
    //console.log('setDateRange success', success);
    document.getElementById(arg + 'Btn').classList = "btn btn-primary btn-xs";
    if (!success) ResetButtonsStyle();
}

function intradayClicked(arg) {
    //console.log('intradayClicked arg=', arg);
    setDateRange('1d', 'price', arg);
}

function enableIntradayButtons() {
    if (typeof nestedListObj !== 'undefined') {
        var selected = nestedListObj.getSelectedItems();
        if (selected !== null && selected.data !== null && selected.data.id !== null) {
            //if (selected.data.id.includes('Hungarian Equities/')) {
            //    intradayClicked('1min');
            //    return;
            //}
        }
    }
    ResetButtonsStyleIntraday();
    document.getElementById('intradaybuttonsdiv').style = "text-align:center;margin-top:15px;display:block;";
}

function ResetButtonsStyleIntraday() {
    // intraday-nel nem kell bollinger
    document.getElementById('bbandperiod').value = 0;
    document.getElementById('bbandup').value = 0;
    document.getElementById('bbanddown').value = 0;
    if (typeof nestedListObj !== 'undefined') {
        var selected = nestedListObj.getSelectedItems();
        if (selected !== null && selected.data !== null && selected.data.id !== null) {
            // Ha magyar intraday akkor nem kell perces osztas
            //if (selected.data.id.includes('Hungarian Equities/')) {
            //    HideIntradayButtonDiv();
            //}
        }
    }
    intradaybtns.forEach(function (btn) {
        document.getElementById(btn + 'Btn').classList = "btn btn-default btn-xs";
    });
}

function MobileUI(){
    var ismobile = CheckIfMobile();
    if (ismobile) {
        var tab_price = document.getElementById('tab_price');
        if (tab_price) tab_price.innerHTML = '<i class="fas fa-list-ul"></i>';
        var tab_list = document.getElementById('tab_list');
        if (tab_list) tab_list.innerHTML = '<i class="fas fa-list-ul"></i>';
        var tab_ta = document.getElementById('tab_ta');
        if (tab_ta) tab_ta.innerHTML = '<i class="far fa-bell"></i>';
        var tab_dr = document.getElementById('tab_dr');
        if (tab_dr) tab_dr.innerHTML = '<i class="far fa-calendar-alt"></i>';
        var tab_chart = document.getElementById('tab_chart');
        if (tab_chart) tab_chart.innerHTML = '<i class="fas fa-chart-line"></i>';
        var tab_stat = document.getElementById('tab_stat');
        if (tab_stat) tab_stat.innerHTML = '<i class="far fa-chart-bar"></i>';

        var tab_si = document.getElementById('tab_si');
        if (tab_si) tab_si.innerHTML = '<i class="far fa-check-square"></i><span id="tab_si_badge" class="badge">0</span>';

        var tab_comparechart = document.getElementById('tab_comparechart');
        if (tab_comparechart) tab_comparechart.innerHTML = '<i class="fas fa-chart-line"></i>';
        
        $("#label_price").remove();
        $("#label_dr").remove();
        $("#label_ta").remove();
        $("#label_chart").remove();
        $("#label_stat").remove();
        $("#label_instrumentlist").remove();
        $("#label_selectedinstruments").remove();
        $("#label_comparedaterange").remove();
        $("#label_comparechart").remove();
        $("#h3_price").remove();
        $("#footercontent").remove();
        //$('#bodycontent').removeClass('container').removeClass('body-content'); 
    }
    return ismobile;
}

function SetFavoriteNumberBadge() {
    var tab_si_badge = document.getElementById('tab_si_badge');
    if (tab_si_badge) {
        tab_si_badge.innerHTML = selectedNumber;
    } 
    //selectedListObj.height = 65 + selectedNumber * 36;
}

function RemoveElements(id){
    var div = document.getElementById(id);
    if (div === null) return;
    while (div.hasChildNodes()) {
        div.removeChild(div.lastChild);
    }
}

function Loader(on) {
    if (on) {
        document.getElementById('loader').style = "visibility:initial";
        document.getElementById('HideEverything').style = "visibility:initial";
    }
    else {
        document.getElementById('loader').style = "visibility:hidden";
        document.getElementById('HideEverything').style = "visibility:hidden";
    }
}

function SetLocalStorage(name, obj) {
    if (typeof Storage !== "undefined") {
        localStorage.setItem(name, JSON.stringify(obj));
        return true;
    }
    return false;
}

function GetLocalStorage(name) {
    if (typeof Storage !== "undefined") {
        var stored = localStorage.getItem(name);
        if (stored === null)
            return false;
        return JSON.parse(stored);
    }
    return false;
}

function HeikinAshiBtnClick(arg) {
    var heikinashibtn = document.getElementById("heikinashibtn");
    switch (heikinashibtn.firstChild.data) {
        case "Off":
            heikinashibtn.firstChild.data = "On";
            $("#heikinashibtn").removeClass('btn-default');
            $("#heikinashibtn").addClass('btn-primary');
            break;
        case "On":
            heikinashibtn.firstChild.data = "Off";
            $("#heikinashibtn").removeClass('btn-primary');
            $("#heikinashibtn").addClass('btn-default');
            break;
    }
}

function CheckIfMobile() {
    var check = false;
    (function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true; })(navigator.userAgent || navigator.vendor || window.opera);
    return check;
}

function SetURL(arg, page) {
    //console.log('SetURL window.location.href', window.location.href);
    //console.log('SetURL ' + page + ' arg', arg);
    const nextState = { additionalInformation: 'Updated the URL with JS' };
    const nextTitle = document.title;
    var url = '';
    var indexOfFirst = window.location.href.indexOf('/Price/Search/');
    url += window.location.href.substring(0, indexOfFirst);
    url += '/Price/Search/';
    if (arg.instrumentFullPath.includes('Hungarian Equities')) {
        url += '?arg=HUN_' + arg.instrumentFullPath.replace('Hungarian Equities/', '');
    }
    else {
        url += '?arg=' + arg.instrumentFullPath.replace('Equities/', '');
    }
    url += '&strFrom=' + arg.startDateStr + '&strTo=' + arg.endDateStr;
    if (arg.interval) {
        url += '&interval=' + arg.interval;
    }
    console.log('SetURL url', url);
    window.history.pushState(nextState, nextTitle, url);
}