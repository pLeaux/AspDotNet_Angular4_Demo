interface AuthConfig {
    clientID: string;
    domain: string;
    callbackURL: string;
    audience: string; // = auth0 application "identifier"
    // testToken: string;
    roles_namespace: string; 
    ignoreAuthorize: boolean;
}

export const AUTH_CONFIG: AuthConfig = {
    ignoreAuthorize: false, // just for testing purposes, if no internet connection awailable
    clientID: 'KR8P31Fcw4UeAV30PBVVCS4DGuxPAKBm', // Vega Client Demo
    // clientID: 'wp6EsCIiq9EJgWKhJuIk3gqi7EiCyQ8h', // Vega Demo API 
    domain: 'leaux-vega-demo.eu.auth0.com', 
    callbackURL: 'http://localhost:49707',
    audience: 'http://localhost:49707/api', // Vega Demo API Identifier (logical)
    // testToken: "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2xlYXV4LXZlZ2EtZGVtby5ldS5hdXRoMC5jb20vIiwic3ViIjoid3A2RXNDSWlxOUVKZ1dLaEp1SWszZ3FpN0VpQ3lROGhAY2xpZW50cyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDk3MDcvYXBpIiwiaWF0IjoxNTU4Mzk4MzYxLCJleHAiOjE1NTg0ODQ3NjEsImF6cCI6IndwNkVzQ0lpcTlFSmdXS2hKdUlrM2dxaTdFaUN5UThoIiwiZ3R5IjoiY2xpZW50LWNyZWRlbnRpYWxzIn0.fsk6dzv9riWWf6sZn_WFV7zPrWYJVaHvlJ1ToyKru3c"
    roles_namespace: 'http://leaux-vega-demo.lpdata.si/roles'  // looks like "auth0.com" is not allowed as a namespace and "http://" is obligatory 
    
};                    
