export interface AuthResponse {

    accessToken: string;

    message: string;

    expiresIn: number;
    
    role:string;
    
    mustChangePassword: boolean;

}