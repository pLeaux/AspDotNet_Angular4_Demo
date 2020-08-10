import { Component, OnInit, NgZone} from '@angular/core';
import { AuthService } from "../../services/auth.service";
import { VehicleService } from "../../services/vehicle.service";
import { BrandVehicleCountType } from "../../../models/vehicle";
 

@Component({
    selector: 'app-admin',
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.css']
})
 
 
 

export class AdminComponent implements OnInit { 
 
    public chartData: ChartDataType;   
    brandVehicleCountList: BrandVehicleCountType[] = []; 

    constructor(private authService: AuthService, private vehicleService: VehicleService, private ngZone: NgZone) {  

    }

    ngOnInit() { 

        var aColor: number;
 

        var getRandomColor = function () {
            var r = Math.floor(Math.random() * 255);
            var g = Math.floor(Math.random() * 255);
            var b = Math.floor(Math.random() * 255);
            return "rgb(" + r + "," + g + "," + b + ")";
        };
 
        this.vehicleService.GetBrandsVehicleCount()
            .subscribe(brands => { 
                this.ngZone.run(() => {   
                    this.chartData = new ChartDataType;
                    this.chartData.labels.length = 0; 
                    this.brandVehicleCountList = brands;  
                    for (let i = 0; i < brands.length; i++) {
                        this.chartData.labels.push(brands[i].brandName);
                        this.chartData.datasets[0].data.push(brands[i].vehicleCount);
                        this.chartData.datasets[0].backgroundColor.push(getRandomColor());
                    }  
                }) 
            });   
    }
}


class ChartDataType {
    labels: string[]; // array of brand names 
    datasets: [
        {
            data: number[]; // vehicles count per brand
            backgroundColor: string[];  // bcolor for particula brand
        }
    ];
    constructor() {
        this.labels = new Array();
        this.datasets = [{ data: [], backgroundColor: [] }];
    }
};
