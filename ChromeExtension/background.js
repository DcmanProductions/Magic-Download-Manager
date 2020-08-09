chrome.downloads.onCreated.addListener(function (item) {
    if (item.state == "in_progress") {
        chrome.downloads.cancel(item.id, function () {
        });
        window.open('magicdm://' + item.finalUrl, "_blank");
    }
});