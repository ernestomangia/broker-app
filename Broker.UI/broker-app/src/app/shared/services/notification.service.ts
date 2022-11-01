import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
    providedIn: 'root'
})
export class NotificationService {

    constructor(private _toastrService: ToastrService) { }

    showSuccess(message: string, title: string) {
        this._toastrService.success(message, title)
    }

    showError(message: string, title: string) {
        this._toastrService.error(message, title)
    }

    showInfo(message: string, title: string) {
        this._toastrService.info(message, title)
    }

    showWarning(message: string, title: string) {
        this._toastrService.warning(message, title)
    }
}
