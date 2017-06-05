/**
 * @author Takano 2016/11/21
 */
var eventTracking = eventTracking || {

    // page type
    pageType: '',

    // set page type
    setPageType: function (pt) {
        this.pageType = pt;
    },

    // send
    send: function (action, label, pt, eventType) {
        if (typeof pt === 'undefined' || pt === null) {
            pt = this.pageType;
        }
        if (typeof eventType === 'undefined' || eventType === null) {
            eventType = 'click';
        }
        var cvPanelDisplayed = '';
        if (typeof cvPanelApp === 'object'){
            cvPanelDisplayed = cvPanelApp.getGAString();
        }
        if (typeof ga === 'function') {
            ga('send', 'event', pt, action, label + cvPanelDisplayed);
        }
        if (typeof laEvent === 'function') {
            laEvent(action, eventType, label + cvPanelDisplayed, pt);
        }
    },
};
