export interface ContactData {
    name: string;
    phone: string;
    email: string;  
}

export interface IdNameType {
    id: number;
    name: string; 
}

export interface VehicleSaveType {
    id: number;
    brandId: number; 
    modelId: number;
    isRegistered: boolean;
    featureIds: number[];
    contact: ContactData;
}

export interface VehicleGetType {
    id: number;
    brand: IdNameType;
    model: IdNameType;
    isRegistered: boolean;
    userID: string;
    contact: ContactData;  
    features: IdNameType[];
}

export interface ModelType {
    id: number;
    name: string;
    brandId: number;
}

export interface VehicleFilterType {
    brandFilter: IdNameType;
    modelFilter: IdNameType; 
    sortKey?: string;
    isSortAsc?: boolean; 
    pageSize?: number;
    pageNo?: number; 
    totalCount?: number;
}

export interface VehicleQueryResultType {
    totalCount_allPages: number;
    vehicles: VehicleGetType[]; 
}

export interface PhotoDescriptionType {
    id: number; 
    fileName: string; 
    vehicleId: number;

}

export interface BrandVehicleCountType {
    brandName: string; 
    vehicleCount: number;
}

 