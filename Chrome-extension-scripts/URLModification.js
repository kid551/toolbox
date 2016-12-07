// ==UserScript==
// @name         Add 'https' prefix for QQ domain.
// @namespace    http://tampermonkey.net/
// @version      0.1
// @description  When meet the qq domain, change its prefix into 'https'.
// @author       kid551
// @match        http://*/*
// @grant        none
// ==/UserScript==

(function() {
    var oldHost = window.location.host;
    var oldURL = window.location.toString();
    
    if(oldHost.includes("qq")) {
        var newURL = "https" + oldURL.substring(4);
        window.location.replace(newURL);
    }
})();