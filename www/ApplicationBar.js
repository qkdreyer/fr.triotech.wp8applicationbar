/**
 * @constructor
 */
var ApplicationBar = function(){};

ApplicationBar.prototype.update = function(success, failure, options){
    cordova.exec(success, failure, "ApplicationBarController", "update", [options]);
};

cordova.addConstructor(function() {

    if (!window.Cordova) {
        window.Cordova = cordova;
    };


    if(!window.plugins) window.plugins = {};
    window.plugins.ApplicationBar = new ApplicationBar();
});

