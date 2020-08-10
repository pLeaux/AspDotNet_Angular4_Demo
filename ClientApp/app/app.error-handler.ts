import * as Raven from 'raven-js';
import { ErrorHandler, Inject, NgZone, isDevMode } from "@angular/core";
import { ToastyService } from "ng2-toasty";
 

export class AppErrorHandler implements ErrorHandler {

    constructor( @Inject(ToastyService) private toastyService: ToastyService, @Inject(NgZone) private ngZone: NgZone) {

    };

    handleError(error: any): void { 
        try {
            console.log("GLOBAL ERROR HANDLER, START !");
            // showHide local tost-message
            this.ngZone.run(() => {
                this.toastyService.error({
                    title: "Error",
                    msg: (error ? (error.originalError) ? error.originalError : error : "An unexpected error happened."),
                    theme: 'bootstrap', //  Possible values: default, bootstrap, material
                    showClose: true,
                    timeout: 5000
                });
            });

            // in Production, log error to cloud service "sentry.io" (config is in "launchSettings.json"), otherwise just log to console
            if (!isDevMode()) {
                ; // Raven.captureException(error); // (error.originalError || error); 
            } else {
                console.log("GLOBAL ERROR HANDLER: " + error);
            }
            ;
        } catch (e) {
            if (isDevMode())
                console.log("ErrorHandler exception: " + e);
        }
    }

}