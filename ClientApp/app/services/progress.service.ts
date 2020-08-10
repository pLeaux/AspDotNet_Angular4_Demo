import { Injectable, NgZone } from '@angular/core';
import { Subject } from "rxjs/Subject";
import { BrowserXhr} from '@angular/http'
import { Observable } from "rxjs/Observable";


class ProgressData {
    total: number;
    percentage: number; 
}


@Injectable()
export class ProgressService { 

    private uploadProgress$: Subject<any>; // Subject: Observable + Extra params  

    constructor (private ngZone: NgZone) {

    } 

    startTracking() { 
        this.uploadProgress$ = new Subject();
        console.log("ProgressService.startTracking(), new Subject():"); 
        console.log(this.uploadProgress$); 
        return (this.uploadProgress$);  
    } 

    notify(progressData: ProgressData) { 
        if (this.uploadProgress$) {
            this.ngZone.run(() => {
                console.log("ProgressService.notify, before next()");
                this.uploadProgress$.next(progressData);  
                console.log("ProgressService.notify, after next()");  
            });
        } else {
            console.log("ProgressService.notify,uploadProgress = NULL !!!!!! ");
        } 
    }

    endTracking() {
        console.log("End Tracking !");
        if (this.uploadProgress$)
            this.uploadProgress$.complete(); // observable method, releases observable resources 
    }

};

@Injectable()
export class BrowserXhrWithProgress extends BrowserXhr {  // Angular.BrowserXhr = JavaScript.Ajax.XMLHttpRequest

    constructor(private progressService: ProgressService) {
        super();
    }

    build(): XMLHttpRequest {
        var req: XMLHttpRequest;
        req = super.build(); 
        req.upload.onprogress = (event: any) => {
            console.log("req.upload.onprogress, event: "); console.log(event);
            var progressData: ProgressData = { total: event.total, percentage: Math.round(event.loaded / event.total * 100) }; 
            this.progressService.notify(progressData); 
        };
        req.upload.onloadend = (event: any) => {
            console.log("req.upload.onloadend: ");
            this.progressService.endTracking() // free resources
        }
        return req;
    }
  

}
 