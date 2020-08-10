import { Component, OnInit, ElementRef, ViewChild, NgZone } from '@angular/core';
import { VehicleService } from "../../services/vehicle.service";
import { FormsModule } from '@angular/forms';
import { VehicleSaveType, VehicleGetType, PhotoDescriptionType } from "../../../models/vehicle";
import * as UnderscoreJS from 'underscore';  
import { Router, ActivatedRoute } from "@angular/router";
import { ToastyService } from "ng2-toasty";
import { Logger } from 'angular2-logger/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/Observable/forkJoin';
import { Subject } from "rxjs/Subject";
import { ProgressService } from "../../services/progress.service";
import { PhotoService } from "../../services/photo.service";
import { AuthService } from "../../services/auth.service";



@Component({
    selector: 'app-view-vehicle',
    templateUrl: './view-vehicle.component.html',
    styleUrls: ['./view-vehicle.component.css']  
})
export class ViewVehicleComponent implements OnInit {

    vehicle: VehicleGetType; //= { "id": 0, "brand": { "id": 0, "name": "" }, "model": { "id": 0, "name": "" }, "isRegistered": false, "userID": ""; "contact": { "name": "", "phone": "", "email": ""}, "features": [] };
    vehicleId: number;
    title: string;
    @ViewChild('fileInput') fileInput: ElementRef; // reference to DOM element with id "fileInput"
    photos: PhotoDescriptionType[] = [];
    progress: any;
    showDebugInfo: boolean = false; 

    constructor(  
        private vehicleService: VehicleService,
        private route: ActivatedRoute,
        private router: Router,
        private toastyService: ToastyService, 
        private progressService: ProgressService,
        private photoService: PhotoService,
        private logger: Logger,
        private ngZone: NgZone,
        private authService: AuthService
        )
    { 
        console.log("ViewVehicleComponent.constructor, is progressService injected: "); 
        console.log(progressService != null);
        route.params.subscribe(param => {
            this.vehicleId = param['id'];
            this.title = "Vehicle #" + this.vehicleId;
        }); 
    }
    
    ngOnInit() {
        console.log("ViewVehicleComponent.ngOnInit() #1");  
        console.log("ngOnInit #1");
        this.vehicleService.GetVehicle(this.vehicleId) 
            .subscribe(vehicleGetTypeResponse => { 
                this.vehicle = vehicleGetTypeResponse; 
            });
        this.photoService.GetVehiclePhotos(this.vehicleId)
            .subscribe(photos => {
                console.log("ViewVehicleComponent.ngOnInit() #2");
                this.photos = photos;
            }); 
        console.log("ViewVehicleComponent.ngOnInit() #3"); 
    }

    
    public deleteVehicle() {  
        if (confirm("Delete selected vehicle?")) {
            this.vehicleService.DeleteVehicle(this.vehicleId)
                .subscribe(res => {
                    this.logger.info("Deleted Vehicle Id #" + res);
                    this.toastyService.success({ title: "Deleted", msg: 'Vehicle successfully deleted.', theme: 'bootstrap', showClose: true, timeout: 5000 });
                    this.router.navigate(["/vehicle-list"]);
                });

        } 
    }

    doUploadPhoto() {  
        var uploadProgress: Subject<any>;

        // checks
        if (this.fileInput == null)
            return false; 
        var fileInputElement: HTMLInputElement = this.fileInput.nativeElement; 
        if (fileInputElement == null)
            return false; 
        if (fileInputElement.files == null)
            return false; 

        // prepare http onProgress service  
        uploadProgress = this.progressService.startTracking();  
        uploadProgress.subscribe((progress) => {  
            this.progress = progress; 
        });
        
        // execute upload
        this.photoService.UploadPhoto(this.vehicleId, fileInputElement.files[0])
            .subscribe(
                res => {  
                    try {
                        this.photos.push(res);
                    } catch (e) {
                        console.log(e);
                    }
                },
                err => {
                    this.ngZone.run(() => {
                        this.toastyService.error({
                            title: "Upload error",
                            msg: err.text() ? err.text() : err,  
                            theme: 'bootstrap',  
                            showClose: true,
                            timeout: 5000
                        });
                    });
                } 
            );   
    }
 
}
