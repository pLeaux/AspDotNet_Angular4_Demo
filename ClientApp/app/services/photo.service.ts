import { Injectable } from '@angular/core';
import { Http, BrowserXhr } from "@angular/http"; 
import { Observable } from "rxjs/Observable";
import { PhotoDescriptionType } from "../../models/vehicle"; 
import { ProgressService } from "./progress.service";
import { AuthService } from "./auth.service";

@Injectable()
export class PhotoService {
    private http: Http;
    private authService: AuthService;

    constructor(http: Http, authService: AuthService) {
        this.http = http;
        this.authService = authService;
    }

    UploadPhoto(vehicleId: number, photoFile: any): Observable<any> {
        var res$: Observable<any>;
        console.log("client, PhotoService.UploadPhoto #1"); 
        var formData = new FormData(); // native JavaScript 
        formData.append('file', photoFile); // 'file' is the param name in "PhotosController.UplodPhoto()" method 
        res$ = this.http.post("/api/vehicles/" + vehicleId + "/photos", formData, this.authService.GetHttpHeaderAuthorizationOptions())
            .map(res => res.json()); 
        return (res$); 
    }

    GetVehiclePhotos(vehicleId: number): Observable<PhotoDescriptionType[]> {
        var photosObservable = this.http.get('/api/vehicles/' + vehicleId +'/photos')
            .map(response => response.json());
        return photosObservable;
    }
 
}
 
