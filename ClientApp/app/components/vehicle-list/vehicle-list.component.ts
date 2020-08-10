import { Component, OnInit } from '@angular/core';
import { VehicleService } from "../../services/vehicle.service";
import { FormsModule } from '@angular/forms';
import { VehicleSaveType, VehicleGetType, IdNameType, ModelType, VehicleFilterType } from "../../../models/vehicle";
import 'rxjs/add/operator/toPromise';
import { PaginationComponent } from '../../custom_components/pagination.component';
import { AuthService } from "../../services/auth.service";

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: [] 
})

/**
*  For demonstration purposes, both client- and server-side filtering is enabled (switch is the checkbox on the form's Debug panel)
*/

export class VehicleListComponent implements OnInit {

    private readonly PAGE_SIZE_DEFAULT = 10;  

    vehicleList: VehicleGetType[] = [];
    vehicleList_unfiltered: VehicleGetType[] = [];
    vehicleList_filtered: VehicleGetType[] = [];
    brands: IdNameType[] = [];  
    models: ModelType[] = [];
    models_unfiltered: ModelType[]; 
    useClientFiltering: boolean = false;  
    showDebugInfog: boolean = false;
    vehicleFilter: VehicleFilterType;  
 
    constructor(private vehicleService: VehicleService, private authService: AuthService) {
        this.resetFilters();
        this.vehicleFilter.pageSize = this.PAGE_SIZE_DEFAULT;  
    }

    
    ngOnInit() {
        console.log('VehicleListComponent.ngOnInit!');
        this.vehicleService.GetBrands(false)
            .subscribe(b => {
                this.brands = b;
            });
        this.vehicleService.GetModels()
            .subscribe(m => {
                this.models = m;
                this.models_unfiltered = m;
            });  
        this.loadSelectedVehicles();  

    }

  

    doOnFilterChange($event: any) {
        console.log("vehicleFilter: " + JSON.stringify(this.vehicleFilter)); 

        // if brand filter changed, adjust models filter 
        if ($event.target.id == "brandFilter") {
            this.vehicleFilter.modelFilter = { id: 0, name: "" };
            this.models = this.models_unfiltered;
            if (this.vehicleFilter.brandFilter.id)
                this.models = this.models_unfiltered.filter(m => m.brandId == this.vehicleFilter.brandFilter.id);
            else
                this.models = []; 
        }; 
        this.loadSelectedVehicles();
    }

    doOnColHeaderClick($event: any) { 
        switch ($event.target.id) {
            case  
                "col_brand":
                this.vehicleFilter.isSortAsc = ((!this.vehicleFilter.sortKey) || ((this.vehicleFilter.sortKey == "brand") && !this.vehicleFilter.isSortAsc) || (this.vehicleFilter.isSortAsc == null)) ? true : false; 
                this.vehicleFilter.sortKey = "brand"; 
                break;
            case "col_model":
                this.vehicleFilter.isSortAsc = ((!this.vehicleFilter.sortKey) || ((this.vehicleFilter.sortKey == "model") && !this.vehicleFilter.isSortAsc) || (this.vehicleFilter.isSortAsc == null)) ? true : false; 
                this.vehicleFilter.sortKey = "model";
                break;
            case "col_contact_name":
                this.vehicleFilter.isSortAsc = (((this.vehicleFilter.sortKey == "contact") && !this.vehicleFilter.isSortAsc) || (this.vehicleFilter.isSortAsc == null)) ? true : false; 
                this.vehicleFilter.sortKey = "contact";
                break;
            default:
                alert('No sorting for clolumn'+ $event.tafget.value +'!');
                break; 
        };  
        this.loadSelectedVehicles(); 
    }

    doOnResetButtonClick() {
        this.resetFilters();
        this.loadSelectedVehicles(); 
    }
 
    resetFilters() { 
        console.log("resetFilters ! ");  
        this.vehicleFilter = {
            brandFilter: { id: 0, name: "" },
            modelFilter: { id: 0, name: "" },
            sortKey: "",
            isSortAsc: true, 
            pageSize: this.PAGE_SIZE_DEFAULT,
            totalCount: this.useClientFiltering ? this.vehicleList_unfiltered.length : 0, 
            pageNo: 1 
        };
        this.models = [];   
    }

    async loadSelectedVehicles() {
        console.log("loadSelectedVehicles #1"); 
        if (this.useClientFiltering) {
            
            if (this.vehicleList_unfiltered.length == 0) {
                var queryResponse = await this.vehicleService.GetVehicles().toPromise(); 
                this.vehicleList_unfiltered = queryResponse.vehicles; 
            };
            this.vehicleList_filtered = this.vehicleList_unfiltered;  
            if (this.vehicleFilter.brandFilter.id)
                this.vehicleList_filtered = this.vehicleList_filtered.filter(v => v.brand.id == this.vehicleFilter.brandFilter.id);
            if (this.vehicleFilter.modelFilter.id) {
                this.vehicleList_filtered = this.vehicleList_filtered.filter(v => v.model.id == this.vehicleFilter.modelFilter.id);
            }; 
            if (this.vehicleFilter.sortKey) {
                switch (this.vehicleFilter.sortKey) {
                    case "brand":
                        this.vehicleList_filtered = this.vehicleList_filtered.sort((v1, v2) => { if (this.vehicleFilter.isSortAsc ? (v1.brand.name < v2.brand.name) : (v1.brand.name > v2.brand.name)) return -1; else return 1; });
                        break;
                    case "model":
                        this.vehicleList_filtered = this.vehicleList_filtered.sort((v1, v2) => { if (this.vehicleFilter.isSortAsc ? (v1.model.name < v2.model.name) : (v1.model.name > v2.model.name)) return -1; else return 1; });
                        break;
                    case "contact":
                        this.vehicleList_filtered = this.vehicleList_filtered.sort((v1, v2) => { if (this.vehicleFilter.isSortAsc ? (v1.contact.name < v2.contact.name) : (v1.contact.name > v2.contact.name)) return -1; else return 1; });
                        break
                };
            }
            // totalCount for all pages, filtered and not yet paged
            this.vehicleFilter.totalCount = this.vehicleList_filtered.length;
            // extract just one page from filtered list
            var pageSize: number = this.vehicleFilter.pageSize ? this.vehicleFilter.pageSize : this.PAGE_SIZE_DEFAULT;
            var pageNo: number = this.vehicleFilter.pageNo ? this.vehicleFilter.pageNo : 1; 
            this.vehicleList = []; 
            for (var i = 0; (i < pageSize)  && ((i + (pageSize * (pageNo-1))) < this.vehicleList_filtered.length); i++) { 
                this.vehicleList.push(this.vehicleList_filtered[i + (pageSize * (pageNo - 1))]); 
            }; 
            console.log("loadSelectedVehicles #6");  
        } else {
            // use Server filtering: filters are sent to server as URI params, http response returns filtered data 
            this.vehicleService.GetVehicles_Filtered(this.vehicleFilter)
                .subscribe(v => {
                    this.vehicleList = v.vehicles;
                    this.vehicleFilter.totalCount = v.totalCount_allPages;
                });
        }
    }

    doOnPageChanged(selectedPage: number) { 
        // alert('Page changed to ' + selectedPage);
        try {
            this.vehicleFilter.pageNo = selectedPage;
            this.loadSelectedVehicles();
        } catch (e) {
            console.log("Ecception cached in doOnPageChanged: \n" + e);
        }
 
    }
}
