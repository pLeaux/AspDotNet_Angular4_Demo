import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router/src";
import { AuthService } from "./auth.service";
import { Observable } from "rxjs/Observable";

@Injectable()
export class CanActivateAdminService implements CanActivate{ 

    constructor(private authService: AuthService) {
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        return this.authService.isAdmin();
    } 


}
