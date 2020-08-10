import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AUTH_CONFIG } from '../../client.settings'; 

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})

export class NavMenuComponent { 

    private clientID: string = AUTH_CONFIG.clientID; 
 
    constructor(private auth: AuthService) {  
    }
 

}
