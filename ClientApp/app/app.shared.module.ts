import { NgModule, ErrorHandler, isDevMode } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule, BrowserXhr } from '@angular/http';
import { RouterModule } from '@angular/router'; 
import { ToastyModule } from 'ng2-toasty'; 
import { Logger } from 'angular2-logger/core';
import * as Raven from 'raven-js';

import { AuthService } from './services/auth.service'; 

import { AppErrorHandler } from "./app.error-handler";
import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';

import { HomeComponent } from './components/home/home.component'; 

import { PaginationComponent } from './custom_components/pagination.component'; 
import { VehicleFormComponent } from './components/vehicle-form/vehicle-form.component';
import { VehicleListComponent } from './components/vehicle-list/vehicle-list.component';
import { ViewVehicleComponent } from './components/view-vehicle/view-vehicle.component';
import { VehicleService } from './services/vehicle.service'; 
import { PhotoService } from './services/photo.service'; 
import { ProgressService, BrowserXhrWithProgress } from './services/progress.service'; 
import { AUTH_PROVIDERS } from "angular2-jwt/angular2-jwt";
import { AdminComponent } from './components/admin/admin.component';
import { CanActivateAdminService } from "./services/can-activate-admin.service";
import { CanActivateAuthenticatedService } from "./services/can-activate-authenticated.service";

import { ChartModule } from 'angular2-chartjs'; 


// configure Raven with Sentry Project Client key (url: "https://sentry.io/settings/leo-puskaric/projects/vega-demo-app/keys/")
try {
    Raven.config('https://4258705e8724417ab5e9f00528102d22@sentry.io/1452907').install();
} catch (e) {
    if (isDevMode())
        console.log(e);
}
       


@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent, 
        HomeComponent,
        VehicleFormComponent,
        VehicleListComponent,
        PaginationComponent,
        ViewVehicleComponent,
        AdminComponent 
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        ChartModule, 
        ToastyModule.forRoot(), 
        RouterModule.forRoot([
            { path: '', redirectTo: 'vehicle-list', pathMatch: 'full' },
            { path: 'home', component: HomeComponent }, 
            { path: 'vehicle-list', component: VehicleListComponent },
            { path: 'vehicle-form', component: VehicleFormComponent, canActivate: [CanActivateAuthenticatedService] }, 
            { path: 'vehicle-form/:id', component: VehicleFormComponent, canActivate: [CanActivateAuthenticatedService] },
            { path: 'view-vehicle/:id', component: ViewVehicleComponent },
            { path: 'admin', component: AdminComponent, canActivate: [CanActivateAdminService] }, 
            { path: '**', redirectTo: 'vehicle-list' } 
        ])
    ], 
    providers: [ 
        { provide: ErrorHandler, useClass: AppErrorHandler },  // replace default errorHandler with my
        { provide: BrowserXhr, useClass: BrowserXhrWithProgress },  // replace default Angular's HttpRequest  with my
        ProgressService, 
        Logger,
        AUTH_PROVIDERS, // AuthHttp + dependent modules
        VehicleService,
        PhotoService,
        AuthService,
        CanActivateAdminService,
        CanActivateAuthenticatedService
              
    ]
})
export class AppModuleShared {
}
