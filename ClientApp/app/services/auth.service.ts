import { Injectable } from '@angular/core';
import { AUTH_CONFIG } from '../client.settings'; 
import { Router, NavigationStart } from '@angular/router'; 
import Auth0Lock from 'auth0-lock';
import { tokenNotExpired, JwtHelper } from 'angular2-jwt'; 
import { RequestOptions, Headers } from "@angular/http";
 
 
@Injectable()
export class AuthService {

    public userProfile: any; 
    public roles: any = []; 
    public userID: string = "";

    lock = new Auth0Lock(AUTH_CONFIG.clientID, AUTH_CONFIG.domain, { 
        autoclose: true,
        auth: {
            redirectUrl: AUTH_CONFIG.callbackURL,
            responseType: 'token id_token',
            params: {
                scope: 'openid email profile' // default 0 just openId
            }, 
            // leop++
            audience: AUTH_CONFIG.audience, 
        }, 
        languageDictionary: {
            // emailInputPlaceholder: "something@youremail.com",
            title: "Login with Auth0"
        },
        additionalSignUpFields: [
            {
                name: "name",
                placeholder: "Input your name"
            }
        ]
    });
  

    constructor(public router: Router) { 

        this.loadUserDataFromLocalStorage();

        this.lock.on("authenticated", (authResult) => {
            this.doOnSuccessfullLogin(authResult);
        });
    }

    private doOnSuccessfullLogin(authResult: any) {
        console.log("Logged in, authResult:", authResult);
        localStorage.setItem('token', authResult.idToken); // jwt expects key "token" and not id_token
        this.lock.getUserInfo(authResult.accessToken, (error, profile) => {
            if (error)
                throw (error);
            localStorage.setItem('profile', JSON.stringify(profile));
            this.loadUserDataFromLocalStorage(); 
        }); 
    }

    private loadUserDataFromLocalStorage() {
        var jwtHelper = new JwtHelper();
        var accessToken: any;
        var decodedToken: any;  
        // get roles
        this.roles = [];  
        accessToken = localStorage.getItem('access_token'); 
        if (accessToken)
            decodedToken = jwtHelper.decodeToken(accessToken ? accessToken : ''); 
        if (accessToken && decodedToken) {
            this.roles = decodedToken[AUTH_CONFIG.roles_namespace] || [];
        } 
        console.log('Roles:', this.roles); 
        // get userProfile from accessToken (see: audience #2):  
        this.userProfile = JSON.parse(localStorage.getItem('profile') || '{}');  
        console.log('Profile', this.userProfile);  

    }

    public login(): void {
       this.lock.show();
    }

    // Call this method in app.component.ts, if using path-based routing
    public handleAuthentication(): void {
        console.log("handleAuthentication Start !");
        this.lock.on('authenticated', (authResult) => {
            console.log("handleAuthentication, authenticated !");
            if (authResult && authResult.accessToken && authResult.idToken) {
                this.setSession(authResult);
                this.router.navigate(['/']);
            }
        });
        this.lock.on('authorization_error', (err) => {
            console.log("handleAuthentication, authentication error !");
            this.router.navigate(['/']);
            console.log(err);
            alert(`Error: ${err.error}. Check the console for further details.`);
        });
        console.log("handleAuthentication END !");
    } 

    private setSession(authResult: any): void {
        // Set the time that the access token will expire at
        const expiresAt = JSON.stringify((authResult.expiresIn * 1000) + new Date().getTime());
        localStorage.setItem('access_token', authResult.accessToken);
        localStorage.setItem('token', authResult.idToken);  // localStorage.setItem('id_token', authResult.idToken);
        localStorage.setItem('expires_at', expiresAt); 
    }

    public logout(): void {
        // Remove tokens and expiry time from localStorage
        localStorage.removeItem('access_token');
        localStorage.removeItem('token');
        localStorage.removeItem('expires_at');
        localStorage.removeItem('profile');
        // Go back to the home route
        this.router.navigate(['/']);
    }

    public isAuthenticated(): boolean {
        // Check whether the current time is past the access token's expiry time 
        // version Mosh (usin angular2-jwt helper method) 
        if (AUTH_CONFIG.ignoreAuthorize)  
            return true;
        else 
            return tokenNotExpired('token');   
        
    }

    public getApiAccessToken(): string {
        var s = localStorage.getItem('access_token');
        var accessToken: string = (s !== null) ? s : ''; 
        return 'Bearer ' +  accessToken; 
    }
     
    public GetHttpHeaderAuthorizationOptions(): RequestOptions { 
        if (this.isAuthenticated()) {
            var accessToken: string = this.getApiAccessToken();
            var requestOptions = new RequestOptions();
            var headers: Headers = new Headers();
            headers.append('Authorization', this.getApiAccessToken());
            // headers.append('Content-Type', 'application/json');
            requestOptions.headers = headers;
            return requestOptions;
        } else {
            throw new Error('Log in, to perform that operation');
        }
    }

    public isAdmin(): boolean {
        // Check whether the current time is past the access token's expiry time, version Mosh (usin angular2-jwt helper method); "ignoreAuthorize" is just for testing  
        if (AUTH_CONFIG.ignoreAuthorize) 
            return true;
        return ( this.isAuthenticated() && (this.roles.indexOf('admin') > -1));     
    }

    public isAuthenticatedUser(userID: string): boolean {
        console.log("isAuthenticatedUser(), userID = " + userID ); 
        if (this.isAuthenticated && this.userProfile && this.userProfile.sub && (this.userProfile.sub == userID)) {
            return true; 
        } else {
            return false;
        }

    }

}