import { Injectable } from '@angular/core';
import { Http, RequestOptionsArgs, Headers, RequestOptions } from '@angular/http'; 
import 'rxjs/add/operator/map';
import { VehicleSaveType, VehicleGetType, VehicleFilterType, VehicleQueryResultType, BrandVehicleCountType } from "../../models/vehicle";
import { Observable } from 'rxjs/Observable';
// import { AuthHttp } from "angular2-jwt/angular2-jwt"; // not working properly, sends tokenId instead of access_token ("wrong audience")
import { AuthService } from './auth.service';


@Injectable() 
export class VehicleService {
     

    constructor(
        private http: Http,
        // private authHttp: AuthHttp, // checks if JWT in local storage and includes tokenID in eaach request automatically
        private authService: AuthService)
    {
        ; 
    }

    GetBrands(includeModels: boolean = false) { 
        if (includeModels == true) 
            var brandsList = this.http.get('/api/brandsWithModels').map(response => response.json())
        else 
            var brandsList = this.http.get('/api/brands').map(response => response.json())
        return brandsList; 
    }

    GetModels() {
        var modelsList = this.http.get('/api/models').map(response => response.json());
        return modelsList;
    }

    GetFeatures() {
        var featuresList = this.http.get('/api/features').map(response => response.json());
        return featuresList;
    }

    PostVehicle(newVehicle: VehicleSaveType) {
        return (
            this.http.post('/api/vehicles', newVehicle, this.authService.GetHttpHeaderAuthorizationOptions())
                .map(res => res.json())
            );
    }

    PutVehicle(vehicle: VehicleSaveType) { 
        return this.http.put('/api/vehicles/' + vehicle.id, vehicle, this.authService.GetHttpHeaderAuthorizationOptions())
                .map(res => res.json());
    }

    GetVehicle(vehicleId: number) {
        var vehicle = this.http.get('/api/vehicles/' + vehicleId).map(response => response.json());
        return vehicle;
    }

    DeleteVehicle(vehicleId: number) {
        var response = this.http.delete('/api/vehicles/' + vehicleId, this.authService.GetHttpHeaderAuthorizationOptions())
            .map(response => response.json());
        return response;
    }

    GetVehicles(): Observable<VehicleQueryResultType> {
        var vehicles: Observable<VehicleQueryResultType> = this.http.get('/api/vehicles')
            .map(response => response.json());
        return vehicles;
    }
    /* backup old
    GetVehicles(): Observable<VehicleGetType[]> {
        var vehicles: Observable<VehicleGetType[]> = this.http.get('/api/vehicles').map(response => response.json());
        return vehicles;
    }
    */

    GetVehicles_Filtered(vehiclesFilter: VehicleFilterType): Observable<VehicleQueryResultType> {
        var vehicles: Observable<VehicleQueryResultType> = this.http.get('/api/vehicles' + this.GetUrlParamsFromFilter(vehiclesFilter, true))
            .map(response => response.json());
        return vehicles;
    }
    /* backups old
    GetVehicles_Filtered(vehiclesFilter: VehicleFilterType): Observable<VehicleGetType[]> {
        var vehicles: Observable<VehicleGetType[]> = this.http.get('/api/vehicles' + this.GetUrlParamsFromFilter(vehiclesFilter, true))
            .map(response => response.json());
        return vehicles;
    }
    */

    GetUrlParamsFromFilter(vehiclesFilter: VehicleFilterType, withPaging: boolean): string {
        var paramsSufix: string = "";  
        if (vehiclesFilter.brandFilter.id) {
            paramsSufix += (paramsSufix ? "&brandId=" : "?brandId=") + vehiclesFilter.brandFilter.id;
        }
        if (vehiclesFilter.modelFilter.id) {
            paramsSufix += (paramsSufix ? "&modelId=" : "?modelId=") + vehiclesFilter.modelFilter.id;
        }
        if (vehiclesFilter.sortKey) {
            paramsSufix += (paramsSufix ? "&sortKey=" : "?sortKey=") + vehiclesFilter.sortKey;
            if (vehiclesFilter.isSortAsc != null) {
                paramsSufix += (paramsSufix ? "&isSortAsc=" : "?isSortAsc=") + vehiclesFilter.isSortAsc;
            }
        }
        if (vehiclesFilter.pageSize && vehiclesFilter.pageNo) {
            paramsSufix += (paramsSufix ? "&pageSize=" : "?pageSize=") + vehiclesFilter.pageSize;
            paramsSufix += (paramsSufix ? "&pageNo=" : "?pageNo=") + vehiclesFilter.pageNo;
        }  
        return encodeURI(paramsSufix); 
    }


    GetBrandsVehicleCount(): Observable<BrandVehicleCountType[]> {
        var brands: Observable<BrandVehicleCountType[]> = this.http.get('/api/vehicles/brands', this.authService.GetHttpHeaderAuthorizationOptions())
            .map(response => response.json());
        return brands;
    }


    Get_HttpRequest_AuthorizationOptions(): RequestOptions { 
        // var requestOptions = new RequestOptions();
        if (this.authService.isAuthenticated()) { 
            var accessToken: string = this.authService.getApiAccessToken(); 
            var requestOptions = new RequestOptions(); 
            var headers: Headers = new Headers();
            headers.append('Authorization', this.authService.getApiAccessToken());
            headers.append('Content-Type', 'application/json');
            requestOptions.headers = headers; 
            return requestOptions; 
        } else {
            throw new Error('You have to be loged in, to perform that operation');
        }  
    }
            
 
 


}
