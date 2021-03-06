﻿
function fmYear(str) {
    // /Date(1417328685970)/
    if (str == null)
        return '';
    var regex = /\/Date([0-9]*)\//;
    var date = new Date(parseInt(str.match(/[0-9]+/)));
    return date.getFullYear();
}

function fmDate(str) {
    // /Date(1417328685970)/
    if (str == null)
        return '';
    var regex = /\/Date([0-9]*)\//;
    var date = new Date(parseInt(str.match(/[0-9]+/)));
    //return date;
    return date.getFullYear() + '/' + (date.getMonth()+1) + '/' + date.getDate();
}

function fmDateTime(str) {
    // /Date(1417328685970)/
    if (str == null)
        return '';
    var regex = /\/Date([0-9]*)\//;
    var date = new Date(parseInt(str.match(/[0-9]+/)));
    var minutes = date.getMinutes();
    if (minutes < 10)
        minutes = '0' + minutes;
    return date.getFullYear() + '/' + (date.getMonth()+1) + '/' + date.getDate() + ' ' + date.getHours() + ':' + minutes;
}

$(document).ready(function () {
    var message = $.cookie('__FlashMessage');
    $.removeCookie('__FlashMessage', { path: '/' });
    if (message != null) {
        var modal = $('#FlashModal');
        modal.find('.modal-body').text(message);
        modal.modal('show');
    }
});
