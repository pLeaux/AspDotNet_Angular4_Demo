import { Component, OnInit } from '@angular/core';  
import { VehicleService } from "../../services/vehicle.service";
import { FormsModule } from '@angular/forms';
import { VehicleSaveType, VehicleGetType } from "../../../models/vehicle";
import * as UnderscoreJS from  'underscore'; // something wrong with that ("cannot find mdule", see: https://github.com/Microsoft/TypeScript/issues/16472)
import { Router, ActivatedRoute } from "@angular/router"; 
import { ToastyService } from "ng2-toasty";
import { Logger } from 'angular2-logger/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/Observable/forkJoin';


enum HttpMethodType { http_post, http_put }; 

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: []
})

/**
*   VehicleFormComponent is used for both Insert and Update of Vehicle (URL "/vehicle-form"  loads blank form for Insert, "/vehicle-form/1" = loads vehicle #1 for Update)
*   
*/

export class VehicleFormComponent implements OnInit {  

    brands: any[];
    models: any[]; 
    features: any[];
    selectedBrandId: number; 
    vehicle: VehicleSaveType = { "id": 0, "brandId": 0, "modelId": 0, "isRegistered": false, "featureIds": [], "contact": { "name": "", "phone": "", email: "" } }; 
    httpMethodType: HttpMethodType; 
    title: string; 
    showDebugInfo: boolean = false;
 

    constructor(
        private vehicleService: VehicleService,
        private route: ActivatedRoute,
        private router: Router,
        private toastyService: ToastyService, 
        private logger: Logger)
    {
        route.params.subscribe(p => {
            if (p['id']) {
                this.httpMethodType = HttpMethodType.http_put;
                this.title = "Vehicle #" + (p['id']);  
                this.vehicle.id = p['id']; 
            } else {
                this.httpMethodType = HttpMethodType.http_post;
                this.title = "New Vehicle";
            } 
        }); 
    }

    ngOnInit() {

        // joined subscriptios
        Observable.forkJoin([
            this.vehicleService.GetBrands(true),
            this.vehicleService.GetModels(),
            this.vehicleService.GetFeatures()
        ])
        .subscribe(
            (data: any) => {
                this.brands = data[0];
                this.models = data[1];
                this.features = data[2];

            },
            (errResponse: any) => {
                if (errResponse.status == 404)  // not found
                    this.router.navigate(["/home"]) ;
            }
        );
        if (this.httpMethodType == HttpMethodType.http_put) {
            this.vehicleService.GetVehicle(this.vehicle.id).subscribe(
                vehicleGetTypeResponse => {
                    this.setVehicle_From_VehicleGetType(vehicleGetTypeResponse)
                },
                errResponse => {
                    if (errResponse.status == 404)  // not found
                        this.router.navigate(["/home"])
                }
            );
        };  
 
    } 

    onBrandChange($event: any) {
        let selectedBrand = this.brands.find(b => b.id == this.vehicle.brandId);
        this.models = selectedBrand ? selectedBrand.models : [];  // if (selectedBrand != null) ... else ...
        delete (this.vehicle.modelId); // Hmmmm .... this actualy deletes property modelId from vehicle object, and not it's value
    }

    onFeatureChange($event: any, featureId: number) {
        if ($event.target.checked) {
            this.vehicle.featureIds.push(featureId);
        } 
        else {
            this.vehicle.featureIds.splice(this.vehicle.featureIds.indexOf(featureId), 1);
        } 
    }

    public submitData($event: any) {
        // this.logger.warn('JUST TESTING LOGGER ON SUBMIT'); 
        $event.preventDefault();
        switch (this.httpMethodType) {
            case HttpMethodType.http_post:  
                this.vehicleService.PostVehicle(this.vehicle)
                    .subscribe(
                    res => {
                        console.log(res);
                        this.toastyService.success({ title: "Success", msg: 'Vehicle data successfully saved.', theme: 'bootstrap', showClose: true, timeout: 5000 });
                        this.router.navigate(["/view-vehicle/" + res.id]);
                    }
                    // errors are processed in global app.error-handler
                    // errRes => { }                       
                    );   
                break;
            case HttpMethodType.http_put: 
                this.vehicleService.PutVehicle(this.vehicle)
                    .subscribe(res => {
                        console.log(res);
                        this.toastyService.success({ title: "Success", msg: 'Vehicle data successfully saved.', theme: 'bootstrap', showClose: true, timeout: 5000 });
                        this.router.navigate(["/view-vehicle/" + this.vehicle.id]);
                    });  
                break;
            default:
                this.logger.error("Unexpected error in method submitData.");
        };    
    }

    setVehicle_From_VehicleGetType(vehicleGT: VehicleGetType) {
        this.vehicle.id = vehicleGT.id;
        this.vehicle.brandId = vehicleGT.brand.id;
        this.vehicle.modelId = vehicleGT.model.id;
        this.vehicle.isRegistered = vehicleGT.isRegistered; 
        this.vehicle.contact = vehicleGT.contact;
        this.vehicle.featureIds = UnderscoreJS.pluck(vehicleGT.features, "id"); 
    }

    // Note: could not use deleteVehicle() in html without declaring it public !?! Which is wierd, because I didnot need to do the same with submitData()
    public deleteVehicle(vehicleId: number) {
        // alert("deleteVehicle client method! Id=" + vehicleId);
        if (confirm("Delete selected vehicle?")) {
            this.vehicleService.DeleteVehicle(vehicleId)
                .subscribe(res => {
                    this.logger.info("Deleted Vehicle Id #"+ res.json());
                    this.toastyService.success({ title: "Deleted", msg: 'Vehicle successfully deleted.', theme: 'bootstrap',  showClose: true, timeout: 5000 });
                });

        } 
    }

}