"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CustomLogger = void 0;
var CustomLogger = /** @class */ (function () {
    function CustomLogger() {
    }
    CustomLogger.prototype.log = function (logLevel, message) {
        console.log(logLevel + " :: " + message);
    };
    return CustomLogger;
}());
exports.CustomLogger = CustomLogger;
