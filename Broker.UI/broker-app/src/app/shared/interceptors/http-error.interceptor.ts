import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, Observable, throwError } from "rxjs";
import { NotificationService } from "../services/notification.service";

@Injectable({
    providedIn: 'root'
})
export class HttpErrorInterceptor implements HttpInterceptor {

    constructor(private _notificationService: NotificationService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req)
            .pipe(
                catchError(error => {
                    let errorMessage = error.error?.message
                        ? error.error.message
                        : error.message;

                    errorMessage += error.status
                        ? ' | Error Code: ' + error.status
                        : '';

                    this._notificationService.showError(errorMessage, "Error");

                    return throwError(error);
                })
            )
    }
}